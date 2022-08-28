using System;
using System.Collections.Generic;
using Contingecy_Contract;
using UnityEngine;

namespace Fix
{
    public class PassiveAbility_1405011_New : PassiveAbilityBase
    {
        private int _cardCount;
        private bool _summonhideUsed;
        private bool _summonUsed;
        private ReverberationBand_Map2 _map;
        private EnemyTeamStageManager_TwistedReverberationBand_Middle _stage;
        private int BreakDmg => isContractFriend? RandomUtil.Range(2, 4) :  RandomUtil.Range(4, 8);
        private bool isContractFriend => ContractLoader.Instance.GetPassiveList().Exists(x => x.Type == "DOswald_Friend");
        private int MinDmg=400;
        private ReverberationBand_Map2 Map
        {
            get
            {
                if (_map == null)
                    _map = BattleSceneRoot.Instance.currentMapObject as ReverberationBand_Map2;
                return _map;
            }
        }
        private EnemyTeamStageManager_TwistedReverberationBand_Middle Stage
        {
            get
            {
                if (_stage == null)
                    _stage = StageController.Instance.EnemyStageManager as EnemyTeamStageManager_TwistedReverberationBand_Middle;
                return _stage;
            }
        }    
        public override void OnRoundStartAfter()
        {
            owner.allyCardDetail.ExhaustAllCards();
            if (owner.IsBreakLifeZero())
                return;
            SetCards();
        }
        public override int GetMinHp()
        {
            return MinDmg-1;
        }
        private void SetCards()
        {
            _cardCount = 0;
            int num = 0;
            if (Stage != null)
                num = Stage.PatternCount();
            switch (num)
            {
                case 0:
                    if (IsJaeheonAlive())
                        AddNewCard(707501);
                    else
                        AddNewCard(707502);
                    AddNewCard(707503);
                    AddNewCard(707504);
                    break;
                case 1:
                case 2:
                    if (IsTanyaAlive())
                    {
                        AddNewCard(707505);
                        AddNewCard(707504);
                    }
                    else
                    {
                        AddNewCard(707506);
                        AddNewCard(707506);
                    }
                    AddNewCard(707508);
                    AddNewCard(707507);
                    break;
                case 3:
                    if (IsJaeheonAlive())
                    {
                        AddNewCard(707509);
                        AddNewCard(707505);
                        AddNewCard(707504);
                        break;
                    }
                    AddNewCard(707509);
                    AddNewCard(707505);
                    AddNewCard(707506);
                    AddNewCard(707504);
                    break;
            }
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            _summonUsed = false;
            _summonhideUsed = false;
            if(isContractFriend)
                MinDmg = (int)owner.hp - owner.MaxHp / 2;
            else
                MinDmg = (int)owner.hp - owner.MaxHp / 4;
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (curCard?.card.GetID() == 707501)
            {
                _summonhideUsed = true;
            }
            else
            {
                if (!(curCard?.card.GetID() == 707502))
                    return;
                _summonUsed = true;
            }
        }
        private void AddNewCard(int id)
        {
            owner.allyCardDetail.AddTempCard(id).SetPriorityAdder(1000 - _cardCount * 100);
            ++_cardCount;
        }
        public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
        {
            BattleUnitModel battleUnitModel = base.ChangeAttackTarget(card, idx);
            int num = -1;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
            {
                if (alive.speedDiceResult.Count > 0 && alive.IsTargetable(owner))
                {
                    int breakGauge = alive.breakDetail.breakGauge;
                    if (breakGauge < num || num < 0)
                    {
                        battleUnitModel = alive;
                        num = breakGauge;
                    }
                }
            }
            return battleUnitModel;
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                alive.TakeBreakDamage(BreakDmg, DamageType.Passive, owner);
        }
        private bool IsTanyaAlive()
        {
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (alive.UnitData.unitData.EnemyUnitId == 1406011)
                    return true;
            }
            return false;
        }
        private bool IsJaeheonAlive()
        {
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (alive.UnitData.unitData.EnemyUnitId == 1407011)
                    return true;
            }
            return false;
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            if (_summonUsed)
            {
                Stage?.AddSpecialChild();
            }
            else
            {
                if (!_summonhideUsed)
                    return;
                if (isContractFriend)
                {
                    BattleUnitModel Bunny= SummonLiberation.Harmony_Patch.SummonUnit(Faction.Enemy, new LorId(1405041), new LorId(1405041), 5);
                    Bunny.formation.ChangePos(new Vector2Int(5, 15));
                }       
                Stage?.AddChilds(Owner);
            }
        }
        public override void OnDie()
        {
            base.OnDie();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(owner.faction == Faction.Enemy ? Faction.Player : Faction.Enemy))
                alive.bufListDetail.AddBuf(new BattleUnitBuf_Oswald_Killed());
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                alive.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Oswald_String)?.Destroy();
                alive.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Oswald_String_Ready)?.Destroy();
            }
            Stage?.OnOswaldDie();
        }
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            base.OnDieOtherUnit(unit);
            if (unit.faction != owner.faction || owner.IsExtinction() || owner.breakDetail.IsBreakLifeZero())
                return;
            owner.TakeBreakDamage(50, DamageType.Passive, owner);
        }
        public override int SpeedDiceNumAdder()
        {
            int num1 = 0;
            int num2 = owner.emotionDetail.SpeedDiceNumAdder();
            if (num2 > 0)
                num1 = -num2;
            return num1;
        }
    }
}