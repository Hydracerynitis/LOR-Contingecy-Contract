using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ContingecyContract_DPhilip_Helios : ContingecyContract
    {
        public ContingecyContract_DPhilip_Helios(int level)
        {
            this.Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId== 1401011;
        }
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() { hpRate=100,breakRate=100 };
        }
        public override void OnRoundEnd_before()
        {
            base.OnRoundEnd_before();
            owner.RecoverHP(owner.bufListDetail.GetKewordBufStack(KeywordBuf.Burn));
        }
    }
    public class ContingecyContract_DPhilip_Restraint : ContingecyContract
    {
        private int count = 0;
        public ContingecyContract_DPhilip_Restraint(int level)
        {
            this.Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1401011;
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            count++;
        }
        public override void OnEndBattle(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnEndBattle(curCard);
            if (count >= 2)
                curCard.target.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Arrest, 1);
            count = 0;
        }
    }
    public class ContingecyContract_DEileen_Smoke : ContingecyContract
    {
        public ContingecyContract_DEileen_Smoke(int level)
        {
            this.Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1402011;
        }
        public override void OnRoundStartAfter()
        {
            base.OnRoundStartAfter();
            owner.allyCardDetail.ExhaustAllCards();
            if (owner.bufListDetail.GetKewordBufStack(KeywordBuf.Smoke) < 9)
                owner.allyCardDetail.AddNewCard(707204).SetPriorityAdder(100);
            if(BattleObjectManager.instance.GetAliveList_opponent(owner.faction).Exists(x => x.bufListDetail.GetKewordBufStack(KeywordBuf.Smoke)>=5))
                owner.allyCardDetail.AddNewCard(707205).SetPriorityAdder(100);
            owner.allyCardDetail.AddNewCard(RandomUtil.SelectOne(707201, 707202)).SetPriorityAdder(90);
            int num = owner.emotionDetail.SpeedDiceNumAdder() + owner.emotionDetail.GetSpeedDiceAdder(0);
            if (num <= 0)
                return;
            for (int index = 0; index < num; ++index)
                owner.allyCardDetail.AddNewCard(RandomUtil.SelectOne(707201, 707202,707203)).SetCostToZero();

        }
        public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
        {
            List<BattleUnitModel> smoker = BattleObjectManager.instance.GetAliveList_opponent(owner.faction).FindAll(x => x.bufListDetail.GetKewordBufStack(KeywordBuf.Smoke) >= 5);
            if ((card.GetID()==707203 || card.GetID() == 707205) && smoker.Count > 0)
                return RandomUtil.SelectOne(smoker);
            return base.ChangeAttackTarget(card, idx);
        }
    }
    public class ContingecyContract_DEileen_DeusEx : ContingecyContract
    {
        private PassiveAbility_1402014 passive;
        public ContingecyContract_DEileen_DeusEx(int level)
        {
            this.Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1402011;
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            passive=(PassiveAbility_1402014) self.passiveDetail.PassiveList.Find(x => x is PassiveAbility_1402014);
        }
        public override void OnRoundStartAfter()
        {
            if (passive.roundcounter == 4)
                passive.roundcounter++;
        }
        public override void OnDrawCard()
        {
            base.OnDrawCard();
            if (owner.allyCardDetail.GetHand().Find(x => x.GetID() == 707206) is BattleDiceCardModel card)
                DeepCopyUtil.EnhanceCard(card, 0, 6);
        }
    }
    public class ContingecyContract_DGreta_Prot : ContingecyContract
    {
        public ContingecyContract_DGreta_Prot(int level)
        {
            this.Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1403011;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
                if (unit != owner)
                {
                    unit.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 4);
                    unit.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.BreakProtection, 4);
                }
        }
    }
    public class ContingecyContract_DGreta_Eat : ContingecyContract
    {
        public ContingecyContract_DGreta_Eat(int level)
        {
            this.Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1403011;
        }
        public override void OnRoundStartAfter()
        {
            base.OnRoundStartAfter();
            BattleDiceCardModel tackle = owner.allyCardDetail.GetHand().Find(x => x.GetID() == 707305);
            /*if (StageController.Instance.RoundTurn % 2 == 0 && owner.emotionDetail.EmotionLevel < 4)
                owner.allyCardDetail.ExhaustACard(tackle);*/
            tackle.SetPriorityAdder(4);
            DeepCopyUtil.EnhanceCard(new int[] { 1, 2, 3 }, tackle, 3, 3);
        }
    }
    public class ContingecyContract_DBremen_Self : ContingecyContract
    {
        public ContingecyContract_DBremen_Self(int level)
        {
            this.Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1404011;
        }
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            return new StatBonus() {breakGageAdder=90 };
        }
    }
    public class ContingecyContract_DBremen_Other : ContingecyContract
    {
        public ContingecyContract_DBremen_Other(int level)
        {
            this.Level = level;
        }
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == 1404011;
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (curCard.target.faction == owner.faction)
            {
                BattleObjectManager.instance.GetAliveList(owner.faction).ForEach(x => x.breakDetail.RecoverBreak(30));
            }
        }
    }
}
