using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAsset : Test {
    [Header("")]
    GameObject goBase;

    // Use this for initialization
    public override void StartTest()
    {
        base.StartTest();
        LoadGos();
        UnloadAsset();
        messageMgr.ShowMesssage("Import asset bundles\nfrom Google drive.");
    }

    // Use this for cleanup on exit
    public override void FinishTest()
    {
        base.FinishTest();
        UnloadAsset();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (ynActive == false) return;
        UpdateNear();
        UpdateRotate();
    }

    void UpdateRotate()
    {
        goBase.transform.Rotate(-.5f, -.5f, .5f);
    }

    void UpdateNear()
    {
        GameObject go = FindNearTouched();
        if (go != null)
        {
            Debug.Log(distNear + " " + distNearFound + " touched " + go.name + "\n");
            string txtAsset = go.name.ToLower(); //.Replace("Asset", "");
            UnloadAsset();
            assetMgr.LoadAssetWWW(txtAsset, goBase);
        }
    }

    void UnloadAsset()
    {
        if (goBase == null) return;
        foreach(Transform t in goBase.transform)
        {
            GameObject go = t.gameObject;
            DestroyImmediate(go);
            Debug.Log("unload asset\n");
        }
    }

    void LoadGos()
    {
        foreach(Transform t in goWorld.transform)
        {
            GameObject go = t.gameObject;
            if (go.name.Contains("Base") == true)
            {
                goBase = go;
            }
            else
            {
                go.transform.LookAt(cam.transform.position);
                gos.Add(go);
            }
        }
    }
}
