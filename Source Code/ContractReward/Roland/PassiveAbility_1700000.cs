using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700000 : PassiveAbilityBase
    {
        public PassiveAbility_1700000()
        {
        }
        public PassiveAbility_1700000(BattleUnitModel unit)
        {
            Init(unit);
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1700002));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1700002));
            this.rare = Rarity.Unique;
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            self.bufListDetail.AddBuf(new NoEGO());
        }
        public override int GetEmotionCoinAdder(int defaultCount)
        {
            return 1;
        }
        private class NoEGO : BattleUnitBuf
        {
            public override bool IsCardChoosable(BattleDiceCardModel card)
            {
                return !card.XmlData.optionList.Contains(CardOption.EGO);
            }
        }
    }
}
