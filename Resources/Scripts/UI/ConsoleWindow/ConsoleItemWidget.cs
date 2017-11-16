using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleItemWidget : MonoBehaviour {

    public Text itemText;

    public void SetText(string txt)
    {
        itemText.text = txt;
    }

    public void SetColor(Color c)
    {
        itemText.color = c;
    }

    public void SetSize(int size)
    {
        itemText.fontSize = size;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
