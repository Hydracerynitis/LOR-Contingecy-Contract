using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1510005 : PassiveAbilityBase
    {
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            if (curCard.GetOriginalDiceBehaviorList().FindAll((Predicate<DiceBehaviour>)(x => x.Type != BehaviourType.Standby)).Count != 1)
                return;
            this.owner.battleCardResultLog?.SetPassiveAbility((PassiveAbilityBase)this);
            curCard.emotionMultiplier = 2;
            curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
            {
                power = 2
            });
            curCard.ApplyDiceAbility(DiceMatch.AllDice, new DiceCardAbility_FocusClash());
        }
    }
}
