using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;
using BaseMod;
using Contingecy_Contract;

namespace ContractReward
{
    public class DiceCardSelfAbility_UnusedCounter : DiceCardSelfAbilityBase, OnStandBy
    {
        public void OnStandBy(BattlePlayingCardDataInUnitModel card, BattleUnitModel unit, List<BattleDiceBehavior> StandByDie)
        {
            unit.bufListDetail.AddBuf(new CounterCheck(StandByDie));
        }

        public override void OnUseCard()
        {
            
        }
        class CounterCheck: BattleUnitBuf
        {
            private List<BattleDiceBehavior> standByDice;
            public CounterCheck(List<BattleDiceBehavior> standByDice)
            {
                this.standByDice = new List<BattleDiceBehavior>(standByDice);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if(_owner.cardSlotDetail.keepCard.GetDiceBehaviorList().Exists(x => standByDice.Contains(x)))
                {
                    _owner.allyCardDetail.DrawCards(1);
                    _owner.cardSlotDetail.RecoverPlayPoint(2);
                }
                Destroy();
            }
        }
    }
}
