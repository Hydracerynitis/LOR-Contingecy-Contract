using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_clashDefense : DiceCardSelfAbilityBase
    {
        public override void OnStartParrying()
        {
            if (card.target.currentDiceAction != null && card.target.currentDiceAction.isKeepedCard)
                return;
            foreach (BattleDiceBehavior diceBehavior in this.card.GetDiceBehaviorList())
            {
                if (IsAttackDice(diceBehavior.behaviourInCard.Detail))
                {
                    diceBehavior.behaviourInCard = diceBehavior.behaviourInCard.Copy();
                    diceBehavior.behaviourInCard.Detail = BehaviourDetail.Guard;
                    diceBehavior.behaviourInCard.Type = BehaviourType.Def;
                    diceBehavior.behaviourInCard.EffectRes = "Shi_G";
                    diceBehavior.behaviourInCard.MotionDetail = MotionDetail.G;
                    diceBehavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 2 });
                }
            }
        }
    }
}
