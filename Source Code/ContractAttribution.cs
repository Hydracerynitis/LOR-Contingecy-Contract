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
        public static List<BattleUnitModel> Inition;
        public static void Init(BattleUnitModel Model)
        {
            Inition.Add(Model);
            List<ContingecyContract> Contracts = new List<ContingecyContract>();
            List<ContingecyContract> BuffContracts = new List<ContingecyContract>();
            List<ContingecyContract> SpecialContracts = new List<ContingecyContract>();
            List<ContingecyContract> StageContracts = new List<ContingecyContract>();
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

                if (Activator.CreateInstance(type, new object[] { contract.level }) is ContingecyContract instance)
                {
                    Debug.Log("Instance of {0}  is created for {1}", type.Name, Model.UnitData.unitData.name);
                    if(!instance.CheckEnemyId(Model.UnitData.unitData.EnemyUnitId))
                    {
                        Debug.Log("Instance of {0} is not found for {1}", type.Name, Model.UnitData.unitData.EnemyUnitId.ToString());
                        continue;
                    }
                    instance.Init(Model);
                    string level = string.Empty;
                    if (contract.Variation > 0)
                        level = contract.level.ToString();
                    instance.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(20210302)) + contract.GetDesc().name+" "+level;
                    instance.desc = string.Format(contract.GetDesc().desc,instance.GetFormatParam);
                    instance.rare = Rarity.Unique;                 
                    if (instance.Type == ContractType.Buff)
                    {
                        BuffContracts.Add(instance);
                        Debug.Log("Instance of {0} is added to Buff List for {1}", type.Name, Model.UnitData.unitData.name);
                    }
                    else if(instance.Type==ContractType.Passive)
                    {
                        Contracts.Add(instance);
                        Debug.Log("Instance of {0} is added to Passive List for {1}", type.Name, Model.UnitData.unitData.name);
                    }
                    else if (instance.Type == ContractType.Special)
                    {
                        SpecialContracts.Add(instance);
                        Debug.Log("Instance of {0} is added to Special List for {1}", type.Name, Model.UnitData.unitData.name);
                    }
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
                if (Activator.CreateInstance(type, new object[] { contract.level }) is ContingecyContract stage)
                {
                    string level = string.Empty;
                    if (contract.Variation > 0)
                        level = contract.level.ToString();
                    stage.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(20210302)) + contract.GetDesc().name + " " + level;
                    stage.desc = string.Format(contract.GetDesc().desc, stage.GetFormatParam);
                    stage.rare = Rarity.Unique;
                    StageContracts.Add(stage);
                }
            }
            List<PassiveAbilityBase> passiveList = Model.passiveDetail.PassiveList;
            passiveList.AddRange(Contracts);
            passiveList.AddRange(BuffContracts);
            passiveList.AddRange(SpecialContracts);
            passiveList.AddRange(StageContracts);
            typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue((object)Model.passiveDetail, (object)passiveList);
            Contracts.AddRange(SpecialContracts);
            BuffContracts.AddRange(SpecialContracts);
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
            if (BuffContracts.Count > 0)
            {
                Model.bufListDetail.AddBuf(new ContractStatBonus(BuffContracts));
                if (Harmony_Patch.CombaltData.ContainsKey(Model.UnitData))
                {
                    Model.SetHp(Harmony_Patch.CombaltData[Model.UnitData]);
                    Model.breakDetail.breakGauge = Model.breakDetail.GetDefaultBreakGauge();
                    CheckPhaseCondition(Model);
                }
                else
                {
                    Model.SetHp((int)Model.passiveDetail.GetStartHp((float)Model.MaxHp));
                    Model.breakDetail.breakGauge = Model.breakDetail.GetDefaultBreakGauge();
                    CheckPhaseCondition(Model);
                }
            }
        }
        public static void CheckPhaseCondition(BattleUnitModel unit)
        {
            if (unit.passiveDetail.PassiveList.Find(x => x is PassiveAbility_250022) is PassiveAbility_250022 Red)
            {
                typeof(PassiveAbility_250022).GetField("_egoCondition", AccessTools.all).SetValue(Red, (int)(0.5 * unit.MaxHp));
            }
            if (unit.passiveDetail.PassiveList.Find(x => x is PassiveAbility_250227) is PassiveAbility_250227 Purple)
            {
                typeof(PassiveAbility_250227).GetField("_teleportCondition", AccessTools.all).SetValue(Purple, (int)(0.5 * unit.MaxHp));
            }
            try
            {
                //给别的mod不上百分比锁血被动适配
            }
            catch
            {

            }
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
        public override StatBonus GetStatBonus()
        {
            StatBonus statbonus = new StatBonus();
            foreach (ContingecyContract contract in Contracts)
            {
                statbonus.AddStatBonus(contract.GetStatBonus(this._owner));
            }
            return statbonus;
        }
    }
    public enum ContractType
    {
        None,
        Passive,
        Buff,
        Special
    }
    public class ContingecyContract : PassiveAbilityBase
    {
        public int Level;
        public virtual ContractType Type => ContractType.None;
        public virtual DiceStatBonus GetDicestatBonus(BattleDiceBehavior behavior) => new DiceStatBonus();
        public virtual bool CheckEnemyId(LorId EnemyId) => true;
        public virtual StatBonus GetStatBonus(BattleUnitModel owner) => new StatBonus();
        public virtual string[] GetFormatParam => Array.Empty<string>();
    }
}
