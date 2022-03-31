using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class BattleUnitBuf_FreshMeat: BattleUnitBuf
    {
        private double damage;
        private double threshold;
        public override string keywordIconId => "Greta_Meat_Librarian";
        public override string keywordId => "GretaMeat";
        public override bool IsActionable() => false;
        public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail) => AtkResist.Normal;
        public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail) => AtkResist.Normal;
        public override bool IsInvincibleBp(BattleUnitModel attacker) => true;
        public override double ChangeDamage(BattleUnitModel attacker, double dmg)
        {
            attacker.RecoverHP((int)dmg);
            if (damage >= threshold)
                return 0;
            else if (damage + dmg >= threshold)
            {
                double output = threshold - damage;
                damage = threshold;
                return output;
            }
            damage += dmg;
            return base.ChangeDamage(attacker, dmg);
        }
        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            damage = 0;
            threshold = 0.1 * this._owner.MaxHp;
            owner.view.SetAltSkin("Blue_Greta_Meat");
            this.stack = 2;
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            this.stack--;
            if (this.stack <= 0)
            {
                this._owner.view.CreateSkin();
                this.Destroy();
            }
        }
    }
}
