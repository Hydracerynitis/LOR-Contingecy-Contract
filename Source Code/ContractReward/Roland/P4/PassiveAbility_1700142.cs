using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700142 : PassiveAbilityBase
    {
        public PassiveAbility_1700142(BattleUnitModel owner)
        {
            name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1700142));
            desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1700142));
            owner.bufListDetail.AddBuf(new Indicator() { stack = 0 }) ;
            Init(owner);
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || target.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Roland_4th_Lie_Ready) != null)
                return;
            target.bufListDetail.AddBuf(new BattleUnitBuf_Roland_4th_Lie_Ready());
        }
        public override void OnFixedUpdateInWaitPhase(float delta)
        {
            if (!owner.allyCardDetail.GetHand().Exists(x => x.GetID() == 1100001))
                owner.allyCardDetail.AddTempCard(1100001);
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            owner.passiveDetail.DestroyPassive(this);
        }
        private class Indicator: BattleUnitBuf
        {
            public override string keywordIconId => "KeterFinal_SuccessLying";
            public override string keywordId => "LearnIndicator";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }

    }
}
