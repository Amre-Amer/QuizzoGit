using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ballroom : MonoBehaviour {
    WebCamTexture cameraTex;
    GameObject plane;
    int side = 5;
    int numThingCopies = 60;
    public Camera cam;
    float zDist = 5;
    public Light lite;
    float xLast;
    float yLast;
    GameObject goBoxes;
    Button buttonAlign;
    Button buttonFOV;
    Canvas canvas;
    Image imageCameraX;
    Image imageCameraY;
    Image imageCameraZ;
    Text textCameraX;
    Text textCameraY;
    Text textCameraZ;
    Image imageBoxesX;
    Image imageBoxesY;
    Image imageBoxesZ;
    Text textBoxesX;
    Text textBoxesY;
    Text textBoxesZ;
    Image imageGyroX;
    Image imageGyroY;
    Image imageGyroZ;
    Text textGyroX;
    Text textGyroY;
    Text textGyroZ;
    Mode mode = Mode.fov;
    Text textFOV;
    GameObject cube;
    Button buttonRadius;
    float radius = 8;
    GameObject parentBoxes;
    float radiusLast;
    Text textRadius;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        buttonAlign = canvas.transform.Find("ButtonAlign").GetComponent<Button>();
        buttonAlign.onClick.AddListener(ButtonAlignClicked);
        buttonFOV = canvas.transform.Find("ButtonFOV").GetComponent<Button>();
        buttonFOV.onClick.AddListener(ButtonFOVClicked);
        buttonRadius = canvas.transform.Find("ButtonRadius").GetComponent<Button>();
        imageCameraX = GameObject.Find("ImageCameraX").GetComponent<Image>();
        imageCameraY = GameObject.Find("ImageCameraY").GetComponent<Image>();
        imageCameraZ = GameObject.Find("ImageCameraZ").GetComponent<Image>();
        textCameraX = GameObject.Find("TextCameraX").GetComponent<Text>();
        textCameraY = GameObject.Find("TextCameraY").GetComponent<Text>();
        textCameraZ = GameObject.Find("TextCameraZ").GetComponent<Text>();
        imageBoxesX = GameObject.Find("ImageBoxesX").GetComponent<Image>();
        imageBoxesY = GameObject.Find("ImageBoxesY").GetComponent<Image>();
        imageBoxesZ = GameObject.Find("ImageBoxesZ").GetComponent<Image>();
        textBoxesX = GameObject.Find("TextBoxesX").GetComponent<Text>();
        textBoxesY = GameObject.Find("TextBoxesY").GetComponent<Text>();
        textBoxesZ = GameObject.Find("TextBoxesZ").GetComponent<Text>();
        imageGyroX = GameObject.Find("ImageGyroX").GetComponent<Image>();
        imageGyroY = GameObject.Find("ImageGyroY").GetComponent<Image>();
        imageGyroZ = GameObject.Find("ImageGyroZ").GetComponent<Image>();
        textGyroX = GameObject.Find("TextGyroX").GetComponent<Text>();
        textGyroY = GameObject.Find("TextGyroY").GetComponent<Text>();
        textGyroZ = GameObject.Find("TextGyroZ").GetComponent<Text>();
        textFOV = GameObject.Find("TextFOV").GetComponent<Text>();
        plane = GameObject.Find("Plane");
        goBoxes = GameObject.Find("Boxes");
        cube = GameObject.Find("Cube");
        goBoxes.transform.position = cam.transform.position;
        textRadius = GameObject.Find("TextRadius").GetComponent<Text>();
    }

    // Use this for initialization
    IEnumerator Start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        cameraTex = new WebCamTexture();
        plane.GetComponent<Renderer>().material.mainTexture = cameraTex;
        cameraTex.Play();
        Input.gyro.enabled = true;
        //LoadThings();
        //LoadBoxes();
        UpdateFOVpos();
        LoadCubes();
        Debug.Log("screen " + Screen.width + " x " + Screen.height + "\n");
    }

    // Update is called once per frame
    void Update () {
        UpdateCamRotation();
        PositionPlane();
        PositionLight();
        UpdateRotate();
        UpdateFOV();
        UpdateCameraInfo();
        UpdateSelect();
        //UpdateRadius();
    }

    void UpdateRadius()
    {
        if (GetMode() != Mode.radius) return;
        //if (radius == radiusLast) return;
        if (parentBoxes != null) Destroy(parentBoxes);
        UpdateRadiuspos();
        LoadBoxes();
        radiusLast = radius;
    }

    void UpdateRadiuspos()
    {
        Vector3 pos = buttonRadius.transform.position;
        radius = 1 + Input.mousePosition.x / Screen.width * 10;
        pos.x = Input.mousePosition.x;
        buttonRadius.transform.position = pos;
        textRadius.text = radius.ToString("F0");
    }

    void UpdateSelect()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.DrawLine(ray.origin, hit.point);
            GameObject go = hit.collider.gameObject;
            if (go.name != "Plane")
            {
                Destroy(go);
            }
        }
    }

    void LoadCubes()
    {
        if (parentBoxes == null)
        {
            parentBoxes = new GameObject("parentBoxes");
        }
        int numCubes = 10000;
        for (int n = 0; n < numCubes; n++)
        {
            float alt = Random.Range(-90, 90);
            float azim = Random.Range(0, 360);
            GameObject go = Instantiate(cube);
            go.transform.parent = parentBoxes.transform;
            go.transform.transform.position = cam.transform.position;
            go.transform.eulerAngles = new Vector3(alt, azim, 0);
            go.transform.position += go.transform.forward * radius;
            float sca = Random.Range(.1f, 1);
            go.transform.localScale = new Vector3(sca, sca, sca / 10);
        }
    }

    Mode GetMode()
    {
        Mode mode = Mode.none;
        if (IsOverButton(buttonAlign) == true) mode = Mode.rotate;
        if (IsOverButton(buttonFOV) == true) mode = Mode.fov;
        if (IsOverButton(buttonRadius) == true) mode = Mode.radius;
        Debug.Log("mode " + mode + "\n");
        return mode;
    }

    bool IsOverButton(Button button)
    {
        bool yn = false;
        float x = button.transform.position.x;
        float y = button.transform.position.y;
        float w = button.GetComponent<RectTransform>().sizeDelta.x;
        float h = button.GetComponent<RectTransform>().sizeDelta.y;
        Rect rect = new Rect(x - w/2, y - h/2, w, h);
        if (rect.Contains(new Vector2(Input.mousePosition.x, Input.mousePosition.y)) == true)
        {
            yn = true;
        }
        return yn;
    }

    void UpdateFOVpos()
    {
        Vector3 pos = buttonFOV.transform.position;
        pos.y = cam.fieldOfView / 180 * Screen.height;
        buttonFOV.transform.position = pos;
        textFOV.text = cam.fieldOfView.ToString("F0") + "°";
    }

    void UpdateFOV()
    {
        if (GetMode() != Mode.fov) return;
        if (Input.GetMouseButtonUp(0) == true)
        {
            UpdateFOVpos();
        }
        if (Input.GetMouseButton(0) == false) return;
        if (Input.GetMouseButtonDown(0) == true)
        {
            xLast = Input.mousePosition.x;
            yLast = Input.mousePosition.y;
        }
        if (xLast != Input.mousePosition.x || yLast != Input.mousePosition.y)
        {
//            Debug.Log("fov\n");
            float fov = Input.mousePosition.y / Screen.height * 180;
            cam.fieldOfView = fov;
            UpdateFOVpos();
            xLast = Input.mousePosition.x;
            yLast = Input.mousePosition.y;
        }
    }

    void UpdateRotate()
    {
        if (GetMode() != Mode.rotate) return;
        if (Input.GetMouseButtonUp(0) == true)
        {
            Vector3 pos = buttonAlign.transform.position;
            pos.x = Screen.width / 2;
            buttonAlign.transform.position = pos;
        }
        if (Input.GetMouseButton(0) == false) return;
        if (Input.GetMouseButtonDown(0) == true)
        {
            xLast = Input.mousePosition.x;
            yLast = Input.mousePosition.y;
        }
        if (xLast != Input.mousePosition.x || yLast != Input.mousePosition.y)
        {
            Vector3 pos = buttonAlign.transform.position;
            pos.x = Input.mousePosition.x;
            buttonAlign.transform.position = pos;
           // Debug.Log("rotate\n");
            float diff = Input.mousePosition.x - xLast;
            goBoxes.transform.Rotate(0, 0, -diff / 100);
            xLast = Input.mousePosition.x;
            yLast = Input.mousePosition.y;
        }
    }

    void ButtonFOVClicked()
    {
        //mode = Mode.fov;
        Debug.Log("FOV\n");
        //Vector3 eul = goBoxes.transform.eulerAngles;
        //eul.z = cam.transform.eulerAngles.y + Input.gyro.attitude.eulerAngles.x;
        //goBoxes.transform.eulerAngles = eul;
    }

    void ButtonAlignClicked()
    {
        //mode = Mode.rotate;
        Debug.Log("align\n");
        //Vector3 eul = goBoxes.transform.eulerAngles;
        //eul.z = cam.transform.eulerAngles.y + Input.gyro.attitude.eulerAngles.x;
        //goBoxes.transform.eulerAngles = eul;
    }

    Quaternion GetGyro()
    {
        return Input.gyro.attitude;
    }

    void UpdateCameraInfo()
    {
        float ang = 0;
        //
        ang = cam.transform.eulerAngles.x;
        UpdateXYX(textCameraX, imageCameraX, ang);
        //
        ang = cam.transform.eulerAngles.y;
        UpdateXYX(textCameraY, imageCameraY, ang);
        //
        ang = cam.transform.eulerAngles.z;
        UpdateXYX(textCameraZ, imageCameraZ, ang);
        //
        //
        ang = goBoxes.transform.eulerAngles.x;
        UpdateXYX(textBoxesX, imageBoxesX, ang);
        //
        ang = goBoxes.transform.eulerAngles.y;
        UpdateXYX(textBoxesY, imageBoxesY, ang);
        //
        ang = goBoxes.transform.eulerAngles.z;
        UpdateXYX(textBoxesZ, imageBoxesZ, ang);
        //
        ang = Input.gyro.attitude.eulerAngles.x;
        UpdateXYX(textGyroX, imageGyroX, ang);
        //
        ang = Input.gyro.attitude.eulerAngles.y;
        UpdateXYX(textGyroY, imageGyroY, ang);
        //
        ang = Input.gyro.attitude.eulerAngles.z;
        UpdateXYX(textGyroZ, imageGyroZ, ang);
    }

    void UpdateXYX(Text text, Image image, float ang)
    {
        text.text = ang.ToString("F0") + "°";
        image.transform.eulerAngles = new Vector3(0, 0, ang);
    }

    void LoadBoxes()
    {
        GameObject goBox = GameObject.Find("Box");
        for (int ix = -side; ix < side; ix++)
        {
            for (int iy = -side; iy < side; iy++)
            {
                for (int iz = -side; iz < side; iz++)
                {
                    GameObject go = Instantiate(goBox);
                    go.transform.parent = goBoxes.transform;
                    go.transform.position = cam.gameObject.transform.position + new Vector3(ix, iy, iz);
//                    boxes[ix, iy, iz] = go;
                }
            }
        }
    }

    void PositionLight()
    {
        lite.transform.position = cam.transform.position + cam.transform.right + cam.transform.up;
        lite.transform.rotation = cam.transform.rotation;
        lite.transform.Rotate(90, 0, 0);
    }

    void PositionPlane()
    {
        float scaX = Mathf.Tan(cam.fieldOfView / 2 * Mathf.Deg2Rad) * zDist / 2;
        float scaY = scaX / cam.aspect;
        plane.transform.localScale = new Vector3(-scaX, 1, scaY);
        //
        Vector3 pos = cam.transform.position + cam.transform.forward * zDist;
        plane.transform.position = pos;
        plane.transform.rotation = cam.transform.rotation;
        plane.transform.Rotate(90, 180, 0);
    }

    void UpdateCamRotation()
    {
        if (Application.isEditor == true) return;
        cam.transform.rotation = Input.gyro.attitude;
        Vector3 eul = cam.transform.eulerAngles;
        cam.transform.eulerAngles = new Vector3(-eul.x, -eul.y, eul.z);
    }

    void LoadThings()
    {
        GameObject goParent = GameObject.Find("Things");
        foreach (Transform t in goParent.transform)
        {
            GameObject goOriginal = t.gameObject;
            for (int n = 0; n < numThingCopies; n++)
            {
                float x = Random.Range(-side, side);
                float y = Random.Range(-side, side);
                float z = Random.Range(-side, side);
                Vector3 pos = new Vector3(x, y, z);
                GameObject go = Instantiate(goOriginal);
                go.transform.position = cam.gameObject.transform.position + pos;
                //things.Add(go);
                //AddToPulsing(go);
            }
        }
    }

}

public enum Mode
{
    none,
    fov,
    rotate,
    radius
}
