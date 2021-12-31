using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOR_DiceSystem;
using HarmonyLib;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class PassiveAbility_1800002 : PassiveAbilityBase
    {
        public int count;
        private BattleDiceCardModel model;
        private Queue<DiceBehaviour> RestDice;
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
            typeof(BattleDiceCardModel).GetField("_xmlData", AccessTools.all).SetValue(model, xml);
        }
    }
}
