using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class Focus: BattleUnitBuf
    {
        private bool isTriggered = false;
        public override string keywordId => "shiFocus";
        public override void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
        {
            base.ChangeDiceResult(behavior, ref diceResult);
        }
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            base.OnRollDice(behavior);
            if (behavior.DiceVanillaValue <= 8 || stack<8)
                return;
            if(behavior.abilityList.Exists(x => x is DiceCardAbility_newYujin))
            {
                behavior._diceResultValue = 50;
            }
            else
            {
                int num = behavior.DiceVanillaValue + behavior.PowerAdder;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = num
                });
            }
            isTriggered = true;
        }
        public override void AfterDiceAction(BattleDiceBehavior behavior)
        {
            if(isTriggered)
                Destroy();
        }
        public static void AddStack(BattleUnitModel unit, int stack)
        {
            if (unit.bufListDetail.GetActivatedBufList().Find(x => x is Focus) is Focus focus)
            {
                if (focus.stack + stack >= 8)
                    focus.stack=8;
                else
                    focus.stack += stack;
            }
            else
            {
                focus = new Focus() { stack = stack };
                unit.bufListDetail.AddBuf(focus);
            }
        }
    }
}
