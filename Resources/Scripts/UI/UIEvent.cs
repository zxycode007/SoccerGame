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
        if (Game.GameManager.instance.NetManager.IsConnected() == false)
        {
            Game.GameManager.instance.NetManager.Connect();
        }
    }

    public void OnClickReady()
    {
        Game.GameManager.instance.NetManager.Ready();
        return;
    }

    public void OnClickQuit()
    {
        Game.GameManager.instance.NetManager.Disconnect();
        Application.Quit();
    }
}
