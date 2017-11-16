using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
 


public class GlobalClient
{
    public static string hostIP = "127.0.0.1";
    static int tcpPort = 1255;
    static int udpPort = 1337;

    static Game.CameraController cameraController;
    static Game.NetManager netManager;
    static Game.PlayerController playerController;
    static TimeManager timeManager;
    static Game.GameManager gameManager;
    static Game.KeyFrameManager keyManager;
    static Game.CommandManager commandManager;
    static Game.GamePlayerManager playerManager;
    static Console console;
    //预置表
    public static Dictionary<string, GameObject> prefabData;
    public static Dictionary<string, Sprite> spriteData;
    static Dictionary<string, ObjectFactory> factories;
    public static readonly Hashtable globalDataTable;
    static Dictionary<EventType, HashSet<GameContext>> eventReceivers;
    static GlobalClient()
    {
        netManager = new Game.NetManager();
        globalDataTable = new Hashtable();
        prefabData = new Dictionary<string, GameObject>();
        spriteData = new Dictionary<string, Sprite>();
        factories = new Dictionary<string, ObjectFactory>();
        eventReceivers = new Dictionary<EventType, HashSet<GameContext>>();
        keyManager = new Game.KeyFrameManager();
        commandManager = new Game.CommandManager();
        //string path = Application.dataPath + "/Resources/Prefabs/";
        string path = "Prefabs/entity/";
        ReadPrefabs(path);
        path = "UI/Textures/";
        ReadSprite(path);
    }

    public static Dictionary<EventType, HashSet<GameContext>> EventReceivers
    {
        get
        {
            return eventReceivers;
        }
    }
 
    public static object CreateObject(string name)
    {
        if (factories == null)
            return null;
        if (factories.ContainsKey(name))
        {
            return factories[name].createObject();
        }
        return null;
    }

    public static Console Debug
    {
        get
        {
            if(console == null)
            {
                console = new Console();
            }
            return console;
        }

    }

    public static Game.GamePlayerManager PlayerManager
    {
        
        get
        {
            if(playerManager == null)
            {
                playerManager = new Game.GamePlayerManager();
            }
            return playerManager;
        }
    }

    public static Game.KeyFrameManager KeyFrameManager
    {
        get
        {
            return keyManager;
        }
    }

    public static Game.CommandManager CommandManager
    {
        get
        {
            return commandManager;
        }
    }

    public static string HostIP
    {
        get
        {
            return hostIP;
        }
        set
        {
            hostIP = value;
        }
    }


    public static int TCP_Port
    {
        get
        {
            return tcpPort;
        }
        set
        {
            tcpPort = value;
        }
    }

    public static int UDP_Port
    {
        get
        {
            return udpPort;
        }
        set
        {
            udpPort = value;
        }
    }


    public static void RegisterObjectFactory<T>() where T : new()
    {
        if (factories == null)
            return;
        ObjectFactory factory = new ObjectFactoryImpl<T>();
        factories.Add(factory.objectName, factory);

    }


    public static Game.NetManager NetWorkManager
    {
        get
        {
            return netManager;
        }
    }

    public static Game.GameManager GameManager
    {
        get
        {
            if(gameManager==null)
            {
                gameManager = Game.GameManager.instance;
            }

            return gameManager;
        }
    }

    public static TimeManager TimeManager
    {
        get
        {
            if(timeManager == null)
            {
                GameObject obj = GameObject.Find("GameManager");
                if (obj != null)
                {
                    timeManager = obj.GetComponent<TimeManager>();
                }
            }
            return timeManager;
        }
    }

    public Game.CameraController CameraController
    {
        get
        {
            if (cameraController == null)
            {
                GameObject obj = GameObject.Find("GameManager");
                if (obj != null)
                {
                    cameraController = obj.GetComponent<Game.CameraController>();
                }
            }
            return cameraController;
        }
    }

    public static Game.PlayerController GetPlayerController()
    {
        if (playerController == null)
        {
            GameObject obj = GameObject.Find("GameManager");
            if (obj != null)
            {
                playerController = obj.GetComponent<Game.PlayerController>();
            }
        }
        return playerController;
    }

    //添加事件接收者
    public static void AddEventReceiver(EventType evt, GameContext context)
    {
        if (eventReceivers == null)
        {
            eventReceivers = new Dictionary<EventType, HashSet<GameContext>>();
        }
        if (eventReceivers.ContainsKey(evt) == false || eventReceivers[evt] == null)
        {
            HashSet<GameContext> set = new HashSet<GameContext>();
            eventReceivers.Add(evt, set);
        }
        eventReceivers[evt].Add(context);
    }

    public static void RemoveEventReceiver(EventType evt, GameContext context)
    {
        if (eventReceivers == null)
        {
            eventReceivers = new Dictionary<EventType, HashSet<GameContext>>();
        }
        if (!eventReceivers.ContainsKey(evt))
            return;
        eventReceivers[evt].Remove(context);
    }
    static void ReadPrefabs(string dirPath)
    {
        
        Object[] objList = Resources.LoadAll(dirPath);//AssetDatabase.LoadAssetAtPath<GameObject>(fileRePath) as GameObject;
        for (int i = 0; i < objList.Length; i++ )
        {
            Object obj = objList[i];
            if (obj != null)
            {
                GameObject go = obj as GameObject;
                //添加到预置表选中
                string key = go.name;
                prefabData.Add(key, go);
                Debug.Log("读取" + key + "到预置表中");
            }

        }

    }

    static void ReadSprite(string dir)
    {
        Object[] objList = Resources.LoadAll(dir);
        for (int i = 0; i < objList.Length; i++)
        {
            Object obj = objList[i];
            if (obj != null)
            {
                if(obj.GetType() != typeof(Sprite))
                {
                    Texture2D tex = obj as Texture2D;
                    Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    string key = obj.name;
                    spriteData.Add(key, sp);
                    Debug.Log("读取" + key + "到Sprite表中");
                }
                
            }

        }
    }


}
