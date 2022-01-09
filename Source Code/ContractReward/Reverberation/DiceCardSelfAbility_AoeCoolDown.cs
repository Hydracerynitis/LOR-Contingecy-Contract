using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_AoeCoolDown : DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return owner.bufListDetail.GetActivatedBufList().Find((x => x is CoolDown)) == null;
        }
        public override void OnUseCard()
        {
            this.OnUseAoe();
            this.owner.bufListDetail.AddBuf(new CoolDown());
        }
        public virtual void OnUseAoe()
        {

        }
        public class CoolDown : BattleUnitBuf
        {
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 4;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                stack--;
                if (stack <= 0)
                    this.Destroy();
            }
        }
    }
}
