using System;
using System.Collections.Generic;
using SL = SummonLiberation.Harmony_Patch;
using UI;
using BaseMod;
using Contingecy_Contract;

namespace ContractReward
{
    public class DiceCardSelfAbility_SummonShade : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            base.OnUseInstance(unit, self, targetUnit);
            unit.personalEgoDetail.RemoveCard(self.GetID());
            BattleUnitModel shade = SL.SummonUnit(unit.faction, Tools.MakeLorId(19910000), Tools.MakeLorId(19910000), PlayerUnitName: CharactersNameXmlList.Instance.GetName(392));
            PassiveAbilityBase passiveAbilityBase = shade.passiveDetail.PassiveList.Find(x => x is PassiveAbility_1990012);
            if (passiveAbilityBase != null)
                (passiveAbilityBase as PassiveAbility_1990012).CopyUnit(targetUnit);
            int num = 0;
            ContractAttribution.Init(shade);
            foreach (BattleUnitModel battleUnitModel in (IEnumerable<BattleUnitModel>)BattleObjectManager.instance.GetList())
                SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnitModel.UnitData.unitData, num++, true);
            BattleObjectManager.instance.InitUI();
        }
    }
}
