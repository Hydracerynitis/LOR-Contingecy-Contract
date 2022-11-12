using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using BaseMod;

namespace ContractReward
{
    public class DiceCardSelfAbility_NoChance: DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            if (owner.bufListDetail.GetActivatedBufList().Exists(x => x is Indicator))
                return;
            owner.bufListDetail.AddBuf(new Indicator());
            int num = StageController.Instance._allCardList.FindAll(x => x.target == owner).Count;
            BattleDiceCardModel playingCard = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(19700003)));
            if (playingCard == null)
                return;
            for (int i=0; i < num; i++)
            {
                owner.cardSlotDetail.keepCard.AddBehaviourForOnlyDefense(playingCard, playingCard.CreateDiceCardBehaviorList()[0]);
            }
        }
        class Indicator: BattleUnitBuf
        {
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}
