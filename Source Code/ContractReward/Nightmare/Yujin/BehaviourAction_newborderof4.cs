using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class BehaviourAction_newborderof4 : BehaviourActionBase
    {
        public override List<RencounterManager.MovingAction> GetMovingAction(
          ref RencounterManager.ActionAfterBehaviour self,
          ref RencounterManager.ActionAfterBehaviour opponent)
        {
            List<RencounterManager.MovingAction> movingAction1 = new List<RencounterManager.MovingAction>();
            int vanillaDiceValue = self.behaviourResultData.vanillaDiceValue;
            int resultDiceMax = self.behaviourResultData.resultDiceMax;
            bool yujinTrigger = vanillaDiceValue >= 50;
            bool enemyFar = false;
            if (opponent.behaviourResultData != null)
                enemyFar = opponent.behaviourResultData.IsFarAtk();
            if (self.result == Result.Win && self.data.actionType == ActionType.Atk && yujinTrigger && !enemyFar)
            {
                RencounterManager.MovingAction movingAction2 = new RencounterManager.MovingAction(ActionDetail.S1, CharMoveState.MoveForward, 30f, false, 0.0f);
                movingAction2.SetEffectTiming(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.NONE);
                movingAction2.customEffectRes = "BorderOfDeath";
                movingAction1.Add(movingAction2);
                RencounterManager.MovingAction movingAction3 = new RencounterManager.MovingAction(ActionDetail.S1, CharMoveState.MoveForward, 0.2f, false, 1f);
                movingAction1.Add(movingAction3);
                RencounterManager.MovingAction movingAction4 = new RencounterManager.MovingAction(ActionDetail.S2, CharMoveState.Stop, updateDir: false, delay: 1.5f);
                movingAction4.SetEffectTiming(EffectTiming.NONE, EffectTiming.PRE, EffectTiming.PRE);
                movingAction1.Add(movingAction4);
                opponent.infoList.Add(new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, updateDir: false, delay: 0.0f));
                opponent.infoList.Add(new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, updateDir: false, delay: 0.0f));
                opponent.infoList.Add(new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, updateDir: false, delay: 1f));
            }
            else
                movingAction1 = base.GetMovingAction(ref self, ref opponent);
            return movingAction1;
        }
    }
}
