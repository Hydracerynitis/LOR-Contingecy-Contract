﻿using System;
using System.Collections.Generic;
using HarmonyLib;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;

namespace Contingecy_Contract
{
    public class ContractAttribution
    {
        public static List<BattleUnitModel> Inition = new List<BattleUnitModel>();
        public static void Init(BattleUnitModel Model)
        {
            Inition.Add(Model);
            List<ContingecyContract> Contracts = new List<ContingecyContract>();
            foreach (Contract contract in Singleton<ContractLoader>.Instance.GetPassiveList())
            {
                if (contract.Faction != Model.faction)
                {
                    Debug.Log("{0}'s faction doesn't match {1}'s", contract.Type, Model.UnitData.unitData.name);
                    continue;
                }
                if(contract.Stageid!=-1 && Singleton<StageController>.Instance.GetStageModel().ClassInfo.id != contract.Stageid)
                {
                    Debug.Log("{0} isn't for stage {1}", contract.Type, Singleton<StageController>.Instance.GetStageModel().ClassInfo.id.ToString());
                    continue;
                }
                System.Type type = System.Type.GetType("Contingecy_Contract.ContingecyContract_" + contract.Type);
                if (type == (System.Type)null)
                {
                    Debug.Log("Instance of {0} is not found for {1}", type.Name, Model.UnitData.unitData.name);
                    continue;
                }
                Debug.Log("Instance of {0} is found for {1}", type.Name, Model.UnitData.unitData.name);

                if (Activator.CreateInstance(type, new object[] { contract.Variant }) is ContingecyContract instance)
                {
                    Debug.Log("Instance of {0}  is created for {1}", type.Name, Model.UnitData.unitData.name);
                    if(!instance.CheckEnemyId(Model.UnitData.unitData.EnemyUnitId))
                    {
                        Debug.Log("Instance of {0} is not found for {1}", type.Name, Model.UnitData.unitData.EnemyUnitId.ToString());
                        continue;
                    }
                    instance.Init(Model);
                    instance.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(20210302)) + contract.GetDesc().name;
                    instance.desc = contract.GetDesc().desc;
                    instance.rare = Rarity.Unique;
                    Contracts.Add(instance);
                }
            }
            foreach(Contract contract in Singleton<ContractLoader>.Instance.GetStageList())
            {
                if (contract.Faction != Model.faction)
                {
                    Debug.Log("{0}'s faction doesn't match {1}'s", contract.Type, Model.UnitData.unitData.name);
                    continue;
                }
                StageClassInfo original = Singleton<StageClassInfoList>.Instance.GetData(Singleton<StageController>.Instance.GetStageModel().ClassInfo.id);
                if (!contract.modifier.IsValid(original))
                {
                    Debug.Log("{0} doesn't match {1}'s requirement",Singleton<StageNameXmlList>.Instance.GetName(original.id), contract.Type);
                    continue;
                }                  
                System.Type type = System.Type.GetType("Contingecy_Contract.ContingecyContract_" + contract.Type);
                if (type == (System.Type)null)
                {
                    Debug.Log("Instance of {0} is not found for {1}", type.Name, Model.UnitData.unitData.name);
                    continue;
                }
                Debug.Log("Instance of {0} is found for {1}", type.Name, Model.UnitData.unitData.name);
                if (Activator.CreateInstance(type, new object[] { contract.Variant }) is ContingecyContract stage)
                {
                    stage.Init(Model);
                    stage.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(20210302)) + contract.GetDesc().name;
                    stage.desc = contract.GetDesc().desc;
                    stage.rare = Rarity.Unique;
                    Contracts.Add(stage);
                }
            }
            List<PassiveAbilityBase> passiveList = Model.passiveDetail.PassiveList;
            passiveList.AddRange(Contracts);
            Model.passiveDetail._passiveList = passiveList;
            foreach (PassiveAbilityBase contract in Contracts)
            {
                try
                {
                    contract.OnWaveStart();
                }
                catch
                {

                }
            }
            if (Contracts.Count > 0)
            {
                Model.bufListDetail.AddBuf(new ContractStatBonus(Contracts));
                if (CCInitializer.CombaltData.ContainsKey(Model.UnitData))
                {
                    Model.SetHp(CCInitializer.CombaltData[Model.UnitData]);
                    Model.breakDetail.breakGauge = Model.breakDetail.GetDefaultBreakGauge();
                    CheckPhaseCondition(Model);
                }
                else if(!IsNewTwistedOswald(Model))
                {
                    Model.SetHp((int)Model.passiveDetail.GetStartHp(Model.MaxHp));
                    Model.breakDetail.breakGauge = Model.breakDetail.GetDefaultBreakGauge();
                    CheckPhaseCondition(Model);
                }
            }
        }

        private static bool IsNewTwistedOswald(BattleUnitModel unit)
        {
            return StageController.Instance.EnemyStageManager is EnemyTeamStageManager_TwistedReverberationBand_Middle TRM && unit.UnitData.unitData.EnemyUnitId==1405011 && unit.hp==TRM._oswaldHp ;
        }

        public static void CheckPhaseCondition(BattleUnitModel unit)
        {
            if (unit.passiveDetail.PassiveList.Find(x => x is PassiveAbility_250022) is PassiveAbility_250022 Red)
                Red._egoCondition = unit.MaxHp / 2;
            if (unit.passiveDetail.PassiveList.Find(x => x is PassiveAbility_250227) is PassiveAbility_250227 Purple)
                Purple._teleportCondition = unit.MaxHp / 2;
        }
    }
    public class ContractStatBonus : BattleUnitBuf
    {
        private readonly List<ContingecyContract> Contracts;
        public ContractStatBonus(List<ContingecyContract> list)
        {
            Contracts = new List<ContingecyContract>();
            Contracts.AddRange(list);
        }
        public override bool IsImmune(BufPositiveType posType)
        {
            if (posType == BufPositiveType.Negative && Contracts.Exists(x => x is ContingecyContract_NoDebuff))
                return true;
            if (posType == BufPositiveType.Positive && Contracts.Exists(x => x is ContingecyContract_NoBuff))
                return true;
            return base.IsImmune(posType);
        }
        public override int GetDamageReductionRate()
        {
            int i = 0;
            Contracts.ForEach(x => i = x.GetDamageReductionRate()>i ? x.GetDamageReductionRate() : i);
            return i;
        }
        public override int GetBreakDamageReductionRate()
        {
            int i = 0;
            Contracts.ForEach(x => i = x.GetBreakDamageReductionRate() > i ? x.GetBreakDamageReductionRate() : i);
            return i;
        }
        public override bool IsCardChoosable(BattleDiceCardModel card)
        {
            return Contracts.Exists(x => x is ContingecyContract_NoEGO) ? !card.XmlData.IsEgo() : base.IsCardChoosable(card);
        }
        public override StatBonus GetStatBonus()
        {
            StatBonus statbonus = new StatBonus();
            foreach (ContingecyContract contract in Contracts)
            {
                statbonus.AddStatBonus(contract.GetStatBonus(_owner));
            }
            return statbonus;
        }
    }
    public enum ContractType
    {
        Passive,
        Buff,
        Special
    }
    public class ContingecyContract : PassiveAbilityBase
    {
        public int Level;
        public virtual DiceStatBonus GetDicestatBonus(BattleDiceBehavior behavior) => new DiceStatBonus();
        public virtual bool CheckEnemyId(LorId EnemyId) => true;
        public virtual StatBonus GetStatBonus(BattleUnitModel owner) => new StatBonus();
        public virtual int GetDamageReductionRate() => 0;
        public virtual int GetBreakDamageReductionRate() => 0;
        public virtual string[] GetFormatParam(string language) => Array.Empty<string>();
    }
}
