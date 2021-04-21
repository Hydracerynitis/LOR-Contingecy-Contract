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
    public class DiceCardSelfAbility_Canard0: DiceCardSelfAbilityBase
    {
        public override void OnEnterCardPhase(BattleUnitModel unit, BattleDiceCardModel self)
        {
            if (self.HasBuf<Changing>())
                return;
            BattleDiceCardBuf change = new Changing();
            self.AddBuf(change);
            change.OnRoundEnd();
        }
        public class Changing : BattleDiceCardBuf
        {
            private bool TeamNear;
            private List<DiceCardXmlInfo> List;
            public Changing()
            {
                List = ItemXmlDataList.instance.GetCardList();
                List.Remove(ItemXmlDataList.instance.GetCardItem(20210304));
            }
            public override void OnRoundEnd()
            {
                TeamNear = false;
                DiceCardXmlInfo xmlinfo = this._card.XmlData.Copy(true);
                DiceCardSpec Spec = xmlinfo.Spec.Copy();
                Spec.Cost= this.GetCost();
                Spec.Ranged = GetRange();
                Spec.affection = CardAffection.Passive;
                if (TeamNear)
                    Spec.affection = CardAffection.TeamNear;
                if (IsFarArea(Spec))
                    Spec.affection = GetAffection();
                typeof(DiceCardXmlInfo).GetField("Spec", AccessTools.all).SetValue(xmlinfo, Spec);
                xmlinfo.Artwork = RandomUtil.SelectOne<DiceCardXmlInfo>(List).Artwork;
                xmlinfo.Script = RandomUtil.SelectOne<DiceCardXmlInfo>(List.FindAll((Predicate<DiceCardXmlInfo>)(x => x.Spec.Ranged != CardRange.Instance))).Script;
                int Length = RandomUtil.Range(1, 3);
                xmlinfo.DiceBehaviourList = new List<DiceBehaviour>(Length);
                for(int i = 0; i< Length; i++)
                {
                    DiceCardXmlInfo card;
                    if (IsFarArea(Spec))
                        card = RandomUtil.SelectOne<DiceCardXmlInfo>(List.FindAll((Predicate<DiceCardXmlInfo>)(x => IsFarArea(x.Spec))));
                    else
                        card = RandomUtil.SelectOne<DiceCardXmlInfo>(List.FindAll((Predicate<DiceCardXmlInfo>)(x => x.Spec.Ranged == CardRange.Near || x.Spec.Ranged == CardRange.Far || x.Spec.Ranged==CardRange.Special)));
                    DiceBehaviour behaviour = RandomUtil.SelectOne<DiceBehaviour>(card.DiceBehaviourList);
                    xmlinfo.DiceBehaviourList.Add(behaviour);
                }
                typeof(BattleDiceCardModel).GetField("_xmlData",AccessTools.all).SetValue(this._card, xmlinfo);
                typeof(BattleDiceCardModel).GetField("_originalXmlData", AccessTools.all).SetValue(this._card, xmlinfo);
                this._card.SetCurrentCost(Spec.Cost);
            }
            private int GetCost()
            {           
                int random = RandomUtil.Range(1, 300);
                if (random <= 60)
                    return 0;
                else if (random <= 120)
                    return 1;
                else if (random <= 180)
                    return 2;
                else if (random <= 240)
                    return 3;
                else if (random <= 270)
                    return 4;
                else
                    return 5;
            }
            private CardRange GetRange()
            {
                int random = RandomUtil.Range(1, 300);
                if (random <= 100)
                    return CardRange.Near;
                else if (random <= 120)
                {
                    TeamNear = true;
                    return CardRange.Near;
                }
                else if (random <= 140)
                    return CardRange.Special;
                else if (random <= 260)
                    return CardRange.Far;
                else if (random <= 280)
                    return CardRange.FarArea;
                else
                    return CardRange.FarAreaEach;
            }
            private CardAffection GetAffection()
            {
                switch (RandomUtil.Range(1, 3))
                {
                    case 1:
                    case 2:
                        return CardAffection.Team;
                    case 3:
                        return CardAffection.All;
                }
                return CardAffection.Passive;
            }
            private bool IsFarArea(DiceCardSpec spec) => spec.Ranged == CardRange.FarAreaEach || spec.Ranged == CardRange.FarArea;
        }
    }
}
