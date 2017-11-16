using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserIconItem : MonoBehaviour
{
    public string m_imagePath;
    public LoginMainWnd m_mainWnd;
     
    // Use this for initialization
    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null && m_mainWnd != null)
        {
            btn.onClick.AddListener(delegate() { m_mainWnd.OnClickIcon(m_imagePath); });
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
