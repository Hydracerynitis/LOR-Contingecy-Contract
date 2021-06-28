using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_BurnCostDown : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[]{"Burn_Keyword"};
        public override void OnUseCard()
        {
            if (card.target == null || card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Burn) == null || card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Burn).stack < 10)
                return;
            List<BattleDiceCardModel> list = this.owner.allyCardDetail.GetHand();
            BattleDiceCardModel Card = RandomUtil.SelectOne<BattleDiceCardModel>(list);
            Card.AddBuf(new BurnCost());
            list.Remove(Card);
            Card = RandomUtil.SelectOne<BattleDiceCardModel>(list);
            Card.AddBuf(new BurnCost());
        }
        private class BurnCost: BattleDiceCardBuf
        {
            public override int GetCost(int oldCost) => 0;
            public override void OnUseCard(BattleUnitModel owner)
            {
                base.OnUseCard(owner);
                this.Destroy();
            }
        }
    }
}
