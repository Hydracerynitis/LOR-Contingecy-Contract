using System;
using System.Collections.Generic;
using SummonLiberation;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1840003 : PassiveAbilityBase
    {
        public override int MaxPlayPointAdder() => -owner.emotionDetail.MaxPlayPointAdderByLevel();
        public override void OnDrawCard()
        {
            base.OnRoundStart();
            List<BattleDiceCardModel> improvise = new List<BattleDiceCardModel>();
            int count = 1;
            if (owner.emotionDetail.EmotionLevel >= 4)
                count += 1;
            for (int i = 0; i < count; i++)
                improvise.Add(RandomUtil.SelectOne<BattleDiceCardModel>(owner.allyCardDetail.GetHand().FindAll( x => x.GetCost()>0 && !improvise.Contains(x))));
            improvise.ForEach(x => x.SetCostToZero());
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            owner.allyCardDetail.GetAllDeck().ForEach(x => x.SetCostToZero(false));
        }
    }
}
