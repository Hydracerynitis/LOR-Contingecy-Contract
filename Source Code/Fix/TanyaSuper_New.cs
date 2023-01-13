using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CC = Contingecy_Contract.CCInitializer;
using UnityEngine;

namespace Fix
{
    public class BehaviourAction_TanyaSpecialAtk_New : BehaviourActionBase
    {
        private BattleUnitModel _opponent;
        private float _airbornDelay = 0.4f;
        private float _knockdownDelay = 0.3f;
        private Vector3 _defaultPos;
        private float _elapsedAirborne;
        private Vector3 _srcAirborne;
        private float _elapsedKnockdown;
        private Vector3 _srcKnockdown;

        public override List<RencounterManager.MovingAction> GetMovingAction(
          ref RencounterManager.ActionAfterBehaviour self,
          ref RencounterManager.ActionAfterBehaviour opponent)
        {
            bool flag = false;
            if (opponent.behaviourResultData != null)
                flag = opponent.behaviourResultData.IsFarAtk();
            BattleUnitModel model = self.view.model;
            if (!CC.CheckTanya(self, 1306011) && !CC.CheckTanya(self, 260011) && !CC.CheckTanya(self, Tools.MakeLorId(18600000)) || (self.result != Result.Win || flag))
                return base.GetMovingAction(ref self, ref opponent);
            self.view.unitBottomStatUI.EnableCanvas(false);
            this._self = self.view.model;
            this._opponent = opponent.view.model;
            this._defaultPos = this._opponent.view.WorldPosition;
            List<RencounterManager.MovingAction> movingActionList = new List<RencounterManager.MovingAction>();
            RencounterManager.MovingAction movingAction1 = new RencounterManager.MovingAction(ActionDetail.Move, CharMoveState.MoveOpponent_Near, delay: 0.2f);
            movingAction1.SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE);
            RencounterManager.MovingAction movingAction2 = new RencounterManager.MovingAction(ActionDetail.S1, CharMoveState.Stop, delay: this._airbornDelay);
            movingAction2.SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE);
            RencounterManager.MovingAction movingAction3 = new RencounterManager.MovingAction(ActionDetail.S2, CharMoveState.Stop, delay: this._knockdownDelay);
            movingAction3.SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE);
            RencounterManager.MovingAction movingAction4 = new RencounterManager.MovingAction(ActionDetail.S3, CharMoveState.Stop, delay: 1f);
            movingAction4.customEffectRes = "FX_Mon_Tanya_Ground";
            movingAction4.SetEffectTiming(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.PRE);
            RencounterManager.MovingAction movingAction5 = new RencounterManager.MovingAction(ActionDetail.S4, CharMoveState.Stop, delay: 0.3f);
            RencounterManager.MovingAction movingAction6 = new RencounterManager.MovingAction(ActionDetail.S5, CharMoveState.Stop, delay: 1f);
            movingAction6.customEffectRes = "FX_Mon_Tanya_Ground";
            movingAction6.SetEffectTiming(EffectTiming.PRE, EffectTiming.PRE, EffectTiming.WITHOUT_DMGTEXT);
            movingActionList.Add(movingAction1);
            movingActionList.Add(movingAction2);
            movingActionList.Add(movingAction3);
            movingActionList.Add(movingAction4);
            movingActionList.Add(movingAction5);
            movingActionList.Add(movingAction6);
            if (opponent.infoList.Count > 0)
                opponent.infoList.Clear();
            if (!opponent.view.UnStopppable)
            {
                RencounterManager.MovingAction movingAction7 = new RencounterManager.MovingAction(ActionDetail.Evade, CharMoveState.MoveBack, 1.5f, delay: 0.2f);
                RencounterManager.MovingAction movingAction8 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Custom, delay: this._airbornDelay);
                movingAction8.SetCustomMoving(new RencounterManager.MovingAction.MoveCustomEvent(this.AirbornedOpponent));
                RencounterManager.MovingAction movingAction9 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Custom, delay: this._knockdownDelay);
                movingAction9.SetCustomMoving(new RencounterManager.MovingAction.MoveCustomEvent(this.KnockdownOpponent));
                RencounterManager.MovingAction movingAction10 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.KnockDown, delay: 1f);
                RencounterManager.MovingAction movingAction11 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.KnockDown, delay: 0.3f);
                RencounterManager.MovingAction movingAction12 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.KnockDown, delay: 1f);
                opponent.infoList.Add(movingAction7);
                opponent.infoList.Add(movingAction8);
                opponent.infoList.Add(movingAction9);
                opponent.infoList.Add(movingAction10);
                opponent.infoList.Add(movingAction11);
                opponent.infoList.Add(movingAction12);
            }
            else
            {
                RencounterManager.MovingAction movingAction7 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, 1.5f, delay: 0.2f);
                RencounterManager.MovingAction movingAction8 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: this._airbornDelay);
                RencounterManager.MovingAction movingAction9 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: this._knockdownDelay);
                RencounterManager.MovingAction movingAction10 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 1f);
                RencounterManager.MovingAction movingAction11 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 0.3f);
                RencounterManager.MovingAction movingAction12 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 1f);
                opponent.infoList.Add(movingAction7);
                opponent.infoList.Add(movingAction8);
                opponent.infoList.Add(movingAction9);
                opponent.infoList.Add(movingAction10);
                opponent.infoList.Add(movingAction11);
                opponent.infoList.Add(movingAction12);
            }
            this._self.view.LockPosY(false);
            this._opponent.view.LockPosY(false);
            return movingActionList;
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
    }
    public class BehaviourAction_TwistedTanyaSuperAtk1_New : BehaviourActionBase
    {
        private BattleUnitModel _opponent;
        private float _disappearDelay = 0.5f;
        private float _attackDelay = 1.5f;
        private float _attackAfterDelay = 1f;
        private ReverberationBand_Map2 _map;
        private Vector3 _defaultPos;

        private ReverberationBand_Map2 Map
        {
            get
            {
                if (_map == null)
                    this._map = SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject as ReverberationBand_Map2;
                return this._map;
            }
        }

        public override List<RencounterManager.MovingAction> GetMovingAction(
          ref RencounterManager.ActionAfterBehaviour self,
          ref RencounterManager.ActionAfterBehaviour opponent)
        {
            bool flag = false;
            if (opponent.behaviourResultData != null)
                flag = opponent.behaviourResultData.IsFarAtk();
            BattleUnitModel model = self.view.model;
            if (!CC.CheckTanya(self,1306011) && !CC.CheckTanya(self, 260011) && !CC.CheckTanya(self, 1406011) && !CC.CheckTanya(self, Tools.MakeLorId(19600000)) && (self.result != Result.Win || flag))
                return base.GetMovingAction(ref self, ref opponent);
            self.view.unitBottomStatUI.EnableCanvas(false);
            this._self = self.view.model;
            this._opponent = opponent.view.model;
            this._defaultPos = this._opponent.view.WorldPosition;
            List<RencounterManager.MovingAction> movingAction1 = new List<RencounterManager.MovingAction>();
            RencounterManager.MovingAction movingAction2 = new RencounterManager.MovingAction(ActionDetail.Default, CharMoveState.Custom, delay: this._disappearDelay);
            movingAction2.SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE);
            movingAction2.SetCustomMoving(new RencounterManager.MovingAction.MoveCustomEventWithElapsed(this.Disappear));
            RencounterManager.MovingAction movingAction3 = new RencounterManager.MovingAction(ActionDetail.S2, CharMoveState.MoveOpponent_Near, delay: this._attackDelay, speed: 15f);
            movingAction3.SetEffectTiming(EffectTiming.NONE, EffectTiming.NONE, EffectTiming.NONE);
            RencounterManager.MovingAction movingAction4 = new RencounterManager.MovingAction(ActionDetail.S3, CharMoveState.Custom, delay: this._attackAfterDelay);
            movingAction4.customEffectRes = "FX_Mon_Tanya_Ground";
            movingAction4.SetEffectTiming(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.PRE);
            movingAction2.SetCustomMoving(new RencounterManager.MovingAction.MoveCustomEventWithElapsed(this.Appear));
            movingAction1.Add(movingAction2);
            movingAction1.Add(movingAction3);
            movingAction1.Add(movingAction4);
            if (opponent.infoList.Count > 0)
                opponent.infoList.Clear();
            RencounterManager.MovingAction movingAction5 = new RencounterManager.MovingAction(ActionDetail.Default, CharMoveState.Stop, 1.5f, delay: this._disappearDelay);
            RencounterManager.MovingAction movingAction6 = new RencounterManager.MovingAction(ActionDetail.Default, CharMoveState.Stop, delay: this._attackDelay);
            RencounterManager.MovingAction movingAction7 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: this._attackAfterDelay);
            opponent.infoList.Add(movingAction5);
            opponent.infoList.Add(movingAction6);
            opponent.infoList.Add(movingAction7);
            this._self.view.LockPosY(false);
            this._opponent.view.LockPosY(false);
            return movingAction1;
        }

        private bool Disappear(float deltaTime, float elapedTime)
        {
            if ((double)elapedTime < 0.5)
                return false;
            this.Map?.TanyaSuperDisappear();
            return true;
        }

        private bool Appear(float deltaTime, float elapedTime)
        {
            if ((double)elapedTime >= (double)Mathf.Epsilon)
                return true;
            this.Map?.TanyaSuperAppear();
            CameraFilterUtil.EarthQuake(0.12f, 0.1f, 90f, 0.3f);
            return false;
        }
    }
    public class BehaviourAction_TwistedTanyaSuperAtk2_New : BehaviourActionBase
    {
        private BattleUnitModel _opponent;
        private float _airbornDelay = 0.4f;
        private float _knockbackDelay = 0.65f;
        private Vector3 _defaultPos;
        private float _elapsedAirborne;
        private Vector3 _srcAirborne;

        public override List<RencounterManager.MovingAction> GetMovingAction(
          ref RencounterManager.ActionAfterBehaviour self,
          ref RencounterManager.ActionAfterBehaviour opponent)
        {
            bool flag = false;
            if (opponent.behaviourResultData != null)
                flag = opponent.behaviourResultData.IsFarAtk();
            BattleUnitModel model = self.view.model;
            if (!CC.CheckTanya(self, 1306011) && !CC.CheckTanya(self, 260011) && !CC.CheckTanya(self, 1406011) && !CC.CheckTanya(self, Tools.MakeLorId(19600000)) && ( self.result != Result.Win || flag))
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
            RencounterManager.MovingAction movingAction4 = new RencounterManager.MovingAction(ActionDetail.S4, CharMoveState.Custom, delay: this._knockbackDelay);
            movingAction4.customEffectRes = "FX_Mon_Tanya2_Punch";
            movingAction4.SetEffectTiming(EffectTiming.PRE, EffectTiming.NONE, EffectTiming.PRE);
            movingAction4.SetCustomMoving(new RencounterManager.MovingAction.MoveCustomEventWithElapsed(this.KnockbackOpponent));
            movingAction1.Add(movingAction2);
            movingAction1.Add(movingAction3);
            movingAction1.Add(movingAction4);
            if (opponent.infoList.Count > 0)
                opponent.infoList.Clear();
            if (!opponent.view.UnStopppable)
            {
                RencounterManager.MovingAction movingAction5 = new RencounterManager.MovingAction(ActionDetail.Evade, CharMoveState.MoveBack, 1.5f, delay: 0.2f);
                RencounterManager.MovingAction movingAction6 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Custom, delay: this._airbornDelay);
                movingAction6.SetCustomMoving(new RencounterManager.MovingAction.MoveCustomEvent(this.AirbornedOpponent));
                RencounterManager.MovingAction movingAction7 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Knockback, 3f, delay: this._knockbackDelay);
                opponent.infoList.Add(movingAction5);
                opponent.infoList.Add(movingAction6);
                opponent.infoList.Add(movingAction7);
            }
            else
            {
                RencounterManager.MovingAction movingAction8 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, 1.5f, delay: 0.2f);
                RencounterManager.MovingAction movingAction9 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: this._airbornDelay);
                RencounterManager.MovingAction movingAction10 = new RencounterManager.MovingAction(ActionDetail.Damaged, CharMoveState.Stop, delay: 1f);
                opponent.infoList.Add(movingAction8);
                opponent.infoList.Add(movingAction9);
                opponent.infoList.Add(movingAction10);
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

        private bool KnockbackOpponent(float deltaTime, float elapedTime)
        {
            if ((double)elapedTime < (double)Mathf.Epsilon)
                CameraFilterUtil.EarthQuake(0.18f, 0.16f, 90f, 0.45f);
            return true;
        }
    }
}
