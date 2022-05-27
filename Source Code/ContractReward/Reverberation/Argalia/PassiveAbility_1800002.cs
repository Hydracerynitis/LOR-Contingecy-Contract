using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOR_DiceSystem;
using HarmonyLib;
using Contingecy_Contract;
using BaseMod;

namespace ContractReward
{
    public class PassiveAbility_1800002 : PassiveAbilityBase, Resonator
    {
        public int count;
        private BattleDiceCardModel model;
        private Queue<DiceBehaviour> RestDice;

        public void ActiveResonate(BattlePlayingCardDataInUnitModel card)
        {
            count += 1;
            if (count == 5)
            {
                Upgrade();
                count = 0;
            }
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            RestDice = new Queue<DiceBehaviour>();
            DiceCardXmlInfo xml = ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(18000021)).Copy(true);
            foreach(DiceBehaviour dice in xml.DiceBehaviourList)
            {
                RestDice.Enqueue(dice);
            }
            xml.DiceBehaviourList.Clear();
            xml.DiceBehaviourList.Add(RestDice.Dequeue());
            model = BattleDiceCardModel.CreatePlayingCard(xml);
            this.owner.allyCardDetail.AddCardToHand(model);
            count = 0;
        }
        public void Upgrade()
        {
            if (RestDice.Count <= 0)
                return;
            model.CopySelf();
            DiceCardXmlInfo xml = model.XmlData;
            xml.DiceBehaviourList.Add(RestDice.Dequeue());
            model._xmlData = xml;
        }
    }
}
