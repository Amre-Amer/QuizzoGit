using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class TestThrust : Test {
    int width = 100;
    int height = 100;
//    Material material;
    GameObject particle;
    GameObject craft;
    //public Button buttonStereo;
    int numParticles = 100;
    float radius = 10;
    //float distNear = 3;
    float distTarget = 12;
    GameObject goCameraRig;
    Camera camMain;
    Camera camLeft;
    Camera camRight;
    int cntFrames;
    List<GameObject> particles = new List<GameObject>();
    List<float> times = new List<float>();
    float distMoveParticle = 2f;
    List<Vector3> looks = new List<Vector3>();
    List<Vector3> targets = new List<Vector3>();
    float distMove = .0075f;
    float smooth = .025f;
    GameObject goLook;
    GameObject goTarget;
    GameObject goWorm;
    List<GameObject> crafts = new List<GameObject>();
    int numCrafts = 10;
    List<GameObject> goLooks = new List<GameObject>();
    List<GameObject> goTargets = new List<GameObject>();
    List<GameObject> worms = new List<GameObject>();
    int numWorms = 100;
    List<GameObject> wormTargets = new List<GameObject>();
    List<GameObject> wormLooks = new List<GameObject>();
    int numWormSegs = 8;
    float smoothWorm = .01f;
    float distNearWorm = 1;
    float distMoveWorm = .01f;
    List<SegClass> segs = new List<SegClass>();
    List<float> wormTimes = new List<float>();
    List<State> wormStates = new List<State>();
    List<float> craftTimes = new List<float>();
    List<State> craftStates = new List<State>();
    int cntFps;
    AudioSource audioSource;
    List<GameObject> vents = new List<GameObject>();
    int numVents = 300;
    GameObject goVent;
    GameObject goCrosshairs;
    Vector3 centerScreen;
    Vector3 camLook;
    Vector3 camTarget;
    int nCraft;
    float camTime;
    float selectDelay = 5;
    float selectTime;
    int nCraftLast;
    int nCraftSelect;
    float rideDelay = 5;
    bool ynRide;
    float rideTime;
    Vector3 posCam = Vector3.zero;
    float ventDistMove = .01f;
    float ventDuration = 15;
    List<bool> craftOns = new List<bool>();
    int nWorm;
    GameObject goParent;

    // Use this for initialization
    public override void StartTest () {
        base.StartTest();
        goParent = new GameObject("parent");
        goParent.transform.parent = goWorld.transform;
        goCameraRig = GameObject.Find("ARCamera");
        camMain = GameObject.Find("ARCamera").GetComponent<Camera>();
        craft = goWorld.transform.Find("Craft").gameObject;
        goLook = goWorld.transform.Find("Look").gameObject;
        goTarget = goWorld.transform.Find("Target").gameObject;
        particle = goWorld.transform.Find("Particle").gameObject;
        goWorm = goWorld.transform.Find("Worm").gameObject;
        goVent = goWorld.transform.Find("Vent").gameObject;
        goCrosshairs = goWorld.transform.Find("Crosshairs").gameObject;
        distNear = 3;
        centerScreen = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        audioSource = GetComponent<AudioSource>();
        //
        if (Application.isEditor == true)
        {
            string path = Application.dataPath + "/texture.png";
            Texture2D texture = CreateTexture();
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
        }
        //
        CreateCrafts();
        CreateParticles();
        CreateWorms();
        CreateVents();
        camLook = crafts[0].transform.position;
        messageMgr.ShowMesssage("Worms avoid\nyour hand!");
    }

    // Use this for cleanup on exit
    public override void FinishTest()
    {
        base.FinishTest();
        //ynActive = false;
        Cleanup();
    }

    void Cleanup()
    {
        crafts.Clear();
        particles.Clear();
        goLooks.Clear();
        goTargets.Clear();
        worms.Clear();
        wormTargets.Clear();
        wormLooks.Clear();
        vents.Clear();
        segs.Clear();
        DestroyImmediate(goParent);
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
        if (ynActive == false) return;
        //return;
        //UpdateCamRotation();
        UpdateCrafts();
        UpdateParticles();
        UpdateWorms();
        UpdateSegs();
        UpdateVents();
        UpdateSelect();
        UpdateCrosshairs();
        cntFrames++;
        cntFps++;
    }

    void SetAudioRandom()
    {
        if (nWorm >= worms.Count) return;
        GameObject go = worms[nWorm];
        SetAudio(go);
        Invoke("SetAudioRandom", Random.Range(0.01f, .05f));
        nWorm++;
    }

    void UpdateVents()
    {
        if (Time.realtimeSinceStartup > ventDuration) return;
        foreach (GameObject goV in vents)
        {
            int n = vents.IndexOf(goV);
            GameObject go = FindNearestVent(goV);
            if (go != null)
            {
                Vector3 vector = goV.transform.position - go.transform.position;
                goV.transform.position += vector.normalized * ventDistMove;
            }
        }
    }

    GameObject FindNearestVent(GameObject go)
    {
        float distMin = -1;
        GameObject goMin = null;
        foreach (GameObject goV in vents)
        {
            if (go != goV)
            {
                float dist = Vector3.Distance(go.transform.position, goV.transform.position);
                if (dist < distMin || distMin < 0)
                {
                    if (dist < goV.transform.localScale.x)
                    {
                        goMin = goV;
                        distMin = dist;
                    }
                }
            }
        }
        return goMin;
    }

    void CreateVents()
    {
        goVent.SetActive(true);
        for (int n = 0; n < numVents; n++)
        {
            GameObject go = Instantiate(goVent);
            go.transform.parent = goParent.transform;
            go.transform.position = Random.onUnitSphere * (radius + distNear);
            go.transform.LookAt(Vector3.zero);
            float s = 2;
            go.transform.localScale = new Vector3(s, s, .125f * s);
            go.GetComponent<Renderer>().material.color = Random.ColorHSV() * .75f + Color.clear * .25f;
            vents.Add(go);
        }
        goVent.SetActive(false);
    }

    void Debugger()
    {
        Debug.Log("ride " + ynRide + "\n");

    }

    void UpdateRide()
    {
        if (ynRide == true)
        {
            if (Time.realtimeSinceStartup - rideTime > rideDelay)
            {
                ynRide = false;
                goCameraRig.transform.position = Vector3.zero;
            }
            else
            {
                int nCraft2 = nCraft--;
                if (nCraft2 < 0) nCraft2 = crafts.Count - 1;
                posCam = crafts[nCraft2].transform.position + crafts[nCraft2].transform.up * .5f;
            }
        }
    }

    void ToggleOn()
    {
        craftOns[nCraft] = !craftOns[nCraft];
    }

    void UpdateCrosshairs()
    {
        if (nCraft != nCraftLast)
        {
            selectTime = Time.realtimeSinceStartup;
        }
        if (Time.realtimeSinceStartup - selectTime < selectDelay)
        {
            float fract = (Time.realtimeSinceStartup - selectTime) / selectDelay;
            float f = fract / .5f;
            Color color = Color.red * (1 - f) + Color.yellow * f;
            if (fract > .5f)
            {
                f = (fract - .5f) / .5f;
                color = Color.yellow * (1 - f) + Color.green * f;
                if (fract > .5f)
                {
                    //if (ynRide == false)
                    //{
                        ynRide = true;
                        rideTime = Time.realtimeSinceStartup;
                        //ToggleAudio();
                        ToggleOn();
                    //}
                }
            }
            goCrosshairs.GetComponent<Renderer>().material.color = color;
        }
        else
        {
            goCrosshairs.GetComponent<Renderer>().material.color = Color.white;
        }
        nCraftLast = nCraft;
    }

    void UpdateSelect()
    {
        goCrosshairs.SetActive(false);
        Ray ray = camMain.ScreenPointToRay(centerScreen);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.DrawLine(ray.origin, hit.point);
            GameObject go = hit.collider.gameObject;
            if (go.transform.parent.gameObject.name == "Craft(Clone)")
            {
                GameObject goC = go.transform.parent.gameObject;
                goCrosshairs.SetActive(true);
                goCrosshairs.transform.position = goC.transform.position;
                goCrosshairs.transform.LookAt(goCameraRig.transform.position);
                //goCrosshairs.transform.Rotate(0, 180, 0);
                nCraft = crafts.IndexOf(goC);
            }
        }
    }

    void UpdateParticlesForCraft(int c)
    {
        GameObject goThruster = crafts[c];
        for (int n = 0; n < numParticles; n++)
        {
            int p = c * numParticles + n;
            GameObject go = particles[p];
            go.transform.position += go.transform.forward * distMoveParticle * n / numParticles * 1;
            GameObject quad = go.transform.Find("Quad").gameObject;
            quad.transform.LookAt(goCameraRig.transform.position);
            quad.transform.Rotate(0, 180, 0);
            float diff = Time.realtimeSinceStartup - times[n];
            if (diff > Random.Range(.1f, 1))
            {
                ResetParticle(go, goThruster);
            }
            if (craftOns[c] == false)
            {
                go.GetComponentInChildren<Renderer>().material.color = Random.ColorHSV();
            }
            else
            {
                go.GetComponentInChildren<Renderer>().material.color = Color.white;
            }
        }
    }

    void CreateParticles()
    {
        particle.SetActive(true);
        for (int c = 0; c < numCrafts; c++)
        {
            CreateParticlesForCraft(c);
        }
        particle.SetActive(false);
    }

    void CreateParticlesForCraft(int c)
    {
        GameObject goThruster = crafts[c];
        for (int n = 0; n < numParticles; n++)
        {
            GameObject go = Instantiate(particle);
            go.transform.parent = goParent.transform;
            particles.Add(go);
            times.Add(Time.realtimeSinceStartup);
            ResetParticle(go, goThruster);
        }
    }

    void UpdateWorms()
    {
        for (int w = 0; w < worms.Count; w++)
        {
            GameObject go = worms[w];
            Vector3 look = wormLooks[w].transform.position;
            Vector3 target = wormTargets[w].transform.position;
            look = smoothWorm * target + (1 - smoothWorm) * look;
            go.transform.LookAt(look);
            wormLooks[w].transform.position = look;
            go.transform.position += go.transform.forward * distMoveWorm;
            float dist = Vector3.Distance(go.transform.position, target);
            if (dist < distNearWorm)
            {
                wormTargets[w].transform.position = Random.onUnitSphere * radius/2;
                wormTimes[w] = Time.realtimeSinceStartup;
                wormStates[w] = State.target;
                go.GetComponent<Renderer>().material.color = Color.cyan;
            }
            UpdateWormAvoid(go);
            UpdateWormState(w);
        }
        goWorm.transform.eulerAngles = goCameraRig.transform.eulerAngles;
    }

    void UpdateWormState(int w)
    {
        GameObject go = worms[w];
        Color color = Color.white;
        switch(wormStates[w])
        {
            case State.normal:
                return;
            case State.avoid:
                color = Color.red;
                break;
            case State.target:
                color = Color.magenta;
                break;
        }
        if (Time.realtimeSinceStartup - wormTimes[w] > .5f)
        {
            go.GetComponent<Renderer>().material.color = Color.yellow;
            wormStates[w] = State.normal;
        }
    }

    void UpdateWormAvoid(GameObject go)
    {
        int w = worms.IndexOf(go);
        GameObject goAvoid = GetNearNeighborWorm(go);
        if (goAvoid != null)
        {
            Vector3 vec = go.transform.position - goAvoid.transform.position;
            wormTargets[w].transform.position = go.transform.position + vec * distNearWorm * 2;
            wormTimes[w] = Time.realtimeSinceStartup;
            wormStates[w] = State.avoid;
            go.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    GameObject GetNearNeighborWorm(GameObject go)
    {
        foreach (GameObject goW in worms)
        {
            if (go != goW)
            {
                float dist = Vector3.Distance(goW.transform.position, go.transform.position);
                if (dist <= distNearWorm)
                {
                    return goW;
                }
            }
        }
        foreach (GameObject goC in crafts)
        {
            float dist = Vector3.Distance(goC.transform.position, go.transform.position);
            if (dist <= distNear)
            {
                return goC;
            }
        }
        float dist0 = Vector3.Distance(goCameraRig.transform.position, go.transform.position);
        if (dist0 <= distNearWorm)
        {
            return goCameraRig.gameObject;
        }
        float distIM = Vector3.Distance(goImageMarker.transform.position, go.transform.position);
        if (distIM <= distNearWorm)
        {
            return goImageMarker.gameObject;
        }
        return null;
    }

    void UpdateSegs()
    {
        foreach(SegClass seg in segs)
        {
            seg.Update();
        }
    }

    void CreateWorms()
    {
        for(int w = 0; w < numWorms; w++)
        {
            GameObject go = Instantiate(goWorm);
            go.transform.parent = goParent.transform;
            go.transform.position = Random.onUnitSphere * radius/2;
            go.GetComponent<Renderer>().material.color = Color.yellow;
            worms.Add(go);
            GameObject goT = Instantiate(goWorm);
            goT.transform.parent = goParent.transform;
            goT.name = "target";
            goT.GetComponent<Renderer>().material.color = (Color.blue + Color.clear) / 2;
            goT.transform.localScale *= .25f;
            goT.transform.position = Random.onUnitSphere * radius;
            wormTargets.Add(goT);
            GameObject goL = Instantiate(goWorm);
            goL.transform.parent = goParent.transform;
            goL.name = "look";
            goL.GetComponent<Renderer>().material.color = Color.magenta;
            goL.transform.localScale *= .25f;
            goL.transform.position = Random.onUnitSphere * radius;
            wormLooks.Add(goL);
            wormTimes.Add(Time.realtimeSinceStartup);
            wormStates.Add(State.normal);
//            SetAudio(go);
            //
            for (int s = 0; s < numWormSegs; s++)
            {
                goT = go;
                if (s > 0) {
                    goT = segs[segs.Count - 1].go;
                }
                GameObject goS = Instantiate(goWorm);
                goS.transform.parent = goParent.transform;
                goS.name = "seg";
                goS.transform.position = Random.onUnitSphere * radius;
                goS.transform.localScale *= (1f / (s + 1.25f));
                goS.GetComponent<Renderer>().material.color = Random.ColorHSV();
                if (goT == null) Debug.Log("goT null\n");
                SegClass seg = new SegClass(goS, goT);
                segs.Add(seg);
            }
        }
    }

    void SetAudio(GameObject go)
    {
        AudioSource audioS = go.GetComponent<AudioSource>();
        audioS.loop = true;
        audioS.Play();
    }

    void CreateCrafts()
    {
        craft.SetActive(true);
        for(int n = 0; n <  numCrafts; n++)
        {
            GameObject go = Instantiate(craft);
            go.transform.parent = goParent.transform;
            go.transform.position = Random.onUnitSphere * radius;
            crafts.Add(go);
            looks.Add(Random.onUnitSphere * radius);
            targets.Add(Random.onUnitSphere * radius);
            GameObject goL = Instantiate(goLook);
            goL.transform.parent = goParent.transform;
            goLooks.Add(goL);
            GameObject goT = Instantiate(goTarget);
            goT.transform.parent = goParent.transform;
            goTargets.Add(goT);
            craftTimes.Add(Time.realtimeSinceStartup);
            craftStates.Add(State.normal);
            //AddAudio(go);
            SetCraftNumber(go);
            craftOns.Add(true);
        }
        craft.SetActive(false);
        //audioSource.enabled = false;
    }

    void SetCraftNumber(GameObject go)
    {
        int n = crafts.IndexOf(go);
        GameObject goT = go.transform.Find("TextMeshPro").gameObject;
        TextMeshPro tm = goT.GetComponent<TextMeshPro>();
        tm.text = n.ToString();
        //Debug.Log("n " + goT.name + "\n");
    }

    void AddAudio(GameObject go)
    {
        go.AddComponent<AudioSource>();
        AudioSource audioS = go.GetComponent<AudioSource>();
        audioS.clip = audioSource.clip;
        audioS.loop = true;
        audioS.Play();
    }

    void UpdateCamRotation()
    {
        if (Application.isEditor == true) return;
        goCameraRig.transform.rotation = Input.gyro.attitude;
        Vector3 eul = goCameraRig.transform.eulerAngles;
        goCameraRig.transform.eulerAngles = new Vector3(-eul.x, -eul.y, eul.z);
    }

    void UpdateCrafts()
    {
        foreach(GameObject go in crafts)
        {
            UpdateCraft(go);
        }
    }

    void UpdateCraft(GameObject go)
    {
        int n = crafts.IndexOf(go);
        Vector3 lookLast = looks[n];
        looks[n] = smooth * targets[n] + (1 - smooth) * looks[n];
        float distL = Vector3.Distance(looks[n], lookLast);
        float limit = distMove * 6;
        if (distL > limit)
        {
            looks[n] = lookLast + (looks[n] - lookLast).normalized * limit;
        }
        if (go == null) Debug.Log("UpdateCraft go = null\n");
        go.transform.LookAt(looks[n]);
        go.transform.position += go.transform.forward * distMove;
        int s = 1;
        if (n % 2 == 0) s = -1;
        go.transform.Rotate(0, 0, cntFrames * s);
        UpdateCraftAvoid(go);
        float dist = Vector3.Distance(go.transform.position, targets[n]);
        if (dist < distNear)
        {
            targets[n] = go.transform.position + go.transform.forward * distTarget + Random.insideUnitSphere * radius;
            targets[n] = targets[n].normalized * Random.Range(radius * .8f, radius * 1.2f);
            craftTimes[n] = Time.realtimeSinceStartup;
            craftStates[n] = State.target;
            SetCraftColor(n, Color.yellow);
        }
        UpdateCraftState(n);
        if (goTargets[n] == null) Debug.Log("goTargets[n] null ynActive " + ynActive + "\n");
        goTargets[n].transform.position = targets[n];
        goLooks[n].transform.position = looks[n];
    }

    void SetCraftColor(int c, Color color)
    {
        GameObject go = crafts[c];
        go.GetComponentInChildren<Renderer>().material.color = color;
    }

    void UpdateCraftState(int w)
    {
        Color color = Color.white;
        switch (craftStates[w])
        {
            case State.normal:
                return;
            case State.avoid:
                color = Color.red;
                break;
            case State.target:
                color = Color.magenta;
                break;
        }
        if (Time.realtimeSinceStartup - craftTimes[w] > .5f)
        {
            SetCraftColor(w, Color.white);
            craftStates[w] = State.normal;
        }
    }

    void UpdateCraftAvoid(GameObject go)
    {
        int n = crafts.IndexOf(go);
        GameObject goAvoid = GetNearNeighborCraft(go);
        if (goAvoid != null)
        {
            targets[n] = go.transform.position + (go.transform.position - goAvoid.transform.position).normalized * distTarget;
            targets[n] = targets[n].normalized * Random.Range(radius * .8f, radius * 1.2f);
            craftTimes[n] = Time.realtimeSinceStartup;
            craftStates[n] = State.avoid;
            SetCraftColor(n, Color.cyan);
        }
    }

    GameObject GetNearNeighborCraft(GameObject go)
    {
        foreach(GameObject goC in crafts)
        {
            if (go != goC)
            {
                float dist = Vector3.Distance(goC.transform.position, go.transform.position);
                if (dist <= distNear)
                {
                    return goC;
                }
            }
        }
        float dist0 = Vector3.Distance(goCameraRig.transform.position, go.transform.position);
        if (dist0 <= distNear)
        {
            return goCameraRig.gameObject;
        }
        return null;
    }

    void UpdateParticles()
    {
        for (int c = 0; c < crafts.Count; c++)
        {
            UpdateParticlesForCraft(c);
        }
    }

    void ResetParticle(GameObject go, GameObject goThruster)
    {
        go.transform.position = goThruster.transform.position + Random.insideUnitSphere * .5f;
        go.transform.eulerAngles = goThruster.transform.eulerAngles;
        go.transform.Rotate(180, 0, 0);
        float s = Random.Range(.5f, 1f);
        go.transform.localScale = new Vector3(s, s, s);
        int n = particles.IndexOf(go);
        times[n] = Time.realtimeSinceStartup;
    }

    Texture2D CreateTexture()
    {
        Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Point;
        Vector2 cen = new Vector2(width / 2, height / 2);
        float max = Vector2.Distance(cen, Vector2.zero) / 2;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 pos = new Vector2(i, j);
                float dist = Vector2.Distance(cen, pos);
                float frac = dist / max;
                float r = 1;
                float g = 1;
                float b = 1;
                float ang = frac * 90;
                float a = Mathf.Cos(ang * Mathf.Deg2Rad) * .5f;
                Color color = new Color(r, g, b, a);
                texture.SetPixel(j, height - 1 - i, color);
            }
        }
        texture.Apply();
        return texture;
    }

    public class SegClass
    {
        public GameObject go;
        public GameObject goTarget;
        public SegClass(GameObject go0, GameObject goTarget0)
        {
            go = go0;
            goTarget = goTarget0;
        }
        public void Update()
        {
            float distNear = goTarget.transform.localScale.z/1.5f;
            Vector3 target = goTarget.transform.position + goTarget.transform.forward * -distNear;
            go.transform.LookAt(target);
            float dist = Vector3.Distance(go.transform.position, target);
            go.transform.position = (go.transform.position + target) / 2;
        }
    }

    public enum State
    {
        normal,
        avoid,
        target
    }
}
