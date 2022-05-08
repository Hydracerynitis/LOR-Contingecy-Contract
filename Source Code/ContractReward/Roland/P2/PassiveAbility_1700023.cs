using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700023 : PassiveAbilityBase
    {
        private int count = 0;
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            count++;
            if (count >= 3 && BattleObjectManager.instance.GetAliveList_opponent(owner.faction).Count!=0)
            {
                DiceCardXmlInfo xml = ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(17000026));
                BattleDiceCardModel card = BattleDiceCardModel.CreatePlayingCard(xml);
                BattlePlayingCardDataInUnitModel aoe = new BattlePlayingCardDataInUnitModel()
                {
                    owner = owner,
                    card = card,
                    cardAbility = card.CreateDiceCardSelfAbilityScript(),
                    slotOrder = RandomUtil.Range(0, owner.speedDiceCount - 1),
                    subTargets = new List<BattlePlayingCardDataInUnitModel.SubTarget>()
                };
                if (aoe.cardAbility != null)
                    aoe.cardAbility.card = aoe;
                List<BattleUnitModel> battleUnitModelList = BattleObjectManager.instance.GetAliveList().FindAll(x => x != owner && x.IsTargetable(owner) && x.speedDiceResult?.Count>0);
                if (battleUnitModelList.Count <= 0)
                    return;
                foreach (BattleUnitModel battleUnitModel in battleUnitModelList)
                    aoe.subTargets.Add(new BattlePlayingCardDataInUnitModel.SubTarget() { target = battleUnitModel, targetSlotOrder = RandomUtil.Range(0, battleUnitModel.speedDiceCount - 1) });
                BattlePlayingCardDataInUnitModel.SubTarget main = RandomUtil.SelectOne(aoe.subTargets);
                aoe.subTargets.Remove(main);
                StageController.Instance.AddAllCardListInBattle(aoe, main.target,main.targetSlotOrder);
                StageController.Instance.ApplyAddedCardList();
                count = 0;
            }
        }
    }
}
