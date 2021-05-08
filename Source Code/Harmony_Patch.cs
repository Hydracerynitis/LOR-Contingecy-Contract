using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UI;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;
using System.IO;
using HarmonyLib;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class Harmony_Patch
    {
        public static List<PassiveXmlInfo> AvailablePassive;
        public static Dictionary<UnitBattleDataModel, int> CombaltData;
        public static Dictionary<BattleUnitModel, int> UnitBookId;
        public static string ModPath;
        public static bool Cheat;
        public static bool Warn;
        public static bool Duel;
        public static bool PassiveAbility_1890003_init;
        public Harmony_Patch()
        {
            Harmony harmony = new Harmony("Hydracerynitis.ContingecyContract");
            ModPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            Debug.LoadNum = 0;
            Debug.ModPatchDebug();
            Harmony_Patch.LoadContract();
            Singleton<ContractLoader>.Instance.bInit=false;
            CombaltData = new Dictionary<UnitBattleDataModel, int>();
            ContractAttribution.Inition = new List<BattleUnitModel>();
            UnitBookId = new Dictionary<BattleUnitModel, int>();
            Warn = false;
            PassiveAbility_1890003_init = false;
            MethodInfo Method1 = typeof(StageNameXmlList).GetMethod("GetName", AccessTools.all);
            MethodInfo Patch1 = typeof(Harmony_Patch).GetMethod("StageNameXmlList_GetName");
            try
            {
                harmony.Patch(Method1, null, new HarmonyMethod(Patch1), null, null);
                Debug.HPDebug(Patch1.Name);
            }
            catch(Exception ex)
            {
                Debug.Error("HP_" + Patch1.Name, ex);
            }
            MethodInfo Method2 =  typeof(StageController).GetMethod("RoundStartPhase_System", AccessTools.all);
            MethodInfo Patch2 = typeof(Harmony_Patch).GetMethod("StageController_RoundStartPhase_System");
            try
            {
                harmony.Patch(Method2, new HarmonyMethod(Patch2),null , null, null);
                Debug.HPDebug(Patch2.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch2.Name, ex);
            }
            MethodInfo Method3 = typeof(StageController).GetMethod("InitStageByInvitation", AccessTools.all);
            MethodInfo Patch3 = typeof(Harmony_Patch).GetMethod("StageController_InitStageByInvitation");
            try
            {
                harmony.Patch(Method3,null , new HarmonyMethod(Patch3), null, null);
                Debug.HPDebug(Patch3.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch3.Name, ex);
            }
            MethodInfo Method4 = typeof(StageController).GetMethod("InitStageByEndContentsStage", AccessTools.all);
            MethodInfo Patch4 = typeof(Harmony_Patch).GetMethod("StageController_InitStageByEndContentsStage");
            try
            {
                harmony.Patch(Method4, null, new HarmonyMethod(Patch4), null, null);
                Debug.HPDebug(Patch4.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch4.Name, ex);
            }
            MethodInfo Method5 = typeof(StageController).GetMethod("EndBattlePhase", AccessTools.all);
            MethodInfo Patch5 = typeof(Harmony_Patch).GetMethod("StageController_EndBattlePhase");
            try
            {
                harmony.Patch(Method5, new HarmonyMethod(Patch5), null, null, null);
                Debug.HPDebug(Patch5.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch5.Name, ex);
            }
            MethodInfo Method6 = typeof(StageController).GetMethod("GameOver", AccessTools.all);
            MethodInfo Patch6 = typeof(Harmony_Patch).GetMethod("StageController_GameOver");
            try
            {
                harmony.Patch(Method6, null, new HarmonyMethod(Patch6), null, null);
                Debug.HPDebug(Patch6.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch6.Name, ex);
            }
            MethodInfo Method7 = typeof(LibraryModel).GetMethod("OnClearStage", AccessTools.all);
            MethodInfo Patch7 = typeof(Harmony_Patch).GetMethod("LibraryModel_OnClearStage");
            try
            {
                harmony.Patch(Method7, null, new HarmonyMethod(Patch7), null, null);
                Debug.HPDebug(Patch7.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch7.Name, ex);
            }
            MethodInfo Method8 = typeof(BattleUnitBuf_Philip_OverHeat).GetMethod("Init", AccessTools.all);
            MethodInfo Patch8 = typeof(Harmony_Patch).GetMethod("BattleUnitBuf_Philip_OverHeat_Init");
            try
            {
                harmony.Patch(Method8, new HarmonyMethod(Patch8),null, null, null);
                Debug.HPDebug(Patch8.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch8.Name, ex);
            }
            MethodInfo Method9 = typeof(BattleObjectManager).GetMethod("Clear", AccessTools.all);
            MethodInfo Patch9 = typeof(Harmony_Patch).GetMethod("BattleObjectManager_Clear");
            try
            {
                harmony.Patch(Method9, new HarmonyMethod(Patch9), null, null, null);
                Debug.HPDebug(Patch9.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch9.Name, ex);
            }
            MethodInfo Method10 = typeof(DebugConsoleScript).GetMethod("Update", AccessTools.all);
            MethodInfo Patch10 = typeof(Harmony_Patch).GetMethod("DebugConsoleScript_Update");
            try
            {
                harmony.Patch(Method10, null, new HarmonyMethod(Patch10), null, null);
                Debug.HPDebug(Patch10.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch10.Name, ex);
            }
            MethodInfo Method11 = typeof(PassiveAbility_1307012).GetMethod("AddThread", AccessTools.all);
            MethodInfo Patch11 = typeof(Harmony_Patch).GetMethod("PassiveAbility_1307012_AddThread");
            try
            {
                harmony.Patch(Method11, new HarmonyMethod(Patch11), null, null, null);
                Debug.HPDebug(Patch11.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch11.Name, ex);
            }
            MethodInfo Method12 = typeof(StageController).GetMethod("InitStageByCreature", AccessTools.all);
            MethodInfo Patch12 = typeof(Harmony_Patch).GetMethod("StageController_InitStageByCreature");
            try
            {
                harmony.Patch(Method12, null , new HarmonyMethod(Patch12), null, null);
                Debug.HPDebug(Patch12.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch12.Name, ex);
            }
        }
        public static void StageNameXmlList_GetName(ref string __result,int id)
        {
            __result = TextDataModel.GetText("ui_ContingecyLevel", (object)Singleton<ContractLoader>.Instance.GetLevel(id), (object)__result);
        }
        public static bool StageController_RoundStartPhase_System(StageType ____stageType)
        {
            if (____stageType != StageType.Invitation)
                return true;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (ContractAttribution.Inition.Contains(alive))
                    continue;
                ContractAttribution.Init(alive);
                if (!Cheat)
                {
                    if (cheatList.Contains(alive.Book.GetBookClassInfoId()) && alive.faction == Faction.Player)
                        Cheat = true;
                }
            }
            return true;
        }
        public static void StageController_InitStageByInvitation(StageClassInfo stage)
        {
            Duel = false;
            if (stage.id == 60002)
                Duel = true;
            if (!PassiveAbility_1890003_init)
                PassiveAbility_1890003_InitList();
            Singleton<ContractLoader>.Instance.Init();
            Cheat = false;
        }
        public static void StageController_InitStageByEndContentsStage(StageClassInfo stage)
        {
            Duel = false;
            if (stage.id == 70010)
                Duel = true;
            if (!PassiveAbility_1890003_init)
                PassiveAbility_1890003_InitList();
            Singleton<ContractLoader>.Instance.Init();
            Cheat = false;
        }
        public static void StageController_InitStageByCreature()
        {
            if (!PassiveAbility_1890003_init)
                PassiveAbility_1890003_InitList();
        }
        public static bool StageController_EndBattlePhase()
        {
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
            {
                if(unit.bufListDetail.GetActivatedBufList().Exists((Predicate<BattleUnitBuf>)(x => x is ContractStatBonus)))
                {
                    if (!Harmony_Patch.CombaltData.ContainsKey(unit.UnitData))
                    {
                        Harmony_Patch.CombaltData.Add(unit.UnitData, (int)unit.hp);
                    }
                    else
                    {
                        Harmony_Patch.CombaltData[unit.UnitData] = (int)unit.hp;
                    }
                }
            }
            ContractAttribution.Inition.Clear();
            return true;
        }
        public static void StageController_GameOver()
        {
            Singleton<ContractLoader>.Instance.Init();
            CombaltData.Clear();
        }
        public static void LibraryModel_OnClearStage(int stageId)
        {
            StageClassInfo info=Singleton<StageClassInfoList>.Instance.GetData(stageId);
            Singleton<ContractRewardSystem>.Instance.CheckReward(info);
        }
        public static bool BattleUnitBuf_Philip_OverHeat_Init(BattleUnitModel owner, BattleUnitBuf_Philip_OverHeat __instance)
        {
            ContingecyContract_Philip_Burn burn = owner.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is ContingecyContract_Philip_Burn)) as ContingecyContract_Philip_Burn;
            if (burn!=null)
            {
                owner.bufListDetail.AddBuf(new ContingecyContract_Philip_Burn.OverHeat_cc(burn.Level));
                __instance.Destroy();
                return false;
            }
            return true;
        }
        public static bool BattleObjectManager_Clear()
        {
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetList())
            {
                unit.Book.SetHp(unit.Book.ClassInfo.EquipEffect.Hp);
                unit.Book.SetBp(unit.Book.ClassInfo.EquipEffect.Break);
                unit.Book.SetSpeedDiceMax(unit.Book.ClassInfo.EquipEffect.Speed);
                unit.Book.SetSpeedDiceMin(unit.Book.ClassInfo.EquipEffect.SpeedMin);
                unit.Book.GetType().GetField("_maxPlayPoint", AccessTools.all).SetValue(unit.Book, unit.Book.ClassInfo.EquipEffect.MaxPlayPoint);
                if (UnitBookId.ContainsKey(unit))
                    unit.Book.ClassInfo.id = UnitBookId[unit];
            }
            UnitBookId.Clear();
            return true;
        }
        public static void DebugConsoleScript_Update()
        {
            if (!Singleton<ContractLoader>.Instance.bInit)
            {
                Singleton<ContractLoader>.Instance.Init();
                Singleton<ContractLoader>.Instance.bInit = true;
            }
        }
        public static bool PassiveAbility_1307012_AddThread(int round, BattleUnitModel ___owner)
        {
            int puppetId = -1;
            int num;
            switch (round)
            {
                case 1:
                    puppetId = 1307021;
                    num = 2;
                    break;
                case 2:
                    puppetId = 1307031;
                    num = 3;
                    break;
                case 3:
                    puppetId = 1307041;
                    num = 3;
                    break;
                case 4:
                    puppetId = 1307051;
                    num = 4;
                    break;
                default:
                    return false;
            }
            if (puppetId == -1)
                return false;
            BattleUnitModel owner = BattleObjectManager.instance.GetAliveList(___owner.faction).Find(x => x.UnitData.unitData.EnemyUnitId == puppetId);
            if (owner == null)
                return false;
            if(owner.bufListDetail.GetActivatedBuf(KeywordBuf.JaeheonPuppetThread) is BattleUnitBuf_Jaeheon_PuppetThread thread)
            {
                thread.stack += num;
            }
            else
            {
                BattleUnitBuf_Jaeheon_PuppetThread jaeheonPuppetThread = new BattleUnitBuf_Jaeheon_PuppetThread();
                jaeheonPuppetThread.Init(owner);
                jaeheonPuppetThread.stack = num;
                owner.bufListDetail.AddBuf((BattleUnitBuf)jaeheonPuppetThread);
            }
            return false;
        }
        public static void LoadContract()
        {
            Debug.PathDebug("/Contracts", PathType.Directory);
            Debug.XmlFileDebug();
            Singleton<ContractXmlList>.Instance.Init();
            foreach (FileInfo file in new DirectoryInfo(ModPath + "/Contracts").GetFiles())
            {            
                using (StringReader stringReader = new StringReader(File.ReadAllText(file.FullName)))
                {
                    try
                    {
                        ContractList List = (ContractList)new XmlSerializer(typeof(ContractList)).Deserialize((TextReader)stringReader);
                        Singleton<ContractXmlList>.Instance.Add(List.ContractsList);
                    }
                    catch (Exception ex)
                    {
                        Debug.Error("XMl", ex);
                    }
                }
            }
        }
        public static void PassiveAbility_1890003_InitList()
        {
            AvailablePassive = new List<PassiveXmlInfo>();
            List<PassiveXmlInfo> list = typeof(PassiveXmlList).GetField("_list", AccessTools.all).GetValue(Singleton<PassiveXmlList>.Instance) as List<PassiveXmlInfo>;
            AvailablePassive.AddRange(list.FindAll(x => x.id > 200000 && x.id < 202000 && x.CanGivePassive == true));
            AvailablePassive.AddRange(list.FindAll(x => x.id > 210000 && x.id < 270000 && x.CanGivePassive == true));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(230016));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(231016));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(232016));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(230028));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(230128));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(230228));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(240008));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(240108));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(240208));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(240508));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(240028));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(240128));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(240228));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(240328));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(240428));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(240528));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(240628));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(250013)); 
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(250022));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(250422));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(250227));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(250327));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(250427));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(250136));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(250236));
            AvailablePassive.Remove(Singleton<PassiveXmlList>.Instance.GetData(250336));
            AvailablePassive.Add(Singleton<PassiveXmlList>.Instance.GetData(10001));
            AvailablePassive.Add(Singleton<PassiveXmlList>.Instance.GetData(10004));
            AvailablePassive.Add(Singleton<PassiveXmlList>.Instance.GetData(10008));
            AvailablePassive.Add(Singleton<PassiveXmlList>.Instance.GetData(1300001));
            PassiveAbility_1890003_init = true;
        }
        public static List<int> cheatList => new List<int> (){ 18000000, 18100000,18700000, 18710000, 18800000, 18900000 };
    }
}
