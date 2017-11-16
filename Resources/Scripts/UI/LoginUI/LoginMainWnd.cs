using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class LoginMainWnd : MonoBehaviour
{
    LoginInfo m_loginInfo;
    GameContext m_context;

    public Text m_userName;
    public Text m_hostIP;
    public Text m_tcpPort;
    public Text m_udpPort;

    public GameObject m_iconPrefab;
    public RectTransform m_iconContent;

    void Awake()
    {
        m_loginInfo = new LoginInfo();
        m_context = new GameContext();
    }

    // Use this for initialization
    void Start()
    {

        AddIcon("iconH001");
        AddIcon("iconH002");
        AddIcon("iconH003");
        AddIcon("iconH004");
        AddIcon("iconH005");
        AddIcon("iconH006");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddIcon(string path)
    {
        if (!GlobalClient.spriteData.ContainsKey(path))
            return;
        GameObject obj = GameObject.Instantiate(m_iconPrefab);
        if(obj != null)
        {
            Image imge = obj.GetComponent<Image>();
            if(imge != null)
            {
                imge.sprite = GlobalClient.spriteData[path];
                UserIconItem item  = obj.GetComponent<UserIconItem>();
                item.m_imagePath = path;
                item.m_mainWnd = this;
                obj.transform.SetParent(m_iconContent);
            }
        }
        
    }

    public void OnClickIcon(string path)
    {
        m_loginInfo.userIconPath = path;
        Debug.Log(string.Format("点击 {0}", path));
    }

    public void OnClickLogin()
    {
        Debug.Log("点击登录!");
        UserLoginArg arg = new UserLoginArg();
        m_loginInfo.userName = m_userName.text;
        m_loginInfo.tcpPort = int.Parse(m_tcpPort.text);
        m_loginInfo.udpPort = int.Parse(m_udpPort.text);
        m_loginInfo.hostIp = m_hostIP.text;
        arg.info = m_loginInfo;
        m_context.FireEvent(this, EventType.EVT_USER_LOGIN, arg);
    }
}
