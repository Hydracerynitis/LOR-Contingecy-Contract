using System;
using System.Collections.Generic;
using System.Linq;
using BaseMod;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    //Replacement Passive
    public class PassiveAbility_1302013_New : PassiveAbility_1302013
    {
        private int GetPhaseThreshold(EileenPhase phase)
        {
            int i = 0;
            switch (phase)
            {
                case EileenPhase.First:
                    i = owner.MaxHp;
                    break;
                case EileenPhase.Second:
                    i = (int)(0.7 * owner.MaxHp);
                    break;
                case EileenPhase.Third:
                    i = (int)(0.3 * owner.MaxHp);
                    break;
                case EileenPhase.None:
                    i = 0;
                    break;
            }
            return i;
        }
        private EileenPhase nextPhase
        {
            get
            {
                switch (currentEileenPhase)
                {
                    case EileenPhase.None:
                        return EileenPhase.First;
                    case EileenPhase.First:
                        return EileenPhase.Second;
                    case EileenPhase.Second:
                        return EileenPhase.Third;
                    case EileenPhase.Third:
                        return EileenPhase.None;
                    default:
                        return EileenPhase.None;
                }
            }
        }
        public override void OnWaveStart()
        {
            CheckChangePhase_CC();
            beliverdeath = false;
            owner.breakDetail.blockRecoverBreakByEvaision = true;
        }
        public void ChangePhase_CC(EileenPhase phase)
        {
            ChangePhase(phase);
            owner.SetHp(GetPhaseThreshold(phase));
        }
        public override void OnRoundEndTheLast()
        {
            if (CheckChangePhase_CC() || owner.IsBreakLifeZero() || !beliverdeath)
                return;
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(owner.faction);
            aliveList.Remove(owner);
            if (aliveList.Count != 0)
                return;
            beliverdeath = false;
            CreateBeliever(4);
        }
        public bool CheckChangePhase_CC()
        {
            if (owner.hp <= GetPhaseThreshold(EileenPhase.Third))
            {
                if (currentEileenPhase == EileenPhase.Third)
                    return false;
                ChangePhase_CC(EileenPhase.Third);
                int num = owner.allyCardDetail.GetHand().Count + 1 - owner.allyCardDetail.maxHandCount;
                if (num > 0)
                    owner.allyCardDetail.DiscardInHand(num);
                owner.allyCardDetail.AddNewCard(specialCards[3]).temporary = true;
                return true;
            }
            else if (owner.hp <= GetPhaseThreshold(EileenPhase.Second))
            {
                if (currentEileenPhase == EileenPhase.Second)
                    return false;
                ChangePhase_CC(EileenPhase.Second);
                int num = owner.allyCardDetail.GetHand().Count + 1 - owner.allyCardDetail.maxHandCount;
                if (num > 0)
                    owner.allyCardDetail.DiscardInHand(num);
                owner.allyCardDetail.AddNewCard(specialCards[3]).temporary = true;
                return true;

            }
            else if (owner.hp <= GetPhaseThreshold(EileenPhase.First))
            {
                if (currentEileenPhase == EileenPhase.First)
                    return false;
                ChangePhase_CC(EileenPhase.First);
                return true;
            }
            return false;
        }

        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            if (unit.UnitData.unitData.EnemyUnitId != 1302021)
                return;
            else
                owner.TakeBreakDamage(owner.breakDetail.GetDefaultBreakGauge() / 4, DamageType.Passive, atkResist: AtkResist.None);
            beliverdeath = true;
        }
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            _dmgReduction = 0;
            int phaseThreshold = GetPhaseThreshold(nextPhase);
            int exceedDmg = (int)owner.hp - dmg;
            if (exceedDmg <= phaseThreshold)
                _dmgReduction = phaseThreshold - exceedDmg;
            return false;
        }
        public override int GetDamageReductionAll() => owner.hp > GetPhaseThreshold(nextPhase) ? _dmgReduction : 10000;
    }
    public class ContingecyContract_Eileen_Production : ContingecyContract
    {
        public override string[] GetFormatParam(string language) => new string[] { (25 + Level * 25).ToString(), GetParam(language) };
        private string GetParam(string language)
        {
            string s = "";
            if (Level >= 1)
                s += StaticDataManager.GetParam("Eileen_Production_param1", language);
            if (Level >= 2)
                s += StaticDataManager.GetParam("Eileen_Production_param2", language);
            if(Level >=3)
                s += StaticDataManager.GetParam("Eileen_Production_param3", language);
            return s;
        }
        private bool init =false;
        private bool IsEileen => owner.UnitData.unitData.EnemyUnitId == 1302011;
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            if (IsEileen)
                return new StatBonus() { hpRate = 25 + Level * 25 };
            return base.GetStatBonus(owner);
        }
        public override int SpeedDiceNumAdder() => IsEileen || Level <1? 0:1;
        public override int MaxPlayPointAdder() => IsEileen || Level<1? 0:2;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (IsEileen || Level < 1)
                return;
            if (!init)
            {
                this.owner.cardSlotDetail.RecoverPlayPoint(MaxPlayPointAdder());
                init = true;
            }
            if (Level < 2)
                return;
            this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Smoke, 2);
            if (Level < 3)
                return;
            this.owner.cardSlotDetail.RecoverPlayPoint(2);
            this.owner.allyCardDetail.DrawCards(2);
        }
    }
    public class ContingecyContract_Eileen_BodyGuard: ContingecyContract
    {
        public override string[] GetFormatParam(string language) => new string[] { (25 + Level * 25).ToString(), (Math.Max(0, -3 + 3 * Level)).ToString() };
        private bool IsEileen => owner.UnitData.unitData.EnemyUnitId == 1302011;
        public override StatBonus GetStatBonus(BattleUnitModel owner)
        {
            if (!IsEileen)
                return new StatBonus() { hpRate = 25 + Level * 25, breakRate= 25 + Level * 25 };
            return base.GetStatBonus(owner);
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (curCard.card.GetID() != 703207)
                return;
            curCard.ApplyDiceStatBonus(DiceMatch.DiceByIdx(1), new DiceStatBonus() { max = Math.Max(0, -3 + 3 * Level) });
        }
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (!IsEileen || BattleObjectManager.instance.GetAliveList(this.owner.faction).FindAll(x => x!= owner).Count<=0)
                return false;
            RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList(this.owner.faction).FindAll(x => x != owner)).TakeDamage(dmg);
            return true;
        }
    }
    public class ContingecyContract_Eileen : ContingecyContract
    {
        public override bool CheckEnemyId(LorId EnemyId) => EnemyId == 1302021;
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            List<DiceCardXmlInfo> Decklist = new List<DiceCardXmlInfo>();
            foreach (LorId i in Singleton<DeckXmlList>.Instance.GetData(Tools.MakeLorId(18220000)).cardIdList)
                Decklist.Add(ItemXmlDataList.instance.GetCardItem(i));
            self.allyCardDetail.Init(Decklist);
            self.allyCardDetail.DrawCards(8);
        }
    }
}
