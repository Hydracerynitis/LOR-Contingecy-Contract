using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using BaseMod;

namespace Contingecy_Contract
{
    public class ContingecyContract_DOswald_Friend : ContingecyContract
    {
        public ContingecyContract_DOswald_Friend(int level)
        {
            Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId.GreaterEqual(1405011) && EnemyId.LesserEqual(1405041);
        }
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            if (owner.UnitData.unitData.EnemyUnitId == 1405011)
                return base.GetStatBonus(owner);
            else
                return new StatBonus() { hpAdder = 100, breakGageAdder = 50 };
        }
    }
    public class ContingecyContract_DOswald_Show : ContingecyContract
    {
        public ContingecyContract_DOswald_Show(int level)
        {
            Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1405011;
        }
    }
    public class ContingecyContract_DTanya_Warrior : ContingecyContract
    {
        private Queue<int> Priority = new Queue<int>();
        EnemyTeamStageManager_TwistedReverberationBand_Middle _stage;
        public ContingecyContract_DTanya_Warrior(int level)
        {
            Level = level;
            _stage = Singleton<StageController>.Instance.EnemyStageManager as EnemyTeamStageManager_TwistedReverberationBand_Middle;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1406011;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            behavior.card.target.TakeDamage(RandomUtil.SelectOne(3, 5));
        }
        public override void OnRoundStartAfter()
        {
            owner.allyCardDetail.ExhaustAllCards();
            Priority.Clear();
            for (int i = 100; i > 40; i -= 10)
                Priority.Enqueue(i);
            int random = RandomUtil.Range(0, 1);
            if (IsOswaldAlive())
            {
                if (IsJaeheonAlive())
                    SetCard_OJ(random);
                else
                    SetCard_O(random);
            }
            else
            {
                if (IsJaeheonAlive())
                    SetCard_J(random);
                else
                    SetCard_None();
            }
        }
        public void SetCard_OJ(int random)
        {
            switch (_stage.PatternCount())
            {
                case 0:
                    owner.allyCardDetail.AddNewCard(707611).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707603).SetPriorityAdder(Priority.Dequeue());
                    if (random == 0)
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    else
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707604).SetPriorityAdder(Priority.Dequeue());
                    if (random == 0)
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    else
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    break;
                case 1:
                case 2:
                    owner.allyCardDetail.AddNewCard(707611).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707604).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707612).SetPriorityAdder(Priority.Dequeue());
                    if (random == 0)
                    {
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    }
                    else
                    {
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    }
                    break;
                case 3:
                    owner.allyCardDetail.AddNewCard(707613).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707603).SetPriorityAdder(Priority.Dequeue());        
                    if (random == 0)
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    else
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707611).SetPriorityAdder(Priority.Dequeue());
                    if (random == 0)
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    else
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    break;
            }
        }
        public void SetCard_O(int random)
        {
            switch (_stage.PatternCount())
            {
                case 0:
                    owner.allyCardDetail.AddNewCard(707611).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707603).SetPriorityAdder(Priority.Dequeue());
                    if (random == 0)
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    else
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707604).SetPriorityAdder(Priority.Dequeue());
                    if (random == 0)
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    else
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    break;
                case 1:
                case 2:
                    owner.allyCardDetail.AddNewCard(707604).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707611).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    if (random == 0)
                    {
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    }
                    else
                    {
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    }
                    break;
                case 3:
                    owner.allyCardDetail.AddNewCard(707613).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707603).SetPriorityAdder(Priority.Dequeue());
                    if (random == 0)
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    else
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707611).SetPriorityAdder(Priority.Dequeue());
                    if (random == 0)
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    else
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    break;
            }
        }
        public void SetCard_J(int random)
        {
            switch (_stage.PatternCount())
            {
                case 0:
                    owner.allyCardDetail.AddNewCard(707611).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707603).SetPriorityAdder(Priority.Dequeue());
                    if (random == 0)
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    else
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707604).SetPriorityAdder(Priority.Dequeue());
                    if (random == 0)
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    else
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    break;
                case 1:
                case 2:
                    owner.allyCardDetail.AddNewCard(707604).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707603).SetPriorityAdder(Priority.Dequeue());
                    if (random == 0)
                    {
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                        owner.allyCardDetail.AddNewCard(707611).SetPriorityAdder(Priority.Dequeue());
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    }
                    else
                    {
                        owner.allyCardDetail.AddNewCard(707611).SetPriorityAdder(Priority.Dequeue());
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    }
                    break;
                case 3:
                    owner.allyCardDetail.AddNewCard(707613).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707603).SetPriorityAdder(Priority.Dequeue());
                    if (random == 0)
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    else
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707611).SetPriorityAdder(Priority.Dequeue());
                    if (random == 0)
                        owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    else
                        owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    break;
            }
        }
        public void SetCard_None()
        {
            owner.allyCardDetail.AddNewCard(707613).SetPriorityAdder(Priority.Dequeue());
            switch (_stage.PatternCount()) 
            {
                case 0:
                case 1:
                    owner.allyCardDetail.AddNewCard(707603).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707604).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707611).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    break;
                case 2:
                case 3:
                    owner.allyCardDetail.AddNewCard(707612).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707602).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707611).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707604).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    owner.allyCardDetail.AddNewCard(707601).SetPriorityAdder(Priority.Dequeue());
                    break;
            }
        }
        private bool IsJaeheonAlive()
        {
            return BattleObjectManager.instance.GetAliveList(owner.faction).Exists(x => x.UnitData.unitData.EnemyUnitId == 1407011);
        }
        private bool IsOswaldAlive() => _stage!=null && _stage.IsOswaldAlive();
    }
    public class ContingecyContract_DTanya_Sand: ContingecyContract
    {
        public ContingecyContract_DTanya_Sand(int level)
        {
            Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1406011;
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            DiceBehaviour standby = new DiceBehaviour() { Min = 5, Dice = 8, Type = BehaviourType.Standby, Detail = BehaviourDetail.Guard, MotionDetail = MotionDetail.G };
            List<BattleDiceBehavior> standbyList = new List<BattleDiceBehavior>();
            for (int i = 0; i < 3; i++)
                standbyList.Add(new BattleDiceBehavior() { behaviourInCard = standby });
            owner.cardSlotDetail.keepCard.AddBehaviours(ItemXmlDataList.instance.GetCardItem(707612), standbyList);

        }
    }
    public class ContingecyContract_DJaeheon_Control : ContingecyContract
    {
        public ContingecyContract_DJaeheon_Control(int level)
        {
            Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1407011;
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                unit.bufListDetail.AddBuf(new TempControl());
        }
        public override void OnDie()
        {
            base.OnDie();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                unit.bufListDetail.FindBuf<BattleUnitBuf_Jaeheon_String>()?.Destroy();
                unit.bufListDetail.FindBuf<TempControl>()?.Destroy();
            }
                
        }
        class TempControl: BattleUnitBuf
        {
            public override void OnRoundStartAfter()
            {
                if (!_owner.bufListDetail.HasBuf<BattleUnitBuf_Oswald_String>())
                {
                    if (_owner.bufListDetail.FindBuf<BattleUnitBuf_Jaeheon_String>() is BattleUnitBuf_Jaeheon_String JS)
                        JS.Add();
                    else
                    {
                        JS = new BattleUnitBuf_Jaeheon_String();
                        _owner.bufListDetail.AddBuf(JS);
                        JS.Add();
                    }
                }
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if(_owner.bufListDetail.FindBuf<BattleUnitBuf_Jaeheon_String>() is BattleUnitBuf_Jaeheon_String JS)
                {
                    if (JS.stack == 1)
                        JS.Destroy();
                    else
                        JS.Reduce();
                }
            }
            public override int GetCardCostAdder(BattleDiceCardModel card)
            {
                return card.GetID()== 707710 || card.GetID()== 707711 ? 2: 0;
            }
        }
    }
    public class ContingecyContract_DJaeheon_Tangle : ContingecyContract
    {
        public ContingecyContract_DJaeheon_Tangle(int level)
        {
            Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1407011;
        }
        public override void OnDrawCard()
        {
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                BattleDiceCardModel Redstring = unit.allyCardDetail.GetHand().Find(x => x.GetID() == 707712);
                if (Redstring == null)
                    continue;
                DeepCopyUtil.EnhanceCard(Redstring, -5, -5);
                Redstring.AddCost(2);
            }
        }
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() { hpRate = 100, breakRate = 100 };
        }
    }
}
