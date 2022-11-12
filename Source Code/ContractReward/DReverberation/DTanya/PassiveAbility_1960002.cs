using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1960002 : PassiveAbilityBase
    {
        private bool isAlone => BattleObjectManager.instance.GetAliveList(owner.faction).Count <= 1;
        public override void OnRoundStart()
        {
            if (!isAlone)
                return;
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2);
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 2);
            owner.allyCardDetail.DrawCards(2);
            owner.cardSlotDetail.RecoverPlayPoint(2);
        }
        public override int SpeedDiceNumAdder()
        {
            return isAlone ? 2 : 0;
        }
    }
}
