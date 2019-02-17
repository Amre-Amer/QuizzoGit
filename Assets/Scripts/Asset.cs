using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class Asset : MonoBehaviour
{
    GameObject goParent;
    AssetBundle bundle;
    Message messageMgr;

    //    gogle drive
    //    change "https://drive.google.com/file/d/1IVLPurpoQi9iNb30cX9hz5SSbf7rN8CW/view?usp=sharing";
    //    to "https://drive.google.com/uc?export=view&id=1IVLPurpoQi9iNb30cX9hz5SSbf7rN8CW";
    // https://drive.google.com/file/d/1yr5VQgmf9alOoMXrrWKMiz6efc7tRCdl/view?usp=sharing
    string url = "https://drive.google.com/uc?export=view&id=1yr5VQgmf9alOoMXrrWKMiz6efc7tRCdl";

    private void Start()
    {
        messageMgr = GetComponent<Message>();
        messageMgr.textLog = GameObject.Find("Log").GetComponent<Text>();
    }

    public void LoadAssetWWW(string txtName0, GameObject goParent0)
    {
        goParent = goParent0;
        string txtName = txtName0.ToLower();
        if (bundle == null)
        {
            Debug.Log("downloading bundle\n");
            messageMgr.textLog.text += "downloading bundle " + txtName + "\n";
            messageMgr.goLoading.SetActive(true);
            StartCoroutine(GetAsset(txtName));
        }
        else
        {
            PlaceAsset(txtName);
        }
    }

    IEnumerator GetAsset(string txtName)
    {
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                bundle = DownloadHandlerAssetBundle.GetContent(uwr);


                string[] assetNames = bundle.GetAllAssetNames();
                Debug.Log("assets " + assetNames.Length + "\n");
                foreach (string txt in assetNames)
                {
                    Debug.Log("asset " + txt + "\n");
                }

                messageMgr.goLoading.SetActive(false);
                PlaceAsset(txtName);

//                bundle.Unload(true);
            }
        }
    }

    void PlaceAsset(string txtName)
    {
        messageMgr.textLog.text += "placing asset " + txtName + "\n";
        GameObject go = Instantiate(bundle.LoadAsset<GameObject>(txtName.ToLower())) as GameObject;
        go.transform.position = goParent.transform.position;
        go.transform.eulerAngles = goParent.transform.eulerAngles;
        go.transform.parent = goParent.transform;
    }
}



