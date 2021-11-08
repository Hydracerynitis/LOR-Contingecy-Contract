using System;
using System.Collections.Generic;
using SummonLiberation;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class PassiveAbility_1820001 : PassiveAbilityBase
    {
        private List<BattleUnitModel> Gear;
        private List<int> timer;
        private bool trigger;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            Gear = new List<BattleUnitModel>() { null,null};
            timer = new List<int>() { 0, 0 };
            trigger = false;
            InitGear(0);
            InitGear(1);
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            if (Gear[0]!=null && Gear[0].IsDead())
            {
                BattleObjectManager.instance.UnregisterUnit(Gear[0]);
                this.owner.TakeBreakDamage((int)(owner.breakDetail.GetDefaultBreakGauge() * 0.2));
                Gear[0] = null;
                if (!trigger)
                    trigger = true;
            }
            if (Gear[0] == null)
            {
                timer[0] += 1;
            }
            if (timer[0] == 3)
            {
                InitGear(0);
                timer[0] = 0;
            }
            if (Gear[1] != null && Gear[1].IsDead())
            {
                BattleObjectManager.instance.UnregisterUnit(Gear[1]);
                this.owner.TakeBreakDamage((int)(owner.breakDetail.GetDefaultBreakGauge() * 0.2),DamageType.Passive);
                Gear[1] = null;
                if (!trigger)
                    trigger = true;
            }
            if (Gear[1] == null)
            {
                timer[1] += 1;
            }
            if (timer[1] == 3)
            {
                InitGear(1);
                timer[1] = 0;
            }
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (trigger)
            {
                this.owner.allyCardDetail.AddNewCard(Tools.MakeLorId(18200004)).temporary = true;
                trigger = false;
            }
        }
        public void InitGear(int index)
        {
            Gear[index] = SummonLiberation.Harmony_Patch.SummonUnit(Faction.Player,Tools.MakeLorId(18210000), Tools.MakeLorId(18210000),PlayerUnitName: "Meat Gear");
            FormationPositionXmlData xml = Singleton<StageController>.Instance.GetCurrentStageFloorModel().GetFormationPosition(owner.index)._xmlInfo;
            Gear[index].formation = new FormationPosition(xml);
            if (index==0)
                Gear[index].formation.ChangePos(new Vector2Int(xml.vector.x-2, xml.vector.y + 5));
            else
                Gear[index].formation.ChangePos(new Vector2Int(xml.vector.x - 2, xml.vector.y - 5));
            Gear[index].moveDetail.ReturnToFormationByBlink();
        }
    }
}
