using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Game
{
    /// <summary>
    /// 动画控制参数
    /// </summary>
    public class AnimationControlParamter
    {
        //动画时间缩放
        public float timeScale { get; set; }
        public bool bLoop { get; set; }
    }


    public class Actor : MonoBehaviour
    {
        //角色属性
        public float speed = 5;
        //球员材质
        public Material materialA1;
        //守门员材质
        public Material materialA2;
        public Material materialB1;
        public Material materialB2;
        //渲染器
        public Renderer renderer;

        public Animator animator;
        public Dictionary<string, AnimatorControllerParameter> defaultAnimationParamters;

        public CreatureView viewObj;
        private GameContext context;



        void OnDestroy()
        {
            //Debug.Log (ToString () + this.GetInstanceID() + "被销毁");
            GlobalClient.RemoveEventReceiver(EventType.EVT_UNIT_MOVE_BEGIN, context);
            GlobalClient.RemoveEventReceiver(EventType.EVT_UNIT_MOVE_END, context);

        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {

        }

        private void Awake()
        {
            context = new GameContext();
            GlobalClient.AddEventReceiver(EventType.EVT_UNIT_MOVE_BEGIN, context);
            GlobalClient.AddEventReceiver(EventType.EVT_UNIT_MOVE_END, context);
            context.UnitMoveBeginHandler += OnBeginMove;
            context.UnitMoveEndHandler += OnEndMove;
        }

        void OnBeginMove(object sender, EventArgs arg)
        {
            UnitMoveBeginEvtArg e = arg as UnitMoveBeginEvtArg;
            if(e.actor == this)
            {
                Debug.Log(e.actor.viewObj.gameGo.name + "begin move");
                SetAnimationStateInteger("speed", (int)(speed));
            }
           
            
        }

        void OnEndMove(object sender, EventArgs arg)
        {
            UnitMoveEndEvtArg e = arg as UnitMoveEndEvtArg;
            if (e.actor == this)
            {
                Debug.Log(e.actor.viewObj.gameGo.name + "end move");
                SetAnimationStateInteger("speed", 0);
            }
        }

        public void init()
        {

        }

        public void SetMaterial(bool isTeamA, bool isKeeper)
        {
            if (isTeamA)
            {
                if (isKeeper)
                {
                    renderer.material = materialA2;
                }
                else
                {
                    renderer.material = materialA1;
                }
            }
            else
            {
                if (isKeeper)
                {
                    renderer.material = materialB2;
                }
                else
                {
                    renderer.material = materialB1;
                }
            }
        }

        public AnimationControlParamter GetAnimationParamter(string animationName)
        {
            AnimationControlParamter paramter = new AnimationControlParamter();
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i].name == animationName)
                {
                    if (clips[i].wrapMode == WrapMode.Loop)
                    {
                        paramter.bLoop = true;

                    }
                    else
                    {
                        paramter.bLoop = false;
                    }
                    break;
                }
            }
#if UNITY_EDITOR
            UnityEditor.Animations.AnimatorController ac = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
            //获取状态机
            UnityEditor.Animations.AnimatorStateMachine asm = ac.layers[0].stateMachine;
            for (int i = 0; i < asm.states.Length; i++)
            {
                UnityEditor.Animations.ChildAnimatorState cs = asm.states[i];
                if (cs.state.name == animationName)
                {
                    paramter.timeScale = cs.state.speed;
                    break;
                }
            }
#endif
            return paramter;
        }

        public void SetAnimationParamter(string animationName, AnimationControlParamter paramter)
        {
            //获取所有动画片段
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i].name == animationName)
                {
                    if (paramter.bLoop)
                    {
                        clips[i].wrapMode = WrapMode.Loop;

                    }
                    else
                    {
                        clips[i].wrapMode = WrapMode.Default;
                    }
                }
            }
#if UNITY_EDITOR
            UnityEditor.Animations.AnimatorController ac = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;

            //获取状态机
            UnityEditor.Animations.AnimatorStateMachine asm = ac.layers[0].stateMachine;
            for (int i = 0; i < asm.states.Length; i++)
            {
                UnityEditor.Animations.ChildAnimatorState cs = asm.states[i];
                if (cs.state.name == animationName)
                {
                    cs.state.speed = paramter.timeScale;


                }
            }
#endif

        }

        public float GetAnimationLength(string animationName)
        {
            float len = 0.0F;
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i].name == animationName)
                {
                    len = clips[i].length;
                    Debug.Log("动作名" + clips[i].name);
                    break;
                }
            }
            return len;

        }

        public void SetAnimationStateInteger(string name, int val)
        {
            if (animator != null)
            {
                animator.SetInteger(name, val);
            }
        }

        public void SetAnimationStateFloat(string name, float val)
        {
            if (animator != null)
            {
                animator.SetFloat(name, val);
            }
        }

        public void SetAnimationStateBool(string name, bool val)
        {
            if (animator != null)
            {
                animator.SetBool(name, val);
            }
        }

        public int GetAnimationStateInteger(string name)
        {
            if (animator != null)
            {
                return animator.GetInteger(name);
            }

            return -1;
        }

        public bool GetAnimationStateBool(string name)
        {
            if (animator != null)
            {
                return animator.GetBool(name);
            }

            return false;
        }


        public float GetAnimationStateFloat(string name)
        {
            if (animator != null)
            {
                return animator.GetFloat(name);
            }
            return 0;
        }



         
    }
}

