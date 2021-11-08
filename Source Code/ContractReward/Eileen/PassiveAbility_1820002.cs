using System;
using System.Collections.Generic;
using SummonLiberation;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class PassiveAbility_1820002 : PassiveAbilityBase
    {
        public override void OnStartBattle()
        {
            BattleDiceCardModel playingCard = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(18200005)));
            if (playingCard == null)
                return;
            foreach (BattleDiceBehavior diceCardBehavior in playingCard.CreateDiceCardBehaviorList())
                this.owner.cardSlotDetail.keepCard.AddBehaviourForOnlyDefense(playingCard, diceCardBehavior);
        }
    }
}
