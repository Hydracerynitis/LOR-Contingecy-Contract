using System;
using System.Collections.Generic;
using UI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fix
{
    public class PassiveAbility_1302013_New : PassiveAbilityBase
    {
        public EileenPhase currentEileenPhase = EileenPhase.None;
        public bool beliverdeath;
        public int[] specialCards = new int[4]{ 703204, 703205, 703206, 703207 };
        private int _dmgReduction;
        public override string debugDesc => "신도가 죽을때마다 처리";
        private int GetPhaseHp()
        {
            int i = 0;
            switch (currentEileenPhase)
            {
                case EileenPhase.First:
                    i = (int)(0.7 * owner.MaxHp);
                    break;
                case EileenPhase.Second:
                    i=(int)(0.3*owner.MaxHp);
                    break;
                case EileenPhase.None:
                    i = owner.MaxHp;
                    break;
            }
            return i;
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.CheckChangePhase();
            this.beliverdeath = false;
            this.owner.breakDetail.blockRecoverBreakByEvaision = true;
        }
        public void ChangePhase(EileenPhase phase)
        {
            this.currentEileenPhase = phase;
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction);
            aliveList.Remove(this.owner);
            this.CreateBeliever(4 - aliveList.Count);
            this.beliverdeath = false;
            this.owner.breakDetail.RecoverBreakLife(this.owner.MaxBreakLife);
            this.owner.breakDetail.nextTurnBreak = false;
            this.owner.turnState = BattleUnitTurnState.WAIT_CARD;
            this.owner.breakDetail.RecoverBreak(this.owner.breakDetail.GetDefaultBreakGauge());
            this.owner.view.charAppearance.ChangeMotion(ActionDetail.Standing);
            this.DestroyNegativeBuf(this.owner.bufListDetail.GetActivatedBufList());
            this.DestroyNegativeBuf(this.owner.bufListDetail.GetReadyBufList());
            this.DestroyNegativeBuf(this.owner.bufListDetail.GetReadyReadyBufList());
            switch (this.currentEileenPhase)
            {
                case EileenPhase.Third:
                    this.owner.SetHp((int)(0.3*this.owner.MaxHp));
                    break;
                case EileenPhase.Second:
                    this.owner.SetHp((int)(0.7* this.owner.MaxHp));
                    break;
                case EileenPhase.First:
                    this.owner.SetHp(this.owner.MaxHp);
                    break;
            }
            foreach (BattleUnitModel battleUnitModel in aliveList)
            {
                battleUnitModel.RecoverHP(battleUnitModel.MaxHp);
                battleUnitModel.ResetBreakGauge();
                battleUnitModel.breakDetail.nextTurnBreak = false;
                battleUnitModel.breakDetail.RecoverBreakLife(battleUnitModel.MaxBreakLife);
                battleUnitModel.turnState = BattleUnitTurnState.WAIT_CARD;
                battleUnitModel.view.charAppearance.ChangeMotion(ActionDetail.Standing);
            }
        }
        private void DestroyNegativeBuf(List<BattleUnitBuf> bufList)
        {
            foreach (BattleUnitBuf buf in bufList)
            {
                if (buf.positiveType == BufPositiveType.Negative)
                    buf.Destroy();
            }
        }
        public override void OnDrawCard()
        {
            if (this.owner.IsBreakLifeZero() || !this.beliverdeath)
                return;
            int num = this.owner.allyCardDetail.GetHand().Count + 4 - this.owner.allyCardDetail.maxHandCount;
            if (num > 0)
                this.owner.allyCardDetail.DiscardInHand(num);
            for (int index = 0; index < this.specialCards.Length; ++index)
                this.owner.allyCardDetail.AddNewCard(this.specialCards[index]).temporary = true;
            this.beliverdeath = false;
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            if (this.CheckChangePhase() || this.owner.IsBreakLifeZero() || !this.beliverdeath)
                return;
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction);
            aliveList.Remove(this.owner);
            if (aliveList.Count != 0)
                return;
            this.beliverdeath = false;
            this.CreateBeliever(4);
        }
        public bool CheckChangePhase()
        {
            double ratio = this.owner.hp / this.owner.MaxHp;
            if (ratio <= 1.0 && ratio > 0.7 && this.currentEileenPhase != EileenPhase.First)
            {
                this.ChangePhase(EileenPhase.First);
                return true;
            }
            else if (ratio <= 0.7 && ratio>0.3 && this.currentEileenPhase != EileenPhase.Second)
            {
                this.ChangePhase(EileenPhase.Second);
                int num = this.owner.allyCardDetail.GetHand().Count+1 - this.owner.allyCardDetail.maxHandCount;
                if (num > 0)
                    this.owner.allyCardDetail.DiscardInHand(num);
                this.owner.allyCardDetail.AddNewCard(this.specialCards[3]).temporary = true;
                return true;
            }
            else if (ratio <= 0.3 && ratio > 0.0 && this.currentEileenPhase != EileenPhase.Third)
            {
                this.ChangePhase(EileenPhase.Third);
                int num = this.owner.allyCardDetail.GetHand().Count+1 - this.owner.allyCardDetail.maxHandCount;
                if (num > 0)
                    this.owner.allyCardDetail.DiscardInHand(num);
                this.owner.allyCardDetail.AddNewCard(this.specialCards[3]).temporary = true;
                return true;
            }
            return false;
        }
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            base.OnDieOtherUnit(unit);
            if (unit.UnitData.unitData.EnemyUnitId != 1302021)
                return;
            else
                this.owner.TakeBreakDamage((int)(0.25*owner.breakDetail.GetDefaultBreakGauge()), DamageType.Passive, atkResist: AtkResist.None);
            this.beliverdeath = true;
        }
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            this._dmgReduction = 0;
            int currentEileenPhase = GetPhaseHp();
            Contingecy_Contract.Debug.Log(currentEileenPhase.ToString());
            if ((double)this.owner.hp - (double)dmg <= currentEileenPhase)
                this._dmgReduction = (int)((double)currentEileenPhase - ((double)this.owner.hp - (double)dmg));
            return base.BeforeTakeDamage(attacker, dmg);
        }
        public override int GetDamageReductionAll() => (double)this.owner.hp > GetPhaseHp() ? this._dmgReduction : 100000;

        public override bool IsImmuneBreakDmg(DamageType type) => type != DamageType.Passive;
        private void CreateBeliever(int count)
        {
            count = count > 4 ? 4 : count;
            for (int index = 0; index < count; ++index)
                Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 1302021, 1 + index)?.SetDeadSceneBlock(false);
            int num = 0;
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetList())
                SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnitModel.UnitData.unitData, num++, true);
            BattleObjectManager.instance.InitUI();
        }
        public enum EileenPhase
        {
            Third = 0,
            Second = 150, // 0x00000096
            First = 350, // 0x0000015E
            None = 500, // 0x000001F4
        }
    }
}
