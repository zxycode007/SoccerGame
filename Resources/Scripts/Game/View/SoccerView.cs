using UnityEngine;
using System.Collections;

namespace Game
{
    /// <summary>
    /// 足球显示对象
    /// </summary>
    public class SoccerView : EntityView
    {

        public override void Create(CharData charData, EntityViewManager viewMap, Vector3 Pos, Quaternion rotation)
        {
            _viewMap = viewMap;
            gameObj = new CreatureEntity();
            gameObj.Init(charData, GlobalClient.GameManager.LogicManager);
            GameObject obj = GlobalClient.prefabData["Soccer"];
            GameObject sp = GameObject.Find("SpawnPoint2");
            gameGo = GameObject.Instantiate(obj, sp.transform.position, sp.transform.rotation) as GameObject;

            gameGo.name = charData.name;
            gameTrans = gameGo.transform;
        }
    }
}

