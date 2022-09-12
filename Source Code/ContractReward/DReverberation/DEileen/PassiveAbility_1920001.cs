using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1920001 : PassiveAbilityBase
    {
        private List<BattleUnitModel> addedUnits = new List<BattleUnitModel>();
        public override void OnWaveStart()
        {
            addedUnits.AddRange(BattleObjectManager.instance.GetAliveList(owner.faction).FindAll(x => !x.passiveDetail.HasPassive<PassiveAbility_240026>()));
            addedUnits.ForEach(x => x.passiveDetail.AddPassive(new PassiveAbility_240026()));
        }
        public override void OnDie()
        {
            addedUnits.ForEach(x => x.passiveDetail.PassiveList.FindAll(y => y is PassiveAbility_240026).ForEach(y => y.destroyed = true));
            BattleObjectManager.instance.GetAliveList(owner.faction).ForEach(x => x.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Smoke, 10));
        }
    }
}
