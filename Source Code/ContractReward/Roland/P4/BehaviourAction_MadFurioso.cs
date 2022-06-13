using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class BehaviourAction_MadFurioso : BehaviourActionBase
    {
        public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
        {
            this._self = self;
            BattleDiceBehavior dice = self.currentDiceAction.currentBehavior;
            FarAreaEffect_MadFurioso silence4thAreaAdd = new GameObject().AddComponent<FarAreaEffect_MadFurioso>();
            silence4thAreaAdd.Init(self);
            if (dice != null)
            {
                silence4thAreaAdd.action = MotionConverter.MotionToAction(dice.behaviourInCard.MotionDetail);
                silence4thAreaAdd.effect = dice.behaviourInCard.EffectRes;
            }
            return (FarAreaEffect)silence4thAreaAdd;
        }
    }
}
