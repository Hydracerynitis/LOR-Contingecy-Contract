﻿using System;
using System.Collections.Generic;
using LOR_DiceSystem;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1830003 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            this.owner.allyCardDetail.AddNewCardToDeck(18300011);
            this.owner.personalEgoDetail.AddCard(18300013);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            owner.personalEgoDetail.GetHand().Find(x => x.GetID() == 18300013).SetCurrentCost(owner.cardSlotDetail.PlayPoint);
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction).FindAll(x => x.IsExtinction()))
            {
                unit.view.EnableView(true);
                unit.Extinct(false);
            }
        }
    }
}
