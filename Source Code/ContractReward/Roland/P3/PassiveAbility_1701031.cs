using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1701031 : PassiveAbilityBase
    {
        BattleUnitModel last;
        public PassiveAbility_1701031()
        {
        }
        public PassiveAbility_1701031(BattleUnitModel unit)
        {
            this.owner = unit;
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1701031));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1701031));
            this.rare = Rarity.Unique;
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            last = curCard.target;
        }
        public override void OnRoundEnd()
        {
            if(last != null)
                last.bufListDetail.AddReadyBuf(new BattleUnitBuf_BlackMark());
            last = null;
        }
    }
}
