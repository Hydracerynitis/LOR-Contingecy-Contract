using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;
using Contingecy_Contract;
using static UnityEngine.UI.CanvasScaler;

namespace ContractReward
{
    public class PassiveAbility_1990012 : PassiveAbilityBase
    {
        public void CopyUnit(BattleUnitModel target)
        {
            if (target == null)
                return;
            owner.UnitData.unitData._customizeData = new UnitCustomizingData(new LorId(1409021),false);
            this.owner.UnitData.unitData.customizeData.SetCustomData(true);
            if (target.Book.IsWorkshop)
                owner.view.ChangeSkin(target.Book.GetBookClassInfoId().packageId + ":" + target.Book.GetCharacterName());
            else
                this.owner.view.ChangeSkin(target.Book.GetCharacterName());
            foreach (BattleDiceCardModel battleDiceCardModel in target.allyCardDetail.GetHand())
            {
                if (battleDiceCardModel.GetSpec().Ranged != CardRange.Instance)
                    this.owner.allyCardDetail.AddNewCard(battleDiceCardModel.GetID());
            }
            foreach (BattleDiceCardModel battleDiceCardModel in target.allyCardDetail.GetDiscarded())
            {
                if (battleDiceCardModel.GetSpec().Ranged != CardRange.Instance)
                    this.owner.allyCardDetail.AddNewCardToDiscarded(battleDiceCardModel.GetID());
            }
            foreach (BattleDiceCardModel battleDiceCardModel in target.allyCardDetail.GetDeck())
            {
                if (battleDiceCardModel.GetSpec().Ranged != CardRange.Instance)
                    this.owner.allyCardDetail.AddNewCardToDeck(battleDiceCardModel.GetID());
            }
            foreach (PassiveAbilityBase passive in target.passiveDetail.PassiveList)
            {
                if (!(passive is ContingecyContract))
                {
                    PassiveAbilityBase instance = Activator.CreateInstance(passive.GetType()) as PassiveAbilityBase;       
                    if (!owner.passiveDetail.PassiveList.Exists(x => x.GetType() == instance.GetType()))
                    {
                        owner.passiveDetail.AddPassive(instance);
                        instance.rare = passive.rare;
                        instance.name = passive.name;
                        instance.desc = passive.desc;
                    }
                }
            }
            owner.passiveDetail.OnCreated();
            owner.OnRoundStartOnlyUI();
            owner.RollSpeedDice();
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            self.personalEgoDetail.AddCard(Tools.MakeLorId(19900102));
            self.personalEgoDetail.AddCard(Tools.MakeLorId(19900103));
        }
        public override void OnRoundEnd()
        {
            owner.personalEgoDetail.AddCard(Tools.MakeLorId(19900102));
            owner.personalEgoDetail.AddCard(Tools.MakeLorId(19900103));
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            this.owner.battleCardResultLog?.SetPassiveAbility(this);
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = 1
            });
        }
    }
}
