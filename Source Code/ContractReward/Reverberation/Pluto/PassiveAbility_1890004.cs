using System;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class PassiveAbility_1890004 : PassiveAbilityBase
    {
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (owner.Book.GetCardListFromCurrentDeck().Exists(x => x.id == curCard.card.GetID()))
                return;
            curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = RandomUtil.Range(1, 2) });
        }
    }
}
