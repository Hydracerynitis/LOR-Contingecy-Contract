using System;
using System.Collections.Generic;
using LOR_DiceSystem;
using HarmonyLib;
using UI;
using System.Reflection;
using System.Linq;
using System.Text;
using BaseMod;

namespace ContractReward
{
    public class PassiveAbility_1870001: PassiveAbilityBase
    {
        private BattleAllyCardDetail Active;
        private BattleAllyCardDetail Passive;
        private BattleUnitModel Angelica;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.owner.personalEgoDetail.AddCard(Tools.MakeLorId(18700101));
            Angelica = null;
            Active = this.owner.allyCardDetail;
            Passive = new BattleAllyCardDetail(this.owner);
            List<DiceCardXmlInfo> list = new List<DiceCardXmlInfo>();
            foreach (LorId i in Singleton<DeckXmlList>.Instance.GetData(Tools.MakeLorId(18700001)).cardIdList)
                list.Add(ItemXmlDataList.instance.GetCardItem(i));
            Passive.Init(list);
        }
        public void Revive()
        {
            List<BattleUnitModel> Dead = BattleObjectManager.instance.GetFriendlyAllList(this.owner.faction).FindAll(x =>x.IsDead()==true);
            Angelica = RandomUtil.SelectOne(Dead);
            BookModel puppet=new BookModel(Singleton<BookXmlList>.Instance.GetData(Tools.MakeLorId(18710000)));
            Contingecy_Contract.CCInitializer.UnitBookId.Add(Angelica, Angelica.Book.GetBookClassInfoId());
            Angelica.Book.SetHp(puppet.HP);
            Angelica.Book.SetBp(puppet.Break);
            Angelica.Book.SetSpeedDiceMax(puppet.SpeedMax);
            Angelica.Book.SetSpeedDiceMin(puppet.SpeedMin);
            Angelica.Book.SetResistHP(BehaviourDetail.Slash, puppet.GetResistHP(BehaviourDetail.Slash));
            Angelica.Book.SetResistHP(BehaviourDetail.Penetrate, puppet.GetResistHP(BehaviourDetail.Penetrate));
            Angelica.Book.SetResistHP(BehaviourDetail.Hit, puppet.GetResistHP(BehaviourDetail.Hit));
            Angelica.Book.SetResistBP(BehaviourDetail.Slash, puppet.GetResistBP(BehaviourDetail.Slash));
            Angelica.Book.SetResistBP(BehaviourDetail.Penetrate, puppet.GetResistBP(BehaviourDetail.Penetrate));
            Angelica.Book.SetResistBP(BehaviourDetail.Hit, puppet.GetResistBP(BehaviourDetail.Hit));
            Angelica.Book._maxPlayPoint = puppet.GetMaxPlayPoint();
            Angelica.Book.ClassInfo._id = puppet.GetBookClassInfoId().id;
            Angelica.Book.ClassInfo.workshopID= puppet.GetBookClassInfoId().packageId;
            Angelica.view.ChangeSkin(puppet.GetCharacterName());
            Angelica.view.ChangeHeight(250);
            Angelica.formation = Singleton<StageController>.Instance.GetCurrentStageFloorModel().GetFormationPosition(Angelica.index);
            Angelica.view.charAppearance.ChangeMotion(ActionDetail.Standing);
            Angelica.cardSlotDetail.LosePlayPoint(Angelica.cardSlotDetail.GetMaxPlayPoint());
            Angelica.cardSlotDetail.RecoverPlayPoint(Angelica.cardSlotDetail.GetMaxPlayPoint());
            Angelica.Revive(Angelica.Book.HP);
            List<DiceCardXmlInfo> Decklist = new List<DiceCardXmlInfo>();
            foreach (LorId i in Singleton<DeckXmlList>.Instance.GetData(Tools.MakeLorId(18710000)).cardIdList)
                Decklist.Add(ItemXmlDataList.instance.GetCardItem(i));
            Angelica.allyCardDetail.Init(Decklist);
            Angelica.allyCardDetail.DrawCards(8);
            Angelica.personalEgoDetail = new BattlePersonalEgoCardDetail(Angelica);
            Angelica.personalEgoDetail.Init();
            Angelica.breakDetail.nextTurnBreak = false;
            Angelica.breakDetail.RecoverBreakLife(1, true);
            Angelica.breakDetail.ResetBreakDefault();
            Angelica.bufListDetail.RemoveBufAll();
            Angelica.bufListDetail.AddBuf(new BattleUnitBuf_AngelicaPuppet(this.owner));
            Angelica.passiveDetail.PassiveList.Clear();
            Angelica.turnState = BattleUnitTurnState.WAIT_CARD;
            Angelica.OnRoundStartOnlyUI();
            Angelica.RollSpeedDice();
            Angelica.view.EnableView(true);
            Angelica.view.StartEgoSkinChangeEffect("Character");
            Angelica.moveDetail.ReturnToFormationByBlink();
            List<PassiveAbilityBase> list1 = Angelica.passiveDetail.PassiveList;
            list1.Add(new PassiveAbility_1800000(Angelica));
            list1.Add(new PassiveAbility_1870101(Angelica));
            list1.Add(new PassiveAbility_1870102(Angelica));
            list1.Add(new PassiveAbility_1870103(Angelica));
            Angelica.passiveDetail._passiveList = list1;
            Contingecy_Contract.ContractAttribution.Inition.Remove(Angelica);
            Contingecy_Contract.ContractAttribution.Init(Angelica);
            int num = 0;
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetList())
                SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnitModel.UnitData.unitData, num++,renderRealtime: true);
            RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction)).bufListDetail.AddBuf(new BattleUnitBuf_AttackTarget());
            BattleObjectManager.instance.InitUI();
            this.owner.personalEgoDetail.AddCard(Tools.MakeLorId(18700103));
            Active = this.owner.allyCardDetail;
            this.owner.allyCardDetail = Passive;
            this.owner.allyCardDetail.DrawCards(8);
            List<PassiveAbilityBase> list2 = this.owner.passiveDetail.PassiveList;
            PassiveAbilityBase active = list2.Find(x => x is PassiveAbility_1870002);
            PassiveAbilityBase passive = new PassiveAbility_1870003(this.owner,Angelica);
            list2.Remove(active);
            list2.Add(passive);
            owner.passiveDetail._passiveList= list2;
        }
        public void ReturnToActive()
        {
            this.owner.personalEgoDetail.AddCard(Tools.MakeLorId(18700102));
            this.owner.personalEgoDetail.RemoveCard(Tools.MakeLorId(18700103));
            Passive = this.owner.allyCardDetail;
            this.owner.allyCardDetail = Active;
            this.owner.allyCardDetail.DrawCards(7);
            List<PassiveAbilityBase> list = this.owner.passiveDetail.PassiveList;
            PassiveAbilityBase passive = list.Find(x => x is PassiveAbility_1870003);
            PassiveAbilityBase active = new PassiveAbility_1870002(this.owner);
            list.Remove(passive);
            list.Add(active);
            owner.passiveDetail._passiveList = list;
        }
        public void ReturnToPassive()
        {
            Angelica.OnRoundStartOnlyUI();
            Angelica.RollSpeedDice();
            Angelica.view.charAppearance.ChangeMotion(ActionDetail.Default);
            this.owner.personalEgoDetail.AddCard(Tools.MakeLorId(18700103));
            Active = this.owner.allyCardDetail;
            this.owner.allyCardDetail = Passive;
            this.owner.allyCardDetail.DrawCards(8);
            List<PassiveAbilityBase> list = this.owner.passiveDetail.PassiveList;
            PassiveAbilityBase active = list.Find(x => x is PassiveAbility_1870002);
            PassiveAbilityBase passive = new PassiveAbility_1870003(owner,Angelica);
            list.Remove(active);
            list.Add(passive);
            owner.passiveDetail._passiveList = list;
        }
        public override void OnDie()
        {
            base.OnDie();
            if(Angelica!=null)
                Angelica.Die();
        }
    }
}
