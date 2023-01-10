using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOR_DiceSystem;
using HarmonyLib;
using Contingecy_Contract;
using BaseMod;

namespace ContractReward
{
    public class PassiveAbility_1900003 : PassiveAbilityBase, Resonator
    {
        public int count;
        private BattleDiceCardModel model;

        public void ActiveResonate(BattlePlayingCardDataInUnitModel card)
        {
            count += 1;
            if (count == 3)
            {
                Upgrade();
                count = 0;
            }
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            DiceCardXmlInfo xml = ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(19000009));
            model = BattleDiceCardModel.CreatePlayingCard(xml);
            this.owner.allyCardDetail.AddCardToHand(model);
            count = 0;
        }
        public void Upgrade()
        {
            DeepCopyUtil.EnhanceCard(model, 1, 1);
        }
    }
}
