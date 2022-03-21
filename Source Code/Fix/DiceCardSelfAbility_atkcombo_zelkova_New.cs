using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace Fix
{
    public class DiceCardSelfAbility_atkcombo_zelkova_New : DiceCardSelfAbilityBase
    {
        public override void OnStartBattleAfterCreateBehaviour() => this.GiveDice();
        private void GiveDice()
        {
            BattleUnitModel battleUnitModel = BattleObjectManager.instance.GetAliveList(this.owner.faction).Find(x => x.UnitData.unitData.EnemyUnitId == 60107);
            if (battleUnitModel == null || this.card == null)
                return;
            List<BattlePlayingCardDataInUnitModel> cardAry = battleUnitModel.cardSlotDetail?.cardAry;
            if (cardAry == null)
                return;
            BattlePlayingCardDataInUnitModel cardDataInUnitModel = null;
            if (cardAry[card.slotOrder]?.card.GetID() == 705213 && this.card != null && this.card.target == cardAry[card.slotOrder].target)
                cardDataInUnitModel = cardAry[card.slotOrder];
            if (cardDataInUnitModel == null)
            {
                for (int index = 0; index < cardAry.Count; ++index)
                {
                    if (cardAry[index]?.card?.GetID() == 705213 && this.card != null && this.card.target == cardAry[index].target)
                    {
                        cardDataInUnitModel = cardAry[index];
                        break;
                    }
                }
            }
            if (cardDataInUnitModel == null)
                return;
            Queue<BattleDiceBehavior> battleDiceBehaviorQueue = new Queue<BattleDiceBehavior>();
            while (this.card.cardBehaviorQueue.Count > 0)
            {
                BattleDiceBehavior diceBehavior = this.card.cardBehaviorQueue.Dequeue();
                if (diceBehavior.Type == BehaviourType.Atk)
                {
                    if (diceBehavior.Index == 0)
                    {
                        diceBehavior.behaviourInCard.MotionDetail = MotionDetail.H;
                        diceBehavior.behaviourInCard.EffectRes = "AngelicaAxe_H";
                    }
                    else if (diceBehavior.Index == 1)
                    {
                        diceBehavior.behaviourInCard.MotionDetail = MotionDetail.J;
                        diceBehavior.behaviourInCard.EffectRes = "AngelicaMace_J";
                    }
                    cardDataInUnitModel.AddDice(diceBehavior);
                }
                else
                    battleDiceBehaviorQueue.Enqueue(diceBehavior);
            }
            foreach (BattleDiceBehavior dice in cardDataInUnitModel.GetDiceBehaviorList())
            {
                BattleDiceBehavior NewDice = new BattleDiceBehavior() { behaviourInCard = dice.behaviourInCard.Copy() };
                if (dice.behaviourInCard.Script != string.Empty)
                {
                    DiceCardAbilityBase instanceDiceCardAbility = Singleton<AssemblyManager>.Instance.CreateInstance_DiceCardAbility(dice.behaviourInCard.Script);
                    if (instanceDiceCardAbility != null)
                        NewDice.AddAbility(instanceDiceCardAbility);
                }
                if (dice.Index == 0)
                {
                    NewDice.behaviourInCard.MotionDetail = MotionDetail.J;
                    NewDice.behaviourInCard.EffectRes = "BS3DurandalUp_J";
                }
                if (dice.Index == 1)
                {
                    NewDice.behaviourInCard.MotionDetail = MotionDetail.H;
                    NewDice.behaviourInCard.EffectRes = "BS3DurandalHit_H";
                }
                card.AddDice(NewDice);
            }
            List<BattleDiceBehavior> dices = new List<BattleDiceBehavior>();
            foreach (DiceBehaviour x in card.GetOriginalDiceBehaviorList().FindAll(x => x.Type == BehaviourType.Standby))
            {
                BattleDiceBehavior NewDice = new BattleDiceBehavior() { behaviourInCard = x.Copy() };
                if (x.Script != string.Empty)
                {
                    DiceCardAbilityBase instanceDiceCardAbility = Singleton<AssemblyManager>.Instance.CreateInstance_DiceCardAbility(x.Script);
                    if (instanceDiceCardAbility != null)
                        NewDice.AddAbility(instanceDiceCardAbility);
                }
                NewDice.behaviourInCard.EffectRes = "Angelica_G";
                dices.Add(NewDice);
            }
            battleUnitModel.cardSlotDetail.keepCard.AddBehaviours(card.card.XmlData, dices);
        }
    }
}
