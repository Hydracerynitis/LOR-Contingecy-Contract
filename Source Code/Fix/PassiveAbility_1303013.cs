﻿using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fix
{
    public class PassiveAbility_1303013_New : PassiveAbilityBase
    {
        private int _cardCount;
        private int _patternCount;
        private bool _init;
        private List<BattleUnitModel> _meat = new List<BattleUnitModel>();
        public override int SpeedDiceNumAdder()
        {
            if (this.owner.passiveDetail.PassiveList.Find(x => x is Contingecy_Contract.ContingecyContract_Greta_Feast) is Contingecy_Contract.ContingecyContract_Greta_Feast Feast && this._patternCount == 1)
                return Feast.GetSackNumAdder();
            return base.SpeedDiceNumAdder();
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!this._init)
            {
                this._init = true;
                Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/Greta/Greta_BloodEffect");
                if ((UnityEngine.Object)original != (UnityEngine.Object)null)
                    UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1, this.owner);
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.owner.faction))
                {
                    if (alive != this.owner)
                    {
                        this.owner.RecoverHP(Mathf.RoundToInt((float)this.owner.MaxHp * (float)((double)alive.hp / (double)alive.MaxHp * 0.1)));
                        alive.TakeDamage(999, DamageType.ETC, this.owner);
                    }
                }
            }
            List<BattleUnitModel> list = new List<BattleUnitModel>();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.owner.faction))
            {
                if (alive != this.owner && alive.IsTargetable(this.owner))
                    list.Add(alive);
            }
            if (list.Count > 0)
            {
                list[0].bufListDetail.AddBuf(new BattleUnitBuf_Greta_Meat());
                if (_patternCount == 0)
                    _patternCount = 1;
            }
            else
                this._patternCount = 0;
            if (this._patternCount == 1)
                new GameObject().AddComponent<SpriteFilter_Queenbee_Spore>().Init("EmotionCardFilter/RedHood_Filter", false, 2f);
            if (owner.passiveDetail.HasPassive<Contingecy_Contract.ContingecyContract_Greta>())
            {
                if (Singleton<StageController>.Instance.RoundTurn % 3 == 1 && (owner.bufListDetail.GetActivatedBuf(KeywordBuf.Resistance) == null || owner.bufListDetail.GetActivatedBuf(KeywordBuf.Resistance).stack == 0))
                {
                    this.owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
                    this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Resistance, 50, this.owner);
                }
                return;
            }
            if (Singleton<StageController>.Instance.RoundTurn % 5 != 1)
                return;
            this.owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
            this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Resistance, 10, this.owner);
        }
        public override BattleUnitModel ChangeAttackTarget(
          BattleDiceCardModel card,
          int idx)
        {
            if (card.GetID() == 703319 || card.GetID() == 703310)
            {
                this.owner.SetSpeedDiceValueAdder(idx, 1 - this.owner.GetSpeed(4));
                if(card.GetID()== 703319)
                {
                    List<BattleUnitModel> meats = BattleObjectManager.instance.GetAliveList_opponent(owner.faction);
                    List<BattlePlayingCardDataInUnitModel> cardArray = this.owner.cardSlotDetail.cardAry.FindAll(x => x != null);
                    List<BattlePlayingCardDataInUnitModel> sacks = cardArray.FindAll(x => x.card.GetID() == 703319);
                    foreach(BattlePlayingCardDataInUnitModel sack in sacks)
                        meats.Remove(sack.target);
                    return RandomUtil.SelectOne<BattleUnitModel>(meats);
                }
            }
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.owner.faction))
            {
                if (alive != this.owner && alive.IsTargetable(this.owner) && alive.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Greta_Meat)) != null)
                    return alive;
            }
            if (this.owner.passiveDetail.HasPassive<Contingecy_Contract.ContingecyContract_Greta_Salt>())
            {
                List<BattleUnitModel> PurpleTear = new List<BattleUnitModel>();
                foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
                {
                    if (!unit.bufListDetail.GetActivatedBufList().Exists(x => x.positiveType == BufPositiveType.Negative))
                        PurpleTear.Add(unit);
                }
                return RandomUtil.SelectOne<BattleUnitModel>(PurpleTear);
            }
            return base.ChangeAttackTarget(card, idx);
        }
        public override void OnRoundStartAfter()
        {
            this.SetCards_phase1();
            ++this._patternCount;
            this._patternCount %= 4;
        }
        private void SetCards_phase1()
        {
            this.owner.allyCardDetail.ExhaustAllCards();
            int num = this.owner.Book.GetSpeedDiceRule(this.owner).Roll(this.owner).Count - 5;
            this._cardCount = 0;
            switch (this._patternCount)
            {
                case 0:
                    this.AddNewCard(703315);
                    if ((double)RandomUtil.valueForProb < 0.5)
                        this.AddNewCard(703317);
                    else
                        this.AddNewCard(703313);
                    this.AddNewCard(703314);
                    this.AddNewCard(703314);
                    if (this.owner.passiveDetail.PassiveList.Find(x => x is Contingecy_Contract.ContingecyContract_Greta_Feast) is Contingecy_Contract.ContingecyContract_Greta_Feast Feast)
                        for (int i = 0; i < Feast.GetSackNumAdder(); i++)
                            this.AddNewCard(703319);
                    this.AddNewCard(703319);
                    break;
                case 1:
                case 2:
                    this.AddNewCard(703315);
                    if (!owner.passiveDetail.HasPassive<Contingecy_Contract.ContingecyContract_Greta_Salt>())
                        this.AddNewCard(703316);
                    if ((double)RandomUtil.valueForProb < 0.5)
                        this.AddNewCard(703317);
                    else
                        this.AddNewCard(703313);
                    this.AddNewCard(703314);
                    this.AddNewCard(703314);
                    if (owner.passiveDetail.HasPassive<Contingecy_Contract.ContingecyContract_Greta_Salt>())
                        this.AddNewCard(703310);
                    break;
                case 3:
                    this.AddNewCard(703318);
                    this.AddNewCard(703316);
                    this.AddNewCard(703314);
                    this.AddNewCard(703314);
                    this.AddNewCard(703310);
                    break;
            }
            for (int index = 0; index < num; ++index)
                this.AddNewCard(this.GetAddedDiceCard());
        }
        private void AddNewCard(int id)
        {
            this.owner.allyCardDetail.AddTempCard(id).SetPriorityAdder(1000 - this._cardCount * 100);
            ++this._cardCount;
        }
        private int GetAddedDiceCard()
        {
            int[] numArray = new int[2] { 703311, 703314 };
            int index = (double)RandomUtil.valueForProb < 0.5 ? 0 : 1;
            if (this._patternCount > 1)
                index = 1;
            return numArray[index];
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            BattleUnitModel target = curCard.target;
            if (target == null || curCard.card.GetID() != 703319 || target.faction == this.owner.faction)
                return;
            this._meat.Add(target);
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (behavior.card.card.GetID() == 703310 && behavior.card.target.UnitData.unitData.EnemyUnitId == 1303021)
                behavior.card.target.Die();
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            if (_meat.Count > 0)
            {
                for (int i = 0; i < _meat.Count; i++)
                {
                    BattleUnitModel meat = _meat[i];
                    if (BattleObjectManager.instance.GetList(this.owner.faction).Find(x => x.IsDead()) == null)
                    {
                        meat.view.EnableView(true);
                        meat.Extinct(false);
                    }
                    else if (meat.faction != owner.faction && !meat.IsDead())
                    {
                        int index = BattleObjectManager.instance.GetList(this.owner.faction).Find(x => x.IsDead()).index;
                        BattleUnitModel NewMeat = Singleton<StageController>.Instance.AddNewUnit(this.owner.faction, 1303021, index);
                        if (NewMeat != null)
                        {
                            int num = 0;
                            NewMeat.passiveDetail.AddPassive(new PassiveAbility_1303021());
                            BattleUnitBuf_Greta_Meat_Librarian gretaMeatLibrarian = new BattleUnitBuf_Greta_Meat_Librarian(meat);
                            NewMeat.bufListDetail.AddBuf(gretaMeatLibrarian);
                            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetList())
                                SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnitModel.UnitData.unitData, num++);
                            BattleObjectManager.instance.InitUI();
                        }
                    }
                }
                this._meat.Clear();
            }
        }
        public override bool AllowTargetChanging(BattleUnitModel attacker, int myCardSlotIdx)
        {
            if (owner.cardSlotDetail.cardAry[myCardSlotIdx] != null && owner.cardSlotDetail.cardAry[myCardSlotIdx].card.GetID() == 703319)
            {
                List<BattlePlayingCardDataInUnitModel> cardArray = this.owner.cardSlotDetail.cardAry.FindAll(x => x != null);
                List<BattlePlayingCardDataInUnitModel> sack = cardArray.FindAll(x => x.card.GetID() == 703319);
                return !sack.Exists(x => x.target == attacker);
            }
            return base.AllowTargetChanging(attacker, myCardSlotIdx);
        }
    }

}
