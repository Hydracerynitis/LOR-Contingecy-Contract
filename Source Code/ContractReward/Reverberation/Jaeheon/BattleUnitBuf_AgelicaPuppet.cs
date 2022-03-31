using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class BattleUnitBuf_AngelicaPuppet: BattleUnitBuf
    {
        private readonly BattleUnitModel Jaehoen;
        public override string keywordId => Jaehoen!=null? "AngelicaPuppet" : "TemporyPuppet";
        public override string keywordIconId => "Jaeheon_PuppetThread";
        public BattleUnitBuf_AngelicaPuppet(BattleUnitModel unit=null)
        {
            Jaehoen = unit;
        }
        public override int SpeedDiceNumAdder() => 1;
        public override bool IsImmortal() => true;
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if (Jaehoen!=null)
                return;
            this.Destroy();
        }
        public override void OnBreakState()
        {
            base.OnBreakState();
            if (Jaehoen==null)
                return;
            if (Jaehoen.passiveDetail.PassiveList.Find(x => x is PassiveAbility_1870001) is PassiveAbility_1870001 passive)
                passive.ReturnToActive();
            this.Destroy();
        }
        public override List<BattleUnitModel> GetFixedTarget()
        {
            if (Jaehoen==null)
                return base.GetFixedTarget();
            return BattleObjectManager.instance.GetAliveList_opponent(this._owner.faction).FindAll((Predicate<BattleUnitModel>)(x => x.bufListDetail.GetActivatedBuf(KeywordBuf.JaeheonMark) !=null));
        }
    }
}
