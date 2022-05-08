using System;
using System.Collections.Generic;
using System.Linq;
using Contingecy_Contract;
using BaseMod;

namespace ContractReward
{
    public class DiceCardSelfAbility_GainSL : DiceCardSelfAbilityBase, TagTeam
    {
        public override string[] Keywords => new string[] { "Combo_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            if(card.card.GetID() == Tools.MakeLorId(17000133))
                this.TriggerTagTeam(owner.faction, Tools.MakeLorId(17000233));
            else if (card.card.GetID() == Tools.MakeLorId(17000233))
                this.TriggerTagTeam(owner.faction, Tools.MakeLorId(17000133));
        }
        public void TagTeamEffect(BattleUnitModel TagTeamMate)
        {
            BattleUnitBuf_SoulLink link = owner.bufListDetail.FindBuf<BattleUnitBuf_SoulLink>();
            if (link == null)
                owner.bufListDetail.AddReadyBuf(new BattleUnitBuf_SoulLink(TagTeamMate) { stack = 2 });
            else
                link.concentrate = true;
        }
    }
}
