using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardAbility_Reassemble : DiceCardAbilityBase
    {
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            for(int i=0; i<3; i++)
            {
                BattleDiceCardModel card = RandomUtil.SelectOne(target.allyCardDetail.GetAllDeck());
                card._xmlData = ItemXmlDataList.instance.GetCardItem(RandomUtil.valueForProb < 0.5 ? 707710 : 707711);
            }
        }
    }
}
