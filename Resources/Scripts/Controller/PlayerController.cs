using UnityEngine;
using System.Collections;
using System;

namespace Game
{

    public class InputData
    {
        public bool bMoved = false;

        public float angle = 0;
        //移动方向
        public Vector3 direction = Vector3.zero;
        //射门键按时常
        public int shootPower = 0;
        //抢球键按时长
        public int stolenPower = 0;
        //小传键按时长
        public int passPower = 0;
    }

    public delegate void MoveDelegate();
    public delegate void ShotDelegate();
    public class PlayerController : MonoBehaviour
    {
        public MoveDelegate moveStart;
        public MoveDelegate moveEnd;
        public ShotDelegate shotStart;
        public ShotDelegate shotEnd;
        private GameContext context;
        /// <summary>
        /// 控制目标
        /// </summary>
        private Transform target;
        private float angle;

        [SerializeField]
        private bool bMoved = false;

        [SerializeField]
        private float moveSpeed = 5;

        //GUI 摇杆控制
        private JoytackController guiJoystackController;

        void Awake()
        {
            context = new GameContext();
            guiJoystackController = JoytackController.instance;
            GlobalClient.AddEventReceiver(EventType.EVT_MOUSE_FIRE1, context);
            GlobalClient.AddEventReceiver(EventType.EVT_MOUSE_FIRE2, context);
            context.MouseFire1Handler += OnMouseFire1;
            context.MouseFire2Handler += OnMouseFire2;
            moveStart = OnMoveStart;
            moveEnd = OnMoveEnd;
        }

        void Start()
        {
            
        }

        public Transform Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (guiJoystackController == null)
            {
                return;
            }
            if (Input.GetButtonDown("Fire2"))
            {
                context.FireEvent(this, EventType.EVT_MOUSE_FIRE2, new MouseFire2EvtArg());
            }
            if (Input.GetButtonDown("Fire1"))
            {
                context.FireEvent(this, EventType.EVT_MOUSE_FIRE1, new MouseFire1EvtArg());
            }
            //更新控制目标的位置
            if (bMoved && target != null)
            {

                //位置的移动
                //Vector3 move = guiJoystackController.movePosNorm * Time.deltaTime * moveSpeed;
                //target.localPosition += move;
                ////从JoytackController移动方向 算出自身的角度
                //angle = Mathf.Atan2(guiJoystackController.movePosNorm.x,
                //    guiJoystackController.movePosNorm.z) * Mathf.Rad2Deg;
                //target.localRotation = Quaternion.Euler(Vector3.up * angle);
               
            }
        }

        private void OnMoveEnd()
        {
            bMoved = false;
             
            if (target != null)
            {
                Actor actor = target.gameObject.GetComponent<Actor>();
                //actor.SetAnimationStateInteger("speed", 0);
                UnitMoveEndEvtArg arg = new UnitMoveEndEvtArg();
                arg.actor = actor;
                context.FireEvent(this, EventType.EVT_UNIT_MOVE_END, arg);
                GlobalClient.GameManager.viewMap.LogicMap.InputCmd(Cmd.UnitMoveEnd, actor.viewObj.gameObj.mCharData.name);
            }


        }

        private void OnMoveStart()
        {
            bMoved = true;
            
            if (target != null)
            {
                Actor actor = target.gameObject.GetComponent<Actor>();
                //actor.SetAnimationStateInteger("speed", ((int)moveSpeed));
                UnitMoveBeginEvtArg arg = new UnitMoveBeginEvtArg();
                arg.actor = actor;
                context.FireEvent(this, EventType.EVT_UNIT_MOVE_BEGIN, arg);
                GlobalClient.GameManager.viewMap.LogicMap.InputCmd(Cmd.UnitMoveBegin, actor.viewObj.gameObj.mCharData.name);
            }

        }


        void OnMouseFire1(object sender, EventArgs arg)
        {
            //Debug.Log ("鼠标点击1");

        }

        void OnMouseFire2(object sender, EventArgs arg)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200))
            {
                Vector3 pos = hit.point;
                if (hit.transform.tag == "Player")
                {

                    GameObject obj = hit.transform.gameObject;
                    Actor actor = obj.GetComponent<Actor>();
                    Debug.Log("点击Player" + obj.name);
                    if(GlobalClient.GameManager.IsHostPlayer())
                    {
                        if(actor.viewObj.gameObj.teamNo == 1)
                        {
                            target = hit.transform;
                            moveSpeed = actor.speed;
                            GlobalClient.GameManager.viewMap.CurViewObj = actor.viewObj;
                            Debug.Log(string.Format("点击teamNo{0}, PlayerName;{1}", actor.viewObj.gameObj.teamNo, obj.name));
                        }
                    }else
                    {
                        if (actor.viewObj.gameObj.teamNo == 0)
                        {
                            target = hit.transform;
                            moveSpeed = actor.speed;
                            GlobalClient.GameManager.viewMap.CurViewObj = actor.viewObj;
                            Debug.Log(string.Format("点击teamNo{0}, PlayerName;{1}", actor.viewObj.gameObj.teamNo, obj.name));
                        }
                    }
                    
                }

            }
        }

        void OnDestroy()
        {
            GlobalClient.RemoveEventReceiver(EventType.EVT_MOUSE_FIRE1, context);
            GlobalClient.RemoveEventReceiver(EventType.EVT_MOUSE_FIRE2, context);
        }


        public bool IsMoved
        {
            get
            {
                return bMoved;
            }
        }

        /// <summary>
        /// 获取操作数据
        /// </summary>
        /// <returns>操作数据</returns>
        public InputData GetPlayerInputData()
        {
            InputData data = new InputData();
            if(bMoved == true)
            {
                data.bMoved = true;
                //Vector3 move = guiJoystackController.movePosNorm * Time.deltaTime * moveSpeed;
                //只需要一个角度，通过速度能算出移动距离
                angle = Mathf.Atan2(guiJoystackController.movePosNorm.x, guiJoystackController.movePosNorm.z) * Mathf.Rad2Deg;
                data.angle = angle;
            }else
            {
                data.bMoved = false;
                //Vector3 move = guiJoystackController.movePosNorm * Time.deltaTime * moveSpeed;
                //只需要一个角度，通过速度能算出移动距离
                angle = Mathf.Atan2(guiJoystackController.movePosNorm.x, guiJoystackController.movePosNorm.z) * Mathf.Rad2Deg;
                data.angle = angle;
            }
            return data;
        }
    }
}


