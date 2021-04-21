using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_ShiftHP: DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            List<BattleUnitModel> list = BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction).FindAll(x => x.bufListDetail.GetActivatedBufList().Find(y => y is BattleUnitBuf_Barrier)!=null);
            if (list.Count < 0)
                return;
            float diff = list[0].hp - this.owner.hp;
            if (diff > 0)
            {
                if (diff > 25)
                    diff = 25;
                list[0].LoseHp((int)diff);
                this.owner.RecoverHP((int)diff);
            }
            this.card.card.exhaust = true;
        }
    }
}
