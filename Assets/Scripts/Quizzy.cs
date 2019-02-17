using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using UnityEngine.Video;

public class Quizzy : MonoBehaviour {
    GameObject goImageMarker;
    GameObject goButtons;
    GameObject goButtonReplay;
    GameObject goButtonLeft;
    GameObject goButtonRight;
    GameObject goButtonUp;
    GameObject goButtonDown;
    GameObject goUniverse;
    float distNear = .1f;
    List<Test> tests = new List<Test>();
    Message messageMgr;
    Audio audioMgr;
    Replay replayMgr;
    Buttons buttonsMgr;
    float angleRotate = 1.5f;
    float angleRotated;
    float angleRotateTarget = 30;
    int angleRotateDirection;
    float distMove = .01f;
    float distMoved;
    float distMoveTarget = 1.3f;
    int nButton;
    Text textTest;
    int nFloor;
    int distMoveDirection;
    bool ynDoneUpDown;
    float distVideoMove = .125f;

    // Use this for initialization
    void Start () {
        goButtonReplay = GameObject.Find("ButtonReplay");
        goButtonLeft = GameObject.Find("ButtonLeft");
        goButtonRight = GameObject.Find("ButtonRight");
        goButtonUp = GameObject.Find("ButtonUp");
        goButtonDown = GameObject.Find("ButtonDown");
        goButtons = GameObject.Find("Buttons");
        goImageMarker = GameObject.Find("ImageMarker");
        goUniverse = GameObject.Find("Universe");
        textTest = GameObject.Find("TextTest").GetComponent<Text>();
        audioMgr = GetComponent<Audio>();
        messageMgr = GetComponent<Message>();
        replayMgr = GetComponent<Replay>();
        buttonsMgr = GetComponent<Buttons>();
        LoadButtonsAndTests();
        StopActiveTests();
        if (Application.isEditor == true)
        {
            Debug.Log("Application Editor\n");
            GameObject go = goImageMarker.transform.Find("Handle").gameObject;
            go.GetComponent<MeshRenderer>().enabled = true;
        }

        //        InvokeRepeating("DemoButtons", 1, 2);
        //GoToButton("TestSound");

        //Invoke("FadeBehindVideo", 3);
    }

    // Update is called once per frame
    void Update () {
        UpdateNearTouch();
        UpdateMoveUpDown();
        UpdateRotateLeftRight();
    }

    void DemoButtons()
    {
        nButton++;
        if (nButton >= buttonsMgr.buttons.Count)
        {
            nButton = 0;
        }
        GoToButton(buttonsMgr.buttons[nButton].name);
    }

    void ResetGoImageMarker()
    {
        goImageMarker.transform.position = Vector3.zero;
    }

    void UpdateRotateLeftRight()
    {
        if (angleRotated >= angleRotateTarget) return;
        goUniverse.transform.Rotate(0, angleRotateDirection * angleRotate, 0);
        angleRotated += angleRotate;
    }

    void UpdateMoveUpDown()
    {
        if (distMoved >= distMoveTarget)
        {
            ynDoneUpDown = true;
            return;
        }
        goUniverse.transform.position += new Vector3(0, distMoveDirection * distMove, 0);
        distMoved += distMove;
    }

    void RotateWorld()
    {
        Debug.Log("rotate " + angleRotateDirection + "\n");
        angleRotated = 0;
    }

    void MoveWorldUpDown()
    {
        Debug.Log("move " + distMoveDirection + "\n");
        ynDoneUpDown = false;
        distMoved = 0;
    }

    void GoToButton(string txtName)
    {
        foreach (GameObject go in buttonsMgr.buttons)
        {
            if (go.name.Contains(txtName) == true)
            {
                goImageMarker.transform.position = go.transform.position;
                return;
            }
        }
    }

    void LoadButtonsAndTests()
    {
        if (buttonsMgr.buttons.Count > 0) return;
        foreach(Transform t in goButtons.transform)
        {
            GameObject go = t.gameObject;
            Debug.Log("add button " + go.name + "\n");
            string txt = go.name;
            Test test = null;
            bool ynStayOn = false;
            if (txt.Contains("ButtonTest") == true)
            {
                txt = txt.Replace("Button", "");
                test = FindTestByName(txt);
                test.InitTest();
            }
            else
            {
                if (go.name.Contains("ButtonVideo") == true)
                {
                    VideoPauseAtStart(go);
                    ynStayOn = true;
                }
            }
            //
            buttonsMgr.AddButton(go, ynStayOn);
            tests.Add(test);
        }
        //
        buttonsMgr.AddButton(goButtonLeft, false);
        tests.Add(null);
        //
        buttonsMgr.AddButton(goButtonRight, false);
        tests.Add(null);
        //
        buttonsMgr.AddButton(goButtonUp, false);
        tests.Add(null);
        //
        buttonsMgr.AddButton(goButtonDown, false);
        tests.Add(null);
        //
    }

    Test FindTestByName(string testName)
    {
        Component[] tests0 = GetComponents(typeof(Test));
        foreach (Test test in tests0)
        {
            if (test.GetType().ToString() == testName)
            {
                return test;
            }
        }
        return null;
    }

    void StartTest(string txtTest)
    {
        StopActiveTests();
        foreach (Test test in tests)
        {
            if (test != null)
            {
                if (test.GetType().ToString() == txtTest)
                {
                    test.StartTest();
                }
            }
        }
    }

    void FinishTest(string txtTest)
    {
        StopActiveTests();
        foreach (Test test in tests)
        {
            if (test != null)
            {
                if (test.GetType().ToString() == txtTest)
                {
                    test.FinishTest();
                }
            }
        }
    }

    void ButtonTestClick(GameObject go)
    {
        StopActiveTests();
        int n = buttonsMgr.buttons.IndexOf(go);
        Test test = tests[n];
        if (test != null)
        {
            test.StartTest();
        }
        audioMgr.PlaySound(ClipType.acquired);
    }


    void Replay()
    {
        replayMgr.state = ReplayState.replay;
        Debug.Log("Replay\n");
    }

    void ButtonClick(GameObject go)
    {
        if (go == goButtonReplay)
        {
            audioMgr.PlaySound(ClipType.yes);
//            audioMgr.PlaySound(ClipType.music);
            StopActiveTests();
            Replay();
        }
        if (go == goButtonLeft)
        {
            audioMgr.PlaySound(ClipType.yes);
            angleRotateDirection = -1;
            RotateWorld();
            buttonsMgr.goButton = null;
        }
        if (go == goButtonRight)
        {
            audioMgr.PlaySound(ClipType.yes);
            angleRotateDirection = 1;
            RotateWorld();
            buttonsMgr.goButton = null;
        }
        if (go == goButtonUp && ynDoneUpDown == true)
        {
            if (nFloor != 0) return;
            nFloor++;
            audioMgr.PlaySound(ClipType.yes);
            distMoveDirection = 1;
            MoveWorldUpDown();
        }
        if (go == goButtonDown && ynDoneUpDown == true)
        {
            if (nFloor != 1) return;
            nFloor--;
            audioMgr.PlaySound(ClipType.yes);
            distMoveDirection = -1;
            MoveWorldUpDown();
        }
        if (go.name.Contains("ButtonVideo") == true)
        {
            audioMgr.PlaySound(ClipType.yes);
//            ToggleButtonVideo(go);
            ChooseButtonVideo(go);
        }
    }

    void ChooseButtonVideo(GameObject goChoice)
    {
        
        foreach(GameObject go in buttonsMgr.buttons)
        {
            if (go.name.ToLower().Contains("video") == true)
            {
                VideoPlayer player = go.GetComponentInChildren<VideoPlayer>();
                if (go == goChoice)
                {
                    VideoPlay(go);
                    Debug.Log("play\n");
                    go.transform.position += go.transform.forward * distVideoMove;
                }
                else
                {
                    if (player.isPlaying)
                    {
                        VideoPauseAtStart(go);
                        Debug.Log("stop\n");
                        go.transform.position += go.transform.forward * -distVideoMove;
                    }
                }
            }
        }
        //FadeBehindVideo(goChoice);
    }

    void FadeBehindVideo()
    {
        foreach (GameObject go in buttonsMgr.buttons)
        {
            if (go.name.ToLower().Contains("video") == true)
            {
                go.transform.position += go.transform.forward * distVideoMove;
            }
        }
    }


    void ToggleButtonVideo(GameObject go)
    {
        VideoPlayer player = go.GetComponentInChildren<VideoPlayer>();
        if (player.isPlaying)
        {
            VideoPauseAtStart(go);
            Debug.Log("stop\n");
        }
        else
        {
            VideoPlay(go);
            Debug.Log("play\n");
        }
    }

    void VideoPlay(GameObject go)
    {
        VideoPlayer player = go.GetComponentInChildren<VideoPlayer>();
        player.Play();
    }

    void VideoPauseAtStart(GameObject go)
    {
        VideoPlayer player = go.GetComponentInChildren<VideoPlayer>();
        //player.audioOutputMode = VideoAudioOutputMode.AudioSource;
        //player.source = VideoSource.VideoClip;
        //player.controlledAudioTrackCount = 1;
        //player.EnableAudioTrack(0, true);
        //player.SetTargetAudioSource(0, audioMgr.audioSource);
        player.Stop();
        player.Play();
        player.Pause();
    }

    void UpdateNearTouch()
    {
        foreach(GameObject go in buttonsMgr.buttons)
        {
            if (IsNear(go) == true) {
                if (go != buttonsMgr.goButton)
                {
                    string txt = go.name.Replace("Button", "");
                    textTest.text = txt;
                    buttonsMgr.UnHighlight();
                    buttonsMgr.goButton = go;
                    if (go.name.Contains("ButtonTest") == true)
                    {
                        ButtonTestClick(go);
                    }
                    else
                    {
                        ButtonClick(go);
                    }
                }
            }
        }
    }

    void StopActiveTests()
    {
        foreach(Test test in tests)
        {
            if (test != null)
            {
                if (test.ynActive == true)
                {
                    test.FinishTest();
                }
            }
        }
    }

    bool IsNear(GameObject go)
    {
        float dist = Vector3.Distance(go.transform.position, goImageMarker.transform.position);
        if (dist <= distNear)
        {
            return true;
        }
        return false;
    }
}

