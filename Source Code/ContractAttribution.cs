using System;
using System.Collections.Generic;
using HarmonyLib;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;
using static UnityEngine.UI.CanvasScaler;

namespace Contingecy_Contract
{
    public class ContractAttribution
    {
        public static List<BattleUnitModel> Inition = new List<BattleUnitModel>();
        public static int OswaldHp=-1;
        public static void Init(BattleUnitModel Model)
        {
            Inition.Add(Model);
            List<ContingecyContract> Contracts = new List<ContingecyContract>();
            List<Contract> activatedContract=new List<Contract>();
            activatedContract.AddRange(Singleton<ContractLoader>.Instance.GetPassiveList());
            activatedContract.AddRange(Singleton<ContractLoader>.Instance.GetStageList());
            foreach (Contract contract in activatedContract)
            {
                if (contract.Faction != Model.faction)
                    continue;
                StageClassInfo original = Singleton<StageClassInfoList>.Instance.GetData(Singleton<StageController>.Instance.GetStageModel().ClassInfo.id);
                if (contract.modifier != null && !contract.modifier.IsValid(original))
                    continue;
                LorId stageId = StageController.Instance.GetStageModel().ClassInfo.id;
                if (contract.Stageid!=-1 && stageId != new LorId(contract.Pid, contract.Stageid))
                    continue;
                System.Type type = StaticDataManager.GetContingencyContract(contract.Type);
                if (type == (System.Type)null)
                    continue;
                ContingecyContract instance = (ContingecyContract)Activator.CreateInstance(type, new object[] { });
                instance.Level = contract.Variant;
                if (instance!=null)
                {
                    if(!instance.CheckEnemyId(Model.UnitData.unitData.EnemyUnitId))
                        continue;
                    instance.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(20210302)) + contract.GetDesc().name;
                    instance.desc = contract.GetDesc().desc;
                    instance.rare = Rarity.Unique;
                    instance.Init(Model);
                    Contracts.Add(instance);
                }
            }
            List<PassiveAbilityBase> passiveList = Model.passiveDetail.PassiveList;
            passiveList.AddRange(Contracts);
            Model.passiveDetail._passiveList = passiveList;
            /*foreach (PassiveAbilityBase contract in Contracts)
            {
                try
                {
                    contract.OnWaveStart();
                }
                catch
                {

                }
            }*/
            if (Contracts.Count > 0)
            {
                Model.bufListDetail.AddBuf(new ContractStatBonus(Contracts));
                Model.breakDetail.breakGauge = Model.breakDetail.GetDefaultBreakGauge();
                ChangePhaseTransitionThreshold(Model);
                if (Model.UnitData.unitData.EnemyUnitId == 1405011)
                {
                    if (OswaldHp <= 0)
                        Model.SetHp((int)Model.passiveDetail.GetStartHp(Model.MaxHp));
                    else
                        Model.SetHp(OswaldHp);
                }
                else if (CCInitializer.CombaltData.ContainsKey(Model.UnitData))
                    Model.SetHp(CCInitializer.CombaltData[Model.UnitData]);
                else
                    Model.SetHp((int)Model.passiveDetail.GetStartHp(Model.MaxHp));
            }
        }

/*        private static bool IsNewTwistedOswald(BattleUnitModel unit)
        {
            return StageController.Instance.EnemyStageManager is EnemyTeamStageManager_TwistedReverberationBand_Middle TRM &&  && unit.hp==TRM._oswaldHp ;
        }*/

        public static void ChangePhaseTransitionThreshold(BattleUnitModel unit)
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
