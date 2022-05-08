using System;
using System.Collections.Generic;
using LOR_DiceSystem;
using Contingecy_Contract;
using BaseMod;

namespace ContractReward
{
    public class DiceCardSelfAbility_RolandLogic : DiceCardSelfAbility_cardPowerDown2target, TagTeam
    {
        public override string[] Keywords => new string[] { "Combo_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            this.TriggerTagTeam(owner.faction, Tools.MakeLorId(17000236));
        }
        public void TagTeamEffect(BattleUnitModel TagTeamMate)
        {
            DiceBehaviour dice = new DiceBehaviour()
            {
                Min=7, Dice=11, Type=BehaviourType.Atk,
                Detail = BehaviourDetail.Penetrate,
                MotionDetail = MotionDetail.Z,
                EffectRes = "BS3DurandalPene_Z"
            };
            BattleDiceBehavior diceBehavior = new BattleDiceBehavior() { behaviourInCard=dice};
            diceBehavior.abilityList.Add(new DiceCardAbility_bind1paralysis1atk() { behavior = diceBehavior });
            card.AddDiceFront(diceBehavior);
            diceBehavior = new BattleDiceBehavior() { behaviourInCard = dice.Copy() };
            diceBehavior.abilityList.Add(new DiceCardAbility_bind1paralysis1atk() { behavior = diceBehavior });
            card.AddDiceFront(diceBehavior);

        }
    }
    public class DiceCardSelfAbility_AngelicaLogic : DiceCardSelfAbility_cardPowerDown2target, TagTeam
    {
        public override string[] Keywords => new string[] { "Combo_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            this.TriggerTagTeam(owner.faction, Tools.MakeLorId(17000136));
        }
        public void TagTeamEffect(BattleUnitModel TagTeamMate)
        {
            DiceBehaviour dice = new DiceBehaviour()
            {
                Min = 7,
                Dice = 11,
                Type = BehaviourType.Atk,
                Detail = BehaviourDetail.Hit,
                MotionDetail = MotionDetail.S1,
                EffectRes = "AngelicaGreatSword_S1"
            };
            BattleDiceBehavior diceBehavior = new BattleDiceBehavior() { behaviourInCard = dice };
            diceBehavior.abilityList.Add(new DiceCardAbility_weak1atk() { behavior = diceBehavior });
            card.AddDice(diceBehavior);
        }
    }
}
