using UnityEngine;
using System.Collections;

namespace Game
{
    /// <summary>
    /// 其它玩家对象
    /// </summary>
    public class ViewPlayer : ViewObj
    {


        public override void Create(CharData charData, ViewMap viewMap, Vector3 Pos, Quaternion rotation)
        {
            _viewMap = viewMap;
            gameObj = new Player();
            gameObj.Init(charData, viewMap.LogicMap);
            GameObject obj = GlobalClient.prefabData["PlayerActor1"];
            gameGo = GameObject.Instantiate(obj, Pos, rotation) as GameObject;
            
            actor = gameGo.GetComponent<Actor>();
            actor.viewObj = this;
            if (Game.GameManager.instance.IsHostPlayer())
            {
                actor.SetMaterial(false, false);
                gameObj.teamNo = 0;
            }
            else
            {
                actor.SetMaterial(true, false);
                gameObj.teamNo = 1;
                 
            }
            gameGo.name = charData.name;
            gameTrans = gameGo.transform;
            gameObj.Position = gameTrans.position;
            gameObj.Direction = gameTrans.forward;
            gameObj.Speed = 0.5f;
        }

        public override void Update()
        {
            base.Update();
            Player player = gameObj as Player;
            if(player.State.GetState() == ActorState.RUN1)
            {
                gameGo.transform.position = gameObj.Position;
                gameGo.transform.localRotation = gameObj.Rotation;
                actor.SetAnimationStateInteger("shotPower", 0);
                actor.SetAnimationStateInteger("InputCmd", 0);
                actor.SetAnimationStateInteger("speed", 5);

            }
            else if (player.State.GetState() == ActorState.STAND)
            {
                actor.SetAnimationStateInteger("shotPower", 0);
                actor.SetAnimationStateInteger("InputCmd", 0);
                actor.SetAnimationStateInteger("speed", 0);
            }
        }
    }
}

