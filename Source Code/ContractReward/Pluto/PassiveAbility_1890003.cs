using System;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1890003 : PassiveAbilityBase
    {
        private BehaviourDetail immuneDetail;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            immuneDetail = RandomUtil.SelectOne<BehaviourDetail>(BehaviourDetail.Slash, BehaviourDetail.Penetrate, BehaviourDetail.Hit);
        }
        public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
        {
            if (detail == immuneDetail)
                return AtkResist.Immune;
            return base.GetResistHP(origin, detail);
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.owner.allyCardDetail.AddNewCardToDeck(18900005);
        }
    }
}
