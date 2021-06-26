using System;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ContingecyContract_Eileen_Production : ContingecyContract
    {
        public ContingecyContract_Eileen_Production(int level)
        {
            Level = level - 1;
        }
        public override ContractType Type => ContractType.Special;
        public override string[] GetFormatParam => new string[] { (25 + Level * 25).ToString(), GetParam() };
        private string GetParam()
        {
            string s = "";
            if (Level >= 1)
                s += TextDataModel.GetText("Eileen_Production_param1");
            if (Level >= 2)
                s += TextDataModel.GetText("Eileen_Production_param2");
            if(Level >=3)
                s += TextDataModel.GetText("Eileen_Production_param3");
            return s;
        }
        private bool isEileen => owner.UnitData.unitData.EnemyUnitId == 1302011;
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            if (isEileen)
                return new StatBonus() { hpRate = 25 + Level * 25 };
            return base.GetStatBonus(owner);
        }
        public override int SpeedDiceNumAdder() => isEileen || Level <1? 0:1;
        public override int MaxPlayPointAdder() => isEileen || Level<1? 0:2;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (isEileen || Level < 1)
                return;
            this.owner.cardSlotDetail.RecoverPlayPoint(MaxPlayPointAdder());
            if (Level < 2)
                return;
            this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Smoke, 2);
            if (Level < 3)
                return;
            this.owner.cardSlotDetail.RecoverPlayPoint(2);
            this.owner.allyCardDetail.DrawCards(2);
        }
    }
    public class ContingecyContract_Eileen_BodyGuard: ContingecyContract
    {
        public ContingecyContract_Eileen_BodyGuard(int level)
        {
            Level = level - 1;
        }
        public override ContractType Type => ContractType.Special;
        public override string[] GetFormatParam => new string[] { (25 + Level * 25).ToString(), (Math.Max(0, -3 + 3 * Level)).ToString() };
        private bool isEileen => owner.UnitData.unitData.EnemyUnitId == 1302011;
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            if (!isEileen)
                return new StatBonus() { hpRate = 25 + Level * 25, breakRate= 25 + Level * 25 };
            return base.GetStatBonus(owner);
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (curCard.card.GetID() != 703207)
                return;
            curCard.ApplyDiceStatBonus(DiceMatch.DiceByIdx(1), new DiceStatBonus() { max = Math.Max(0, -3 + 3 * Level) });
        }
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (!isEileen || BattleObjectManager.instance.GetAliveList(this.owner.faction).FindAll(x => x!= owner).Count<=0)
                return false;
            RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(this.owner.faction).FindAll(x => x != owner)).TakeDamage(dmg);
            return true;
        }
    }
    public class ContingecyContract_Eileen : ContingecyContract
    {
        public ContingecyContract_Eileen(int level)
        {
            Level = level;
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            List<DiceCardXmlInfo> Decklist = new List<DiceCardXmlInfo>();
            foreach (int i in Singleton<DeckXmlList>.Instance.GetData(18220000).cardIdList)
                Decklist.Add(ItemXmlDataList.instance.GetCardItem(i));
            self.allyCardDetail.Init(Decklist);
            self.allyCardDetail.DrawCards(8);
        }
    }
}
