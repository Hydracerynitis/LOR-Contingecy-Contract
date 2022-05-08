using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;
using LOR_XML;

namespace Fix
{
    public class FarAreaEffect_MadFurioso : FarAreaEffect
    {
        public ActionDetail action= ActionDetail.Slash2;
        public string effect= "BS4DurandalDown_J2";
        private float _endDelay = 0.175f;
        private float _moveSpeed = 4.2f;
        private float _atkDelay = 0.175f;
        private List<BattleFarAreaPlayManager.VictimInfo> _victimList;
        private float _elapsedStart;
        private float _elapsedGiveDamage;
        private float _elapsedEndAction;
        private Vector3 _srcPosAtkOneTarget;
        private Vector3 _dstPosAtkOneTarget;
        private Vector3 _offset_Start = new Vector3(12f, 0.0f, 0.0f);
        private Vector3 _offset_End = new Vector3(-12f, 0.0f, 0.0f);
        private BattleFarAreaPlayManager.VictimInfo _currentVictim;

        public override bool HasIndependentAction => true;

        public override void Init(BattleUnitModel self, params object[] args)
        {
            base.Init(self, args);
            this.state = EffectState.Start;
            this._elapsedStart = 0.0f;
            this._elapsedGiveDamage = 0.0f;
            this._elapsedEndAction = 0.0f;
            this._dstPosAtkOneTarget = Vector3.zero;
            this._srcPosAtkOneTarget = Vector3.zero;
            this.isRunning = false;
            this._currentVictim = null;
            Singleton<BattleFarAreaPlayManager>.Instance.SetActionDelay(0.0f, 0.0f);
            Singleton<BattleFarAreaPlayManager>.Instance.SetUIDelay(0.0f);
            Singleton<BattleFarAreaPlayManager>.Instance.SetRollDiceDelay(0.01f);
            Singleton<BattleFarAreaPlayManager>.Instance.SetPrintRollDiceDelay(0.0f);
        }

        public override bool ActionPhase(
          float deltaTime,
          BattleUnitModel attacker,
          List<BattleFarAreaPlayManager.VictimInfo> victims,
          ref List<BattleFarAreaPlayManager.VictimInfo> defenseVictims)
        {
            bool flag = false;
            if (this.state == FarAreaEffect.EffectState.Start)
            {
                this._elapsedStart += deltaTime;
                if ((double)this._elapsedStart > 0.05)
                {
                    this._elapsedStart = 0.0f;
                    this.state = FarAreaEffect.EffectState.GiveDamage;
                    this._victimList = new List<BattleFarAreaPlayManager.VictimInfo>((IEnumerable<BattleFarAreaPlayManager.VictimInfo>)victims);
                }
            }
            else if (this.state == FarAreaEffect.EffectState.GiveDamage)
            {
                if ((double)this._elapsedGiveDamage < (double)Mathf.Epsilon)
                {
                    List<BattleFarAreaPlayManager.VictimInfo> victimList = this._victimList;
                    if (victimList.Count > 0 && this._victimList.Exists(x => !x.unitModel.IsDead()))
                    {
                        List<BattleFarAreaPlayManager.VictimInfo> victimInfoList = new List<BattleFarAreaPlayManager.VictimInfo>();
                        foreach (BattleFarAreaPlayManager.VictimInfo victim in this._victimList)
                        {
                            if (!victim.unitModel.IsDead())
                                victimInfoList.Add(victim);
                        }
                        BattleFarAreaPlayManager.VictimInfo victimInfo = victimInfoList[UnityEngine.Random.Range(0, victimInfoList.Count)];
                        int num = (double)UnityEngine.Random.Range(0.0f, 1f) > 0.5 ? 1 : -1;
                        attacker.view.WorldPosition = victimInfo.unitModel.view.WorldPosition + this._offset_Start * (float)num;
                        attacker.UpdateDirection(victimInfo.unitModel.view.WorldPosition);
                        this._srcPosAtkOneTarget = victimInfo.unitModel.view.WorldPosition + this._offset_Start * (float)num;
                        this._dstPosAtkOneTarget = victimInfo.unitModel.view.WorldPosition + this._offset_End * (float)num;
                        victimInfo.unitModel.UpdateDirection(attacker.view.WorldPosition);
                        this._victimList.Remove(victimInfo);
                        List<BattleUnitModel> battleUnitModelList = new List<BattleUnitModel>() { attacker, victimInfo.unitModel };
                        this._currentVictim = victimInfo;
                    }
                }
                this._elapsedGiveDamage += deltaTime;
                if (this._currentVictim != null && (double)this._elapsedGiveDamage > (double)this._atkDelay)
                {
                    if (this._currentVictim.playingCard?.currentBehavior != null)
                    {
                        if (attacker.currentDiceAction.currentBehavior.DiceResultValue > this._currentVictim.playingCard.currentBehavior.DiceResultValue)
                        {
                            attacker.currentDiceAction.currentBehavior.GiveDamage(this._currentVictim.unitModel);
                            if (this._currentVictim.unitModel.IsDead())
                                this._currentVictim.unitModel.view.DisplayDlg(DialogType.DEATH, new List<BattleUnitModel>() { _self });
                            this._currentVictim.unitModel.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
                            this._currentVictim.destroyedDicesIndex.Add(this._currentVictim.playingCard.currentBehavior.Index);
                        }
                        else
                        {
                            this._currentVictim.unitModel.view.charAppearance.ChangeMotion(ActionDetail.Guard);
                            this._currentVictim.unitModel.UpdateDirection(attacker.view.WorldPosition);
                            if (!defenseVictims.Contains(this._currentVictim))
                                defenseVictims.Add(this._currentVictim);
                        }
                    }
                    else
                    {
                        attacker.currentDiceAction.currentBehavior.GiveDamage(this._currentVictim.unitModel);
                        if (this._currentVictim.unitModel.IsDead())
                            this._currentVictim.unitModel.view.DisplayDlg(DialogType.DEATH, new List<BattleUnitModel>(){ _self });
                        this._currentVictim.unitModel.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
                    }
                    ActionDetail actionDetail = action;
                    SingletonBehavior<DiceEffectManager>.Instance.CreateBehaviourEffect(effect, 1f, this._self.view, this._currentVictim.unitModel.view);
                    attacker.view.charAppearance.ChangeMotion(actionDetail);
                    this._self.view.charAppearance.soundInfo.PlaySound(MotionConverter.ActionToMotion(actionDetail), true);
                    double X = (double)UnityEngine.Random.Range(0.04f, 0.08f);
                    float num1 = UnityEngine.Random.Range(0.04f, 0.08f);
                    float num2 = UnityEngine.Random.Range(70f, 90f);
                    float num3 = UnityEngine.Random.Range(0.1f, 0.15f);
                    double Y = (double)num1;
                    double speed = (double)num2;
                    double time = (double)num3;
                    CameraFilterUtil.EarthQuake((float)X, (float)Y, (float)speed, (float)time);
                    SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(this._currentVictim.unitModel, this._currentVictim.unitModel.faction, this._currentVictim.unitModel.hp, this._currentVictim.unitModel.breakDetail.breakGauge);
                    SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(attacker, attacker.faction, attacker.hp, attacker.breakDetail.breakGauge);
                    this._currentVictim = (BattleFarAreaPlayManager.VictimInfo)null;
                }
                if ((double)Vector3.SqrMagnitude(this._dstPosAtkOneTarget - this._srcPosAtkOneTarget) > (double)Mathf.Epsilon && (double)this._elapsedGiveDamage > (double)this._atkDelay)
                    attacker.view.WorldPosition = Vector3.Lerp(this._srcPosAtkOneTarget, this._dstPosAtkOneTarget, this._elapsedGiveDamage * this._moveSpeed);
                if ((double)this._elapsedGiveDamage > (double)this._endDelay)
                {
                    this._elapsedGiveDamage = 0.0f;
                    this._srcPosAtkOneTarget = Vector3.zero;
                    this._dstPosAtkOneTarget = Vector3.zero;
                    if (this._victimList == null || this._victimList.Count == 0)
                        this.state = FarAreaEffect.EffectState.End;
                    else if (!this._victimList.Exists((Predicate<BattleFarAreaPlayManager.VictimInfo>)(x => !x.unitModel.IsDead())))
                    {
                        this._victimList.Clear();
                        this.state = FarAreaEffect.EffectState.End;
                    }
                }
            }
            else if (this.state == FarAreaEffect.EffectState.End)
            {
                this._elapsedEndAction += deltaTime;
                if ((double)this._elapsedEndAction > 0.00999999977648258)
                {
                    this._self.view.charAppearance.ChangeMotion(ActionDetail.Default);
                    this.state = FarAreaEffect.EffectState.None;
                    this._elapsedEndAction = 0.0f;
                    this._isDoneEffect = true;
                    UnityEngine.Object.Destroy((UnityEngine.Object)this.gameObject);
                }
            }
            else if (this._self.moveDetail.isArrived)
                flag = true;
            return flag;
        }
    }
}
