using Contingecy_Contract;
using ContractReward;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static BattleUnitInformationUI;

namespace ContractReward
{
    public class DiceCardSelfAbility_HanaDraw : DiceCardSelfAbility_drawCard
    {
        public override void OnUseCard()
        {
            if (owner.bufListDetail.HasBuf<BattleUnitBuf_hana2>())
            {
                DiceBehaviour addDice = new DiceBehaviour()
                {
                    Min = 4,
                    Dice = 8,
                    Type = BehaviourType.Def,
                    Detail = BehaviourDetail.Guard,
                    MotionDetail = MotionDetail.G,
                    EffectRes = "Hana_G",
                };
                for(int i=1; i<=2; i++)
                {
                    BattleDiceBehavior diceBehavior = new BattleDiceBehavior();
                    diceBehavior.behaviourInCard = addDice.Copy();
                    diceBehavior.SetIndex(i);
                    card.AddDice(diceBehavior);
                }
            }
        }
    }
}

