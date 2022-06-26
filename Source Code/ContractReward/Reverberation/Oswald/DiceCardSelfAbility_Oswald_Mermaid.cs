using BaseMod;
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
    public class DiceCardSelfAbility_Oswald_Mermaid : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            BattleUnitModel mermaid = SummonLiberation.Harmony_Patch.SummonUnit(Faction.Player, Tools.MakeLorId(18520000), Tools.MakeLorId(18520000), PlayerUnitName: CharactersNameXmlList.Instance.GetName(253));
            FormationPositionXmlData xml = Singleton<StageController>.Instance.GetCurrentStageFloorModel().GetFormationPosition(unit.index)._xmlInfo;
            mermaid.formation = new FormationPosition(xml);
            mermaid.formation.ChangePos(new Vector2Int(xml.vector.x + 5, xml.vector.y));
            mermaid.moveDetail.ReturnToFormationByBlink();
            mermaid.allyCardDetail = new BattleAllyCardDetail(mermaid);
            mermaid.allyCardDetail.Init(owner.UnitData.unitData.GetDeckForBattle(2));
            mermaid.allyCardDetail.ReturnAllToDeck();
            mermaid.allyCardDetail.DrawCards(4);
            unit.personalEgoDetail.RemoveCard(Tools.MakeLorId(18500008));
            unit.personalEgoDetail.RemoveCard(Tools.MakeLorId(18500009));
        }
    }
}
