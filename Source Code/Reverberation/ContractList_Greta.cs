using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI;
using UnityEngine;

namespace Contingecy_Contract
{
    //Replace Passive
    public class CC_Greta_Phase1 : PassiveAbilityBase
    {
        private int _cardCount;
        private int _patternCount;
        private List<BattleUnitModel> _meat;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _meat = new List<BattleUnitModel>();
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Resistance, 10, owner);
        }
        public override int SpeedDiceNumAdder()
        {
            if (owner.passiveDetail.PassiveList.Find(x => x is Contingecy_Contract.ContingecyContract_Greta_Feast) is Contingecy_Contract.ContingecyContract_Greta_Feast Feast && _patternCount == 2)
                return Feast.GetSackNumAdder();
            return base.SpeedDiceNumAdder();
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            if (_meat.Count > 0)
            {
                for (int i = 0; i < _meat.Count; i++)
                {
                    BattleUnitModel meat = _meat[i];
                    if (BattleObjectManager.instance.GetList(owner.faction).Find(x => x.IsDead()) == null)
                    {
                        meat.view.EnableView(true);
                        meat.Extinct(false);
                    }
                    else if (meat.faction != owner.faction && !meat.IsDead())
                    {
                        int index = BattleObjectManager.instance.GetList(owner.faction).Find(x => x.IsDead()).index;
                        BattleUnitModel NewMeat = Singleton<StageController>.Instance.AddNewUnit(owner.faction, 1303021, index);
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
                _meat.Clear();
            }
            if (!owner.passiveDetail.HasPassive<Contingecy_Contract.ContingecyContract_Greta_Feast>() && (double)owner.hp > 0.4 * owner.MaxHp)
                return;
            if ((double)owner.hp > 0.6 * owner.MaxHp)
                return;
            owner.passiveDetail.AddPassive(new CC_Greta_Phase2() { 
                rare = Rarity.Rare,
                desc=PassiveDescXmlList.Instance.GetDesc(1303013),
                name=PassiveDescXmlList.Instance.GetName(1303013)
            });
            owner.passiveDetail.DestroyPassive(this);
        }
        public override int GetMinHp()
        {
            return owner.passiveDetail.HasPassive<Contingecy_Contract.ContingecyContract_Greta_Feast>() ? (int)(owner.MaxHp * 0.5) : (int)(owner.MaxHp * 0.3);
        }
        public override BattleUnitModel ChangeAttackTarget(
          BattleDiceCardModel card,
          int idx)
        {
            if (card.GetID() == 703319 || card.GetID() == 703310)
            {
                owner.SetSpeedDiceValueAdder(idx, 1 - owner.GetSpeed(4));
                if (card.GetID() == 703319)
                {
                    List<BattleUnitModel> meats = BattleObjectManager.instance.GetAliveList_opponent(owner.faction);
                    List<BattlePlayingCardDataInUnitModel> cardArray = owner.cardSlotDetail.cardAry.FindAll(x => x != null);
                    List<BattlePlayingCardDataInUnitModel> sacks = cardArray.FindAll(x => x.card.GetID() == 703319);
                    foreach (BattlePlayingCardDataInUnitModel sack in sacks)
                        meats.Remove(sack.target);
                    return RandomUtil.SelectOne<BattleUnitModel>(meats);
                }
            }
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (alive != owner && alive.IsTargetable(owner) && alive.bufListDetail.GetActivatedBufList().Find((x => x is BattleUnitBuf_Greta_Meat)) != null)
                    return alive;
            }
            if (owner.passiveDetail.HasPassive<Contingecy_Contract.ContingecyContract_Greta_Salt>())
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
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_patternCount > 1)
            {
                List<BattleUnitModel> list = new List<BattleUnitModel>();
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(owner.faction))
                {
                    if (alive != owner && alive.IsTargetable(owner))
                        list.Add(alive);
                }
                if (list.Count > 0)
                    RandomUtil.SelectOne<BattleUnitModel>(list).bufListDetail.AddBuf(new BattleUnitBuf_Greta_Meat());
                else
                    _patternCount = 0;
            }
            if (_patternCount == 2)
                new GameObject().AddComponent<SpriteFilter_Queenbee_Spore>().Init("EmotionCardFilter/RedHood_Filter", false, 2f);
            if (owner.passiveDetail.HasPassive<Contingecy_Contract.ContingecyContract_Greta>())
            {
                if (Singleton<StageController>.Instance.RoundTurn % 3 == 1 && (owner.bufListDetail.GetActivatedBuf(KeywordBuf.Resistance) == null || owner.bufListDetail.GetActivatedBuf(KeywordBuf.Resistance).stack == 0))
                {
                    owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
                    owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Resistance, 50, owner);
                }
                return;
            }
            if (Singleton<StageController>.Instance.RoundTurn % 5 != 1)
                return;
            owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Resistance, 10, owner);
        }
        public override void OnRoundStartAfter()
        {
            SetCards_phase1();
            ++_patternCount;
            _patternCount %= 5;
        }
        private void SetCards_phase1()
        {
            owner.allyCardDetail.ExhaustAllCards();
            int num = owner.Book.GetSpeedDiceRule(owner).Roll(owner).Count - 5;
            _cardCount = 0;
            switch (_patternCount)
            {
                case 0:
                    AddNewCard(703312);
                    AddNewCard(703312);
                    AddNewCard(703313);
                    AddNewCard(703313);
                    AddNewCard(703311);
                    break;
                case 1:
                    AddNewCard(703312);
                    AddNewCard(703312);
                    AddNewCard(703313);
                    if (BattleObjectManager.instance.GetAliveList(owner.faction).Count <= 1)
                    {
                        AddNewCard(703311);
                        if (owner.passiveDetail.PassiveList.Find(x => x is Contingecy_Contract.ContingecyContract_Greta_Feast) is Contingecy_Contract.ContingecyContract_Greta_Feast Feast)
                            for (int i = 0; i < Feast.GetSackNumAdder(); i++)
                                AddNewCard(703319);
                        AddNewCard(703319);
                        break;
                    }
                    AddNewCard(703313);
                    AddNewCard(703311);
                    break;
                case 2:
                    AddNewCard(703315);
                    AddNewCard(703315);
                    if (!owner.passiveDetail.HasPassive<Contingecy_Contract.ContingecyContract_Greta_Salt>())
                        AddNewCard(703316);
                    AddNewCard(703317);
                    AddNewCard(703314);
                    if (owner.passiveDetail.HasPassive<Contingecy_Contract.ContingecyContract_Greta_Salt>())
                        AddNewCard(703310);
                    break;
                case 3:
                    AddNewCard(703315);
                    if (!owner.passiveDetail.HasPassive<Contingecy_Contract.ContingecyContract_Greta_Salt>())
                        AddNewCard(703316);
                    AddNewCard(703317);
                    AddNewCard(703314);
                    AddNewCard(703314);
                    if (owner.passiveDetail.HasPassive<Contingecy_Contract.ContingecyContract_Greta_Salt>())
                        AddNewCard(703310);
                    break;
                case 4:
                    AddNewCard(703315);
                    AddNewCard(703317);
                    AddNewCard(703314);
                    AddNewCard(703314);
                    AddNewCard(703310);
                    break;
            }
            for (int index = 0; index < num; ++index)
                AddNewCard(GetAddedDiceCard());
        }
        private void AddNewCard(int id)
        {
            owner.allyCardDetail.AddTempCard(id).SetPriorityAdder(1000 - _cardCount * 100);
            ++_cardCount;
        }
        private int GetAddedDiceCard() => new int[2]
        {
            703311,
            703314
        }[(double)RandomUtil.valueForProb < 0.5 ? 0 : 1];
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            BattleUnitModel target = curCard.target;
            if (target == null || curCard.card.GetID() != 703319 || target.faction == owner.faction)
                return;
            _meat.Add(target);
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (behavior.card.card.GetID() == 703310 && behavior.card.target.UnitData.unitData.EnemyUnitId == 1303021)
                behavior.card.target.Die();
        }
        public override void OnDie()
        {
            base.OnDie();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (alive == owner)
                    alive.Die();
            }
        }
        public override bool AllowTargetChanging(BattleUnitModel attacker, int myCardSlotIdx)
        {
            if (owner.cardSlotDetail.cardAry[myCardSlotIdx] != null && owner.cardSlotDetail.cardAry[myCardSlotIdx].card.GetID() == 703319)
            {
                List<BattlePlayingCardDataInUnitModel> cardArray = owner.cardSlotDetail.cardAry.FindAll(x => x != null);
                List<BattlePlayingCardDataInUnitModel> sack = cardArray.FindAll(x => x.card.GetID() == 703319);
                return !sack.Exists(x => x.target == attacker);
            }
            return base.AllowTargetChanging(attacker, myCardSlotIdx);
        }
    }
    public class CC_Greta_Phase2 : PassiveAbilityBase
    {
        private int _cardCount;
        private int _patternCount;
        private bool _init;
        private List<BattleUnitModel> _meat = new List<BattleUnitModel>();
        public override int SpeedDiceNumAdder()
        {
            if (owner.passiveDetail.PassiveList.Find(x => x is Contingecy_Contract.ContingecyContract_Greta_Feast) is Contingecy_Contract.ContingecyContract_Greta_Feast Feast && _patternCount == 1)
                return Feast.GetSackNumAdder();
            return base.SpeedDiceNumAdder();
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!_init)
            {
                _init = true;
                Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/Greta/Greta_BloodEffect");
                if ((UnityEngine.Object)original != (UnityEngine.Object)null)
                    UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1, owner);
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(owner.faction))
                {
                    if (alive != owner)
                    {
                        owner.RecoverHP(Mathf.RoundToInt((float)owner.MaxHp * (float)((double)alive.hp / (double)alive.MaxHp * 0.1)));
                        alive.TakeDamage(999, DamageType.ETC, owner);
                    }
                }
            }
            List<BattleUnitModel> list = new List<BattleUnitModel>();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (alive != owner && alive.IsTargetable(owner))
                    list.Add(alive);
            }
            if (list.Count > 0)
            {
                list[0].bufListDetail.AddBuf(new BattleUnitBuf_Greta_Meat());
                if (_patternCount == 0)
                    _patternCount = 1;
            }
            else
                _patternCount = 0;
            if (_patternCount == 1)
                new GameObject().AddComponent<SpriteFilter_Queenbee_Spore>().Init("EmotionCardFilter/RedHood_Filter", false, 2f);
            if (owner.passiveDetail.HasPassive<Contingecy_Contract.ContingecyContract_Greta>())
            {
                if (Singleton<StageController>.Instance.RoundTurn % 3 == 1 && (owner.bufListDetail.GetActivatedBuf(KeywordBuf.Resistance) == null || owner.bufListDetail.GetActivatedBuf(KeywordBuf.Resistance).stack == 0))
                {
                    owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
                    owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Resistance, 50, owner);
                }
                return;
            }
            if (Singleton<StageController>.Instance.RoundTurn % 5 != 1)
                return;
            owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Resistance, 10, owner);
        }
        public override BattleUnitModel ChangeAttackTarget(
          BattleDiceCardModel card,
          int idx)
        {
            if (card.GetID() == 703319 || card.GetID() == 703310)
            {
                owner.SetSpeedDiceValueAdder(idx, 1 - owner.GetSpeed(4));
                if (card.GetID() == 703319)
                {
                    List<BattleUnitModel> meats = BattleObjectManager.instance.GetAliveList_opponent(owner.faction);
                    List<BattlePlayingCardDataInUnitModel> cardArray = owner.cardSlotDetail.cardAry.FindAll(x => x != null);
                    List<BattlePlayingCardDataInUnitModel> sacks = cardArray.FindAll(x => x.card.GetID() == 703319);
                    foreach (BattlePlayingCardDataInUnitModel sack in sacks)
                        meats.Remove(sack.target);
                    return RandomUtil.SelectOne<BattleUnitModel>(meats);
                }
            }
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (alive != owner && alive.IsTargetable(owner) && alive.bufListDetail.GetActivatedBufList().Find((Predicate<BattleUnitBuf>)(x => x is BattleUnitBuf_Greta_Meat)) != null)
                    return alive;
            }
            if (owner.passiveDetail.HasPassive<Contingecy_Contract.ContingecyContract_Greta_Salt>())
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
            SetCards_phase1();
            ++_patternCount;
            _patternCount %= 4;
        }
        private void SetCards_phase1()
        {
            owner.allyCardDetail.ExhaustAllCards();
            int num = owner.Book.GetSpeedDiceRule(owner).Roll(owner).Count - 5;
            _cardCount = 0;
            switch (_patternCount)
            {
                case 0:
                    AddNewCard(703315);
                    if ((double)RandomUtil.valueForProb < 0.5)
                        AddNewCard(703317);
                    else
                        AddNewCard(703313);
                    AddNewCard(703314);
                    AddNewCard(703314);
                    if (owner.passiveDetail.PassiveList.Find(x => x is Contingecy_Contract.ContingecyContract_Greta_Feast) is Contingecy_Contract.ContingecyContract_Greta_Feast Feast)
                        for (int i = 0; i < Feast.GetSackNumAdder(); i++)
                            AddNewCard(703319);
                    AddNewCard(703319);
                    break;
                case 1:
                case 2:
                    AddNewCard(703315);
                    if (!owner.passiveDetail.HasPassive<Contingecy_Contract.ContingecyContract_Greta_Salt>())
                        AddNewCard(703316);
                    if ((double)RandomUtil.valueForProb < 0.5)
                        AddNewCard(703317);
                    else
                        AddNewCard(703313);
                    AddNewCard(703314);
                    AddNewCard(703314);
                    if (owner.passiveDetail.HasPassive<Contingecy_Contract.ContingecyContract_Greta_Salt>())
                        AddNewCard(703310);
                    break;
                case 3:
                    AddNewCard(703318);
                    AddNewCard(703316);
                    AddNewCard(703314);
                    AddNewCard(703314);
                    AddNewCard(703310);
                    break;
            }
            for (int index = 0; index < num; ++index)
                AddNewCard(GetAddedDiceCard());
        }
        private void AddNewCard(int id)
        {
            owner.allyCardDetail.AddTempCard(id).SetPriorityAdder(1000 - _cardCount * 100);
            ++_cardCount;
        }
        private int GetAddedDiceCard()
        {
            int[] numArray = new int[2] { 703311, 703314 };
            int index = (double)RandomUtil.valueForProb < 0.5 ? 0 : 1;
            if (_patternCount > 1)
                index = 1;
            return numArray[index];
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            BattleUnitModel target = curCard.target;
            if (target == null || curCard.card.GetID() != 703319 || target.faction == owner.faction)
                return;
            _meat.Add(target);
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
                    if (BattleObjectManager.instance.GetList(owner.faction).Find(x => x.IsDead()) == null)
                    {
                        meat.view.EnableView(true);
                        meat.Extinct(false);
                    }
                    else if (meat.faction != owner.faction && !meat.IsDead())
                    {
                        int index = BattleObjectManager.instance.GetList(owner.faction).Find(x => x.IsDead()).index;
                        BattleUnitModel NewMeat = Singleton<StageController>.Instance.AddNewUnit(owner.faction, 1303021, index);
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
                _meat.Clear();
            }
        }
        public override bool AllowTargetChanging(BattleUnitModel attacker, int myCardSlotIdx)
        {
            if (owner.cardSlotDetail.cardAry[myCardSlotIdx] != null && owner.cardSlotDetail.cardAry[myCardSlotIdx].card.GetID() == 703319)
            {
                List<BattlePlayingCardDataInUnitModel> cardArray = owner.cardSlotDetail.cardAry.FindAll(x => x != null);
                List<BattlePlayingCardDataInUnitModel> sack = cardArray.FindAll(x => x.card.GetID() == 703319);
                return !sack.Exists(x => x.target == attacker);
            }
            return base.AllowTargetChanging(attacker, myCardSlotIdx);
        }
    }
    public class ContingecyContract_Greta_Salt : ContingecyContract
    {
        private Dictionary<BattleUnitModel, int> Salted = new Dictionary<BattleUnitModel, int>();
        public override bool CheckEnemyId(LorId EnemyId) => EnemyId == 1303011;
        public override string[] GetFormatParam(string language) => new string[] {Level.ToString(),Level.ToString() };
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            behavior.card.target.bufListDetail.AddKeywordBufByEtc(RandomUtil.SelectOne<KeywordBuf>(KeywordBuf.Paralysis, KeywordBuf.Vulnerable, KeywordBuf.Binding), 1);
        }
        public override int OnGiveKeywordBufByCard(BattleUnitBuf buf, int stack, BattleUnitModel target)
        {
            return buf.bufType == KeywordBuf.Bleeding ? Level : 0;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            foreach(BattleUnitModel unit in Salted.Keys)
            {
                unit.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak,Level*Salted[unit]);
                unit.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Disarm, Level * Salted[unit]);
            }
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            if (curCard.card.GetID() == 703319)
            {
                if (!Salted.ContainsKey(curCard.target))
                    Salted.Add(curCard.target, 1);
                else
                    Salted[curCard.target] += 1;
            }
            base.OnUseCard(curCard);
        }
        public override void OnMakeBreakState(BattleUnitModel target)
        {
            base.OnMakeBreakState(target);
            if (!Salted.ContainsKey(target))
                Salted.Add(target, 1);
            else
                Salted[target] += 1;
        }
    }
    public class ContingecyContract_Greta_Feast : ContingecyContract
    {
        public override bool CheckEnemyId(LorId EnemyId) => EnemyId == 1303011;
        public override string[] GetFormatParam(string language) => new string[] { (Level-1).ToString(),(50*(int)(Math.Pow(2,Level-1))).ToString()};
        public int GetSackNumAdder() => BattleObjectManager.instance.GetAliveList(owner.faction).Count == 1 ? Level - 1 : 0;
    }
    public class ContingecyContract_Greta: ContingecyContract
    {
        public override bool CheckEnemyId(LorId EnemyId) => EnemyId == 1303011;
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            this.owner.bufListDetail.RemoveBufAll(KeywordBuf.Resistance);
            this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Resistance, 100);
        }
        /*public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.owner.bufListDetail.RemoveBufAll(KeywordBuf.Resistance);
            this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Resistance, 100);
        }*/
    }
}
