using Contingecy_Contract;
using ContractReward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_HanaShi : HexagramAbility
    {
        public override HexagramCardBuf EnhanceCardBuf()
        {
            return new ShiBuff();
        }
        class ShiBuff: HexagramCardBuf, StartBattleInHandBuf
        {
            public override string keywordIconId => "师Buf";
            public override string keywordId => "HanaShi";
            public void OnStartBattle_inHand(BattleUnitModel unit)
            {
                BattlePlayingCardDataInUnitModel dummpyPage = new BattlePlayingCardDataInUnitModel();
                dummpyPage.owner = unit;
                dummpyPage.card = _card;
                dummpyPage.target = RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(unit.faction).FindAll(x =>
                        BattleUnitModel.IsTargetableUnit(_card, unit, x)));
                dummpyPage.cardAbility= _card.CreateDiceCardSelfAbilityScript();
                if (dummpyPage.cardAbility == null)
                    return;
                dummpyPage.cardAbility.card = dummpyPage;
                dummpyPage.cardAbility.OnUseCard();
                unit.allyCardDetail.DiscardACardByAbility(_card);
            }
        }
    }
}
