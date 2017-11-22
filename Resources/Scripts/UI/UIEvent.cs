using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIEvent : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickConnect()
    {
        if (GlobalClient.NetWorkManager.IsConnected() == false)
        {
            GlobalClient.NetWorkManager.Connect();
        }
    }

    public void OnClickReady()
    {
        GlobalClient.NetWorkManager.Ready();
        return;
    }

    public void OnClickQuit()
    {
        GlobalClient.NetWorkManager.Disconnect();
        Application.Quit();
    }
}
