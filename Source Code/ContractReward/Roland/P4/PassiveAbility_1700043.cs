using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700043 : PassiveAbilityBase
    {
        private int _patternCount = 0;
        private int _strCnt = 0;
        private BehaviourDetail _currentBuf=BehaviourDetail.Slash;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.owner.passiveDetail.AddPassive((PassiveAbilityBase)new PassiveAbility_170303());
            this.owner.passiveDetail.OnCreated();
        }
        public override void OnRoundStartAfter()
        {
            switch (this._patternCount)
            {
                case 0:
                    this.owner.passiveDetail.AddPassive((PassiveAbilityBase)new PassiveAbility_170304());
                    this.owner.passiveDetail.OnCreated();
                    break;
                case 1:
                    ++this._strCnt;
                    this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, this._strCnt, this.owner);
                    this.owner.passiveDetail.AddPassive((PassiveAbilityBase)new PassiveAbility_170305());
                    this.owner.passiveDetail.OnCreated();
                    break;
                case 2:
                    this.owner.passiveDetail.AddPassive((PassiveAbilityBase)new PassiveAbility_170306());
                    this.owner.passiveDetail.OnCreated();
                    break;
                case 3:
                    this.owner.passiveDetail.AddPassive((PassiveAbilityBase)new PassiveAbility_170307());
                    this.owner.passiveDetail.OnCreated();
                    break;
            }
            ++this._patternCount;
            this._patternCount %= 4;
            switch (this._currentBuf)
            {
                case BehaviourDetail.Slash:
                    this.owner.bufListDetail.AddBuf(new BattleUnitBuf_Roland_4th_DmgReduction_Slash());
                    this._currentBuf = BehaviourDetail.Penetrate;
                    break;
                case BehaviourDetail.Penetrate:
                    this.owner.bufListDetail.AddBuf(new BattleUnitBuf_Roland_4th_DmgReduction_Penetrate());
                    this._currentBuf = BehaviourDetail.Hit;
                    break;
                case BehaviourDetail.Hit:
                    this.owner.bufListDetail.AddBuf(new BattleUnitBuf_Roland_4th_DmgReduction_Hit());
                    this._currentBuf = BehaviourDetail.Slash;
                    break;
            }
        }
    }
}
