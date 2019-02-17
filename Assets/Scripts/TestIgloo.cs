using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIgloo : Test
{
    [Header("")]
    GameObject goTmp;
    float rad = .75f;
    float angDelta = 5;
    float size = .05f;
    float pitchMin = -45;
    float pitchMax = 15;

    // Use this for initialization
    public override void StartTest()
    {
        base.StartTest();
        // add gameobjects to gos[]
        //distNear = .2f;
        LoadGos();
        messageMgr.ShowMesssage("Tap to clear!");
    }

    // Use this for cleanup on exit
    public override void FinishTest()
    {
        foreach(GameObject go in gos)
        {
            DestroyImmediate(go);
        }
        base.FinishTest();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (ynActive == false) return;
        UpdateNear();
    }

    // Use this to affect touched gos[] object
    void UpdateNear()
    {
        GameObject go = FindNear();
        if (go != null)
        {
            //
            int n = gos.IndexOf(go);
            DestroyImmediate(go);
            gos.RemoveAt(n);
            audioMgr.PlaySound(ClipType.acquired);
        }
    }

    void LoadGos()
    {
        for(float yaw = 0; yaw < 360; yaw += angDelta)
        {
            for (float pitch = pitchMin; pitch <= pitchMax; pitch += angDelta)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.localScale = new Vector3(size, size, size/10);
                go.transform.position = cam.transform.position;
                go.transform.eulerAngles = new Vector3(pitch, yaw, 0);
                go.transform.position += go.transform.forward * rad;
                go.GetComponent<Renderer>().material.color = Random.ColorHSV();
                gos.Add(go);
            }
        }
    }
}