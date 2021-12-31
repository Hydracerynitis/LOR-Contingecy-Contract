using System;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_AllyCounter: DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            BattleUnitModel target = this.card.target;
            DiceCardXmlInfo cardItem = this.card.card.XmlData;
            List<BattleDiceBehavior> behaviourList = new List<BattleDiceBehavior>();
            int num=0;
            foreach (DiceBehaviour diceBehaviour2 in cardItem.DiceBehaviourList)
            {
                BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior
                {
                    behaviourInCard = diceBehaviour2.Copy()
                };
                battleDiceBehavior.SetIndex(num++);
                behaviourList.Add(battleDiceBehavior);
            }
            target.cardSlotDetail.keepCard.AddBehaviours(cardItem, behaviourList);

        }
        public override bool IsOnlyAllyUnit() => true;
    }
}
