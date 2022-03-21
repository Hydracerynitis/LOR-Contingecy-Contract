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
    public class DiceCardSelfAbility_atkcombo_allas_New : DiceCardSelfAbilityBase
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
            BattlePlayingCardDataInUnitModel cardDataInUnitModel=null;
            if(cardAry[card.slotOrder]?.card.GetID() == 705214 && this.card != null && this.card.target == cardAry[card.slotOrder].target)
                cardDataInUnitModel = cardAry[card.slotOrder];
            if (cardDataInUnitModel == null)
            {
                for (int index = 0; index < cardAry.Count; ++index)
                {
                    if (cardAry[index]?.card?.GetID() == 705214 && this.card != null && this.card.target == cardAry[index].target)
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
                    diceBehavior.behaviourInCard.MotionDetail = MotionDetail.Z;
                    diceBehavior.behaviourInCard.EffectRes = "AngelicaLance_Z";
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
                NewDice.behaviourInCard.MotionDetail = MotionDetail.Z;
                NewDice.behaviourInCard.EffectRes = "BS3DurandalPene_Z";
                card.AddDice(NewDice);
            }
        }
    }
}
