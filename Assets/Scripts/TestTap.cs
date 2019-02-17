using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTap : Test {
    int cntDone;

    // Use this for initialization
    public override void StartTest () {
        base.StartTest();
        messageMgr.ShowMesssage("Tap these\nobjects!");
        cntDone = 0;
        LoadWorld();
        PopUpScore("hello");
    }

    // Use this for cleanup on exit
    public override void FinishTest()
    {
        base.FinishTest();
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
        if (ynActive == false) return;
        UpdateScore();
        UpdateNear();
    }

    void UpdateScore()
    {
        if (cntDone == gos.Count)
        {
            //PostResults();
            cntDone++;
            audioMgr.PlaySound(ClipType.done);
        }
    }

    void UpdateNear()
    {
        if (ynActive == false) return;
        GameObject go = FindNearTouched();
        if (go != null)
        {
            if (go.GetComponent<Renderer>().material.color == Color.white)
            {
                cntDone++;
                PopUpScore(cntDone.ToString());
            }
            go.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    void LoadWorld()
    {
        foreach(Transform t in goWorld.transform)
        {
            GameObject go = t.gameObject;
            go.GetComponent<Renderer>().material.color = Color.white;
            gos.Add(go);
        }
    }
}
