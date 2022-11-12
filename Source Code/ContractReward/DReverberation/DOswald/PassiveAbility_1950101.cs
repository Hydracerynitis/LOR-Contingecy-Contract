using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;
using Contingecy_Contract;

namespace ContractReward
{
    public class PassiveAbility_1950101 : PassiveAbilityBase
    {
        private int Boost = 0;
        public void Conjoin(List<BattleUnitModel> conjoined)
        {
            List<BattleDiceCardModel> cards = new List<BattleDiceCardModel>();
            foreach (BattleUnitModel unit in conjoined)
            {
                foreach(PassiveAbilityBase passive in unit.passiveDetail.PassiveList)
                {
                    if (RandomUtil.valueForProb > 0.5)
                        continue;
                    if (passive.InnerTypeId != 1 && !(passive is ContingecyContract))
                    {
                        PassiveAbilityBase instance = Activator.CreateInstance(passive.GetType()) as PassiveAbilityBase;
                        instance.rare = passive.rare;
                        if (!owner.passiveDetail.PassiveList.Exists(x => x.GetType()==instance.GetType()))
                            owner.passiveDetail.AddPassive(instance);
                    }
                }
                foreach(BattleDiceCardModel card in unit.allyCardDetail.GetAllDeck())
                {
                    if (RandomUtil.valueForProb < 0.5)
                        cards.Add(BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(card.GetID())));
                }
                int lost =(int) unit.MaxHp /5;
                unit.LoseHp(lost);
                Boost += lost;
            }
            owner.RecoverHP(Boost);
            owner.allyCardDetail.AddCardToDeck(cards);
            owner.allyCardDetail.ReturnAllToDeck();
            owner.allyCardDetail.DrawCards(3);
        }
        public override void OnRoundStart()
        {
            owner.allyCardDetail.DrawCards(2);
        }
        public override int GetMaxHpBonus()
        {
            return Boost;
        }
    }
}
