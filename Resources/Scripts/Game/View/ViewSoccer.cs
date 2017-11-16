using UnityEngine;
using System.Collections;

namespace Game
{
    /// <summary>
    /// 足球显示对象
    /// </summary>
    public class ViewSoccer : ViewObj
    {

        public override void Create(CharData charData, ViewMap viewMap, Vector3 Pos, Quaternion rotation)
        {
            _viewMap = viewMap;
            gameObj = new Player();
            gameObj.Init(charData, viewMap.LogicMap);
            GameObject obj = GlobalClient.prefabData["Soccer"];
            GameObject sp = GameObject.Find("SpawnPoint2");
            gameGo = GameObject.Instantiate(obj, sp.transform.position, sp.transform.rotation) as GameObject;

            //gameGo.transform.position = sp.transform.position;
            // gameGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gameGo.name = charData.name;
            gameTrans = gameGo.transform;
        }
    }
}

