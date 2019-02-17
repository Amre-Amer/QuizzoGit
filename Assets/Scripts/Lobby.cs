using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class Lobby : MonoBehaviour
{
    Text textFps;
    Button buttonLogin;
    Button buttonMonoStereo;
    Button buttonCreds;
    UnityEngine.UI.Image imageBackground;
    [HideInInspector]
    public InputField inputEmail;
    [HideInInspector]
    public InputField inputFirst;
    [HideInInspector]
    public InputField inputLast;
    public bool ynStereo;
    int cntFrames;
    int cntFps;
    Persistent persistentMgr;

    // Use this for initialization
    private void Awake()
    {
        persistentMgr = GetComponent<Persistent>();
        buttonLogin = GameObject.Find("ButtonLogin").GetComponent<Button>();
        buttonMonoStereo = GameObject.Find("ButtonMonoStereo").GetComponent<Button>();
        buttonCreds = GameObject.Find("ButtonCredentials").GetComponent<Button>();
        imageBackground = GameObject.Find("Credentials").GetComponent<UnityEngine.UI.Image>();
        inputEmail = GameObject.Find("Email").GetComponent<InputField>();
        inputFirst = GameObject.Find("First").GetComponent<InputField>();
        inputLast = GameObject.Find("Last").GetComponent<InputField>();
        textFps = GameObject.Find("TextFps").GetComponent<Text>();
        buttonLogin.onClick.AddListener(ButtonLoginClicked);
        buttonMonoStereo.onClick.AddListener(ButtonMonoStereoClicked);
        buttonCreds.onClick.AddListener(ButtonCredsClicked);
        CheckIfEditor();
        imageBackground.gameObject.SetActive(false);
        CheckIfNeeded();
        InvokeRepeating("UpdateFps", 1, 1);
    }

    private void Update()
    {
        cntFrames++;
        cntFps++;
    }

    void UpdateFps()
    {
        textFps.text = cntFps + " fps";
        cntFps = 0;
    }

    void CheckIfEditor()
    {
        if (Application.isEditor == true)
        {
            PositionalDeviceTracker pdt = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();
            if (pdt != null && pdt.IsActive)

            {
                Debug.Log("stopping device pose\n");
                pdt.Stop();
            }
        }
    }

    void CheckIfNeeded()
    {
        string txt = persistentMgr.ReadPathFile();
        if (txt == null)
        {
            Invoke("ButtonCredsClicked", 1);
            Invoke("HideCredentials", 5);
        }
        else
        {
            LoadCredentials();
        }
    }

    void HideCredentials()
    {
        imageBackground.gameObject.SetActive(false);
    }

    void ButtonMonoStereoClicked()
    {
        ynStereo = !ynStereo;
        if (ynStereo == true)
        {
            MixedRealityController.Instance.SetMode(MixedRealityController.Mode.VIEWER_AR_DEVICETRACKER);
            buttonMonoStereo.GetComponentInChildren<Text>().text = "stereo";
        }
        else
        {
            MixedRealityController.Instance.SetMode(MixedRealityController.Mode.HANDHELD_AR_DEVICETRACKER);
            buttonMonoStereo.GetComponentInChildren<Text>().text = "mono";
        }
    }

    void ButtonCredsClicked()
    {
        if (imageBackground.gameObject.activeSelf == false)
        {
            imageBackground.gameObject.SetActive(true);
            LoadCredentials();
        }
        else
        {
            imageBackground.gameObject.SetActive(false);
        }
    }

    void LoadCredentials()
    {
        string txt = persistentMgr.ReadPathFile();
        if (txt == null) return;
        string[] stuff = txt.Split(',');
        if (stuff.Length != 3) return;
        inputEmail.text = stuff[0];
        inputFirst.text = stuff[1];
        inputLast.text = stuff[2];
    }

    void ButtonLoginClicked()
    {
        imageBackground.gameObject.SetActive(false);
        string txt = inputEmail.text + "," + inputFirst.text + "," + inputLast.text;
        persistentMgr.WriteToPathFile(txt);
        LoadCredentials();
        Debug.Log("Credentials " + txt + "\n");
    }
}
