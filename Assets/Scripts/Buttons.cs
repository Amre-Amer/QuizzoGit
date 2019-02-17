using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour {
    float scaleUpTargetOrig = 1.125f;
    float scaleUpTarget;
    float scaleUp;
    float durationScaleUp = .25f;
    [HideInInspector]
    public GameObject goButton;
    float smoothHighlight = .05f;
    [HideInInspector]
    public List<GameObject> buttons = new List<GameObject>();
    [HideInInspector]
    public List<Vector3> buttonScales = new List<Vector3>();
    bool ynHighlight;

    // Use this for initialization
    void Start () {
        InvokeRepeating("ToggleHighlight", durationScaleUp, durationScaleUp);
    }

    // Update is called once per frame
    void Update () {
        UpdateHighlight();
    }

    void UpdateHighlight()
    {
        if (goButton == null) return;
        scaleUp = smoothHighlight * scaleUpTarget + (1 - smoothHighlight) * scaleUp;
        int n = buttons.IndexOf(goButton);
        goButton.transform.localScale = buttonScales[n] * scaleUp;
    }

    void ToggleHighlight()
    {
        ynHighlight = !ynHighlight;
        if (ynHighlight == true)
        {
            scaleUpTarget = scaleUpTargetOrig;
            if (goButton != null && goButton.name.ToLower().Contains("video") == true)
            {
                scaleUpTarget = 1;
            }
        }
        else
        {
            scaleUpTarget = 1;
        }
    }

    public void UnHighlight()
    {
        if (goButton == null) return;
        int n = buttons.IndexOf(goButton);
        goButton.transform.localScale = buttonScales[n];
    }

    public void AddButton(GameObject go, bool ynStayOn)
    {
        buttons.Add(go);
        buttonScales.Add(go.transform.localScale);
    }
}
