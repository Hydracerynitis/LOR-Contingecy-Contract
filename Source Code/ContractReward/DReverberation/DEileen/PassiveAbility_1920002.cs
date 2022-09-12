using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1920002 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            BattleUnitBuf activatedBuf = this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.Smoke);
            if (activatedBuf == null || activatedBuf.stack <= 0)
                return;
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction);
            foreach (BattleUnitModel battleUnitModel in aliveList)
                battleUnitModel.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Smoke, activatedBuf.stack/2, this.owner);
        }
    }
}
