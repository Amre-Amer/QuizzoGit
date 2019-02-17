using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour {
    [HideInInspector]
    public GameObject goPopUp;
    [HideInInspector]
    public GameObject goPopUpScore;
    [HideInInspector]
    public Text textLog;
    [HideInInspector]
    public GameObject goLoading;
    [HideInInspector]
    public GameObject goAngle;
    [HideInInspector]
    public GameObject goMessage;
    [HideInInspector]
    public float angGoAngle;
    [HideInInspector]
    public GameObject goReplay;
    GameObject cam;
    float distPopUp = .6f;
    Vector3 scaleGoLoading;
    float timeStart;
    float duration = .1f;
    bool ynOn;
    float distGoLoading = .6f;
    GameObject goMessages;
    Audio audioMgr;
    string txtMessage;
    float durationReplayOn = .75f;
    float durationReplayOff = .25f;

    // Use this for initialization
    void Start () {
        audioMgr = GetComponent<Audio>();
        textLog = GameObject.Find("Log").GetComponent<Text>();
        goMessages = GameObject.Find("Messages");
        goPopUp = goMessages.transform.Find("PopUp").gameObject;
        goPopUpScore = goMessages.transform.Find("PopUpScore").gameObject;
        goLoading = goMessages.transform.Find("Loading").gameObject;
        goMessage = goMessages.transform.Find("Message").gameObject;
        goAngle = goMessages.transform.Find("Angle").gameObject;
        goReplay = goMessages.transform.Find("Replay").gameObject;
        cam = GameObject.Find("ARCamera");
        goPopUpScore.SetActive(false);
        goPopUp.SetActive(false);
        scaleGoLoading = goLoading.transform.localScale;
        goLoading.SetActive(false);
        goMessage.SetActive(false);
        InvokeRepeating("UpdateLog", 1, 1);
        goAngle.SetActive(false);
        goReplay.SetActive(false);
        Invoke("ReplayOff", durationReplayOn);
    }

    // Update is called once per frame
    void Update () {
        UpdatePopUpScore();
        UpdatePopUp();
        UpdateGoLoading();
        UpdateGoAngle();
        UpdateGoMessage();
    }

    void UpdateReplay()
    {
        if (goReplay.activeSelf == false) return;
        goReplay.transform.eulerAngles = cam.transform.eulerAngles;
        goReplay.transform.position = cam.transform.position + cam.transform.forward * distPopUp;
    }

    public void ShowReplay()
    {
        goReplay.SetActive(true);
    }

    public void HideReplay()
    {
        goReplay.SetActive(false);
    }

    void ReplayOn()
    {
        goReplay.GetComponent<TextMesh>().color = Color.magenta;
        Invoke("ReplayOff", durationReplayOn);
    }

    void ReplayOff()
    {
        goReplay.GetComponent<TextMesh>().color = Color.clear;
        Invoke("ReplayOn", durationReplayOff);
    }

    public void ShowMessage0()
    {
        goMessage.SetActive(true);
        goMessage.GetComponent<TextMesh>().text = txtMessage;
        audioMgr.PlaySound(ClipType.acknowledge);
        Invoke("HideMessage", 3);
    }

    public void ShowMesssage(string txt)
    {
        txtMessage = txt;
        Invoke("ShowMessage0", 1);
    }

    public void HideMessage()
    {
        goMessage.SetActive(false);
    }

    void  UpdateGoMessage()
    {
        if (goMessage.activeSelf == false) return;
        goMessage.transform.eulerAngles = cam.transform.eulerAngles;
        goMessage.transform.position = cam.transform.position + cam.transform.forward * distPopUp;
    }

    void UpdateGoAngle()
    {
        if (goAngle.activeSelf == false) return;
        goAngle.transform.position = cam.transform.position;
        goAngle.transform.eulerAngles = cam.transform.eulerAngles;
        goAngle.transform.position += goAngle.transform.forward * distPopUp;
        goAngle.transform.Rotate(0, 0, angGoAngle);
    }

    void UpdateGoLoading()
    {
        if (goLoading.activeSelf == false) return;
        if (Time.realtimeSinceStartup - timeStart >= duration)
        {
            timeStart = Time.realtimeSinceStartup;
            ynOn = !ynOn;
            if (ynOn == true)
            {
                goLoading.transform.localScale = scaleGoLoading * 1.5f;
            }
            else
            {
                goLoading.transform.localScale = scaleGoLoading;
            }
        }
        goLoading.transform.eulerAngles = cam.transform.eulerAngles;
        goLoading.transform.position = cam.transform.position + cam.transform.forward * distGoLoading;
    }

    void UpdateLog()
    {
        textLog.text += ".\n";
    }

    public void UpdatePopUp()
    {
        if (goPopUp.activeSelf == true)
        {
            goPopUp.transform.eulerAngles = cam.transform.eulerAngles;
            goPopUp.transform.position = cam.transform.position + cam.transform.forward * distPopUp;
        }
    }

    public void UpdatePopUpScore()
    {
        if (goPopUpScore.activeSelf == true)
        {
            goPopUpScore.transform.eulerAngles = cam.transform.eulerAngles;
            goPopUpScore.transform.position = cam.transform.position + cam.transform.forward * distPopUp;
            goPopUpScore.transform.localScale += new Vector3(.001f, .001f, .001f);
            goPopUpScore.GetComponent<Renderer>().material.color = Random.ColorHSV();
            if (goPopUpScore.transform.localScale.x >= .05f)
            {
                goPopUpScore.SetActive(false);
            }
        }
    }
}
