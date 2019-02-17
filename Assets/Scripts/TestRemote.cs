using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRemote : Test {
    GameObject goThis;
    GameObject goOther;
    Vector3 posThis;
    Vector3 posOther;

    private void Awake()
    {
    }

    // Use this for initialization
    public override void StartTest()
    {
        base.StartTest();
        // add gameobjects to gos[]
        goThis = goWorld.transform.Find("This").gameObject;
        goOther = goWorld.transform.Find("Other").gameObject;
        if (posThis == Vector3.zero)
        {
            posThis = goThis.transform.position;
            posOther = goOther.transform.position;
        } else {
            goThis.transform.position = posThis;
            goOther.transform.position = posOther;
        }
        udpMgr.ynActive = true;
        messageMgr.ShowMesssage("Control object on\nanother device!");
    }

    // Use this for cleanup on exit
    public override void FinishTest()
    {
        base.FinishTest();
        udpMgr.ynActive = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (ynActive == false) return;
        goOther.transform.position = udpMgr.posOther;
        UpdateNear();
        UpdateGoThis();
        udpMgr.SendThis(goThis.transform.position);
    }

    // Use this to affect touched gos[] object
    void UpdateNear()
    {
        GameObject go = FindNear();
        if (go != null)
        {
            //
        }
    }

    void UpdateGoThis()
    {
        float dist = Vector3.Distance(goImageMarker.transform.position, goThis.transform.position);
        if (dist <= distNear)
        {
            goThis.transform.position = goImageMarker.transform.position;
        }
    }
}
