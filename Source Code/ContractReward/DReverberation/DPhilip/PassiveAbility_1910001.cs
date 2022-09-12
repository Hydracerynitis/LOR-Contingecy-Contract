using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1910001 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            BattleObjectManager.instance.GetAliveList(owner.faction).ForEach(x => x.bufListDetail.AddBuf(new Helious(owner)));
        }
        public class Helious: BattleUnitBuf
        {
            private BattleUnitModel Philip;
            private BattlePlayingCardDataInUnitModel CurrentCard=null;
            public Helious(BattleUnitModel philip)
            {
                Philip = philip;
            }
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                if (Philip == null || Philip.IsDead() || behavior.card==CurrentCard)
                    return;
                CurrentCard = behavior.card;
                behavior.card.target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, 2);
            }
            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                if(Philip != null && Philip.IsDead())
                {
                    _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Burn, 1);
                }
            }
        }
    }
}
