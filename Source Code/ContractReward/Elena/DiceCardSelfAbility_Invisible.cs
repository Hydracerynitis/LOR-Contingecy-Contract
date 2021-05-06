using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_Invisible: DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                if (owner.IsTargetable(unit))
                    return true;
            }
            return false;
        }
        public override void OnStartBattle()
        {
            base.OnUseCard();
            this.owner.bufListDetail.AddBuf(new InvisibleCheck());
        }
        public class InvisibleCheck: BattleUnitBuf
        {
            private bool hit;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                hit = false;
            }
            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                base.OnTakeDamageByAttack(atkDice, dmg);
                hit = true;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if (!hit)
                {
                    this._owner.bufListDetail.AddBuf(new Invisible());
                }
                this.Destroy();
            }
            public class Invisible: BattleUnitBuf
            {
                public override bool IsTargetable() => false;
                public override void OnRoundEnd() => this.Destroy();
            }
        }
    }
}
