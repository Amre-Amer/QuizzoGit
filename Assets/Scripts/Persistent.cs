using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistent : MonoBehaviour
{
    string pth;

    // Use this for initialization
    void Awake()
    {
        pth = System.IO.Path.Combine(Application.persistentDataPath, @"credentials.txt");
        //path = Application.dataPath + @"credentials.txt";
        Debug.Log("path " + pth + "\n");
    }

    public string ReadPathFile()
    {
        if (System.IO.File.Exists(pth) == false) return null;
        string text = System.IO.File.ReadAllText(pth);
        return text;
    }

    public void WriteToPathFile(string txt)
    {
        Debug.Log("path " + pth + "\n");
        System.IO.File.WriteAllText(pth, txt);
    }
}
