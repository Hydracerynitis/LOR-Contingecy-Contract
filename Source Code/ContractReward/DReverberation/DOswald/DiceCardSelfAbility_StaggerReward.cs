using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class DiceCardSelfAbility_StaggerReward: DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            owner.bufListDetail.AddBuf(new StaggerCheck(card.target));
        }
        class StaggerCheck: BattleUnitBuf
        {
            private BattleUnitModel unit;
            public StaggerCheck(BattleUnitModel unit)
            {
                this.unit = unit;
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if (unit.IsBreakLifeZero())
                {
                    foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(_owner.faction))
                    {
                        unit.allyCardDetail.DrawCards(3);
                        unit.cardSlotDetail.RecoverPlayPoint(3);
                    }
                }
                Destroy();
            }
        }
    }
}
