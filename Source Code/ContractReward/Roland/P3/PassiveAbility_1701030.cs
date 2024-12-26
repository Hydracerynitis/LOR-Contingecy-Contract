using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1701030 : PassiveAbilityBase
    {
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1701030));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1701030));
            this.rare = Rarity.Unique;
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            if (curCard.target.hp<=GetMinHp())
                curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = 1 });
        }
        private int GetMinHP()
        {
            float minHp = 9999f;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                if ((double)alive.hp < (double)minHp)
                    minHp = alive.hp;
            }
            return (int)minHp;
        }
    }
}
