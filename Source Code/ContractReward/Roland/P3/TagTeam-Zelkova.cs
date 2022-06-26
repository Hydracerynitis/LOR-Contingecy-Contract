using System;
using System.Collections.Generic;
using LOR_DiceSystem;
using Contingecy_Contract;
using BaseMod;

namespace ContractReward
{
    public class DiceCardSelfAbility_RolandZelkova : DiceCardSelfAbility_drawCard, TagTeam
    {
        public override string[] Keywords => new string[] { "Combo_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            this.TriggerTagTeam(owner.faction, Tools.MakeLorId(17000234));
        }
        public void TagTeamEffect(BattleUnitModel TagTeamMate)
        {
            DiceBehaviour dice = new DiceBehaviour()
            {
                Min=4, Dice=8, Type=BehaviourType.Atk,Detail=BehaviourDetail.Slash,MotionDetail=MotionDetail.H,EffectRes= "BS3DurandalHit_H"
            };
            BattleDiceBehavior diceBehavior = new BattleDiceBehavior() { behaviourInCard=dice};
            diceBehavior.abilityList.Add(new DiceCardAbility_bleeding3atk() { behavior = diceBehavior });
            card.AddDiceFront(diceBehavior);
        }
    }
    public class DiceCardSelfAbility_AngelicaZelkova : DiceCardSelfAbility_drawCard, TagTeam
    {
        public override string[] Keywords => new string[] { "Combo_Keyword" };
        public override void OnUseCard()
        {
            base.OnUseCard();
            this.TriggerTagTeam(owner.faction, Tools.MakeLorId(17000134));
        }
        public void TagTeamEffect(BattleUnitModel TagTeamMate)
        {
            DiceBehaviour dice = new DiceBehaviour()
            {
                Min = 3,
                Dice = 8,
                Type = BehaviourType.Atk,
                Detail = BehaviourDetail.Slash,
                MotionDetail = MotionDetail.H,
                EffectRes = "AngelicaAxe_H"
            };
            BattleDiceBehavior diceBehavior = new BattleDiceBehavior() { behaviourInCard = dice };
            diceBehavior.abilityList.Add(new DiceCardAbility_recoverBreak5atk() { behavior = diceBehavior });
            card.AddDice(diceBehavior);
        }
    }
}
