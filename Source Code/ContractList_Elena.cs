using System;
using System.IO;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using ContractReward;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ContingecyContract_Elena_Cross : ContingecyContract
    {
        private bool ExtraHit;
        public ContingecyContract_Elena_Cross(int level)
        {
            Level = level - 1;
        }
        public override ContractType Type => ContractType.Special;
        public override string[] GetFormatParam => new string[] { Level.ToString(),((Level - 1)*25).ToString() };
        private List<BattlePlayingCardDataInUnitModel> extrahit=new List<BattlePlayingCardDataInUnitModel>();
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(Faction.Player))
            {
                if (unit.bufListDetail.GetActivatedBufList().Find(x => x is CrossBurn) == null)
                    unit.bufListDetail.AddBuf(new CrossBurn(Level));
            }
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            ExtraHit = false;
            if (extrahit.Contains(curCard))
            {
                curCard.ApplyDiceStatBonus(DiceMatch.AllAttackDice, new DiceStatBonus() { dmgRate = (Level - 1) * 25 });
                ExtraHit = true;
            }
            extrahit.Remove(curCard);
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (ExtraHit)
                return;
            CrossBurn crossburn = (CrossBurn) behavior.card.target.bufListDetail.GetActivatedBufList().Find(x => x is CrossBurn);
            if (crossburn == null)
            {
                crossburn = new CrossBurn(Level);
                behavior.card.target.bufListDetail.AddBuf(crossburn);
            }
            if (crossburn.stack >= 1)
            {
                BattlePlayingCardDataInUnitModel attack = new BattlePlayingCardDataInUnitModel() { owner = this.owner, card = behavior.card.card,cardAbility= behavior.card.card.CreateDiceCardSelfAbilityScript()};
                if (attack.cardAbility != null)
                    attack.cardAbility.card = attack;
                attack.ResetCardQueue();
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction == Faction.Player ? Faction.Enemy : Faction.Player);
                if (aliveList.Count > 0)
                    Singleton<StageController>.Instance.AddAllCardListInBattle(attack, RandomUtil.SelectOne<BattleUnitModel>(aliveList));
                extrahit.Add(attack);
                crossburn.stack -= 1;
            }
            crossburn.Hit += 1;
            if (crossburn.Hit == 3)
            {
                crossburn.Hit = 0;
                crossburn.stack += Level;
            }
        }
        public class CrossBurn : BattleUnitBuf
        {
            private int Level;
            public int Hit;
            protected override string keywordId => "CrossBurn";
            protected override string keywordIconId => "Wolf_Scar";
            public override int paramInBufDesc => (Level - 1) * 25;
            public CrossBurn(int level)
            {
                Level = level;
                Hit = 0;
                this.stack = 0;
            }
        }
    }
    public class ContingecyContract_Elena_Zombie : ContingecyContract
    {
        public ContingecyContract_Elena_Zombie(int level)
        {
            Level = level - 1;
        }
        public override ContractType Type => ContractType.Special;
        public override string[] GetFormatParam => new string[] { ( 15+ 5*Level).ToString(), (Level * 20).ToString() };
        public override int SpeedDiceNumAdder() => IsElena? this.GetVictimList().Count: 0;
        private bool IsElena => this.owner.UnitData.unitData.EnemyUnitId == 1308011;
        private List<BattleUnitModel> GetVictimList()
        {
            List<BattleUnitModel> list = new List<BattleUnitModel>();
            if (!IsElena)
                return list;
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(Faction.Player))
            {
                if (unit.hp < unit.MaxHp * (0.15 + Level * 0.05))
                    list.Add(unit);
            }
            return list;
        }
        public override void OnAfterRollSpeedDice()
        {
            base.OnAfterRollSpeedDice();
            List<BattleUnitModel> victim = this.GetVictimList();
            for(int i = this.owner.speedDiceResult.Count-1; victim.Count > 0; i--)
            {
                this.owner.SetCurrentOrder(i);
                this.owner.speedDiceResult[i].isControlable = false;
                BattleDiceCardModel card = this.owner.allyCardDetail.AddTempCard(18800008);
                BattleUnitModel target = victim[0];
                if (target != null)
                {
                    int targetSlot = UnityEngine.Random.Range(0, target.speedDiceResult.Count);
                    this.owner.cardSlotDetail.AddCard(card, target, targetSlot);
                }
                victim.RemoveAt(0);
            }
        }
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            if(IsElena)
                return new StatBonus() { hpRate=Level*20};
            return base.GetStatBonus(owner);
        }
        public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
        {
            return RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction).FindAll(x => !x.passiveDetail.PassiveList.Exists(y => y is Zombie)));
        }
    }
    public class ContingecyContract_Elena : ContingecyContract
    {
        private Queue<int> Priority;
        public ContingecyContract_Elena(int level)
        {
            this.Level = level;
        }
        public override int SpeedDiceNumAdder() => Cross==null? 1:0;
        public override ContractType Type => ContractType.Special;
        private PassiveAbility_1308021 Cross => (PassiveAbility_1308021) this.owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_1308021);
        public override void OnRoundStartAfter()
        {
            int round = Singleton<StageController>.Instance.RoundTurn;
            Priority = new Queue<int>();
            for (int x = 90; x > 0; x -= 10)
                Priority.Enqueue(x);
            this.owner.allyCardDetail.ExhaustAllCards();
            if (Cross==null)
                AddCard_Elena(round);
            else
                AddCard_Cross(round);
            base.OnRoundStart();
        }
        private void AddCard_Elena(int round)
        {

            if (round % 2 != 0 && BattleObjectManager.instance.GetAliveList().Find(x => x.UnitData.unitData.EnemyUnitId==1308021)!=null)
                this.owner.allyCardDetail.AddNewCard(703803).SetPriorityAdder(Priority.Dequeue());
            else
                this.owner.allyCardDetail.AddNewCard(703801).SetPriorityAdder(Priority.Dequeue());
            if (round % 3 == 2)
                this.owner.allyCardDetail.AddNewCard(703805).SetPriorityAdder(Priority.Dequeue());
            else
                this.owner.allyCardDetail.AddNewCard(703802).SetPriorityAdder(Priority.Dequeue());
            for (; this.owner.allyCardDetail.GetHand().Count < this.owner.Book.GetSpeedDiceRule(this.owner).diceNum;)
                this.owner.allyCardDetail.AddNewCard(703801).SetPriorityAdder(Priority.Dequeue());
            this.owner.allyCardDetail.AddNewCard(703804).SetPriorityAdder(Priority.Dequeue());
            this.owner.allyCardDetail.AddNewCard(703804).SetPriorityAdder(Priority.Dequeue());
            foreach (BattleDiceCardModel card in this.owner.allyCardDetail.GetHand())
                card.SetCostToZero();
        }
        private void AddCard_Cross(int round)
        {
            int slow = (int) typeof(PassiveAbility_1308021).GetField("_speedDiceValuAdder",AccessTools.all).GetValue(Cross);
            slow = 0;
            if (round % 2 == 0 && BattleObjectManager.instance.GetAliveList().Find(x => x.UnitData.unitData.EnemyUnitId == 1308011) != null)
                this.owner.allyCardDetail.AddNewCard(703815).SetPriorityAdder(Priority.Dequeue());
            else
                this.owner.allyCardDetail.AddNewCard(703811).SetPriorityAdder(Priority.Dequeue());
            if (round % 3 == 0)
            {
                this.owner.allyCardDetail.AddNewCard(703814).SetPriorityAdder(Priority.Dequeue());
                slow = -5;
            }
            else
                this.owner.allyCardDetail.AddNewCard(703811).SetPriorityAdder(Priority.Dequeue());
            this.owner.allyCardDetail.AddNewCard(703812).SetPriorityAdder(Priority.Dequeue());
            this.owner.allyCardDetail.AddNewCard(703813).SetPriorityAdder(Priority.Dequeue());
            for (; this.owner.allyCardDetail.GetHand().Count < this.owner.Book.GetSpeedDiceRule(this.owner).diceNum;)
                this.owner.allyCardDetail.AddNewCard(703813).SetPriorityAdder(Priority.Dequeue());
            typeof(PassiveAbility_1308021).GetField("_speedDiceValuAdder",AccessTools.all).SetValue(Cross, slow);
        }
    }
}