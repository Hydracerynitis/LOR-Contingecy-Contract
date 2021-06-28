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
    public class DiceCardSelfAbility_elenaMinionStrong : DiceCardSelfAbilityBase
    {
        private bool _successAttack;
        public override void OnUseCard() => this._successAttack = false;
        public override void OnSucceedAttack()
        {
            this._successAttack = true;
            this.card.target.bufListDetail.AddBuf(new BattleUnitBuf_elenaStrongOnce());
        }
        public override void OnEndBattle()
        {
            if (!this._successAttack)
                return;
            List<BattleUnitModel> aliveListOpponent = BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction);
            aliveListOpponent.Remove(this.owner);
            aliveListOpponent.RemoveAll(x => x.bufListDetail.HasBuf<BattleUnitBuf_elenaStrongOnce>());
            if (aliveListOpponent.Count <= 0)
                return;
            BattleUnitModel target = RandomUtil.SelectOne<BattleUnitModel>(aliveListOpponent);
            Singleton<StageController>.Instance.AddAllCardListInBattle(this.card, target);
            target.bufListDetail.AddBuf(new BattleUnitBuf_elenaStrongOnce());
        }
        public class BattleUnitBuf_elenaStrongOnce : BattleUnitBuf
        {
            public override void OnRoundEnd() => this.Destroy();
        }
    }
}
