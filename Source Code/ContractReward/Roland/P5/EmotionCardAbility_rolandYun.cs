using BaseMod;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class EmotionCardAbility_rolandYun : EmotionCardAbilityBase
    {
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            base.OnDieOtherUnit(unit);
            if (unit.faction != _owner.faction)
                return;
            _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, _owner);
        }
        public override int GetDamageReduction(BattleDiceBehavior behavior)
        {
            return 2;
        }
    }
}
