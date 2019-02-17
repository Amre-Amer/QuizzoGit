using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBolt : Test {
    float distNearBolt = .04f;
    float distInRange = .08f;
    Vector3 posLocalWrenchPoint = new Vector3(-1, 0, 0);
    GameObject imageMarkerBody;
    List<GameObject> sets = new List<GameObject>();
    List<Vector3> posOriginals = new List<Vector3>();
    List<Vector3> eulOriginals = new List<Vector3>();
    List<GameObject> wrenchParents = new List<GameObject>();
    List<GameObject> wrenchHolders = new List<GameObject>();
    List<GameObject> boltParents = new List<GameObject>();
    List<GameObject> boltHolders = new List<GameObject>();
    List<GameObject> indicators = new List<GameObject>();
    List<bool> ynTurnBolts = new List<bool>();

    // Use this for initialization
    public override void StartTest () {
        base.StartTest();
        distNear = .25f;
        imageMarkerBody = goImageMarker.transform.Find("Handle").gameObject;

        foreach(Transform t in goWorld.transform)
        {
            GameObject set = t.gameObject;
            if (set.name.Contains("Set") == true)
            {
                sets.Add(set);
                wrenchParents.Add(set.transform.Find("WrenchParent").gameObject);
                wrenchHolders.Add(set.transform.Find("WrenchParent/WrenchHolder").gameObject);
                boltParents.Add(set.transform.Find("BoltParent").gameObject);
                boltHolders.Add(set.transform.Find("BoltParent/BoltHolder").gameObject);
                indicators.Add(set.transform.Find("Indicator").gameObject);
                indicators[indicators.Count -  1].GetComponentInChildren<Renderer>().material.color = Color.white;
                posOriginals.Add(wrenchParents[wrenchParents.Count - 1].transform.position);
                eulOriginals.Add(wrenchParents[wrenchParents.Count - 1].transform.eulerAngles);
                ynTurnBolts.Add(false);
            }
        }
        messageMgr.ShowMesssage("Turn bolts\nwith wrench");
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
        if (ynActive == false) return;
        UpdateMonitor();
        UpdateTurn();
	}

    public override void FinishTest()
    {
        base.FinishTest();
        sets.Clear();
        wrenchParents.Clear();
        wrenchHolders.Clear();
        boltParents.Clear();
        boltHolders.Clear();
        indicators.Clear();
        posOriginals.Clear();
        eulOriginals.Clear();
    }

    void UpdateTurn()
    {
        for (int n = 0; n < sets.Count; n++)
        {
            boltParents[n].GetComponentInChildren<Renderer>().material.color = Color.white;
            wrenchParents[n].GetComponentInChildren<Renderer>().material.color = Color.white;
            float dist = Vector3.Distance(wrenchParents[n].transform.position, boltParents[n].transform.position);
            if (dist <= distInRange)
            {
                wrenchParents[n].GetComponentInChildren<Renderer>().material.color = Color.yellow;
                if (dist <= distNearBolt)
                {
                    boltParents[n].GetComponentInChildren<Renderer>().material.color = Color.yellow;
                    if (IsWrenchAngleMatchingBolt(n) == true)
                    {
                        ynTurnBolts[n] = true;
                        indicators[n].GetComponentInChildren<Renderer>().material.color = Color.green;
                    }
                    else
                    {
                        if (ynTurnBolts[n] == false)
                        {
                            boltParents[n].GetComponentInChildren<Renderer>().material.color = Color.yellow;
                        }
                    }
                }
            }
            else
            {
                ynTurnBolts[n] = false;
            }
            if (ynTurnBolts[n] == true)
            {
                wrenchParents[n].GetComponentInChildren<Renderer>().material.color = Color.green;
                boltParents[n].GetComponentInChildren<Renderer>().material.color = Color.green;
                boltHolders[n].transform.localEulerAngles = wrenchParents[n].transform.localEulerAngles;
            }
        }
    }

    bool IsWrenchAngleMatchingBolt(int n)
    {
        bool yn = false;
        Vector3 posWrenchPoint = wrenchHolders[n].transform.TransformPoint(posLocalWrenchPoint);
        Vector3 vec = boltParents[n].transform.InverseTransformVector(posWrenchPoint - boltParents[n].transform.position);
        float ang = Mathf.Atan2(vec.x, vec.z) * Mathf.Rad2Deg;
        if (ang < 0) ang += 360;
        for (float angCheck = 0; angCheck < 360; angCheck += 60)
        {
            if (Mathf.Abs(ang - angCheck) <= 5)
            {
                yn = true;
            }
        }
        return yn;
    }

    // Update is called once per frame
    void UpdateMonitor()
    {
        bool ynFound = false;
        for (int n = 0; n < sets.Count; n++)
        {
            GameObject wrench = wrenchParents[n];
            float dist = Vector3.Distance(posOriginals[n], goImageMarker.transform.position);
            if (dist <= distNear)
            {

                ynFound = true;
                wrench.transform.position = goImageMarker.transform.position;
                wrench.transform.eulerAngles = goImageMarker.transform.eulerAngles;
            }
            else
            {
                wrench.transform.position = posOriginals[n];
                wrench.transform.eulerAngles = eulOriginals[n];
            }
        }
        imageMarkerBody.SetActive(!ynFound);
        imageMarkerBody.GetComponent<MeshRenderer>().enabled = !ynFound;
    }
}
