using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1510008 : PassiveAbilityBase
    {
        public override void OnStartBattle()
        {
            BattleDiceCardModel playingCard = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(15100013)));
            if (playingCard == null)
                return;
            foreach (BattleDiceBehavior diceCardBehavior in playingCard.CreateDiceCardBehaviorList())
                this.owner.cardSlotDetail.keepCard.AddBehaviourForOnlyDefense(playingCard, diceCardBehavior);
        }
    }
}
