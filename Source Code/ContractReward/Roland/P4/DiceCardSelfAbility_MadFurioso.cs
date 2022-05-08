using BaseMod;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOR_DiceSystem;

namespace ContractReward
{
    public class DiceCardSelfAbility_MadFurioso : DiceCardSelfAbilityBase
    {
        private List<LorId> GetCount(BattleUnitModel owner) => (owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_1700041) as PassiveAbility_1700041)._usedCount;
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return GetCount(owner).Count > 0;
        }
        public override void OnUseCard()
        {
            base.OnUseCard();
            BattleDiceBehavior battleDiceBehavior = this.card.cardBehaviorQueue.Dequeue();
            this.card.cardBehaviorQueue.Clear();
            List<LorId> usedCount=GetCount(owner);
            while (usedCount.Count > 0)
            {
                int i = usedCount[0].id;
                usedCount.RemoveAt(0);
                owner.allyCardDetail.AddNewCardToDeck(Tools.MakeLorId(i));
                BattleDiceBehavior newDice = new BattleDiceBehavior() { behaviourInCard = battleDiceBehavior.behaviourInCard.Copy() };
                switch (i % 10)
                {
                    case 1:
                        newDice.behaviourInCard.MotionDetail = MotionDetail.S11;
                        newDice.behaviourInCard.EffectRes = "BlackSilence_4th_Lance_S11";
                        break;
                    case 2:
                        newDice.behaviourInCard.MotionDetail = MotionDetail.S7;
                        newDice.behaviourInCard.EffectRes = "BlackSilence_4th_GreatSword_S7";
                        break;
                    case 3:
                        newDice.behaviourInCard.MotionDetail = MotionDetail.S5;
                        newDice.behaviourInCard.EffectRes = "BlackSilence_4th_MaceAxe_S5";
                        break;
                    case 4:
                        newDice.behaviourInCard.MotionDetail = MotionDetail.S6;
                        newDice.behaviourInCard.EffectRes = "BlackSilence_4th_Hammer_S6";
                        break;
                    case 5:
                        newDice.behaviourInCard.MotionDetail = MotionDetail.S2;
                        newDice.behaviourInCard.EffectRes = "BlackSilence_4th_LongSword_S2";
                        break;
                    case 6:
                        newDice.behaviourInCard.MotionDetail = MotionDetail.S4;
                        newDice.behaviourInCard.EffectRes = "BlackSilence_4th_ShortSword_S4";
                        break;
                    case 7:
                        newDice.behaviourInCard.MotionDetail = MotionDetail.S9;
                        newDice.behaviourInCard.EffectRes = "BlackSilence_4th_DualWield1_S9";
                        break;
                    case 8:
                        newDice.behaviourInCard.MotionDetail = MotionDetail.S8;
                        newDice.behaviourInCard.EffectRes = "BlackSilence_4th_Shotgun_S8";
                        break;
                    case 9:
                        newDice.behaviourInCard.MotionDetail = MotionDetail.S10;
                        newDice.behaviourInCard.EffectRes = "BlackSilence_4th_DualWield2_S10";
                        break;
                }
                this.card.AddDice(newDice);
            }
            usedCount.Clear();
            owner.allyCardDetail.Shuffle();
            while(owner.allyCardDetail.GetHand().Count<4)
                owner.allyCardDetail.DrawCards(1);
            owner.cardSlotDetail.RecoverPlayPoint(owner.cardSlotDetail.GetMaxPlayPoint());
        }
    }
}
