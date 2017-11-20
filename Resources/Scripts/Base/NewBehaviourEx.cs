using UnityEngine;
using System.Collections;

public class MonoBehaviourEx : MonoBehaviour
{
    private GameContext context;
    /// <summary>
    /// 重新设置初值
    /// </summary>
    public virtual void Reset()
    {

    }

    void Awake()
    {
        context = new GameContext();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void Create()
    {

    }
}
