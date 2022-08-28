using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMod;

namespace ContractReward
{
    public class BattleUnitBuf_SoulLink: BattleUnitBuf
    {
        private readonly BattleUnitModel partner;
        private bool crack = false;
        public bool concentrate=false;
        public override string keywordId => "CC_SpiritLink";
        public override string keywordIconId => "SpiritLink";
        public override bool IsStraighten() => true;
        public BattleUnitBuf_SoulLink(BattleUnitModel p)
        {
            partner = p; 
        }
        public override void OnRoundEnd()
        {
            base.OnRoundStart();
            if (!crack && concentrate)
                stack += 2;
            crack = false;
            concentrate = false;
            if (stack <= 0)
                Destroy();
        }
        public override void OnLoseParrying(BattleDiceBehavior behavior)
        {
            if (behavior.card.target.bufListDetail.HasBuf<CrackSoulLink>())
            {
                crack = true;
                stack -= 1;
                if (stack <= 0)
                    Destroy();
                BattleUnitBuf_SoulLink buf = partner.bufListDetail.FindBuf<BattleUnitBuf_SoulLink>();
                if (buf != null)
                {
                    buf.stack -= 1;
                    buf.crack = true;
                    if (buf.stack <= 0)
                        buf.Destroy();
                }             
            }
            else
                behavior.card.target.bufListDetail.AddBuf(new CrackSoulLink());
            base.OnLoseParrying(behavior);
        }
        public override void Destroy()
        {
            base.Destroy();
            _owner.breakDetail.LoseBreakGauge(_owner.breakDetail.GetDefaultBreakGauge()/4);
            _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Paralysis, 4);
        }
        class CrackSoulLink : BattleUnitBuf
        {
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}
