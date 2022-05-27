using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700141 : PassiveAbilityBase
    {
        public PassiveAbility_1700141(BattleUnitModel owner)
        {
            name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1700141));
            desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1700141));
            owner.bufListDetail.AddBuf(new EmotionCardAbility_heart.BattleUnitBuf_Emotion_Heart_Eager());
            Init(owner);
        }
        public override int GetMaxHpBonus() => (int)(owner.UnitData.unitData.MaxHp * 0.15);

        public override int GetSpeedDiceAdder(int speedDiceResult) => RandomUtil.Range(1, 2);
    }
}
