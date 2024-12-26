using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1870102: PassiveAbilityBase
    {
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1870102));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1870102));
            this.rare = Rarity.Rare;
        }
        public override void OnBattleEnd_alive()
        {
            base.OnBattleEnd_alive();
            this.owner.DieFake();
        }
        public override bool isImmortal => true;
        public override int SpeedDiceBreakedAdder() => HasPuppet()? 0: 10;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!HasPuppet())
            {
                this.owner.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
                return;
            }
            this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1);
            this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 1);
            this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 1);
        }
        public override void OnKill(BattleUnitModel target)
        {
            base.OnKill(target);
            RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction)).bufListDetail.AddBuf(new BattleUnitBuf_AttackTarget());
        }
        private bool HasPuppet() => this.owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_AngelicaPuppet) != null;
    }
}
