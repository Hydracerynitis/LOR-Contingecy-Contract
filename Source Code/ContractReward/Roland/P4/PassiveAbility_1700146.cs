using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700146 : PassiveAbilityBase
    {
        private bool _hit;
        private bool hitPerCard;
        public PassiveAbility_1700146(BattleUnitModel owner)
        {
            name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1700146));
            desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1700146));
            owner.bufListDetail.AddBuf(new Indicator() { stack=0});
            Init(owner);
        }
        private class Indicator: BattleUnitBuf
        {
            public override string keywordIconId => "KeterFinal_Lung3";
            public override string keywordId => "PulsationIndicator";
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            this._hit = false;
        }

        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            owner.passiveDetail.DestroyPassive(this);
            if (this._hit)
                return;
            this.owner.breakDetail.TakeBreakDamage(15, DamageType.Passive, this.owner);    
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            hitPerCard = false;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (hitPerCard)
                return;
            _hit = true;
            hitPerCard = true;
            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, this.owner);
        }

        public override int SpeedDiceNumAdder()
        {
            return -1;
        }
    }
}
