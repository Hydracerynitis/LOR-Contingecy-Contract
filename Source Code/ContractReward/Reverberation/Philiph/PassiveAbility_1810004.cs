using System;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class PassiveAbility_1810004: PassiveAbilityBase
    {
        private Battle.CreatureEffect.CreatureEffect _effect;
        public PassiveAbility_1810004(BattleUnitModel unit)
        {
            this.owner = unit;
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1810004));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1810004));
            this.rare = Rarity.Unique;
            this._effect = SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("Philip/Philip_Aura_Body", 1f, unit.view, unit.view);
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            behavior?.card?.target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, 2, this.owner);
        }

        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            base.OnTakeDamageByAttack(atkDice, dmg);
            atkDice?.owner?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, 2, this.owner);
            this.owner.battleCardResultLog?.SetCreatureEffectSound("Battle/Philip_Hit");
        }
        public void Destroy()
        {
            if (!((UnityEngine.Object)this._effect != (UnityEngine.Object)null))
                return;
            this._effect.ManualDestroy();
            this._effect = (Battle.CreatureEffect.CreatureEffect)null;
        }
        public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
        {
            if (detail == BehaviourDetail.Slash)
                return AtkResist.Normal;
            return base.GetResistHP(origin, detail);
        }
        public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
        {
            if (detail == BehaviourDetail.Slash)
                return AtkResist.Normal;
            return base.GetResistHP(origin, detail);
        }
    }
}
