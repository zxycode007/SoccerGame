using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
/// <summary>
/// 
/// </summary>
public class PlayerItem : MonoBehaviour {

    GamePlayer m_player;

    public Image m_image;

	// Use this for initialization
	void Start () {
		
	}
	void Awake()
    {
        m_image = gameObject.GetComponent<Image>();
    }
	// Update is called once per frame
	void Update () {
		
	}

    void SetImage(string path)
    {
        Sprite sp = Resources.Load<Sprite>(path);
        if(sp != null  && m_image != null)
        {
            m_image.sprite = sp;
        }
    }

    public GamePlayer PlayerInfo
    {
        get
        {
            return m_player;
        }
        set
        {
            m_player = value;
            SetImage(m_player.PlayerIcon);
        }
    }
}
}

