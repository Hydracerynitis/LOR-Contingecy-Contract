﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sound;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;
using BaseMod;
using static UnityEngine.UI.CanvasScaler;

namespace ContractReward
{
    public class PassiveAbility_1870003: PassiveAbilityBase
    {
        private BattleUnitModel Angelica;
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1870003));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1870003));
            this.rare = Rarity.Unique;
        }
        public void SetAngelica(BattleUnitModel angelica)
        {
            Angelica = angelica;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (behavior.Type != BehaviourType.Standby)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = 1
            });
        }
        public override void OnLoseParrying(BattleDiceBehavior behavior)
        {
            base.OnLoseParrying(behavior);
            if (behavior.Type != BehaviourType.Standby)
                return;
            if(Angelica!=null)
                Angelica.TakeBreakDamage(8);
            this.owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.PrintSound));
        }
        public override void OnBreakState()
        {
            base.OnBreakState();
            if (Angelica != null)
            {
                Angelica.TakeBreakDamage(Angelica.breakDetail.breakGauge);
                this.PrintSound();
            }

        }
        private void PrintSound() => SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Battle/Puppet_Break");
    }
}
