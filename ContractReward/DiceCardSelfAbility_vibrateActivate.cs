using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_vibrateActivate : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            BattleUnitBuf activatedBuf = this.card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Vibrate);
            if (activatedBuf == null || this.card.speedDiceResultValue != activatedBuf.stack)
                return;
            this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
            {
                power = 4
            });
        }
    }
}
