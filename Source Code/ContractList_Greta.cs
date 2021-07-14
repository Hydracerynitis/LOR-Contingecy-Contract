using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ContingecyContract_Greta_Salt : ContingecyContract
    {
        public ContingecyContract_Greta_Salt(int Level)
        {
            this.Level = Level;
            Salted = new Dictionary<BattleUnitModel, int>();
        }
        private Dictionary<BattleUnitModel, int> Salted;
        public override ContractType Type => ContractType.Special;
        public override string[] GetFormatParam => new string[] {Level.ToString(),Level.ToString() };
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            behavior.card.target.bufListDetail.AddKeywordBufByEtc(RandomUtil.SelectOne<KeywordBuf>(KeywordBuf.Paralysis, KeywordBuf.Vulnerable, KeywordBuf.Binding), 1);
        }
        public override int OnGiveKeywordBufByCard(BattleUnitBuf buf, int stack, BattleUnitModel target)
        {
            return buf.bufType == KeywordBuf.Bleeding ? Level : 0;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            foreach(BattleUnitModel unit in Salted.Keys)
            {
                unit.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak,Level*Salted[unit]);
                unit.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Disarm, Level * Salted[unit]);
            }
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            if (curCard.card.GetID() == 703319)
            {
                if (!Salted.ContainsKey(curCard.target))
                    Salted.Add(curCard.target, 1);
                else
                    Salted[curCard.target] += 1;
            }
            base.OnUseCard(curCard);
        }
        public override void OnMakeBreakState(BattleUnitModel target)
        {
            base.OnMakeBreakState(target);
            if (!Salted.ContainsKey(target))
                Salted.Add(target, 1);
            else
                Salted[target] += 1;
        }
    }
    public class ContingecyContract_Greta_Feast : ContingecyContract
    {
        public ContingecyContract_Greta_Feast(int Level)
        {
            this.Level = Level;
        }
        public override ContractType Type => ContractType.Special;
        public override string[] GetFormatParam => new string[] { (Level-1).ToString(),(50*(int)(Math.Pow(2,Level-1))).ToString()};
        public int GetSackNumAdder() => BattleObjectManager.instance.GetAliveList(owner.faction).Count == 1 ? Level - 1 : 0;
    }
    public class ContingecyContract_Greta: ContingecyContract
    {
        public ContingecyContract_Greta(int Level)
        {
            this.Level = Level;
        }
        public override ContractType Type => ContractType.Special;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.owner.bufListDetail.RemoveBufAll(KeywordBuf.Resistance);
            this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Resistance,100);
        }
    }
}
