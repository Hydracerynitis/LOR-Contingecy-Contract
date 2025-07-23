using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;
using System.Collections;

namespace ContractReward
{
    public class PassiveAbility_1710003 : PassiveAbilityBase
    {
        private List<PassiveAbility_260003.DmgInfo> _dmgInfos = new List<PassiveAbility_260003.DmgInfo>();
        private List<PassiveAbility_260003.DmgInfo> _breakdmgInfos = new List<PassiveAbility_260003.DmgInfo>();
        public override void Init(BattleUnitModel self)
        {
            _dmgInfos.Add(new PassiveAbility_260003.DmgInfo() { type = BehaviourDetail.Slash });
            _dmgInfos.Add(new PassiveAbility_260003.DmgInfo() { type = BehaviourDetail.Penetrate });
            _dmgInfos.Add(new PassiveAbility_260003.DmgInfo() { type = BehaviourDetail.Hit });
            _breakdmgInfos.Add(new PassiveAbility_260003.DmgInfo() { type = BehaviourDetail.Slash });
            _breakdmgInfos.Add(new PassiveAbility_260003.DmgInfo() { type = BehaviourDetail.Penetrate });
            _breakdmgInfos.Add(new PassiveAbility_260003.DmgInfo() { type = BehaviourDetail.Hit });
            base.Init(self);
        }
        public override void OnRoundStart()
        {
            _dmgInfos.Sort((x, y) => y.dmg - x.dmg);
            _breakdmgInfos.Sort((x, y) => y.dmg - x.dmg);
            BehaviourDetail behaviourDetail1 = BehaviourDetail.None;
            BehaviourDetail behaviourDetail2 = BehaviourDetail.None;
            if (_dmgInfos[0].dmg > 0)
                behaviourDetail1 = _dmgInfos[0].type;
            if (_breakdmgInfos[0].dmg > 0)
                behaviourDetail2 = _breakdmgInfos[0].type;
            List<BattleUnitModel> benifits=new List<BattleUnitModel>() { owner};
            List<BattleUnitModel> otherAlly = BattleObjectManager.instance.GetAliveList(owner.faction).FindAll(x => x != owner);
            for (int count=2; otherAlly.Count > 0 && count > 0; --count)
            {
                BattleUnitModel battleUnitModel = RandomUtil.SelectOne<BattleUnitModel>(otherAlly);
                otherAlly.Remove(battleUnitModel);
                benifits.Add(battleUnitModel);
            }
            benifits.ForEach(x => x.bufListDetail.AddBuf(new ResistsModifier()
            {
                hpTarget = behaviourDetail1,
                bpTarget = behaviourDetail2,
            }));
            _dmgInfos.ForEach(x => x.dmg = 0);
            _breakdmgInfos.ForEach(x => x.dmg = 0);
        }

        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            PassiveAbility_260003.DmgInfo dmgInfo = _dmgInfos.Find(x => x.type == atkDice.Detail);
            if (dmgInfo == null)
                return;
            dmgInfo.dmg += dmg;
        }

        public override void OnTakeBreakDamageByAttack(BattleDiceBehavior atkDice, int breakdmg)
        {
            PassiveAbility_260003.DmgInfo dmgInfo = _breakdmgInfos.Find(x => x.type == atkDice.Detail);
            if (dmgInfo == null)
                return;
            dmgInfo.dmg += breakdmg;
        }

        public class ResistsModifier : BattleUnitBuf
        {
            public BehaviourDetail hpTarget = BehaviourDetail.None;
            public BehaviourDetail bpTarget = BehaviourDetail.None;
            public AtkResist getResist()
            {
                return AtkResist.Endure;
            }
            public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
            {
                if (hpTarget == BehaviourDetail.None)
                    return base.GetResistHP(origin, detail);
                return hpTarget == detail && origin<= getResist() ? getResist() : base.GetResistHP(origin, detail);
            }

            public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
            {
                if (bpTarget == BehaviourDetail.None)
                    return base.GetResistBP(origin, detail);
                return bpTarget == detail && origin <= getResist() ? getResist() : base.GetResistBP(origin, detail);
            }

            public override void OnRoundEnd() => Destroy();
        }
    }
}
