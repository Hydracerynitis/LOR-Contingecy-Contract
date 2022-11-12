using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SummonLiberation;
using LOR_DiceSystem;
using Contingecy_Contract;
using static UnityEngine.UI.CanvasScaler;
using System.Reflection;

namespace ContractReward
{
    public class PassiveAbility_1970003 : PassiveAbilityBase
    {
        private List<BattleUnitModel> victims=new List<BattleUnitModel>();
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            BattleObjectManager.instance.GetAliveList(owner.faction).FindAll(x => x.IsDead() && x.UnitData.unitData.EnemyUnitId.id <= 40006 && x.UnitData.unitData.EnemyUnitId.id >= 40004).ForEach(x => BattleObjectManager.instance.UnregisterUnit(x));
            foreach(BattleUnitModel model in victims)
            {
                int id = RandomUtil.SelectOne(0,1,2);
                BattleUnitModel puppet = Harmony_Patch.SummonUnit(owner.faction, new LorId(id + 40004), new LorId(140004 + id), PlayerUnitName: CharactersNameXmlList.Instance.GetName(id + 92));
                FormationPositionXmlData xml = DeepCopyUtil.CopyVector(model.formation._xmlInfo);
                xml.vector.x = -xml.vector.x;
                puppet.formation = new FormationPosition(xml);
                puppet.bufListDetail.AddBuf(new PuppetUnctrollable());
                puppet.cardSlotDetail.RecoverPlayPoint(puppet.cardSlotDetail.GetMaxPlayPoint());
                puppet.moveDetail.ReturnToFormationByBlink();
            }
            victims.Clear();
        }
        public override void OnKill(BattleUnitModel target)
        {
            base.OnKill(target);
            victims.Add(target);
        }
        class PuppetUnctrollable : BattleUnitBuf
        {
            public override bool IsControllable => false;
            public override StatBonus GetStatBonus()
            {
                return new StatBonus() { hpAdder = 25, breakAdder = 25 };
            }
        }
    }
}
