using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using LOR_DiceSystem;
using Contingecy_Contract;

namespace ContractReward
{
    public class PassiveAbility_1700021 : PassiveAbilityBase
    {
        private List<BattleUnitModel> body=new List<BattleUnitModel>(){ };
        public override void OnUnitCreated()
        {
            base.OnUnitCreated();
            owner.view.ChangeHeight(600);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetList(x => x.IsDead() && !body.Contains(x)))
            {
                int index = unit.index;
                if (owner.faction==unit.faction)
                    BattleObjectManager.instance.UnregisterUnit(unit);
                else
                {
                    body.Add(unit);
                    int i = 0;
                    for (; i < BattleObjectManager.instance.GetList(owner.faction).Count; i++)
                        if (BattleObjectManager.instance.GetUnitWithIndex(owner.faction, i)==null)
                            break;
                    index = i;
                }
                BattleUnitModel byproduct = SummonLiberation.Harmony_Patch.SummonUnit(owner.faction, new LorId(60106), new LorId(170102),index, PlayerUnitName: CharactersNameXmlList.Instance.GetName(170));
                FormationPositionXmlData xml = DeepCopyUtil.CopyVector(unit.formation._xmlInfo);
                if (unit.faction != owner.faction)
                    xml.vector.x = -xml.vector.x;
                byproduct.formation = new FormationPosition(xml);
                PassiveAbilityBase rolandPassive = new PassiveAbility_1700000();
                rolandPassive.Init(byproduct);
                byproduct.passiveDetail._passiveList.Insert(0, rolandPassive);
                byproduct.bufListDetail.AddBuf(new Unctrollable());
                byproduct.moveDetail.ReturnToFormationByBlink();
                ContractAttribution.Init(byproduct);
                owner.LoseHp(20);
            }
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
            {
                if (unit != owner)
                    unit.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.HeavySmoke, 1);
            }
        }
        class Unctrollable : BattleUnitBuf
        {
            public override bool IsControllable => false;
        }
    }
}
