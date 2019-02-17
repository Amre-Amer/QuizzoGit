using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TestLens : Test {
    GameObject meshGo;
    MeshCamMesh meshManager;
    Texture2D tex;
    //[Range(-1000f, 1000f)]
    float amplitude = 10;
    int cntFrames;
    WebCamTexture cam_texture;
    WebCamDevice[] cam_devices;
    bool ynFrontCam;
    Camera camVR;
    Material mat;
    RenderTexture rt;
    GameObject goMirrors;
    //GameObject goBackgroundPlane;
    List<GameObject> mirrors = new List<GameObject>();
    int resWebCamX = 64;
    int resWebCamY = 64;

        // Use this for initialization
	public override void StartTest () {
        base.StartTest();
        Debug.Log("TestLens\n");
        camVR = goWorld.transform.Find("Camera").gameObject.GetComponent<Camera>();
        camVR.transform.position = goImageMarker.transform.position;
        camVR.transform.eulerAngles = goImageMarker.transform.eulerAngles;
        camVR.transform.parent = goImageMarker.transform;
        camVR.transform.Rotate(90, 0, 0);
        //
        mat = Resources.Load<Material>("Lens") as Material;
        rt = Resources.Load<RenderTexture>("Lens") as RenderTexture;
        camVR.targetTexture = rt;
        //
        meshManager = new MeshCamMesh();
        BlitImage();
        meshGo = meshManager.CreateMeshGoFromTexture(tex);
        meshGo.transform.position = goImageMarker.transform.position;
        meshGo.transform.eulerAngles = goImageMarker.transform.eulerAngles;
        meshGo.transform.parent = goImageMarker.transform;
        meshGo.transform.localScale = new Vector3(.02f, .02f, .02f);
        meshGo.transform.localPosition = Vector3.zero;
        amplitude = 5f;
        cam_devices = WebCamTexture.devices;
        //cam_texture = new WebCamTexture(cam_devices[0].name, resWebCamX, resWebCamY, 30);
        /*
        if (cam_texture != null)
        {
            cam_texture.Play();
            Debug.Log("resWeb:" + resWebCamX + " x " + resWebCamY + "\n");
            Debug.Log("webTex:" + cam_texture.width + " x " + cam_texture.height + "\n");
            Debug.Log("playing...\n");
        }
        */
        //

        //        goBackgroundPlane = GameObject.Find("BackgroundPlane");
        goMirrors = goWorld.transform.Find("Mirrors").gameObject;
//        Material matPassThru = goBackgroundPlane.GetComponent<Renderer>().material;
//        if (matPassThru != null)
//        {
            foreach (Transform t in goMirrors.transform)
            {
                GameObject go = t.transform.gameObject;
                mirrors.Add(go);
                //go.GetComponent<Renderer>().material.mainTexture = cam_texture;
            }
//        }
        messageMgr.ShowMesssage("Point at\nobjects!");
    }

    public override void FinishTest()
    {
        base.FinishTest();
        if (camVR != null) camVR.transform.parent = goWorld.transform;
        DestroyImmediate(meshGo);
        mirrors.Clear();
    }

    // Update is called once per frame
    public override void Update () {
        if (ynActive == false) return;
        BlitImage();
        meshManager.meshHeight = amplitude;
        meshManager.UpdateMeshGoWithTexture(meshGo, tex);
        UpdateMirrors();
        cntFrames--;
	}

    void UpdateMirrors()
    {
        foreach(GameObject go in mirrors)
        {
            int n = mirrors.IndexOf(go);
            if (n > 0)
            {
                go.transform.Rotate(.5f, -.5f, .5f);
            }
        }
    }

    void BlitImage()
    {
        if (tex != null) {
            DestroyImmediate(tex);
        }
        tex = new Texture2D(rt.width, rt.height);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;
    }
}
