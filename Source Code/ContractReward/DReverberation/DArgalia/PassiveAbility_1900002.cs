using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;
using Contingecy_Contract;

namespace ContractReward
{
    public class PassiveAbility_1900002 : PassiveAbilityBase
    {
        public override void OnRoundEnd()
        {
            base.OnRoundStart();
            if (StageController.Instance.RoundTurn % 3 == 2)
                BattleObjectManager.instance.GetAliveList(owner.faction).ForEach(x => x.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.UpSurge, 1));
        }
        public override void OnDie()
        {
            base.OnDie();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                unit.bufListDetail.RemoveBufAll(KeywordBuf.UpSurge);
                unit.bufListDetail.AddAutoBufByEtc<BattleUnitBuf_Disarray>(3, readyType: BufReadyType.NextRound);
            };
        }
    }
}
