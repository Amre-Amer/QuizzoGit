using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour {
    [HideInInspector]
    public AudioSource audioSource;
    [HideInInspector]
    public AudioClip clipYes;
    [HideInInspector]
    public AudioClip clipNo;
    [HideInInspector]
    public AudioClip clipAcquired;
    [HideInInspector]
    public AudioClip clipLost;
    [HideInInspector]
    public AudioClip clipMusic;
    [HideInInspector]
    public AudioClip clipTurn;
    [HideInInspector]
    public AudioClip clipDone;
    [HideInInspector]
    public AudioClip clipAcknowledge;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        clipYes = Resources.Load("ClipYes") as AudioClip;
        clipNo = Resources.Load("ClipNo") as AudioClip;
        clipAcquired = Resources.Load("ClipAcquired") as AudioClip;
        clipLost = Resources.Load("ClipLost") as AudioClip;
        clipMusic = Resources.Load("ClipMusic") as AudioClip;
        clipTurn = Resources.Load("ClipTurn") as AudioClip;
        clipDone = Resources.Load("ClipDone") as AudioClip;
        clipAcknowledge = Resources.Load("ClipAcknowledge") as AudioClip;
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound(ClipType clipType)
    {
        audioSource.clip = GetClipByType(clipType);
        audioSource.Play();
    }

    AudioClip GetClipByType(ClipType clipType)
    {
        switch(clipType)
        {
            case ClipType.yes:
                return clipYes;
            case ClipType.no:
                return clipNo;
            case ClipType.acquired:
                return clipAcquired;
            case ClipType.lost:
                return clipLost;
            case ClipType.music:
                return clipMusic;
            case ClipType.turn:
                return clipTurn;
            case ClipType.acknowledge:
                return clipAcknowledge;
            case ClipType.done:
                return clipDone;
        }
        return null;
    }
}

public enum ClipType
{
    unknown,
    yes,
    no,
    acquired,
    lost,
    music,
    turn,
    acknowledge,
    done
}
