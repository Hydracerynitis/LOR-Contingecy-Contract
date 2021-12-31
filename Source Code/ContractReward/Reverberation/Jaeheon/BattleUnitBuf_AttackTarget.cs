using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class BattleUnitBuf_AttackTarget: BattleUnitBuf
    {
        public override KeywordBuf bufType => KeywordBuf.JaeheonMark;
        protected override string keywordId => "Mark";
        protected override string keywordIconId => "Jaeheon_Mark";
    }
}
