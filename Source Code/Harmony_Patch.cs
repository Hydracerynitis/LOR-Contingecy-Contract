using System;
using UI;
using LOR_BattleUnit_UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using GameSave;
using ContractReward;
using System.Diagnostics;
using LOR_DiceSystem;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using HarmonyLib;
using BaseMod;
using System.Threading.Tasks;
using System.Reflection.Emit;
using Mod;
using TMPro;

namespace Contingecy_Contract
{
    public class Harmony_Patch
    {
        public static int PatchNum;
        public static Harmony harmony;
        public static List<DiceBehaviour> passive18900002_Makred;
        public static Dictionary<UnitBattleDataModel, int> CombaltData;
        public static Dictionary<BattleUnitModel, LorId> UnitBookId;
        public static Dictionary<int, int> ThumbPathDictionary;
        public static string ModPath;
        public static bool Duel;
        public static ChallengeProgress Progess;
        public Harmony_Patch()
        {
            harmony = new Harmony("Hydracerynitis.ContingecyContract");
            ModPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            Debug.ModPatchDebug();
            LoadContract();
            LoadThumb();
            Duel = false;
            PatchNum = 0;
            Singleton<ContractLoader>.Instance.bInit=false;
            CombaltData = new Dictionary<UnitBattleDataModel, int>();
            ContractAttribution.Inition = new List<BattleUnitModel>();
            UnitBookId = new Dictionary<BattleUnitModel, LorId>();
            Progess = new ChallengeProgress();
            passive18900002_Makred = new List<DiceBehaviour>();
            ModifyEnsemble();
            MethodInfo Method1 = typeof(StageNameXmlList).GetMethod("GetName", new Type[] { typeof(int) });
            Patch(Method1, "StageNameXmlList_GetName_int", false);
            MethodInfo Method2 =  typeof(StageController).GetMethod("RoundStartPhase_System", AccessTools.all);
            Patch(Method2, "StageController_RoundStartPhase_System", true);
            MethodInfo Method3 = typeof(StageController).GetMethod("InitStageByInvitation", AccessTools.all);
            Patch(Method3, "StageController_InitStageByInvitation", true);
            MethodInfo Method4 = typeof(StageController).GetMethod("InitStageByEndContentsStage", AccessTools.all);
            Patch(Method4, "StageController_InitStageByEndContentsStage", true);
            MethodInfo Method5 = typeof(StageController).GetMethod("EndBattlePhase", AccessTools.all);
            Patch(Method5, "StageController_EndBattlePhase", true);
            MethodInfo Method6 = typeof(StageController).GetMethod("GameOver", AccessTools.all);
            Patch(Method6, "StageController_GameOver", false);
            MethodInfo Method7 = typeof(LibraryModel).GetMethod("OnClearStage", AccessTools.all);
            Patch(Method7, "LibraryModel_OnClearStage", false);
            MethodInfo Method8 = typeof(BattleUnitBuf_Philip_OverHeat).GetMethod("Init", AccessTools.all);
            Patch(Method8, "BattleUnitBuf_Philip_OverHeat_Init", true);
            MethodInfo Method9 = typeof(BattleObjectManager).GetMethod("Clear", AccessTools.all);
            Patch(Method9, "BattleObjectManager_Clear", true);
            MethodInfo Method10 = typeof(PassiveAbility_1307012).GetMethod("AddThread", AccessTools.all);
            Patch(Method10, "PassiveAbility_1307012_AddThread", true);
            MethodInfo Method11 = typeof(StageNameXmlList).GetMethod("GetName", new Type[] { typeof(StageClassInfo) });
            Patch(Method11, "StageNameXmlList_GetName_xml", false);
            MethodInfo Method12 = typeof(StageController).GetMethod("InitCommon", AccessTools.all);
            Patch(Method12, "StageController_InitCommon", true);
            MethodInfo Method14 = typeof(PlayHistoryModel).GetMethod("LoadFromSaveData", AccessTools.all);
            Patch(Method14, "PlayHistoryModel_LoadFromSaveData", false);
            MethodInfo Method15 = typeof(PlayHistoryModel).GetMethod("GetSaveData", AccessTools.all);
            Patch(Method15, "PlayHistoryModel_GetSaveData", false);
            MethodInfo Method16 = typeof(BattleCardAbilityDescXmlList).GetMethod("GetAbilityDesc", new Type[]{ typeof(DiceBehaviour)});
            Patch(Method16, "BattleCardAbilityDescXmlList_GetAbilityDesc", false);
            MethodInfo Method17 = typeof(StageController).GetMethod("StartAction",AccessTools.all);
            Patch(Method17, "StageController_StartAction", true);
            MethodInfo Method18 = typeof(BattleUnitBuf_Resistance).GetMethod("get_keywordId", AccessTools.all);
            Patch(Method18, "BattleUnitBuf_Resistance_get_keywordId", false);
            MethodInfo Method19 = typeof(DiceCardSelfAbility_elenaMinionStrong).GetMethod("OnSucceedAttack", new Type[] { });
            Patch(Method19, "DiceCardSelfAbility_elenaMinionStrong_OnSucceedAttack", false);
            MethodInfo Method20 = typeof(DiceCardSelfAbility_greta_trample).GetMethod("OnSucceedAttack", new Type[] { });
            Patch(Method20, "DiceCardSelfAbility_greta_trample_OnSucceedAttack", false);
            MethodInfo Method21 = typeof(BattleUnitBuf_Greta_Meat_Librarian).GetMethod("OnBreakState", AccessTools.all);
            Patch(Method21, "BattleUnitBuf_Greta_Meat_Librarian_OnBreakState", false);
            MethodInfo Method22 = typeof(BattleUnitBuf_Greta_Meat_Librarian).GetMethod("OnDie", AccessTools.all);
            Patch(Method22, "BattleUnitBuf_Greta_Meat_Librarian_OnDie", true);
            MethodInfo Method23 = typeof(BattleUnitBuf_Greta_Meat).GetMethod("OnTakeDamageByAttack", AccessTools.all);
            Patch(Method23, "BattleUnitBuf_Greta_Meat_OnTakeDamageByAttack", true);
            MethodInfo Method24 = typeof(BattleUnitModel).GetMethod("CheckCardAvailable", AccessTools.all);
            Patch(Method24, "BattleUnitModel_CheckCardAvailable", false);
            MethodInfo Method25 = typeof(AssemblyManager).GetMethod("CreateInstance_DiceCardSelfAbility", AccessTools.all);
            Patch(Method25, "AssemblyManager_CreateInstance_DiceCardSelfAbility", false);
            MethodInfo Method26 = typeof(AssemblyManager).GetMethod("CreateInstance_BehaviourAction", AccessTools.all);
            Patch(Method26, "AssemblyManager_CreateInstance_BehaviourAction", false);
            MethodInfo Method27 = typeof(AssemblyManager).GetMethod("CreateInstance_PassiveAbility", AccessTools.all);
            Patch(Method27, "AssemblyManager_CreateInstance_PassiveAbility", false);
            MethodInfo Method28 = typeof(DropBookInventoryModel).GetMethod("GetBookList_invitationBookList", AccessTools.all);
            Patch(Method28, "DropBookInventoryModel_GetBookList_invitationBookList", false);
            MethodInfo Method29 = typeof(UIInvitationDropBookSlot).GetMethod("SetData_DropBook", AccessTools.all);
            Patch(Method29, "UIInvitationDropBookSlot_SetData_DropBook", false);
            MethodInfo Method30 = typeof(StageController).GetMethod("CheckStoryBeforeBattle", AccessTools.all);
            Patch(Method30, "StageController_CheckStoryBeforeBattle", true);
            MethodInfo Method31 = typeof(DiceBehaviour).GetMethod("Copy", AccessTools.all);
            Patch(Method31, "DiceBehaviour_Copy", false);
        }
        public static void Patch(MethodInfo method, string patchName, bool prefix)
        {
            PatchNum ++;
            MethodInfo patch= typeof(Harmony_Patch).GetMethod(patchName);
            try
            {
                if (prefix)
                    harmony.Patch(method, prefix: new HarmonyMethod(patch));
                else
                    harmony.Patch(method, postfix: new HarmonyMethod(patch));
                Debug.Log("Patch {0}: {1} succeed", PatchNum.ToString(),patch.Name);
            }
            catch(Exception ex)
            {
                Debug.Error(PatchNum.ToString() + " :HP_" + patch.Name, ex);
            }
        }
        public static void StageNameXmlList_GetName_int(ref string __result,int id)
        {
            if(UI.UIController.Instance.CurrentUIPhase==UIPhase.Invitation || UI.UIController.Instance.CurrentUIPhase == UIPhase.BattleSetting || UI.UIController.Instance.CurrentUIPhase==UIPhase.DUMMY)
            {
                LorId newId = new LorId(id);
                if (CheckDuel(newId) || CheckPlaceHolder(newId))
                    return;
                if (Singleton<StageController>.Instance.battleState == StageController.BattleState.None)
                    Singleton<ContractLoader>.Instance.Init();
                __result = TextDataModel.GetText("ui_ContingecyLevel", (object)Singleton<ContractLoader>.Instance.GetLevel(newId), (object)__result);
            }
        }
        public static void StageNameXmlList_GetName_xml(ref string __result, StageClassInfo stageInfo)
        {
            if (UI.UIController.Instance.CurrentUIPhase == UIPhase.Invitation || UI.UIController.Instance.CurrentUIPhase == UIPhase.BattleSetting || UI.UIController.Instance.CurrentUIPhase == UIPhase.DUMMY)
            {
                if (CheckDuel(stageInfo.id) || CheckPlaceHolder(stageInfo.id))
                    return;
                if (Singleton<StageController>.Instance.battleState == StageController.BattleState.None)
                    Singleton<ContractLoader>.Instance.Init();
                __result = TextDataModel.GetText("ui_ContingecyLevel", (object)Singleton<ContractLoader>.Instance.GetLevel(stageInfo), (object)__result);
            }
        }
        public static bool StageController_RoundStartPhase_System()
        {
            if (Duel)
                return true;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (ContractAttribution.Inition.Contains(alive))
                    continue;
                ContractAttribution.Init(alive);
            }
            return true;
        }
        public static void StageController_InitStageByInvitation(StageClassInfo stage)
        {
            Duel = false;
            if (CheckDuel(stage.id))
                Duel = true;
        }
        public static void StageController_InitStageByEndContentsStage(StageClassInfo stage)
        {
            Duel = false;
            if (CheckDuel(stage.id))
                Duel = true;
        }
        public static void StageController_InitCommon(ref StageClassInfo stage)
        {
            if (Duel)
                return;
            stage = DeepCopyUtil.CopyXml(stage);
            ContractModification.Init(stage);
        }
        public static bool StageController_EndBattlePhase()
        {
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
            {
                if(unit.bufListDetail.GetActivatedBufList().Exists(x => x is ContractStatBonus))
                {
                    if (!CombaltData.ContainsKey(unit.UnitData))
                    {
                        CombaltData.Add(unit.UnitData, (int)unit.hp);
                    }
                    else
                    {
                        CombaltData[unit.UnitData] = (int)unit.hp;
                    }
                }
            }
            ContractAttribution.Inition.Clear();
            return true;
        }
        public static void StageController_GameOver()
        {
            CombaltData.Clear();
        }
        public static void LibraryModel_OnClearStage(LorId stageId)
        {
            StageClassInfo info=Singleton<StageClassInfoList>.Instance.GetData(stageId);
            Singleton<ContractRewardSystem>.Instance.CheckReward(info);
            if (LibraryModel.Instance.PlayHistory.Clear_EndcontentsAllStage == 1 && stageId == 70010)
                LibraryModel.Instance.PlayHistory.Start_TheBlueReverberationPrimaryBattle = 0;
        }
        public static void PlayHistoryModel_LoadFromSaveData(SaveData data)
        {
            Progess.LoadFromSaveData(data.GetData("ContingecyContract_ChallengeProgress"));
        }
        public static void PlayHistoryModel_GetSaveData(ref SaveData __result)
        {
            __result.AddData("ContingecyContract_ChallengeProgress", Progess.GetSaveData());
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
                {
                    unit.Book.ClassInfo._id = UnitBookId[unit].id;
                    unit.Book.ClassInfo.workshopID = UnitBookId[unit].packageId;
                }

            }
            UnitBookId.Clear();
            return true;
        }
        public static void BattleUnitModel_CheckCardAvailable(ref bool __result,BattleUnitModel __instance)
        {
            if (__result && SingletonBehavior<BattleManagerUI>.Instance.selectedAllyDice != null)
            {
                int index = (int)typeof(SpeedDiceUI).GetField("_speedDiceIndex", AccessTools.all).GetValue(SingletonBehavior<BattleManagerUI>.Instance.selectedAllyDice);
                if (!__instance.speedDiceResult[index].isControlable)
                    __result = false;
            }
        }
        public static bool BattleUnitBuf_Philip_OverHeat_Init(BattleUnitModel owner, BattleUnitBuf_Philip_OverHeat __instance)
        {
            if (owner.passiveDetail.PassiveList.Find(x => x is ContingecyContract_Philip_Burn) is ContingecyContract_Philip_Burn burn)
            {
                owner.bufListDetail.AddBuf(new ContingecyContract_Philip_Burn.OverHeat_cc(burn.Level));
                __instance.Destroy();
                return false;
            }
            return true;
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
                owner.bufListDetail.AddBuf(jaeheonPuppetThread);
            }
            return false;
        }
        public static void BattleCardAbilityDescXmlList_GetAbilityDesc(ref string __result,DiceBehaviour behaviour)
        {
            if (passive18900002_Makred.Contains(behaviour))
            {
                __result=__result.Insert(0, TextDataModel.GetText("marked_dice_desc"));
                Debug.Log(behaviour.Detail.ToString()+" "+behaviour.Min.ToString()+"-"+behaviour.Dice.ToString());
            }
        }
        public static bool StageController_StartAction(BattlePlayingCardDataInUnitModel card)
        {
            if (card.target.passiveDetail.HasPassive<PassiveAbility_1860001>())
            {
                BattlePlayingCardDataInUnitModel retaliate= ((PassiveAbility_1860001)card.target.passiveDetail.PassiveList.Find(x => x is PassiveAbility_1860001)).Retaliate(card);
                if (retaliate == null)
                    return true;
                Singleton<StageController>.Instance.sp(card, (retaliate));
                return false;
            }
            else if (card.target.passiveDetail.HasPassive<ContingecyContract_Tanya_Solo>())
            {
                BattlePlayingCardDataInUnitModel retaliate = ((ContingecyContract_Tanya_Solo)card.target.passiveDetail.PassiveList.Find(x => x is ContingecyContract_Tanya_Solo)).Retaliate(card);
                if (retaliate == null)
                    return true;
                Singleton<StageController>.Instance.sp(card, (retaliate));
                return false;
            }
            return true;
        }
        public static void BattleUnitBuf_Resistance_get_keywordId(ref string __result,BattleUnitModel ____owner)
        {
            if (____owner != null && ____owner.Book != null && ____owner.Book.GetBookClassInfoId() == 18300000)
                __result = "Resistance";
        }
        public static void DiceCardSelfAbility_greta_trample_OnSucceedAttack(DiceCardSelfAbility_greta_trample __instance)
        {
            __instance.card.target.bufListDetail.AddBuf(new BattleUnitBuf_Greta_Trampled());
        }
        public static void DiceCardSelfAbility_elenaMinionStrong_OnSucceedAttack(DiceCardSelfAbility_elenaMinionStrong __instance)
        {
            __instance.card.target.bufListDetail.AddBuf(new DiceCardSelfAbility_elenaMinionStrong.BattleUnitBuf_elenaStrongOnce());
        }
        public static void BattleUnitBuf_Greta_Meat_Librarian_OnBreakState(ref float ___hp,BattleUnitModel ____origin, BattleUnitModel ____owner)
        {
            if (____origin == null)
                return;
            double ratio = ____owner.hp / ____owner.MaxHp;
            double hp = ____origin.hp - (1-ratio) * ____origin.MaxHp;
            ___hp = Mathf.Max(1f, (float)hp);
        }
        public static bool BattleUnitBuf_Greta_Meat_Librarian_OnDie(ref BattleUnitModel ____origin, BattleUnitModel ____owner, float ___hp)
        {
            if (____origin == null)
                return false;
            if (____owner.breakDetail.IsBreakLifeZero())
            {
                ____origin.SetHp((int)___hp);
                ____origin.breakDetail.RecoverBreakLife(____origin.MaxBreakLife);
                ____origin.breakDetail.nextTurnBreak = false;
                ____origin.turnState = BattleUnitTurnState.WAIT_CARD;
                ____origin.breakDetail.breakGauge = 0;
                ____origin.breakDetail.RecoverBreak(Mathf.RoundToInt((float)____origin.breakDetail.GetDefaultBreakGauge() * 0.75f));
                ____origin.view.EnableView(true);
                ____origin.Extinct(false);
            }
            else
                ____origin.Die();
            return false;
        }
        public static bool BattleUnitBuf_Greta_Meat_OnTakeDamageByAttack(BattleDiceBehavior atkDice)
        {
            BattleUnitModel owner = atkDice?.owner;
            if (owner == null || owner.faction != Faction.Enemy || owner.UnitData.unitData.EnemyUnitId != 1303011)
                return false;
            double heal = 8;
            if (owner.passiveDetail.PassiveList.Find(x => x is ContingecyContract_Greta_Feast) is ContingecyContract_Greta_Feast Feast)
                heal *= (1 + 0.5 * Math.Pow(2, Feast.Level - 1));
            owner.RecoverHP((int)heal);
            string str = "Creature/MustSee_Scream";
            string src = (double)RandomUtil.valueForProb >= 0.5 ? str + "2" : str + "1";
            owner.battleCardResultLog?.SetCreatureEffectSound(src);
            return false;
        }
        public static void AssemblyManager_CreateInstance_DiceCardSelfAbility(ref DiceCardSelfAbilityBase __result)
        {
            if (__result is DiceCardSelfAbility_Jaeheon_AreaDt)
                __result = new Fix.DiceCardSelfAbility_Jaeheon_AreaDt_New();
        }
        public static void AssemblyManager_CreateInstance_BehaviourAction(ref BehaviourActionBase __result)
        {
            if (__result is BehaviourAction_TanyaSpecialAtk)
                __result = new Fix.BehaviourAction_TanyaSpecialAtk_New();
        }
        public static void AssemblyManager_CreateInstance_PassiveAbility(ref PassiveAbilityBase __result)
        {
            if (__result is PassiveAbility_1302013)
                __result = new Fix.PassiveAbility_1302013_New();
            else if (__result is PassiveAbility_1303012)
                __result = new Fix.PassiveAbility_1303012_New();
            else if (__result is PassiveAbility_1303013)
                __result = new Fix.PassiveAbility_1303013_New();
        }
        public static void DropBookInventoryModel_GetBookList_invitationBookList(ref List<LorId> __result)
        {
            if (LibraryModel.Instance.PlayHistory.Clear_EndcontentsAllStage == 1)
            {
                __result.Add(Tools.MakeLorId(70001));
                __result.Add(Tools.MakeLorId(70002));
                __result.Add(Tools.MakeLorId(70003));
                __result.Add(Tools.MakeLorId(70004));
                __result.Add(Tools.MakeLorId(70005));
                __result.Add(Tools.MakeLorId(70006));
                __result.Add(Tools.MakeLorId(70007));
                __result.Add(Tools.MakeLorId(70008));
                __result.Add(Tools.MakeLorId(70009));
                __result.Add(Tools.MakeLorId(70010));
            }
        }
        public static void UIInvitationDropBookSlot_SetData_DropBook(ref TextMeshProUGUI ___txt_bookNum, LorId bookId)
        {
            if (Singleton<DropBookInventoryModel>.Instance.GetBookCount(bookId) == 0)
                ___txt_bookNum.text = "∞";
        }
        public static bool StageController_CheckStoryBeforeBattle(ref bool __result)
        {
            LorId id = Singleton<StageController>.Instance.GetStageModel().ClassInfo.id;
            if (id.IsBasic() && id.id >= 70001 && id.id <= 70010 && LibraryModel.Instance.PlayHistory.Clear_EndcontentsAllStage == 1 && LibraryModel.Instance.PlayHistory.Start_TheBlueReverberationPrimaryBattle == 0)
            {
                __result = false;
                return false;
            }
            return true;
        }
        public static void DiceBehaviour_Copy(DiceBehaviour __instance, DiceBehaviour __result)
        {
            if(passive18900002_Makred.Contains(__instance))
                passive18900002_Makred.Add(__result);
        }
        public static void ModifyEnsemble()
        {
            List<StageClassInfo> Ensemble = Singleton<StageClassInfoList>.Instance.GetAllDataList().FindAll(x => x.id.IsBasic() && x.id.id >= 70001 && x.id.id <= 70010);
            foreach (StageClassInfo info in Ensemble)
            {
                if (info.invitationInfo.combine != StageCombineType.BookRecipe)
                {
                    info.invitationInfo.combine = StageCombineType.BookRecipe;
                    info.invitationInfo.needsBooks.Add(Tools.MakeLorId(info.id.id));
                    Singleton<StageClassInfoList>.Instance.recipeCondList.Add(info);
                }
            }
        }
        public static bool CheckDuel(LorId stageId)
        {
            return stageId == 60002 || stageId == 70010 || stageId == 60007;
        }
        public static bool CheckPlaceHolder(LorId stageId)
        {
            return stageId == Tools.MakeLorId(1800000) || stageId == Tools.MakeLorId(1800007);
        }
        public static void LoadContract()
        {
            ThumbPathDictionary = new Dictionary<int, int>();
            Debug.PathDebug("/Contracts", PathType.Directory);
            Debug.XmlFileDebug("/Contracts");
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
                        Debug.Error("XML", ex);
                    }
                }
            }
        }
        public static void LoadThumb()
        {
            Debug.PathDebug("/Staticinfo/ThumbPath", PathType.Directory);
            Debug.XmlFileDebug("/Staticinfo/ThumbPath");
            foreach (FileInfo file in new DirectoryInfo(ModPath + "/Staticinfo/ThumbPath").GetFiles())
            {
                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(File.ReadAllText(file.FullName));
                    foreach (XmlNode node in xml.SelectNodes("ThumbPath/Path"))
                    {
                        string str = string.Empty;
                        if (node.Attributes.GetNamedItem("id") != null)
                            str = node.Attributes.GetNamedItem("id").InnerText;
                        int key = Int32.Parse(str);
                        int value = Int32.Parse(node.InnerText);
                        ThumbPathDictionary[key] = value;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Error("ThumbLoadError", ex);
                }
            }
        }
    }
}
