using System;
using System.Collections.Generic;
using LOR_DiceSystem;
using HarmonyLib;
using UI;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.owner.personalEgoDetail.AddCard(18700101);
            Angelica = null;
            Active = this.owner.allyCardDetail;
            Passive = new BattleAllyCardDetail(this.owner);
            List<DiceCardXmlInfo> list = new List<DiceCardXmlInfo>();
            foreach (int i in Singleton<DeckXmlList>.Instance.GetData(18700001).cardIdList)
                list.Add(ItemXmlDataList.instance.GetCardItem(i));
            Passive.Init(list);
        }
        public void Revive()
        {
            List<BattleUnitModel> Dead = BattleObjectManager.instance.GetFriendlyAllList(this.owner.faction).FindAll((Predicate<BattleUnitModel>)(x =>x.IsDead()==true));
            Angelica = RandomUtil.SelectOne<BattleUnitModel>(Dead);
            BookModel puppet=new BookModel(Singleton<BookXmlList>.Instance.GetData(18710000));
            Contingecy_Contract.Harmony_Patch.UnitBookId.Add(Angelica, Angelica.Book.GetBookClassInfoId());
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
            Angelica.Book.GetType().GetField("_maxPlayPoint", AccessTools.all).SetValue(Angelica.Book, puppet.GetMaxPlayPoint());
            Angelica.Book.ClassInfo.id = puppet.GetBookClassInfoId();
            Angelica.view.ChangeSkin(puppet.GetCharacterName());
            Angelica.formation = Singleton<StageController>.Instance.GetCurrentStageFloorModel().GetFormationPosition(Angelica.index);
            Angelica.view.charAppearance.ChangeMotion(ActionDetail.Standing);
            Angelica.cardSlotDetail.LosePlayPoint(Angelica.cardSlotDetail.GetMaxPlayPoint());
            Angelica.cardSlotDetail.RecoverPlayPoint(Angelica.cardSlotDetail.GetMaxPlayPoint());
            Angelica.Revive(Angelica.Book.HP);
            List<DiceCardXmlInfo> Decklist = new List<DiceCardXmlInfo>();
            foreach (int i in Singleton<DeckXmlList>.Instance.GetData(18710000).cardIdList)
                Decklist.Add(ItemXmlDataList.instance.GetCardItem(i));
            Angelica.allyCardDetail.Init(Decklist);
            Angelica.allyCardDetail.DrawCards(8);
            Angelica.breakDetail.nextTurnBreak = false;
            Angelica.breakDetail.RecoverBreakLife(1, true);
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
            list1.Add(new PassiveAbility_1870102(Angelica,this.owner));
            list1.Add(new PassiveAbility_1870103(Angelica));
            typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue((object)Angelica.passiveDetail, (object)list1);
            Contingecy_Contract.ContractAttribution.Inition.Remove(Angelica);
            Contingecy_Contract.ContractAttribution.Init(Angelica);
            int num = 0;
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetList())
                SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnitModel.UnitData.unitData, num++,renderRealtime: true);
            RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction)).bufListDetail.AddBuf(new BattleUnitBuf_AttackTarget());
            BattleObjectManager.instance.InitUI();
            this.owner.personalEgoDetail.AddCard(18700103);
            Active = this.owner.allyCardDetail;
            this.owner.allyCardDetail = Passive;
            this.owner.allyCardDetail.DrawCards(8);
            List<PassiveAbilityBase> list2 = this.owner.passiveDetail.PassiveList;
            PassiveAbilityBase active = list2.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_1870002));
            PassiveAbilityBase passive = new PassiveAbility_1870003(this.owner,Angelica);
            list2.Remove(active);
            list2.Add(passive);
            typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue((object)this.owner.passiveDetail, (object)list2);
        }
        public void ReturnToActive()
        {
            this.owner.personalEgoDetail.AddCard(18700102);
            this.owner.personalEgoDetail.RemoveCard(18700103);
            Passive = this.owner.allyCardDetail;
            this.owner.allyCardDetail = Active;
            this.owner.allyCardDetail.DrawCards(7);
            List<PassiveAbilityBase> list = this.owner.passiveDetail.PassiveList;
            PassiveAbilityBase passive = list.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_1870003));
            PassiveAbilityBase active = new PassiveAbility_1870002(this.owner);
            list.Remove(passive);
            list.Add(active);
            typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue((object)this.owner.passiveDetail, (object)list);
        }
        public void ReturnToPassive()
        {
            Angelica.OnRoundStartOnlyUI();
            Angelica.RollSpeedDice();
            Angelica.view.charAppearance.ChangeMotion(ActionDetail.Default);
            this.owner.personalEgoDetail.AddCard(18700103);
            Active = this.owner.allyCardDetail;
            this.owner.allyCardDetail = Passive;
            this.owner.allyCardDetail.DrawCards(8);
            List<PassiveAbilityBase> list = this.owner.passiveDetail.PassiveList;
            PassiveAbilityBase active = list.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_1870002));
            PassiveAbilityBase passive = new PassiveAbility_1870003(this.owner,Angelica);
            list.Remove(active);
            list.Add(passive);
            typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue((object)this.owner.passiveDetail, (object)list);
        }
        public override void OnDie()
        {
            base.OnDie();
            if(Angelica!=null)
                Angelica.Die();
        }
    }
}
