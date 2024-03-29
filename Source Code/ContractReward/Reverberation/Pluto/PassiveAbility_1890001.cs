﻿using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1890001 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.owner.personalEgoDetail.AddCard(Tools.MakeLorId(18900011));
            this.owner.personalEgoDetail.AddCard(Tools.MakeLorId(18900012));
            this.owner.personalEgoDetail.AddCard(Tools.MakeLorId(18900013));
            this.owner.personalEgoDetail.AddCard(Tools.MakeLorId(18900014));
            this.owner.personalEgoDetail.AddCard(Tools.MakeLorId(18900015));
        }
        public override void OnDrawCard()
        {
            base.OnDrawCard();
            if (owner.allyCardDetail.GetHand().Count <= 4)
                owner.allyCardDetail.DrawCards(1);
        }
    }
}
