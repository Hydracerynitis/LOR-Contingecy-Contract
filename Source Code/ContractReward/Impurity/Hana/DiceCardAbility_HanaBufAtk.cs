using BaseMod;
using Contingecy_Contract;
using ContractReward;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardAbility_HanaBufAtk : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            this.owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 2, this.owner);
            if(owner.bufListDetail.HasBuf<BattleUnitBuf_hana2>())
                owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Endurance, 3, this.owner);
        }
    }
}
