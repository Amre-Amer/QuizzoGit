using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBuild : Test
{
    GameObject goBall;
    GameObject goParent;
    Vector3 posLast;

    // Use this for initialization
    public override void StartTest()
    {
        base.StartTest();
        // add gameobjects to gos[]
        goParent = new GameObject("parent");
        goBall = goWorld.transform.Find("Ball").gameObject;
        goBall.SetActive(false);
        distNear = .01f;
        messageMgr.ShowMesssage("Blow bubbles?");
    }

    // Use this for cleanup on exit
    public override void FinishTest()
    {
        base.FinishTest();
        DestroyImmediate(goParent);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (ynActive == false) return;
        float dist = Vector3.Distance(goImageMarker.transform.position, posLast);
        if (dist > distNear)
        {
            Trail();
        }
        posLast = goImageMarker.transform.position;
    }

    void Trail()
    {
        goBall.SetActive(true);
        GameObject go = Instantiate(goBall, goParent.transform);
        go.transform.position = goImageMarker.transform.position;
        Color color = Random.ColorHSV();
        color.a = .25f;
        go.GetComponent<Renderer>().material.color = color;
        goBall.SetActive(false);
    }

}
