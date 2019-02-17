using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [HideInInspector]
    public GameObject goWorld;
    [HideInInspector]
    public GameObject goImageMarker;
    [HideInInspector]
    public bool ynActive;
    [HideInInspector]
    public float distNear;
    [HideInInspector]
    public float distNearFound;
    [HideInInspector]
    public List<GameObject> gos = new List<GameObject>();
    [HideInInspector]
    public Quizzy quizzyMgr;
    [HideInInspector]
    public Lobby lobbyMgr;
    [HideInInspector]
    public Post postMgr;
    [HideInInspector]
    public Track trackMgr;
    [HideInInspector]
    public Asset assetMgr;
    [HideInInspector]
    public Message messageMgr;
    [HideInInspector]
    public Udp udpMgr;
    [HideInInspector]
    public GameObject goHandle;
    GameObject goNearFoundTouch;
    [HideInInspector]
    public GameObject cam;
    [HideInInspector]
    public Audio audioMgr;
    [HideInInspector]
    public Persistent persistentMgr;
    [HideInInspector]
    public Replay replayMgr;
    [HideInInspector]
    public Buttons buttonsMgr;
    Vector3 scaleGoHighlight;
    GameObject goHighlight;

    // Use this for initialization of base class
    public void InitTest()
    {
        goWorld = GameObject.Find("World" + GetType());
        Debug.Log("world " + goWorld.name + "\n");
        goImageMarker = GameObject.Find("ImageMarker");
        audioMgr = GetComponent<Audio>();
        cam = GameObject.Find("ARCamera");
        goHandle = GameObject.Find("Handle");
        quizzyMgr = GetComponent<Quizzy>();
        lobbyMgr = GetComponent<Lobby>();
        postMgr = GetComponent<Post>();
        trackMgr = GetComponent<Track>();
        assetMgr = GetComponent<Asset>();
        messageMgr = GetComponent<Message>();
        udpMgr = GetComponent<Udp>();
        persistentMgr = GetComponent<Persistent>();
        replayMgr = GetComponent<Replay>();
        buttonsMgr = GetComponent<Buttons>();
        goWorld.SetActive(false);
    }

    // Use this for initialization
    public virtual void StartTest()
    {
        messageMgr.textLog.text += "starting test " + GetType() + "\n";
        ynActive = true;
        goWorld.SetActive(true);
        if (replayMgr.state != ReplayState.replay)
        {
            replayMgr.state = ReplayState.record;
            Debug.Log(GetType() + " starting record\n");
        }
        ResetTest();
    }

    // Use this for cleanup on exit
    public virtual void FinishTest()
    {
        messageMgr.goMessage.SetActive(false);
        //messageMgr.HideReplay();
        messageMgr.textLog.text += "finishing test " + GetType() + "\n";
        ynActive = false;
        goWorld.SetActive(false);
        if (replayMgr.state != ReplayState.replay)
        {
            replayMgr.state = ReplayState.inactive;
            PostResults();
            PostReplay();
        }
        ResetTest();
    }

    public virtual void Update()
    {

    }

    public void PopUpScore(string txt)
    {
        messageMgr.goPopUpScore.SetActive(true);
        messageMgr.goPopUpScore.transform.localScale = new Vector3(.001f, .001f, .001f);
        messageMgr.goPopUpScore.GetComponent<TextMesh>().text = txt;
    }

    void ResetTest()
    {
        gos.Clear();
        audioMgr.audioSource.Stop();
        trackMgr.StartDuration();
        distNear = .1f;
        goHandle.GetComponent<Renderer>().material.color = Color.white;
    }

    public bool IsNear(GameObject go)
    {
        float dist = Vector3.Distance(go.transform.position, goImageMarker.transform.position);
        if (dist <= distNear)
        {
            distNearFound = dist;
            return true;
        }
        return false;
    }

    public GameObject FindNear()
    {
        foreach (GameObject go in gos)
        {
            if (IsNear(go) == true)
            {
                return go;
            }
        }
        return null;
    }

    public GameObject FindNearTouched()
    {
        foreach (GameObject go in gos)
        {
            if (IsNear(go) == true && go != goNearFoundTouch)
            {
                HighlightGo(go);
                audioMgr.PlaySound(ClipType.yes);
                goNearFoundTouch = go;
                return goNearFoundTouch;
            }
        }
        return null;
    }

    void HighlightGo(GameObject go)
    {
        goHighlight = go;
        scaleGoHighlight = goHighlight.transform.localScale;
        goHighlight.transform.localScale *= 1.1f;
        Invoke("HighlightOffGo", 1);
    }

    void HighlightOffGo()
    {
        goHighlight.transform.localScale = scaleGoHighlight;
    }

    public void PostResults()
    {
        string txtDuration = trackMgr.duration.ToString();
        string txtTimeOff = trackMgr.durationLost.ToString();
        string txtCntOff = trackMgr.cntOff.ToString();
        string txtTest = GetTestName();
        postMgr.PostResult(lobbyMgr.inputEmail.text, lobbyMgr.inputFirst.text, lobbyMgr.inputLast.text, txtTest, txtDuration, txtTimeOff, txtCntOff);
    }

    public void PostReplay()
    {
        string txtTest = GetTestName();
        string replays = "";
        string s = "";
        for(int n = 0; n < replayMgr.posReplay.Count; n++)
        {
            string txtPosX = replayMgr.posReplay[n].x.ToString();
            string txtPosY = replayMgr.posReplay[n].y.ToString();
            string txtPosZ = replayMgr.posReplay[n].z.ToString();
            string txtEulX = replayMgr.eulReplay[n].x.ToString();
            string txtEulY = replayMgr.eulReplay[n].y.ToString();
            string txtEulZ = replayMgr.eulReplay[n].z.ToString();
            replays += s + txtPosX + "," + txtPosY + "," + txtPosZ + "," + txtEulX + "," + txtEulY + "," + txtEulZ;
            s = ";";
        }
        postMgr.PostReplay(lobbyMgr.inputEmail.text, lobbyMgr.inputFirst.text, lobbyMgr.inputLast.text, txtTest, replays);
        Debug.Log("PostReplay\n");
    }

    string GetTestName()
    {
        string monoStereo = "";
        if (lobbyMgr.ynStereo == true) monoStereo = " stereo";
        string txtTest = this.GetType() + monoStereo;
        return txtTest;
    }

    private void OnApplicationQuit()
    {
        if (ynActive == true)
        {
            FinishTest();
        }
    }
}

