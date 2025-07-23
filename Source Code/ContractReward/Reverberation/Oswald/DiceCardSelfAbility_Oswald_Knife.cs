using BaseMod;
using Contingecy_Contract;
using LOR_DiceSystem;
using Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ContractReward
{
    public class DiceCardSelfAbility_Oswald_Knife : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            BattleUnitModel knife = SummonLiberation.Harmony_Patch.SummonUnit(Faction.Player, Tools.MakeLorId(18510000), Tools.MakeLorId(18510000), PlayerUnitName: CharactersNameXmlList.Instance.GetName(252));
            FormationPositionXmlData xml = Singleton<StageController>.Instance.GetCurrentStageFloorModel().GetFormationPosition(unit.index)._xmlInfo;
            knife.formation = new FormationPosition(xml);
            knife.formation.ChangePos(new Vector2Int(xml.vector.x + 5, xml.vector.y));
            knife.moveDetail.ReturnToFormationByBlink();
            knife.allyCardDetail = new BattleAllyCardDetail(knife);
            knife.allyCardDetail.Init(owner.UnitData.unitData.GetDeckForBattle(1));
            knife.allyCardDetail.ReturnAllToDeck();
            knife.allyCardDetail.DrawCards(4);
            ContractAttribution.Init(knife);
            unit.personalEgoDetail.RemoveCard(Tools.MakeLorId(18500008));
            unit.personalEgoDetail.RemoveCard(Tools.MakeLorId(18500009));
        }
    }
}
