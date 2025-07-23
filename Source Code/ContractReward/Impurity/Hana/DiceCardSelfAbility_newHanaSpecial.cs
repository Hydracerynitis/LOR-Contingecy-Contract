using Contingecy_Contract;
using ContractReward;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_newHanaSpecial : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[2]
          {
            "newHanaSpecial1",
            "newHanaSpecial2"
          };

        public override void OnUseCard()
        {
            List<BattleDiceCardModel> hand = this.owner.personalEgoDetail.GetHand();
            if (!hand.Exists(x => x.GetID() == 701021))
                owner.personalEgoDetail.AddCard(701021);
            if (!hand.Exists(x => x.GetID() == 701022))
                owner.personalEgoDetail.AddCard(701022);
            if (!hand.Exists(x => x.GetID() == 701023))
                owner.personalEgoDetail.AddCard(701023);
            if (!hand.Exists(x => x.GetID() == 701024))
                owner.personalEgoDetail.AddCard(701024);
            BehaviourAction_HanaSpecial.motionCount = 0;
        }

        public override void OnStartBattle()
        {
            BattleUnitBuf battleUnitBuf = owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_hanaBufCommon);
            if (battleUnitBuf == null)
                return;
            if (battleUnitBuf is BattleUnitBuf_hana1)
                owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.AllPowerUp, 1,owner);
            if (battleUnitBuf is BattleUnitBuf_hana2)
            {
                BattleObjectManager.instance.GetAliveList(owner.faction).ForEach(x =>
                {
                    x.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Protection, 3, owner);
                    x.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.BreakProtection, 3, owner);
                });
            }
            if (battleUnitBuf is BattleUnitBuf_hana3)
                owner.bufListDetail.AddReadyBuf(new DiceCardSelfAbility_nextCostDown.BattleUnitBuf_costAllDown());
            if (battleUnitBuf is BattleUnitBuf_hana4){
                owner.allyCardDetail.DrawCards(3);
            }
        }
    }
}
