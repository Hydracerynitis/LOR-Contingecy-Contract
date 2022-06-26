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
    public class DiceCardSelfAbility_BleedPlus : DiceCardSelfAbilityBase
    {

        public override void OnStartBattle() => this.owner.bufListDetail.AddBuf(new BleedPlus());

        public class BleedPlus : BattleUnitBuf
        {
            public override int OnGiveKeywordBufByCard(
              BattleUnitBuf cardBuf,
              int stack,
              BattleUnitModel target)
            {
                return cardBuf.bufType == KeywordBuf.Bleeding ? 1 : 0;
            }

            public override void OnRoundEnd() => this.Destroy();
        }
    }
}
