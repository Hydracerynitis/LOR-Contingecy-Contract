using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700143 : PassiveAbilityBase
    {
        public PassiveAbility_1700143(BattleUnitModel owner)
        {
            name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1700143));
            desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1700143));
            owner.bufListDetail.AddBuf(new Indicator() { stack = 0 }) ;
            Init(owner);
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            BattleUnitModel target = behavior.card.target;
            if (target.bufListDetail.GetActivatedBufList().Find(x => x is Nail) is Nail n)
                n.ProcessAttack(behavior);
            else
            {
                switch (behavior.Detail)
                {
                    case BehaviourDetail.Slash:
                        target.bufListDetail.AddBuf(new SlashNail());
                        break;
                    case BehaviourDetail.Penetrate:
                        target.bufListDetail.AddBuf(new PenetrateNail());
                        break;
                    case BehaviourDetail.Hit:
                        target.bufListDetail.AddBuf(new HitNail());
                        break;
                }
            }
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            owner.passiveDetail.DestroyPassive(this);
        }
        private class Indicator: BattleUnitBuf
        {
            public override string keywordIconId => "KeterFinal_SilenceGirl_Nail";
            public override string keywordId => "NailHammerIndicator";
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }

    }
    public class Nail: BattleUnitBuf
    {
        protected virtual BehaviourDetail detail=>BehaviourDetail.None;
        protected virtual void Proc()
        {

        }
        public void ProcessAttack(BattleDiceBehavior behaviour)
        {
            if (detail == behaviour.Detail)
                stack++;
            else
            {
                Proc();
                Destroy();
            }
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            Destroy();
        }
    }
    public class SlashNail: Nail
    {
        public override string keywordId => "SlashNail";
        public override string keywordIconId => "DanteWeakPoint_Bp_Slash";
        protected override BehaviourDetail detail => BehaviourDetail.Slash;
        protected override void Proc()
        {
            _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 3 * stack);
            _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 1 * stack);
        }
    }
    public class PenetrateNail : Nail
    {
        public override string keywordId => "PenetrateNail";
        public override string keywordIconId => "DanteWeakPoint_Bp_Penetrate";
        protected override BehaviourDetail detail => BehaviourDetail.Penetrate;
        protected override void Proc()
        {
            _owner.TakeDamage(5*stack);
        }
    }
    public class HitNail: Nail
    {
        public override string keywordId => "HitNail";
        public override string keywordIconId => "DanteWeakPoint_Bp_Hit";
        protected override BehaviourDetail detail => BehaviourDetail.Hit;
        protected override void Proc()
        {
            _owner.TakeBreakDamage(5 * stack);
        }
    }

}
