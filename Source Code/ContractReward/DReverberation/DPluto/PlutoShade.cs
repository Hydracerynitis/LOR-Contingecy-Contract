using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class DiceCardSelfAbility_PlutoShade1 : DiceCardSelfAbility_energy1
    {
        public override void OnApplyCard()
        {
            base.OnApplyCard();
            owner.personalEgoDetail.RemoveCard(Tools.MakeLorId(19900103));
        }
        public override void OnReleaseCard()
        {
            owner.personalEgoDetail.AddCard(Tools.MakeLorId(19900103));
        }
    }
    public class DiceCardSelfAbility_PlutoShade2 : DiceCardSelfAbility_powerUp1thisRound
    {
        public override void OnApplyCard()
        {
            base.OnApplyCard();
            owner.personalEgoDetail.RemoveCard(Tools.MakeLorId(19900102));
        }
        public override void OnReleaseCard()
        {
            owner.personalEgoDetail.AddCard(Tools.MakeLorId(19900102));
        }
    }
}
