using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1930003 : PassiveAbilityBase
    {
        private bool istakeDamaged;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            this.istakeDamaged = false;
        }
        public override void AfterTakeDamage(BattleUnitModel attacker, int dmg) => this.istakeDamaged = true;

        public override void OnRoundEndTheLast()
        {
            if (this.istakeDamaged)
                return;
            this.owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, this.Owner);
            owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, 1, this.Owner);
            this.owner.RecoverHP(15);
        }
    }
}
