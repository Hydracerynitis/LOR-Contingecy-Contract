using LOR_DiceSystem;
using System;
using Contingecy_Contract;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1890002 : PassiveAbilityBase
    {
        private BattleDiceCardModel steal;
        public override void OnFixedUpdateInWaitPhase(float delta)
        {
            base.OnFixedUpdateInWaitPhase(delta);
            if (Harmony_Patch.passive18900002_Makred.Count > 0)
                return;
            List<DiceCardXmlInfo> uniques = new List<DiceCardXmlInfo>();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                foreach(BattlePlayingCardDataInUnitModel playingcard in unit.cardSlotDetail.cardAry)
                {
                    if (playingcard == null || uniques.Contains(ItemXmlDataList.instance.GetCardItem(playingcard.card.GetID())))
                        continue;
                    uniques.Add(ItemXmlDataList.instance.GetCardItem(playingcard.card.GetID()));
                }
            }
            if (uniques.Count <= 0)
                return;
            List<DiceCardXmlInfo> MarkedCard = new List<DiceCardXmlInfo>();
            for (int i = 0; i < (int)(uniques.Count / 2); i++)
            {
                DiceCardXmlInfo card = RandomUtil.SelectOne<DiceCardXmlInfo>(uniques);
                while (MarkedCard.Contains(card))
                    card = RandomUtil.SelectOne<DiceCardXmlInfo>(uniques);
                MarkedCard.Add(card);
            }
            foreach (DiceCardXmlInfo mark in MarkedCard)
            {
                Harmony_Patch.passive18900002_Makred.Add(RandomUtil.SelectOne<DiceBehaviour>(mark.DiceBehaviourList.FindAll(x=> x.Type!=BehaviourType.Standby)));
            }
            steal = null;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (behavior.TargetDice!=null && Harmony_Patch.passive18900002_Makred.Contains(behavior.TargetDice.behaviourInCard))
            {
                steal = BattleDiceCardModel.CreatePlayingCard(behavior.card.target.currentDiceAction.card.XmlData.Copy());
                steal.owner = owner;
                steal.XmlData.optionList.Add(CardOption.ExhaustOnUse);
            }
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            if (Harmony_Patch.passive18900002_Makred.Contains(behavior.TargetDice.behaviourInCard)&&steal!=null)
            {
                steal.SetCostToZero();
            }
        }
        public override void OnEndBattle(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnEndBattle(curCard);
            if (steal != null)
            {
                owner.allyCardDetail.AddCardToHand(steal);
                steal = null;
            }
        }
        public override void OnRoundStart()
        {
            Harmony_Patch.passive18900002_Makred.Clear();
        }
    }
}
