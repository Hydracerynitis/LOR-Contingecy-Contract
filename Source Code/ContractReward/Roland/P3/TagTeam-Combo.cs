using System;
using System.Collections.Generic;
using System.Linq;
using Contingecy_Contract;
using BaseMod;

namespace ContractReward
{
    public class DiceCardSelfAbility_RolandCombo: DiceCardSelfAbility_rolandCombo, TagTeam
    {
        public override string[] Keywords => new string[] { "Combo_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            this.TriggerTagTeam(owner.faction, Tools.MakeLorId(17000231));
        }
        public void TagTeamEffect(BattleUnitModel TagTeamMate)
        {
            card.ApplyDiceStatBonus(x => x.index==1,new DiceStatBonus() { min=2,max=2});
        }
    }
    public class DiceCardSelfAbility_AngelicaCombo : DiceCardSelfAbility_rolandCombo, TagTeam
    {
        public override string[] Keywords => new string[] { "Combo_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            this.TriggerTagTeam(owner.faction, Tools.MakeLorId(17000131));
        }
        public void TagTeamEffect(BattleUnitModel TagTeamMate)
        {
            card.ApplyDiceStatBonus(x => x.index == 0, new DiceStatBonus() { max=5});
        }
    }
}
