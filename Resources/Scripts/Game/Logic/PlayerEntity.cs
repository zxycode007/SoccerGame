using UnityEngine;
using System.Collections;

namespace Game
{
    public class PlayerEntity : CreatureEntity
    {
        public override void Init(CharData charData, LogicEntityManager gameMap)
        {
            base.Init(charData, gameMap);
            GlobalClient.GameManager.LogicManager.playerEntity = this;
        }
    }
}

