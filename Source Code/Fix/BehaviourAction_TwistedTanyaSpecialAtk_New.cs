using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;

namespace Fix
{
    public class BehaviourAction_TwistedTanyaSpecialAtk_New : BehaviourActionBase
    {
        private BattleUnitModel _opponent;
        private float _airbornDelay = 0.4f;
        private float _knockdownDelay = 0.3f;
        private Vector3 _defaultPos;
        private float _elapsedAirborne;
        private Vector3 _srcAirborne;
        private float _elapsedKnockdown;
        private Vector3 _srcKnockdown;
        private Vector3 _dstKnockdown;

        public override List<RencounterManager.MovingAction> GetMovingAction(
          ref RencounterManager.ActionAfterBehaviour self,
          ref RencounterManager.ActionAfterBehaviour opponent)
        {
            bool flag = false;
            if (opponent.behaviourResultData != null)
                flag = opponent.behaviourResultData.IsFarAtk();
            BattleUnitModel model = self.view.model;
            if (!CheckTanya(self,1306011) && !CheckTanya(self,260011) && !CheckTanya(self,1406011) && !CheckTanya(self,Tools.MakeLorId(19600000)) || self.result != Result.Win || flag)
                return base.GetMovingAction(ref self, ref opponent);
            self.view.unitBottomStatUI.EnableCanvas(false);
            this._self = self.view.model;
            this._opponent = opponent.view.model;
            this._defaultPos = this._opponent.view.WorldPosition;
            List<RencounterManager.MovingAction> movingAction1 = new List<RencounterManager.MovingAction>();
            RencounterManager.MovingAction movingAction2 = new RencounterManager.MovingAction(ActionDetail.Move, CharMoveState.MoveOpponent_Near, delay: 0.2f);
            movingAction2.SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE);
            RencounterManager.MovingAction movingAction3 = new RencounterManager.MovingAction(ActionDetail.S1, CharMoveState.Stop, delay: this._airbornDelay);
            movingAction3.SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE);
            RencounterManager.MovingAction movingAction4 = new RencounterManager.MovingAction(ActionDetail.S2, CharMoveState.Stop, delay: this._knockdownDelay);
            movingAction4.SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE);
            RencounterManager.MovingAction movingAction5 = new RencounterManager.MovingAction(ActionDetail.S3, CharMoveState.Stop, delay: 1f);
            movingAction5.customEffectRes = "FX_Mon_Tanya_Ground";
            movingAction5.SetEffectTiming(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.PRE);
            movingAction1.Add(movingAction2);
            movingAction1.Add(movingAction3);
            movingAction1.Add(movingAction4);
            movingAction1.Add(movingAction5);
            if (opponent.infoList.Count > 0)
                opponent.infoList.Clear();
            if (!opponent.view.UnStopppable)
            {
                RencounterManager.MovingAction movingAction6 = new RencounterManager.MovingAction(ActionDetail.Evade, CharMoveState.MoveBack, 1.5f, delay: 0.2f);
                RencounterManager.MovingAction movingAction7 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Custom, delay: this._airbornDelay);
                movingAction7.SetCustomMoving(new RencounterManager.MovingAction.MoveCustomEvent(this.AirbornedOpponent));
                RencounterManager.MovingAction movingAction8 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Custom, delay: this._knockdownDelay);
                movingAction8.SetCustomMoving(new RencounterManager.MovingAction.MoveCustomEvent(this.KnockdownOpponent));
                RencounterManager.MovingAction movingAction9 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.KnockDown, delay: 1f);
                opponent.infoList.Add(movingAction6);
                opponent.infoList.Add(movingAction7);
                opponent.infoList.Add(movingAction8);
                opponent.infoList.Add(movingAction9);
            }
            else
            {
                RencounterManager.MovingAction movingAction10 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, 1.5f, delay: 0.2f);
                RencounterManager.MovingAction movingAction11 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: this._airbornDelay);
                RencounterManager.MovingAction movingAction12 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: this._knockdownDelay);
                RencounterManager.MovingAction movingAction13 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 1f);
                opponent.infoList.Add(movingAction10);
                opponent.infoList.Add(movingAction11);
                opponent.infoList.Add(movingAction12);
                opponent.infoList.Add(movingAction13);
            }
            this._self.view.LockPosY(false);
            this._opponent.view.LockPosY(false);
            return movingAction1;
        }

        private bool AirbornedOpponent(float deltaTime)
        {
            if ((double)this._elapsedAirborne < (double)Mathf.Epsilon)
            {
                this._srcAirborne = this._opponent.view.WorldPosition;
                this._opponent.view.WorldPosition = this._srcAirborne + new Vector3(0.0f, 1f, 0.0f);
            }
            this._elapsedAirborne += deltaTime / this._airbornDelay;
            if ((double)this._elapsedAirborne < 1.0)
                return false;
            this._elapsedAirborne = 0.0f;
            return true;
        }

        private bool KnockdownOpponent(float deltaTime)
        {
            if ((double)this._elapsedKnockdown < (double)Mathf.Epsilon)
            {
                this._srcKnockdown = this._opponent.view.WorldPosition;
                this._srcKnockdown.y = 0.0f;
                this._opponent.view.WorldPosition = this._srcKnockdown;
                this._opponent.view.KnockDown(true);
                this._self.view.LockPosY(true);
                this._opponent.view.LockPosY(true);
                this._elapsedKnockdown = 0.0f;
            }
            return true;
        }
        private bool CheckTanya(RencounterManager.ActionAfterBehaviour self, int id)
        {
            return CheckTanya(self, new LorId(id));
        }
        private bool CheckTanya(RencounterManager.ActionAfterBehaviour self, LorId id)
        {
            return self.view.model.Book.GetBookClassInfoId() == id || self.view.model.customBook.ClassInfo.id == id;
        }
    }
}
