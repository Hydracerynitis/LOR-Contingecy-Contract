using System;
using System.Collections.Generic;
using System.Linq;
using Contingecy_Contract;
using BaseMod;

namespace ContractReward
{
    public class DiceCardSelfAbility_RolandAura : DiceCardSelfAbilityBase, TagTeam
    {
        public override string[] Keywords => new string[] { "Combo_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            this.TriggerTagTeam(owner.faction, Tools.MakeLorId(17000232));
        }
        public void TagTeamEffect(BattleUnitModel TagTeamMate)
        {
            owner.allyCardDetail.DrawCards(2);
            TagTeamMate.allyCardDetail.DrawCards(2);
        }
    }
    public class DiceCardSelfAbility_AngelicaAura : DiceCardSelfAbilityBase, TagTeam
    {
        public override string[] Keywords => new string[] { "Combo_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            this.TriggerTagTeam(owner.faction, Tools.MakeLorId(17000132));
        }
        public void TagTeamEffect(BattleUnitModel TagTeamMate)
        {
            owner.cardSlotDetail.RecoverPlayPoint(3);
            TagTeamMate.cardSlotDetail.RecoverPlayPoint(3);
        }
    }
}
