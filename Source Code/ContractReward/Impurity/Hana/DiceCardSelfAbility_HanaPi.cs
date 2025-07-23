using ContractReward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_HanaPi : HexagramAbility
    {
        public override HexagramCardBuf EnhanceCardBuf()
        {
            return new PiBuff();
        }
        class PiBuff: HexagramCardBuf
        {
            private int trigger = 0;
            public override string keywordIconId => "否Buf";
            public override string keywordId => "HanaPi";
            public override void OnUseCard(BattleUnitModel owner, BattlePlayingCardDataInUnitModel playingCard)
            {
                base.OnUseCard(owner, playingCard);
                if(trigger ==0 && RandomUtil.valueForProb >= 0.5)
                {
                    List<BattleUnitModel> available_enemy = BattleObjectManager.instance.GetAliveList_opponent(owner.faction).FindAll(x =>
                        BattleUnitModel.IsTargetableUnit(_card, owner, x));
                    StageController.Instance.AddAllCardListInBattle(playingCard, RandomUtil.SelectOne(available_enemy));
                    trigger = 1;
                }
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                trigger = 0;
            }
        }
    }
}
