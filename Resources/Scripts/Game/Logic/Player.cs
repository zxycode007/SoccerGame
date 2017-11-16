using UnityEngine;
using System.Collections;

namespace Game
{
    /// <summary>
    /// 球员
    /// </summary>
    public class Player : GameObj
    {
        /// <summary>
        /// 当前角色状态
        /// </summary>
        BaseActorState _mActorState;
        
         
   
        public BaseActorState State
        {
            get
            {
                return _mActorState;
            }
            set
            {
                _mActorState = value;
            }
        }

        public override void Init(CharData charData, GameMap gameMap)
        {
            base.Init(charData, gameMap);
            _mActorState = new StandActorState(this);
            
        }

        public void DoSkill(int skillId)
        {
            //Debug.LogError("释放技能,技能id = " + skillId);
        }

        public void SetState(BaseActorState state)
        {
            _mActorState = state;
        }

        public override void Update()
        {
            //更新逻辑状态
            _mActorState.Update();
            
        }

        
    }
}
