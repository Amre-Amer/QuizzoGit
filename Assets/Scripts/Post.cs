using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class Post : MonoBehaviour
{
    string url = "http://msi/post.php";
    string email = "me@amre-amer.com";
    string firstname = "Amre";
    string lastname = "Amer";
    string test = "PLACEHOLDER TEST";
    string duration = "1";
    string timeOff = "2.4";
    string cntOff = "1";
    string urlReplay = "http://msi/postReplay.php";
    string replays;

    public void PostResult(string email0, string firstname0, string lastname0, string test0, string duration0, string timeOff0, string cntOff0)
    {
        email = email0;
        firstname = firstname0;
        lastname = lastname0;
        test = test0;
        duration = duration0;
        timeOff = timeOff0;
        cntOff = cntOff0;
        StartCoroutine(PostResult0());
    }

    IEnumerator PostResult0()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("firstname", firstname);
        form.AddField("lastname", lastname);
        form.AddField("test", test);
        form.AddField("duration", duration);
        form.AddField("timeOff", timeOff);
        form.AddField("cntOff", cntOff);
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string txt = "POST -------------------------------------\n";
                txt += "  email " + email + " ";
                txt += " firstname " + firstname;
                txt += " lastname " + lastname + " ";
                txt += " test " + test + " ";
                txt += " duration " + duration + " ";
                txt += " timeOff " + timeOff + " ";
                txt += " cntOff " + cntOff + " ";
                Debug.Log(txt + "\n");
                Debug.Log("POST successful!\n");
                StringBuilder sb = new StringBuilder();
                foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
                {
                    sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                }
                Debug.Log(sb + "\n");
                Debug.Log(www.downloadHandler.text + "\n");
            }
        }
    }

    public void PostReplay(string email0, string firstname0, string lastname0, string test0, string replays0)
    {
        email = email0;
        firstname = firstname0;
        lastname = lastname0;
        test = test0;
        replays = replays0;
        StartCoroutine(PostReplay0());
    }

    IEnumerator PostReplay0()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("firstname", firstname);
        form.AddField("lastname", lastname);
        form.AddField("test", test);
        form.AddField("replays", replays);
        using (UnityWebRequest www = UnityWebRequest.Post(urlReplay, form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string txt = "POST -------------------------------------\n";
                txt += "  email " + email + " ";
                txt += " firstname " + firstname;
                txt += " lastname " + lastname + " ";
                txt += " test " + test + " ";
                txt += " replays " + replays + " ";
                Debug.Log(txt + "\n");
                Debug.Log("POST replay successful!\n");
                StringBuilder sb = new StringBuilder();
                foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
                {
                    sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                }
                Debug.Log(sb + "\n");
                Debug.Log(www.downloadHandler.text + "\n");
            }
        }
    }

}