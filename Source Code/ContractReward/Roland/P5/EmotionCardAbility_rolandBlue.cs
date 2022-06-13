using BaseMod;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class EmotionCardAbility_rolandBlue : EmotionCardAbilityBase
    {
        private int coolDown=0;
        public override int GetDamageReduction(BattleDiceBehavior behavior)
        {
            return behavior.card.card.XmlData.Spec.Ranged==CardRange.Far? 999999: 0;
        }
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _owner.bufListDetail.AddBuf(new Nullready() { stack = coolDown });
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();        
            if(coolDown>0)
                coolDown--;
            _owner.bufListDetail.AddBuf(new Nullready() { stack = coolDown });
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (coolDown == 0)
            {
                behavior.card.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.NullifyPower, 1);
                coolDown = 3;
            }
        }
        public class Nullready : BattleUnitBuf
        {
            public override string keywordIconId => "NullifyPowerReady";
            public override string keywordId => "rolandBlueCoolDown";
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                base.OnSuccessAttack(behavior);
                if (stack == 0)
                    stack = 3;
            }
            public override void OnRoundEnd() => this.Destroy();
        }
    }
}
