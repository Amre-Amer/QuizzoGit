using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Replay : MonoBehaviour {
    GameObject goImageMarker;
    GameObject goImageMarkerBody;
    [HideInInspector]
    public ReplayState state = ReplayState.inactive;
    public ReplayState stateLast = ReplayState.inactive;
    [HideInInspector]
    public List<Vector3> posReplay = new List<Vector3>();
    [HideInInspector]
    public List<Vector3> eulReplay = new List<Vector3>();
    int nReplay;
    [HideInInspector]
    Message messageMgr;

    // Use this for initialization
    void Start () {
        messageMgr = GetComponent<Message>();
        goImageMarker = GameObject.Find("ImageMarker");
        goImageMarkerBody = goImageMarker.transform.Find("Handle").gameObject;
    }

    // Update is called once per frame
    void Update() {
        UpdateState();
    }

    void UpdateState() { 
        if (state != stateLast)
        {
            switch(state)
            {
                case ReplayState.record:
                    Debug.Log("record\n");
                    TurnAROnOff(true);
                    posReplay.Clear();
                    eulReplay.Clear();
                    goImageMarkerBody.GetComponent<Renderer>().material.color = Color.red;
                    messageMgr.HideReplay();
                    break;
                case ReplayState.replay:
                    Debug.Log(posReplay.Count + " replay\n");
                    nReplay = 0;
                    TurnAROnOff(false);
                    messageMgr.ShowReplay();
                    break;
                case ReplayState.inactive:
                    Debug.Log("inactive\n");
                    nReplay = 0;
                    TurnAROnOff(true);
                    messageMgr.HideReplay();
                    break;
            }
        }
        if (state == ReplayState.record)
        {
            posReplay.Add(goImageMarker.transform.position);
            eulReplay.Add(goImageMarker.transform.eulerAngles);
        }
        if (state == ReplayState.replay)
        {
            if (nReplay < posReplay.Count)
            {
                goImageMarker.transform.position = posReplay[nReplay];
                goImageMarker.transform.eulerAngles = eulReplay[nReplay];
                nReplay++;
            }
            else
            {
                state = ReplayState.inactive;
                Debug.Log("inactive\n");
                nReplay = 0;
                TurnAROnOff(true);
                messageMgr.HideReplay();
            }
        }
       //Debug.Log("state " + state + "\n");
        stateLast = state;
  	}

    public void TurnAROnOff(bool yn)
    {
        ImageTargetBehaviour imageTB = goImageMarker.GetComponent<ImageTargetBehaviour>();
        imageTB.enabled = yn;
        if (yn == true)
        {
            goImageMarkerBody.GetComponent<Renderer>().material.color = Color.white;
        }
        else
        {
            goImageMarkerBody.GetComponent<Renderer>().material.color = Color.blue;
        }
    }
}

public enum ReplayState
{
    inactive,
    record,
    replay
}
