using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1900000 : PassiveAbilityBase
    {
        public PassiveAbility_1900000()
        {
        }
        public PassiveAbility_1900000(BattleUnitModel unit)
        {
            Init(unit);
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1900000));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1900000));
            this.rare = Rarity.Unique;
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            BattleAllyCardDetail newDeck = new BattleAllyCardDetail(self);
            List<DiceCardXmlInfo> combined = owner.UnitData.unitData.GetDeckForBattle(0);
            combined.AddRange(owner.UnitData.unitData.GetDeckForBattle(1));
            combined.Sort(SortUtil.CardInfoCompByCost);
            newDeck.Init(combined);
            self.allyCardDetail = newDeck;
        }
    }
}
