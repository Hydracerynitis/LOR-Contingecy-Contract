using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1860003 : PassiveAbilityBase
    {
        public override void OnFixedUpdateInWaitPhase(float delta)
        {
            List<BattleUnitModel> aliveListOpponent = BattleObjectManager.instance.GetAliveList_opponent(owner.faction);
            foreach (BattleUnitModel battleUnitModel in aliveListOpponent)
            {
                if (battleUnitModel.IsTargetable(owner))
                    continue;
                battleUnitModel.bufListDetail.AddBuf(new BattleUnitBuf_nullfyNotTargetable());
                battleUnitModel.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 3);
                battleUnitModel.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.BreakProtection, 3);
                SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(battleUnitModel, battleUnitModel.faction, battleUnitModel.hp, battleUnitModel.breakDetail.breakGauge);
                battleUnitModel.view.speedDiceSetterUI.SetSpeedDicesBeforeRoll(battleUnitModel.speedDiceResult, battleUnitModel.faction);
                battleUnitModel.view.speedDiceSetterUI.SetSpeedDicesAfterRoll(battleUnitModel.speedDiceResult);
            }
        }
        public class BattleUnitBuf_nullfyNotTargetable : BattleUnitBuf
        {
            public override bool NullifyNotTargetable() => true;

            public override void OnRoundEnd() => this.Destroy();
        }
    }
}
