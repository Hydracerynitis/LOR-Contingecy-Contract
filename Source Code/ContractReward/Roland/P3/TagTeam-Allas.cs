using System;
using System.Collections.Generic;
using LOR_DiceSystem;
using Contingecy_Contract;
using BaseMod;

namespace ContractReward
{
    public class DiceCardSelfAbility_RolandAllas : DiceCardSelfAbility_cardPowerDown2target, TagTeam
    {
        public override string[] Keywords => new string[] { "Combo_Keyword"};
        public override void OnUseCard()
        {
            base.OnUseCard();
            this.TriggerTagTeam(owner.faction, Tools.MakeLorId(17000235));
        }
        public void TagTeamEffect(BattleUnitModel TagTeamMate)
        {
            DiceBehaviour dice = new DiceBehaviour()
            {
                Min=5, Dice=8, Type=BehaviourType.Atk,
                Detail = BehaviourDetail.Penetrate,
                MotionDetail = MotionDetail.Z,
                EffectRes = "BS3DurandalPene_Z"
            };
            BattleDiceBehavior diceBehavior = new BattleDiceBehavior() { behaviourInCard=dice};
            diceBehavior.abilityList.Add(new DiceCardAbility_protection2atkMostHp() { behavior = diceBehavior });
            card.AddDiceFront(diceBehavior);
        }
    }
    public class DiceCardSelfAbility_AngelicaAllas : DiceCardSelfAbility_cardPowerDown2target, TagTeam
    {
        public override string[] Keywords => new string[] { "Combo_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            this.TriggerTagTeam(owner.faction, Tools.MakeLorId(17000135));
        }
        public void TagTeamEffect(BattleUnitModel TagTeamMate)
        {
            DiceBehaviour dice = new DiceBehaviour()
            {
                Min = 4,
                Dice = 8,
                Type = BehaviourType.Atk,
                Detail = BehaviourDetail.Penetrate,
                MotionDetail = MotionDetail.Z,
                EffectRes = "AngelicaLance_Z"
            };
            BattleDiceBehavior diceBehavior = new BattleDiceBehavior() { behaviourInCard = dice };
            diceBehavior.abilityList.Add(new DiceCardAbility_weak1atk() { behavior = diceBehavior });
            card.AddDice(diceBehavior);
        }
    }
}
