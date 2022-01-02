using LOR_DiceSystem;
using Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_Oswald_daze : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        { 
            int defaultBreakGauge = targetUnit.breakDetail.GetDefaultBreakGauge();
            targetUnit.breakDetail.TakeBreakDamage(defaultBreakGauge, DamageType.Card_Ability, this.owner, AtkResist.None);
            targetUnit.bufListDetail.AddReadyBuf(new LibrarianDaze());
            targetUnit.view.speedDiceSetterUI.DeselectAll();
            targetUnit.view.speedDiceSetterUI.BreakDiceAll(true);
            targetUnit.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
            PrintSound();
        }
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return !BattleObjectManager.instance.GetAliveList(owner.faction).Exists(x => x.bufListDetail.HasBuf<LibrarianDaze>());
        }
        public override bool IsOnlyAllyUnit()
        {
            return true;
        }
        private void PrintSound() => SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Battle/Oswald_Attract");
        public class LibrarianDaze : BattleUnitBuf
        {
            private bool _bRecoveredBreak;
            public override bool IsControllable => false;
            protected override string keywordId => "LibrarianDaze";
            protected override string keywordIconId => "Oswald_Daze";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 2;
                _bRecoveredBreak = false;
            }
            public override void OnRoundStart()
            {
                if (!this._bRecoveredBreak)
                {
                    this._owner.breakDetail.RecoverBreakLife(this._owner.MaxBreakLife);
                    this._owner.breakDetail.nextTurnBreak = false;
                    this._owner.turnState = BattleUnitTurnState.WAIT_CARD;
                    this._owner.breakDetail.RecoverBreak(this._owner.breakDetail.GetDefaultBreakGauge());
                    this._bRecoveredBreak = true;
                    this._owner.view.charAppearance.ChangeMotion(ActionDetail.Standing);
                }
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2, this._owner);
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 2, this._owner);
                this._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 3, this._owner);
                this._owner.cardSlotDetail.RecoverPlayPoint(3);
            }
            public override bool IsCardChoosable(BattleDiceCardModel card) => (card == null || card.GetSpec().Ranged != CardRange.FarArea) && card.GetSpec().Ranged != CardRange.FarAreaEach && base.IsCardChoosable(card);
            public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int currentSlot)
            {
                BattleUnitModel battleUnitModel = null;
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList_opponent(_owner.faction);
                aliveList.Remove(_owner);
                if (aliveList.Count > 0)
                    battleUnitModel = aliveList[UnityEngine.Random.Range(0, aliveList.Count)];
                return battleUnitModel;
            }
            public override void OnRoundEnd()
            {
                --this.stack;
                if (this._owner.IsBreakLifeZero())
                {
                    foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction))
                    {
                        if (alive.passiveDetail.HasPassive<PassiveAbility_1850002>())
                            alive.TakeBreakDamage(30, DamageType.Buf);
                    }
                    this.Destroy();
                    this._owner.breakDetail.RecoverBreakLife(this._owner.MaxBreakLife);
                    this._owner.breakDetail.nextTurnBreak = false;
                    this._owner.turnState = BattleUnitTurnState.WAIT_CARD;
                    this._owner.breakDetail.RecoverBreak(this._owner.breakDetail.GetDefaultBreakGauge());
                }
                else
                {
                    if (this.stack > 0)
                        return;
                    this.Destroy();
                }
            }
            public override void OnLoseParrying(BattleDiceBehavior behavior)
            {
                base.OnLoseParrying(behavior);
                this._owner.TakeBreakDamage(this._owner.breakDetail.GetDefaultBreakGauge() / 20, DamageType.Buf);
            }
        }
    }
}
