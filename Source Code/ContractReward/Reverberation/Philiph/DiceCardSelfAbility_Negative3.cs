using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_Negative3 : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            owner.emotionDetail.CreateEmotionCoin(EmotionCoinType.Negative, 3);
        }
    }
}
