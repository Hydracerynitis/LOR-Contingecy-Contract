using System;
using System.Collections.Generic;
using LOR_DiceSystem;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1830001 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Resistance, 5);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (Singleton<StageController>.Instance.RoundTurn % 5 == 1)
            {
                owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Resistance, 5);
            }
        }
    }
}
