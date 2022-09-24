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
    public class DiceCardSelfAbility_Alert : DiceCardSelfAbilityBase
    {
        public override void OnEnterCardPhase(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnEnterCardPhase(unit, self);
            if (unit.allyCardDetail.GetHand().Contains(self))
                unit.bufListDetail.AddBuf(new CheckInHand(self));
        }
        public override void OnStartBattle()
        {
            owner.allyCardDetail.DrawCards(1);
        }
        public class CheckInHand: BattleUnitBuf_Extention
        {
            BattleDiceCardModel TargetCard;
            public CheckInHand(BattleDiceCardModel card)
            {
                TargetCard = card;
            }
            public override void OnStartBattle()
            {
                base.OnStartBattle();
                if (TargetCard!=null && _owner.allyCardDetail.GetHand().Contains(TargetCard))
                {
                    DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(TargetCard.GetID());
                    DiceBehaviour diceBehaviour1 = new DiceBehaviour();
                    List<BattleDiceBehavior> behaviourList = new List<BattleDiceBehavior>();
                    int num = 0;
                    foreach (DiceBehaviour diceBehaviour2 in cardItem.DiceBehaviourList)
                    {
                        BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
                        battleDiceBehavior.behaviourInCard = diceBehaviour2.Copy();
                        battleDiceBehavior.SetIndex(num++);
                        behaviourList.Add(battleDiceBehavior);
                    }
                    _owner.cardSlotDetail.keepCard.AddBehaviours(cardItem, behaviourList);
                    _owner.allyCardDetail.DiscardACardByAbility(TargetCard);
                    _owner.allyCardDetail.DrawCards(1);
                }
            }
        }
    }
}
