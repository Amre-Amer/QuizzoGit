using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enterprise : MonoBehaviour {
    GameObject goImageMarker;
    GameObject goEnterprise;
    float rad = 2;
    float distNear = .3f;
    Vector3 target;
    Vector3 look;
    float distMove = .01f;
    float smooth = .025f;
    GameObject goTarget;
    GameObject goLook;

    // Use this for initialization
    void Start () {
        goTarget = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        goTarget.transform.localScale = new Vector3(.1f, .1f, .1f);
        goTarget.GetComponent<Renderer>().material.color = Color.blue;
        goLook = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        goLook.transform.localScale = new Vector3(.1f, .1f, .1f);
        goLook.GetComponent<Renderer>().material.color = Color.cyan;
        goImageMarker = GameObject.Find("ImageMarker");
        goEnterprise = GameObject.Find("Enterprise");
        goEnterprise.transform.position = GetRandomPos();
        target = GetRandomPos();
        look = GetRandomPosForward();
    }

    // Update is called once per frame
    void Update () {
        look = smooth * target + (1 - smooth) * look;
        goEnterprise.transform.LookAt(look);
        goEnterprise.transform.position += goEnterprise.transform.forward * distMove;
        float dist = Vector3.Distance(goEnterprise.transform.position, look);
        if (dist < distNear)
        {
            target = GetRandomPosForward();
        }
        goLook.transform.position = look;
        goTarget.transform.position = target;
    }

    Vector3 GetRandomPos()
    {
        return goImageMarker.transform.position + Random.onUnitSphere * rad;
    }

    Vector3 GetRandomPosForward()
    {
        Vector3 pos = goEnterprise.transform.position + goEnterprise.transform.forward * distNear * 3;
        pos += Random.onUnitSphere * distNear;
        Vector3 vec = pos - goImageMarker.transform.position;
        pos = goImageMarker.transform.position + vec.normalized * rad;
        if (pos.y <= 0)
        {
            pos.y = 0;
        }
        return pos;
    }
}
