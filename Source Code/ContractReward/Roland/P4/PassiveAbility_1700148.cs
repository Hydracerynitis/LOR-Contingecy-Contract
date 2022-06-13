using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700148 : PassiveAbilityBase
    {
        public PassiveAbility_1700148(BattleUnitModel owner)
        {
            name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1700148));
            desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1700148));
            owner.bufListDetail.AddBuf(new Indicator() { stack=0});
            Init(owner);
        }
        private class Indicator: BattleUnitBuf
        {
            public override string keywordIconId => "SnowSword";
            public override string keywordId => "FrostIndicator";
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            owner.passiveDetail.DestroyPassive(this);
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (behavior.card.speedDiceResultValue > behavior.card.target.speedDiceResult[behavior.card.targetSlotOrder].value)
            {
                int dmg = RandomUtil.Range(2, 4);
                behavior.card.target.TakeDamage(dmg);
                behavior.card.target.TakeBreakDamage(dmg);
            }
        }
    }
}
