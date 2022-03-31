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
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            self.bufListDetail.AddBuf(new NoEGO());
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
