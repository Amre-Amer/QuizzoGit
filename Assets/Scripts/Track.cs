using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Vuforia;

public class Track : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;
    GameObject goImageMarker;
    bool ynAcquired;
    bool ynChange;
    [HideInInspector]
    public float duration;
    [HideInInspector]
    public float durationLost;
    [HideInInspector]
    public int cntOff;
    Message messageMgr;
    Audio audioMgr;

    void Start()
    {
        goImageMarker = GameObject.Find("ImageMarker");
        audioMgr = GetComponent<Audio>();
        messageMgr = GetComponent<Message>();
        mTrackableBehaviour = goImageMarker.GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    private void Update()
    {
        UpdateMarkerAcquiredLost();
        if (mTrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.DETECTED || mTrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.TRACKED)
        {
            duration += Time.deltaTime;
        }
        else {
            durationLost += Time.deltaTime;
            duration += Time.deltaTime;
        }
    }

    public void UpdateMarkerAcquiredLost()
    {
        if (ynChange == false) return;
        if (ynAcquired == true)
        {
            OnMarkerAcquired();
        }
        else
        {
            OnMarkerLost();
        }
        ynChange = false;
    }

    public void StartDuration()
    {
        cntOff = 0;
        duration = 0;
        durationLost = 0;
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED)
        {
            ynChange = true;
            ynAcquired = true;
            Debug.Log("Acquired\n");
        }
        else
        {
            ynChange = true;
            ynAcquired = false;
            Debug.Log("Lost\n");
        }
    }

    void OnMarkerAcquired()
    {
        audioMgr.PlaySound(ClipType.acquired);
    }

    void OnMarkerLost()
    {
        audioMgr.PlaySound(ClipType.lost);
        messageMgr.goPopUp.SetActive(true);
        Invoke("HidePopUp", 2);
        cntOff++;
    }

    void HidePopUp()
    {
        messageMgr.goPopUp.SetActive(false);
    }
}