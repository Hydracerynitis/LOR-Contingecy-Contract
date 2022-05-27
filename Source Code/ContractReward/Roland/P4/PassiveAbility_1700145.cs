using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700145 : PassiveAbilityBase
    {
        public PassiveAbility_1700145(BattleUnitModel owner)
        {
            name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1700145));
            desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1700145));
            owner.bufListDetail.AddBuf(new Indicator() { stack = 0 });
            Init(owner);
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            owner.passiveDetail.DestroyPassive(this);
        }
        private class Indicator: BattleUnitBuf
        {
            public override string keywordIconId => "SnowQueen_Stun";
            public override string keywordId => "BlizzardIndicator";
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }

    }
}
