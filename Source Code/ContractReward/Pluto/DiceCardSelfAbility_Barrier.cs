﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_Barrier : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            base.OnUseCard();
            BattleUnitModel victim = this.card.target;
            if (victim != null)
            {
                BattleUnitBuf unitBufPlutoBarrier = new BattleUnitBuf_Barrier() { Pluto = this.card.owner };
                victim.bufListDetail.AddBuf(unitBufPlutoBarrier);
            }
            List<BattleDiceCardModel> Cardlist = victim.allyCardDetail.GetAllDeck();
            BattleDiceCardModel Card = RandomUtil.SelectOne<BattleDiceCardModel>(Cardlist);
            Cardlist.Remove(Card);
            Card = this.owner.allyCardDetail.AddNewCard(Card.XmlData.id);
            Card.SetCurrentCost(0);
            Card.AddBuf(new Exhaust());
            Card = RandomUtil.SelectOne<BattleDiceCardModel>(Cardlist);
            Card = this.owner.allyCardDetail.AddNewCard(Card.XmlData.id);
            Card.SetCurrentCost(0);
            Card.AddBuf(new Exhaust());
            this.card.card.exhaust = true;
            this.card.card.exhaust = true;
        }
        public class Exhaust : BattleDiceCardBuf
        {
            public override void OnUseCard(BattleUnitModel owner)
            {
                base.OnUseCard(owner);
                this._card.exhaust = true;
            }
        }
    }
}