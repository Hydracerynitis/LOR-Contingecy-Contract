using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1880102: PassiveAbilityBase
    {
        public override int GetSpeedDiceAdder(int speedDiceResult) => -3;
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            List<BattlePlayingCardDataInUnitModel> cardArray = this.owner.cardSlotDetail.cardAry.FindAll(x=> x!=null);
            List<BattlePlayingCardDataInUnitModel> cards = cardArray.FindAll(x => x.card.GetID() == 18810001 || x.card.GetID() == 18810002 || x.card.GetID() == 18810003);
            if (cards.Count <= 0)
                return;
            BattleUnitModel target = RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction).FindAll(x => x.IsTargetable(this.owner)));
            BattlePlayingCardDataInUnitModel card = new BattlePlayingCardDataInUnitModel()
            {
                card = RandomUtil.SelectOne<BattlePlayingCardDataInUnitModel>(cards).card,
                owner = this.owner
            };           
            Singleton<StageController>.Instance.AddAllCardListInBattle(card, target);
        }
    }
}
