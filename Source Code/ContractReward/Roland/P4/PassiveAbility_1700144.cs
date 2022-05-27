using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700144 : PassiveAbilityBase
    {
        private bool isParry;
        private int reduce;
        public PassiveAbility_1700144(BattleUnitModel owner)
        {
            name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1700144));
            desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1700144));
            owner.bufListDetail.AddBuf(new Indicator() { stack = 0 }) ;
            Init(owner);
        }
        public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
        {
            isParry = true;
        }
        public override void OnStartTargetedByAreaAtk(BattlePlayingCardDataInUnitModel attackerCard)
        {
            isParry = false;
        }
        public override void OnStartTargetedOneSide(BattlePlayingCardDataInUnitModel attackerCard)
        {
            isParry = false;
        }
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            if (isParry)
            {
                behavior.UpdateDiceFinalValue();
                reduce = behavior.DiceResultValue;
            }
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            owner.passiveDetail.DestroyPassive(this);
        }
        public override int GetDamageReduction(BattleDiceBehavior behavior)
        {
            if (isParry)
                return reduce;
            else
                return -RandomUtil.Range(2, 4);
        }
        private class Indicator: BattleUnitBuf
        {
            public override string keywordIconId => "KeterFinal_SilenceGirl_Gaze_Defense";
            public override string keywordId => "GuiltIndicator";
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }

    }
}
