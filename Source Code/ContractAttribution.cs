using System;
using System.Collections.Generic;
using HarmonyLib;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            foreach (Contract contract in Singleton<ContractLoader>.Instance.GetList())
            {
                if (contract.Faction != Model.faction)
                {
                    Debug.Log(string.Format("{0}'s faction doesn't match {1}'s",contract.Type, Model.UnitData.unitData.name));
                    continue;
                }
                if (contract.Enemy.Count > 0 && !contract.Enemy.Contains(Model.UnitData.unitData.EnemyUnitId))
                {
                    Debug.Log(string.Format("{0} isn't for {1}", contract.Type, Model.UnitData.unitData.EnemyUnitId));
                    continue;
                }
                System.Type type = System.Type.GetType("Contingecy_Contract.ContingecyContract_" + contract.Type);
                if (type == (System.Type)null)
                {
                    Debug.InstanceDebug(type.Name, " is not found", Model.UnitData.unitData.name);
                    continue;
                }
                Debug.InstanceDebug(type.Name, " is found", Model.UnitData.unitData.name);
                if (Activator.CreateInstance(type, new object[] { contract.level }) is ContingecyContract instance)
                {
                    Debug.InstanceDebug(type.Name, " is created",Model.UnitData.unitData.name);
                    instance.Init(Model);
                    string level = string.Empty;
                    if (contract.Variation > 0)
                        level = (contract.level - contract.BaseLevel).ToString();
                    instance.name = Singleton<PassiveDescXmlList>.Instance.GetName(20210302) + contract.GetDesc(TextDataModel.CurrentLanguage).name+level;
                    instance.desc = string.Format(contract.GetDesc(TextDataModel.CurrentLanguage).desc,instance.GetFormatParam);
                    if (contract.level <=2)
                        instance.rare = Rarity.Uncommon;
                    if (contract.level == 3)
                        instance.rare = Rarity.Rare;
                    if (contract.level >= 4)
                        instance.rare = Rarity.Unique;                 
                    if (instance.Type == ContractType.Buff || instance.Type == ContractType.Special)
                    {
                        BuffContracts.Add(instance);
                        Debug.InstanceDebug(type.Name, " is added to Buff List",Model.UnitData.unitData.name);
                    }
                    else
                    {
                        Contracts.Add(instance);
                        Debug.InstanceDebug(type.Name, " is added to Passive List", Model.UnitData.unitData.name);
                    }
                }
            }
            List<PassiveAbilityBase> passiveList = Model.passiveDetail.PassiveList;
            passiveList.AddRange(Contracts);
            passiveList.AddRange(BuffContracts);
            typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue((object)Model.passiveDetail, (object)passiveList);
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
                }
                else
                {
                    Model.SetHp((int)Model.passiveDetail.GetStartHp((float)Model.MaxHp));
                    Model.breakDetail.breakGauge = Model.breakDetail.GetDefaultBreakGauge();
                }
            }
        }
    }
    public class ContractStatBonus : BattleUnitBuf
    {
        List<ContingecyContract> Contracts;
        public ContractStatBonus(List<ContingecyContract> list)
        {
            Contracts = new List<ContingecyContract>();
            Contracts.AddRange(list);
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
        public virtual StatBonus GetStatBonus(BattleUnitModel owner) => new StatBonus();
        public virtual string[] GetFormatParam => new string[0];
    }
}
