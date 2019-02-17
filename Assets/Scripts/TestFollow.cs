using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFollow : Test {
    [Header("")]
    GameObject goTarget;
    float distTarget = .75f;
    float ang;

    // Use this for initialization
    public override void StartTest () {
        base.StartTest();
        goTarget = goWorld.transform.Find("Target").gameObject;
        distNear *= 8;
        ang = 0;
        LoadGos();
        messageMgr.ShowMesssage("Follow\nthe disk.");
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
        UpdateTarget();
        UpdateNear();
        UpdateDone();
    }

    void LoadGos()
    {
        gos.Add(goTarget);
    }

    void UpdateDone()
    {
        ang += .5f;
        if (ang >= 360)
        {
            //PostResults();
            FinishTest();
        }
    }

    void UpdateTarget()
    {
        goTarget.transform.position = cam.transform.position;
        Vector3 eul = new Vector3(0, ang, 0);
        goTarget.transform.eulerAngles = eul;
        goTarget.transform.position += goTarget.transform.forward * distTarget;
        goTarget.transform.Rotate(-90, 0, 0);
    }

    void UpdateNear()
    {
        GameObject go = FindNear();
        if (go != null)
        {
            if (go == goTarget)
            {
                go.GetComponent<Renderer>().material.color = Color.white;
                if (distNearFound <= distNear / 2)
                {
                    if (distNearFound <= distNear / 4)
                    {
                        go.GetComponent<Renderer>().material.color = Color.green;
                    }
                    else
                    {
                        go.GetComponent<Renderer>().material.color = Color.yellow;
                    }
                }
                else
                {
                    go.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
    }
}
