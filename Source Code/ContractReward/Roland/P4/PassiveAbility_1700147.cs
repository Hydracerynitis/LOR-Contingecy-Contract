using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700147 : PassiveAbilityBase
    {
        public PassiveAbility_1700147(BattleUnitModel owner)
        {
            name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1700147));
            desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1700147));
            owner.bufListDetail.AddBuf(new Indicator() { stack = 0 }) ;
            Init(owner);
        }
        private class Indicator: BattleUnitBuf
        {
            public override string keywordIconId => "KeterFinal_FerventBeats";
            public override string keywordId => "FerventIndicator";
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            owner.passiveDetail.DestroyPassive(this);
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            self.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2);
            self.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 2);
            self.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection,4);
            self.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 3);
        }
    }
}
