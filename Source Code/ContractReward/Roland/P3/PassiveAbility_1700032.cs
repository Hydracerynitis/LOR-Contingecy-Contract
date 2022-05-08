using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700032 : PassiveAbilityBase
    {
        public PassiveAbility_1700032()
        {
        }
        public PassiveAbility_1700032(BattleUnitModel unit)
        {
            this.owner = unit;
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1700032));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1700032));
            this.rare = Rarity.Unique;
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            if (curCard.target.bufListDetail.HasBuf<BattleUnitBuf_BlackMark>())
                curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = 2 });
        }
    }
}
