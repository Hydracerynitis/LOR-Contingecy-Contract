using BaseMod;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_DanceTogether : DiceCardSelfAbilityBase
    {
        public override bool IsOnlyAllyUnit()
        {
            return true;
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            base.OnUseInstance(unit, self, targetUnit);
            PassiveAbility_1700031 passive = unit.passiveDetail.FindPassive<PassiveAbility_1700031>();
            if(passive != null)
            {
                unit.personalEgoDetail.RemoveCard(Tools.MakeLorId(17000331));
                passive.Dance(targetUnit);
            }
        }
    }
}
