using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1510003 : PassiveAbilityBase
    {
        public override void OnStartBattle()
        {
            BattleDiceCardModel playingCard = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(15100011)));
            if (playingCard == null)
                return;
            BattleDiceBehavior defenseDice = playingCard.CreateDiceCardBehaviorList()[0];
            int unusedSpeedDice=owner.cardSlotDetail.cardAry.FindAll(x => x==null).Count();
            for (int i = 0; i < 1+unusedSpeedDice*2; i++)
            {
                owner.cardSlotDetail.keepCard.AddBehaviourForOnlyDefense(playingCard, defenseDice);
            }
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            if (behavior.Detail==BehaviourDetail.Guard)
            {
                Focus.AddStack(owner, 1);
            }
        }
    }
}
