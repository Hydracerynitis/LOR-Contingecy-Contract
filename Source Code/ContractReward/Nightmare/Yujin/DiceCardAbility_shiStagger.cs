using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;

namespace ContractReward
{
    public class DiceCardAbility_shiStagger : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            BattleUnitModel target = behavior.card.target;
            List<BattlePlayingCardDataInUnitModel> remainingPage = StageController.Instance.GetAllCards();
            BattlePlayingCardDataInUnitModel enemyNextMove = remainingPage.Find(x => x.owner == target &&
            x != target.currentDiceAction && !x.isDestroyed && x.target == owner);
            if (enemyNextMove != null)
            {
                enemyNextMove.DestroyPlayingCard();
            }
        }
    }
}
