using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : Test {
    GameObject goTimerHand;
    float timerLimit = 12;

    // Use this for initialization
    public override void StartTest()
    {
        base.StartTest();
        goTimerHand = goWorld.transform.Find("TimerHand").gameObject;
        LoadSounds();
        Invoke("FinishTimerHand", timerLimit);
        messageMgr.ShowMesssage("Play sounds\nfor a while.");
    }

    // Use this for cleanup on exit
    public override void FinishTest()
    {
        base.FinishTest();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (ynActive == false) return;
        UpdateTimerHand();
        UpdateNear();
    }

    void UpdateTimerHand()
    {
        float angDelta = -.5f;
        goTimerHand.transform.Rotate(angDelta, 0, 0);
    }

    void FinishTimerHand()
    {
        if (ynActive == false) return;
        //PostResults();
        FinishTest();
    }

    void UpdateNear()
    {
        if (ynActive == false) return;
        GameObject go = FindNear();
        if (go != null)
        {
            Debug.Log("sssssss");
            PlaySound(go);
        }
    }

    void LoadSounds()
    {
        foreach (Transform t in goWorld.transform)
        {
            GameObject go = t.gameObject;
            if (go.name.Contains("Sound") == true)
            {
                go.GetComponent<Renderer>().material.color = Random.ColorHSV();
                gos.Add(go);
            }
        }
    }

    void PlaySound(GameObject go)
    {
        AudioSource audioSource = go.GetComponent<AudioSource>();
        if (audioSource.isPlaying == false)
        {
            audioSource.Play();
        }
    }
}
