using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlank : Test
{
    // Use this for initialization
    public override void StartTest()
    {
        base.StartTest();
        // add gameobjects to gos[]
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

}
