using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700149 : PassiveAbilityBase
    {
        public PassiveAbility_1700149(BattleUnitModel owner)
        {
            name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1700149));
            desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1700149));
            owner.bufListDetail.AddBuf(new Indicator() { stack=0});
            Init(owner);
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (behavior.card.target.bufListDetail.FindBuf<Hand>() is Hand h && !h.IsDestroyed())
                h.OnHit();
            else
                behavior.card.target.bufListDetail.AddBuf(new Hand());

        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            owner.passiveDetail.DestroyPassive(this);
        }
        public class Hand : BattleUnitBuf
        {
            public override string keywordIconId => "BloodBath_Hand";

            public override string keywordId => "Bloodbath_Hands";


            public void OnHit()
            {
                if (++stack >= 3)
                {
                    int dmg = RandomUtil.Range(3, 10);
                    _owner.breakDetail.TakeBreakDamage(dmg);
                    Destroy();
                    _owner.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_PaleHand_Hit", 3f);
                }
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
        private class Indicator: BattleUnitBuf
        {
            public override string keywordIconId => "BloodBath_Hand";
            public override string keywordId => "HandIndicator";
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }

    }
}
