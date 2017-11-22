using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    /// <summary>
    /// 当前玩家自身对象
    /// </summary>
    public class PlayerView : CreatureView
    {

        public override void Create(CharData charData, EntityViewManager viewMap, Vector3 Pos, Quaternion rotation)
        {
            _viewMap = viewMap;
            gameObj = new PlayerEntity();
            gameObj.Init(charData, GlobalClient.GameManager.LogicManager);
             
            GameObject obj = GlobalClient.prefabData["PlayerActor1"];
            gameGo = GameObject.Instantiate(obj, Pos, rotation) as GameObject;
            gameGo.name = charData.name;
            gameTrans = gameGo.transform;
            actor = gameGo.GetComponent<Actor>();
            actor.viewObj = this;
            if(GameManager.instance.IsHostPlayer())
            {
                actor.SetMaterial(true, false);
                gameObj.teamNo = 1;
            }else
            {
                actor.SetMaterial(false, false);
                gameObj.teamNo = 0;
            }
            gameObj.Position = gameTrans.position;
            gameObj.Direction = gameTrans.forward;
            gameObj.Speed = 0.5f;
            //Camera.main.gameObject.AddComponent<CameraController>();
            //CameraController cc = Camera.main.gameObject.GetComponent<CameraController>();
            //cc.targeTransform = gameTrans;
            //cc.y = 5;
            //cc.z = -10;
        }
    }
}