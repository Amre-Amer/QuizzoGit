using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Net;

public class Udp : MonoBehaviour
{
    UdpClient client;
    Thread receiveThread;
    int port = 26000;
    int cntFrames;
    IPEndPoint remoteEndPointSender;
    string ipLocal = "?";
    [HideInInspector]
    public Vector3 posOther;
    [HideInInspector]
    public bool ynActive;
    bool ynActiveLast;

    // Start is called before the first frame update
    void Start()
    {
        // move out of UDP to TestRemote
        ipLocal = GetIpLocal();
        Debug.Log("ip local " + ipLocal + "\n");
    }

    // Update is called once per frame
    void Update()
    {
        if (ynActive == ynActiveLast) return;
        if (ynActive == true)
        {
            StartUdp();
        }
        else
        {
            FinishUdp();
        }
        ynActiveLast = ynActive;
    }

    void StartUdp()
    {
        Debug.Log("Start Udp\n");
        remoteEndPointSender = new IPEndPoint(IPAddress.Broadcast, port);
        InitReceiveUDP();
    }

    public void SendThis(Vector3 pos)
    {
        string txt = Vector3ToString(pos);
        byte[] data = System.Text.Encoding.ASCII.GetBytes(txt);
        client.Send(data, data.Length, remoteEndPointSender);
        cntFrames++;
    }

    void InitReceiveUDP()
    {
        Application.runInBackground = true;
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
        Debug.Log("listening on port " + port + "\n");
    }

    void ReceiveData()
    {
        client = new UdpClient(port);
        byte[] data;
        while (true)
        {
            try
            {
                IPEndPoint anyIp = new IPEndPoint(IPAddress.Any, port);
                data = client.Receive(ref anyIp);
                string ipFrom = anyIp.Address.ToString();
                if (ipFrom != ipLocal)
                {
                    string txt = System.Text.Encoding.ASCII.GetString(data);
                    posOther = StringToVector3(txt);
                }
            }
            catch (Exception err)
            {
                Debug.Log("error " + err + "\n");
            }
        }
    }

    Vector3 StringToVector3(string txt)
    {
        string[] stuff = txt.Split(',');
        if (stuff.Length != 3) return Vector3.zero;
        float x = float.Parse(stuff[0]);
        float y = float.Parse(stuff[1]);
        float z = float.Parse(stuff[2]);
        return new Vector3(x, y, z);
    }

    string Vector3ToString(Vector3 pos)
    {
        return pos.x + "," + pos.y + "," + pos.z;
    }

    string GetIpLocal()
    {
        IPHostEntry host;
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "?";
    }

    void FinishUdp()
    {
        Debug.Log("Finish Udp\n");
        if (client != null)
        {
            client.Close();
            client = null;
        }
        if (receiveThread != null)
        {
            receiveThread.Abort();
            receiveThread = null;
        }
    }

    private void OnDisable()
    {
        FinishUdp();
    }
}
