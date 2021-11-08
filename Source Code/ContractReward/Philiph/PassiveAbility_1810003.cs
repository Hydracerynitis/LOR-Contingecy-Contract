using BaseMod;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1810003: PassiveAbilityBase
    {
        public PassiveAbility_1810003()
        {
        }
        public PassiveAbility_1810003(BattleUnitModel unit)
        {
            this.owner = unit;
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1810003));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1810003));
            this.rare = Rarity.Unique;
        }
        public override void OnRoundStartAfter()
        {
            base.OnRoundStartAfter();
            BattleUnitBuf_burnDown battleUnitBufBurnDown = new BattleUnitBuf_burnDown();
            battleUnitBufBurnDown.stack = 1;
            this.owner.bufListDetail.AddBuf(battleUnitBufBurnDown);
        }
        public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
        {
            if (detail == BehaviourDetail.Penetrate)
                return AtkResist.Endure;
            return base.GetResistHP(origin, detail);
        }
        public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
        {
            if (detail == BehaviourDetail.Penetrate)
                return AtkResist.Endure;
            return base.GetResistHP(origin, detail);
        }
    }
}
