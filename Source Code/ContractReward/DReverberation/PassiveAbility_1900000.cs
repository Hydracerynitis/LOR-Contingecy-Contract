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
            List<DiceCardXmlInfo> combined = owner.UnitData.unitData.GetCardList(0);
            combined.AddRange(owner.UnitData.unitData.GetCardList(1));
            if (owner.Book.ClassInfo.RangeType != EquipRangeType.Range && combined.Count < 9)
            {
                int[] numArray = new int[5] { 2, 2, 2, 2, 2 };
                foreach (DiceCardXmlInfo diceCardXmlInfo in combined)
                {
                    if (diceCardXmlInfo.id.IsBasic() && diceCardXmlInfo.id.id >= 1 && diceCardXmlInfo.id.id <= 5)
                        --numArray[diceCardXmlInfo.id.id - 1];
                }
                for (int count = combined.Count; count < 9; ++count)
                {
                    for (int id = 1; id <= 5; ++id)
                    {
                        if (numArray[id - 1] > 0)
                        {
                            --numArray[id - 1];
                            combined.Add(ItemXmlDataList.instance.GetCardItem(id));
                            break;
                        }
                    }
                }
            }
            combined.Sort(SortUtil.CardInfoCompByCost);
            newDeck.Init(combined);
            self.allyCardDetail = newDeck;
        }
    }
}
