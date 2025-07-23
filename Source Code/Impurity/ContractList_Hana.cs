using BaseMod;
using LOR_DiceSystem;
using SaveTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ContingecyContract_Hana_Web : ContingecyContract
    {
        public override string[] GetFormatParam(string language) => new string[] { StaticDataManager.GetParam("Hana_Web_param" + Level.ToString(), language)};
        public class New_Passive_260002 : PassiveAbilityBase
        {
            public override void OnAddKeywordBufByCardForEvent( KeywordBuf keywordBuf, int stack, BufReadyType readyType)
            {
                if (keywordBuf != KeywordBuf.Strength && keywordBuf != KeywordBuf.Endurance && keywordBuf != KeywordBuf.Protection && keywordBuf != KeywordBuf.BreakProtection)
                    return;
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction);
                aliveList.Remove(this.owner);
                aliveList.ForEach(x =>
                {
                    switch (readyType)
                    {
                        case BufReadyType.ThisRound:
                            x.bufListDetail.AddKeywordBufThisRoundByEtc(keywordBuf, stack);
                            break;
                        case BufReadyType.NextRound:
                            x.bufListDetail.AddKeywordBufByEtc(keywordBuf, stack);
                            break;
                        case BufReadyType.NextNextRound:
                            break;
                    }
                });
                
            }
        }
        public class New_Passive_260003: PassiveAbilityBase
        {
            private int Level;
            private List<PassiveAbility_260003.DmgInfo> _dmgInfos = new List<PassiveAbility_260003.DmgInfo>();
            private List<PassiveAbility_260003.DmgInfo> _breakdmgInfos = new List<PassiveAbility_260003.DmgInfo>();
            public void Initialise(int Level)
            {
                _dmgInfos.Add(new PassiveAbility_260003.DmgInfo() { type = BehaviourDetail.Slash });
                _dmgInfos.Add(new PassiveAbility_260003.DmgInfo() { type = BehaviourDetail.Penetrate });
                _dmgInfos.Add(new PassiveAbility_260003.DmgInfo() { type = BehaviourDetail.Hit });
                _breakdmgInfos.Add(new PassiveAbility_260003.DmgInfo() { type = BehaviourDetail.Slash });
                _breakdmgInfos.Add(new PassiveAbility_260003.DmgInfo() { type = BehaviourDetail.Penetrate });
                _breakdmgInfos.Add(new PassiveAbility_260003.DmgInfo() { type = BehaviourDetail.Hit });
                this.Level= Level;
            }
            public override void OnRoundStart()
            {
                _dmgInfos.Sort((x, y) => y.dmg - x.dmg);
                _breakdmgInfos.Sort((x, y) => y.dmg - x.dmg);
                BehaviourDetail behaviourDetail1 = BehaviourDetail.None;
                BehaviourDetail behaviourDetail2 = BehaviourDetail.None;
                if (_dmgInfos[0].dmg > 0)
                    behaviourDetail1 = _dmgInfos[0].type;
                if (_breakdmgInfos[0].dmg > 0)
                    behaviourDetail2 = _breakdmgInfos[0].type;
                BattleObjectManager.instance.GetAliveList(owner.faction).ForEach(x => x.bufListDetail.AddBuf(new ResistsModifier()
                {
                    hpTarget = behaviourDetail1,
                    bpTarget = behaviourDetail2,
                    Level = Level
                }) );
                _dmgInfos.ForEach(x => x.dmg = 0);
                _breakdmgInfos.ForEach(x => x.dmg = 0);
            }

            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                PassiveAbility_260003.DmgInfo dmgInfo = _dmgInfos.Find(x => x.type == atkDice.Detail);
                if (dmgInfo == null)
                    return;
                dmgInfo.dmg += dmg;
            }

            public override void OnTakeBreakDamageByAttack(BattleDiceBehavior atkDice, int breakdmg)
            {
                PassiveAbility_260003.DmgInfo dmgInfo = _breakdmgInfos.Find(x => x.type == atkDice.Detail);
                if (dmgInfo == null)
                    return;
                dmgInfo.dmg += breakdmg;
            }

            public class ResistsModifier : BattleUnitBuf
            {
                public BehaviourDetail hpTarget = BehaviourDetail.None;
                public BehaviourDetail bpTarget = BehaviourDetail.None;
                public int Level;
                public AtkResist getResist()
                {
                    switch(Level)
                    {
                        case 1:
                            return AtkResist.Endure;
                        case 2:
                            return AtkResist.Resist;
                        case 3:
                            return AtkResist.Immune;
                    }
                    return AtkResist.Endure;
                }
                public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
                {
                    if (hpTarget == BehaviourDetail.None)
                        return base.GetResistHP(origin, detail);
                    return hpTarget == detail ? getResist() : base.GetResistHP(origin, detail);
                }

                public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
                {
                    if (bpTarget == BehaviourDetail.None)
                        return base.GetResistBP(origin, detail);
                    return bpTarget == detail ? getResist() : base.GetResistBP(origin, detail);
                }

                public override void OnRoundEnd() => Destroy();
            }
        }
    }
    public class ContingecyContract_Hana_Protocol : ContingecyContract
    {
        public BattleAllyCardDetail gainPassiveAllyCard=null;
        public override string[] GetFormatParam(string language) => new string[] { GetParam(language) };
        private string GetParam(string language)
        {
            string s = "";
            if (Level >= 1)
                s += StaticDataManager.GetParam("Hana_Protocol_param1", language);
            if (Level >= 2)
                s += StaticDataManager.GetParam("Hana_Protocol_param2", language);
            if (Level >= 3)
                s += StaticDataManager.GetParam("Hana_Protocol_param3", language);
            return s;
        }
        public override int OnAddKeywordBufByCard(BattleUnitBuf buf, int stack)
        {
            if (owner.passiveDetail.HasPassive<PassiveAbility_260002>())
            {
                if(buf.bufType==KeywordBuf.Strength || buf.bufType == KeywordBuf.Endurance 
                    || buf.bufType == KeywordBuf.Protection || buf.bufType == KeywordBuf.BreakProtection)
                {
                    return stack;
                }
            }
            return base.OnAddKeywordBufByCard(buf, stack);
        }
        public override void OnRoundStartAfter()
        {
            base.OnRoundStartAfter();
            if (gainPassiveAllyCard != null)
            {
                GainPassive(gainPassiveAllyCard);
                gainPassiveAllyCard = null;
            }
            if (owner.passiveDetail.HasPassive<PassiveAbility_260002>())
            {
                foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
                {
                    if (unit == owner) continue;
                    unit.bufListDetail.AddBuf(new Protection());
                }
            }
        }
        public void GainPassive(BattleAllyCardDetail allyCard)
        {
            owner.passiveDetail.AddPassive(new PassiveAbility_260002()
            {
                name= PassiveXmlList.Instance.GetData(260002).name,
                desc =PassiveXmlList.Instance.GetData(260002).desc,
                rare = Rarity.Unique
            }) ;
            if (ContractLoader.Instance.ExistContract("Hana_Olivier"))
            {
                if(!owner.passiveDetail.HasPassive<PassiveAbility_260003>())
                    owner.passiveDetail.AddPassive(new PassiveAbility_260003()
                    {
                        name = PassiveXmlList.Instance.GetData(260003).name,
                        desc = PassiveXmlList.Instance.GetData(260003).desc,
                        rare = Rarity.Unique
                    });
                if (!owner.passiveDetail.HasPassive<PassiveAbility_260004>())
                    owner.passiveDetail.AddPassive(new PassiveAbility_260004()
                    {
                        name = PassiveXmlList.Instance.GetData(260004).name,
                        desc = PassiveXmlList.Instance.GetData(260004).desc,
                        rare = Rarity.Unique
                    });
            }
            int handcount = owner.allyCardDetail.GetHand().Count;
            DeckXmlInfo MirinaeDeck = DeckXmlList.Instance.GetData(160002);
            List<DiceCardXmlInfo> MirinaeCard=new List<DiceCardXmlInfo>();
            foreach (LorId cardId in MirinaeDeck.cardIdList)
            {
                DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(cardId);
                if (cardItem != null)
                    MirinaeCard.Add(cardItem);
            }
            owner.passiveDetail.OnCreated();
            owner.allyCardDetail.ExhaustAllCards();
            owner.allyCardDetail.Init(MirinaeCard);
            owner.allyCardDetail.DrawCards(handcount);
            owner.RecoverHP(owner.MaxHp);
            owner.RecoverBreakLife(1);
            owner.ResetBreakGauge();
            owner.turnState = BattleUnitTurnState.WAIT_CARD;
            owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
            owner.bufListDetail.AddBuf(new Avenging() { Level = Level });
        }
        public override void OnDie()
        {
            base.OnDie();
            if (!owner.passiveDetail.HasPassive<PassiveAbility_260002>() && !owner.passiveDetail.HasPassiveInReady<PassiveAbility_260002>())
                return;
            List<BattleUnitModel> candidates = BattleObjectManager.instance.GetAliveList(owner.faction).FindAll(x => x.passiveDetail.HasPassive<ContingecyContract_Hana_Protocol>());
            BattleUnitModel nextHost = RandomUtil.SelectOne(candidates);
            ContingecyContract_Hana_Protocol cc = nextHost.passiveDetail.FindPassive<ContingecyContract_Hana_Protocol>();
            if (cc != null)
                cc.gainPassiveAllyCard=owner.allyCardDetail;
        }
        class Avenging : BattleUnitBuf
        {
            public int Level;
            public override int SpeedDiceNumAdder()
            {
                return Level>=2? 1:0;
            }
            public override int GetCardCostAdder(BattleDiceCardModel card)
            {
                return Level>=3? -100: 0;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
        class Protection: BattleUnitBuf
        {
            public override int GetBreakDamageReductionRate()
            {
                return 30;
            }
            public override int GetDamageReductionRate()
            {
                return 30;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
    public class ContingecyContract_Hana : ContingecyContract
    {
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return base.CheckEnemyId(EnemyId);
        }
        public class New_Passive_260001: PassiveAbilityBase
        {
            private int _gun = 701021;
            private int _gon = 701022;
            private int _gam = 701023;
            private int _li = 701024;
            private List<int> _onlyEnemy = new List<int>();
            public override void OnRoundStartAfter()
            {
                if (owner.RollSpeedDice().FindAll(x => !x.breaked).Count <= 0 || owner.IsBreakLifeZero())
                    return;
                if (_onlyEnemy.Count<=0)
                {
                    _onlyEnemy.Add(_gun);
                    _onlyEnemy.Add(_gon);
                    _onlyEnemy.Add(_gam);
                    _onlyEnemy.Add(_li);
                }
                for(int i = 0; i < 2; i++)
                {
                    int num2 = RandomUtil.SelectOne<int>(_onlyEnemy);
                    if (num2 == _gun)
                        owner.bufListDetail.AddBuf(new NewHanaBuf1());
                    if (num2 == _gon)
                        owner.bufListDetail.AddBuf(new NewHanaBuf2());
                    if (num2 == _gam)
                        owner.bufListDetail.AddBuf(new NewHanaBuf3());
                    if (num2 == _li)
                        owner.bufListDetail.AddBuf(new NewHanaBuf4());
                    _onlyEnemy.Remove(num2);
                }
            }
        }
        public class NewHanaBuf1 : BattleUnitBuf_hanaBufCommon
        {
            public override string keywordIconId => "HanaPassive1";
            public override string keywordId => "CC_NewHanaPassive1";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 1,owner);
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                if (!IsAttackDice(behavior.Detail))
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = 3,
                    breakDmg = 3
                });
            }
            public override void OnRoundEnd() => Destroy();
        }
        public class NewHanaBuf2 : BattleUnitBuf_hanaBufCommon
        {
            public override string keywordIconId => "HanaPassive2";
            public override string keywordId => "CC_NewHanaPassive2";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance, 2, owner);
            }
            public override int GetBreakDamageReductionRate() => 30;

            public override int GetDamageReductionRate() => 30;

            public override void OnRoundEnd() => Destroy();
        }
        public class NewHanaBuf3 : BattleUnitBuf_hanaBufCommon
        {
            public override string keywordIconId => "HanaPassive3";
            public override string keywordId => "CC_NewHanaPassive3";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                if (owner.cardSlotDetail.PlayPoint < owner.cardSlotDetail.GetMaxPlayPoint())
                    owner.cardSlotDetail.RecoverPlayPoint(1);
                else
                {
                    List<BattleUnitModel> ally = BattleObjectManager.instance.GetAliveList(owner.faction).FindAll(x => x.cardSlotDetail.PlayPoint < x.cardSlotDetail.GetMaxPlayPoint());
                    ally.Sort((x, y) => x.PlayPoint - y.PlayPoint);
                    if (ally.Count > 0)
                        ally[0].cardSlotDetail.RecoverPlayPoint(1);
                }
            }

            public override void OnRoundEnd() => Destroy();
        }
        public class NewHanaBuf4 : BattleUnitBuf_hanaBufCommon
        {
            public override string keywordIconId => "HanaPassive4";
            public override string keywordId => "CC_NewHanaPassive4";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                owner.allyCardDetail.DrawCards(1);
                int handCount = owner.allyCardDetail.GetHand().Count;
                if (handCount > 0)
                    owner.allyCardDetail.GetHand()[handCount - 1].AddBuf(new CostDecreaseBuf());
            }
            public override void OnRoundEnd() => Destroy();
        }
    }
    public class ContingecyContract_Hana_Olivier : ContingecyContract
    {
        public override bool CheckEnemyId(LorId EnemyId)
        {
            return EnemyId == Tools.MakeLorId(60004);
            
        }
        /*public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (ContractLoader.Instance.ExistContract("Hana_Web"))
            {
                desc = StaticDataManager.GetParam("Hana_Olivier_Web", TextDataModel.CurrentLanguage);
            }
            if (ContractLoader.Instance.ExistContract("Hana_Protocol"))
            {
                desc = StaticDataManager.GetParam("Hana_Olivier_Protocol", TextDataModel.CurrentLanguage);
            }
        }*/
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            if (ContractLoader.Instance.ExistContract("Hana_Web"))
            { 
                foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(self.faction))
                {
                    if (unit == self) continue;
                    unit.bufListDetail.AddBuf(new TwelveFixers());
                }
                desc = StaticDataManager.GetParam("Hana_Olivier_Web", TextDataModel.CurrentLanguage);
            }
            if (ContractLoader.Instance.ExistContract("Hana_Protocol"))
            {
                desc = StaticDataManager.GetParam("Hana_Olivier_Protocol", TextDataModel.CurrentLanguage);
            }
        }
        public override void OnDie()
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (unit == owner) continue;
                unit.bufListDetail.RemoveBufAll(typeof(TwelveFixers));
            }
        }

        class TwelveFixers: BattleUnitBuf
        {
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                int num1 = 2;
                int num2 = 0;
                if (_owner.emotionDetail.EmotionLevel >= 3)
                    num2 = 1;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    min = num1,
                    max = num2
                });
            }
        }
    }
    public class StageModifier_Hana_Olivier : StageModifier
    {
        public override int GetPriority()
        {
            return 5;
        }
        public override void Modify(ref StageClassInfo info)
        {
            StageWaveInfo secondWave = DeepCopyUtil.CopyXml(info.waveList[1]);
            secondWave.enemyUnitIdList[2] = Tools.MakeLorId(60004);
            info.waveList[1] = secondWave;
        }
        public override bool IsValid(StageClassInfo info)
        {
            return info.id == 60001;
        }
    }
}
