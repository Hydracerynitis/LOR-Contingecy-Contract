using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700042 : PassiveAbilityBase
    {
        public override float DmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None) => type == DamageType.Buf ? 0.5f : base.DmgFactor(dmg, type, keyword);
        public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
        {
            return AtkResist.Endure;
        }
        public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
        {
            return AtkResist.Endure;
        }
    }
}
