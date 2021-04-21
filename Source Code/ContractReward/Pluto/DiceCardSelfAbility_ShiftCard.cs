using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_ShiftCard : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            List<BattleUnitModel> list = BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction).FindAll(x => x.bufListDetail.GetActivatedBufList().Find(y => y is BattleUnitBuf_Barrier) != null);
            if (list.Count < 0)
                return;
            List<BattleDiceCardModel> Cardlist = list[0].allyCardDetail.GetAllDeck();
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
        }
        public class Exhaust: BattleDiceCardBuf
        {
            public override void OnUseCard(BattleUnitModel owner)
            {
                base.OnUseCard(owner);
                this._card.exhaust = true;
            }
        }
    }
}
