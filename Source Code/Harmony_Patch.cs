using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using GameSave;
using ContractReward;
using System.Diagnostics;
using UI;
using LOR_DiceSystem;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using HarmonyLib;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace Contingecy_Contract
{
    public class Harmony_Patch
    {
        public static List<DiceBehaviour> passive18900002_Makred;
        public static Dictionary<UnitBattleDataModel, int> CombaltData;
        public static Dictionary<BattleUnitModel, int> UnitBookId;
        public static Dictionary<int, int> ThumbPathDictionary;
        public static string ModPath;
        public static bool Duel;
        public static ChallengeProgress Progess;
        public Harmony_Patch()
        {
            Harmony harmony = new Harmony("Hydracerynitis.ContingecyContract");
            ModPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            Debug.ModPatchDebug();
            LoadContract();
            LoadThumb();
            Singleton<ContractLoader>.Instance.bInit=false;
            CombaltData = new Dictionary<UnitBattleDataModel, int>();
            ContractAttribution.Inition = new List<BattleUnitModel>();
            UnitBookId = new Dictionary<BattleUnitModel, int>();
            Progess = new ChallengeProgress();
            passive18900002_Makred = new List<DiceBehaviour>();
            MethodInfo Method1 = typeof(StageNameXmlList).GetMethod("GetName", AccessTools.all);
            MethodInfo Patch1 = typeof(Harmony_Patch).GetMethod("StageNameXmlList_GetName");
            try
            {
                harmony.Patch(Method1, null, new HarmonyMethod(Patch1), null, null);
                Debug.Log("Patch: {0} succeed",Patch1.Name);
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
                Debug.Log("Patch: {0} succeed", Patch2.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch2.Name, ex);
            }
            MethodInfo Method3 = typeof(StageController).GetMethod("InitStageByInvitation", AccessTools.all);
            MethodInfo Patch3 = typeof(Harmony_Patch).GetMethod("StageController_InitStageByInvitation");
            try
            {
                harmony.Patch(Method3, new HarmonyMethod(Patch3),null , null, null);
                Debug.Log("Patch: {0} succeed", Patch3.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch3.Name, ex);
            }
            MethodInfo Method4 = typeof(StageController).GetMethod("InitStageByEndContentsStage", AccessTools.all);
            MethodInfo Patch4 = typeof(Harmony_Patch).GetMethod("StageController_InitStageByEndContentsStage");
            try
            {
                harmony.Patch(Method4, new HarmonyMethod(Patch4), null, null, null);
                Debug.Log("Patch: {0} succeed", Patch4.Name);
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
                Debug.Log("Patch: {0} succeed", Patch5.Name);
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
                Debug.Log("Patch: {0} succeed", Patch6.Name);
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
                Debug.Log("Patch: {0} succeed", Patch7.Name);
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
                Debug.Log("Patch: {0} succeed", Patch8.Name);
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
                Debug.Log("Patch: {0} succeed", Patch9.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch9.Name, ex);
            }
            MethodInfo Method10 = typeof(PassiveAbility_1307012).GetMethod("AddThread", AccessTools.all);
            MethodInfo Patch10 = typeof(Harmony_Patch).GetMethod("PassiveAbility_1307012_AddThread");
            try
            {
                harmony.Patch(Method10, new HarmonyMethod(Patch10), null, null, null);
                Debug.Log("Patch: {0} succeed", Patch10.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch10.Name, ex);
            }
            MethodInfo Method11 = typeof(StageController).GetMethod("InitStageByCreature", AccessTools.all);
            MethodInfo Patch11 = typeof(Harmony_Patch).GetMethod("StageController_InitStageByCreature");
            try
            {
                harmony.Patch(Method11, null , new HarmonyMethod(Patch11), null, null);
                Debug.Log("Patch: {0} succeed", Patch11.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch11.Name, ex);
            }
            MethodInfo Method12 = typeof(StageController).GetMethod("InitCommon", AccessTools.all);
            MethodInfo Patch12 = typeof(Harmony_Patch).GetMethod("StageController_InitCommon");
            try
            {
                harmony.Patch(Method12, new HarmonyMethod(Patch12),null , null, null);
                Debug.Log("Patch: {0} succeed", Patch12.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch12.Name, ex);
            }
            MethodInfo Method13 = typeof(BookModel).GetMethod("GetThumbPath", AccessTools.all);
            MethodInfo Patch13 = typeof(Harmony_Patch).GetMethod("BookModel_GetThumbPath");
            try
            {
                harmony.Patch(Method13,null , new HarmonyMethod(Patch13), null, null);
                Debug.Log("Patch: {0} succeed", Patch13.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch13.Name, ex);
            }
            MethodInfo Method14 = typeof(PlayHistoryModel).GetMethod("LoadFromSaveData", AccessTools.all);
            MethodInfo Patch14 = typeof(Harmony_Patch).GetMethod("PlayHistoryModel_LoadFromSaveData");
            try
            {
                harmony.Patch(Method14, null, new HarmonyMethod(Patch14), null, null);
                Debug.Log("Patch: {0} succeed", Patch14.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch14.Name, ex);
            }
            MethodInfo Method15 = typeof(PlayHistoryModel).GetMethod("GetSaveData", AccessTools.all);
            MethodInfo Patch15 = typeof(Harmony_Patch).GetMethod("PlayHistoryModel_GetSaveData");
            try
            {
                harmony.Patch(Method15, null, new HarmonyMethod(Patch15), null, null);
                Debug.Log("Patch: {0} succeed", Patch15.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch15.Name, ex);
            }
            MethodInfo Method16 = typeof(BattleCardAbilityDescXmlList).GetMethod("GetAbilityDesc", new Type[]{ typeof(DiceBehaviour)});
            MethodInfo Patch16 = typeof(Harmony_Patch).GetMethod("BattleCardAbilityDescXmlList_GetAbilityDesc");
            try
            {
                harmony.Patch(Method16, null, new HarmonyMethod(Patch16), null, null);
                Debug.Log("Patch: {0} succeed", Patch16.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch16.Name, ex);
            }
            MethodInfo Method17 = typeof(StageController).GetMethod("StartAction",AccessTools.all);
            MethodInfo Patch17 = typeof(Harmony_Patch).GetMethod("StageController_StartAction");
            try
            {
                harmony.Patch(Method17, new HarmonyMethod(Patch17), null, null, null);
                Debug.Log("Patch: {0} succeed", Patch17.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch17.Name, ex);
            }
            MethodInfo Method18 = typeof(BattleUnitBuf_Resistance).GetMethod("get_keywordId", AccessTools.all);
            MethodInfo Patch18 = typeof(Harmony_Patch).GetMethod("BattleUnitBuf_Resistance_get_keywordId");
            try
            {
                harmony.Patch(Method18, null, new HarmonyMethod(Patch18), null, null);
                Debug.Log("Patch: {0} succeed", Patch18.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch18.Name, ex);
            }
            MethodInfo Method19 = typeof(DiceCardSelfAbility_elenaMinionStrong).GetMethod("OnSucceedAttack", new Type[] { });
            MethodInfo Patch19 = typeof(Harmony_Patch).GetMethod("DiceCardSelfAbility_elenaMinionStrong_OnSucceedAttack");
            try
            {
                harmony.Patch(Method19, null, new HarmonyMethod(Patch19), null, null);
                Debug.Log("Patch: {0} succeed", Patch19.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch19.Name, ex);
            }
            MethodInfo Method20 = typeof(DiceCardSelfAbility_greta_trample).GetMethod("OnSucceedAttack", new Type[] { });
            MethodInfo Patch20 = typeof(Harmony_Patch).GetMethod("DiceCardSelfAbility_greta_trample_OnSucceedAttack");
            try
            {
                harmony.Patch(Method20, null, new HarmonyMethod(Patch20), null, null);
                Debug.Log("Patch: {0} succeed", Patch20.Name);
            }
            catch (Exception ex)
            {
                Debug.Error("HP_" + Patch20.Name, ex);
            }
            //MethodInfo Method18 = typeof(BehaviourAction_TanyaSpecialAtk).GetMethod("GetMovingAction", AccessTools.all);
            //MethodInfo Patch18 = typeof(Harmony_Patch).GetMethod("BehaviourAction_TanyaSpecialAtk_GetMovingAction");
            //try
            //{
            //harmony.Patch(Method18, null, null, new HarmonyMethod(Patch18), null);
            //Debug.Log("Patch: {0} succeed", Patch18.Name);
            //}
            //catch (Exception ex)
            //{
            //Debug.Error("HP_" + Patch18.Name, ex);
            //}
        }
        public static void StageNameXmlList_GetName(ref string __result,int id)
        {
            if (CheckDuel(id) || id== 1800000)
                return;
            Singleton<ContractLoader>.Instance.Init();
            __result = TextDataModel.GetText("ui_ContingecyLevel", (object)Singleton<ContractLoader>.Instance.GetLevel(id), (object)__result);
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
        public static void StageController_InitStageByCreature()
        {
        }
        public static void StageController_InitCommon(ref StageClassInfo stage)
        {
            if (Duel)
                return;
            stage = CopyXml(stage);
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
        public static void LibraryModel_OnClearStage(int stageId)
        {
            StageClassInfo info=Singleton<StageClassInfoList>.Instance.GetData(stageId);
            Singleton<ContractRewardSystem>.Instance.CheckReward(info);
        }
        public static void BookModel_GetThumbPath(ref string __result,BookXmlInfo ____classInfo)
        {
            if(ThumbPathDictionary.ContainsKey(____classInfo.id))
                __result= "Sprites/Books/Thumb/" + ThumbPathDictionary[____classInfo.id].ToString();
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
                    unit.Book.ClassInfo.id = UnitBookId[unit];
            }
            UnitBookId.Clear();
            return true;
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
                owner.bufListDetail.AddBuf((BattleUnitBuf)jaeheonPuppetThread);
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
        public static IEnumerable<CodeInstruction>  BehaviourAction_TanyaSpecialAtk_GetMovingAction(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            int InsertIndex = -1;
            Label label = generator.DefineLabel();
            List<CodeInstruction> codeInstructionList = new List<CodeInstruction>(instructions);
            for (int index = 0; index < codeInstructionList.Count; ++index)
            {
                if (InsertIndex==-1)
                {
                    if (codeInstructionList[index].opcode == OpCodes.Bne_Un)
                    {
                        InsertIndex = index - 26;              
                        ((List<Label>)codeInstructionList[index + 1].labels).Add(label);
                    }
                }
            }
            if (InsertIndex == -1)
                Debug.Log("Not Found");
            else
            {
                codeInstructionList.Insert(InsertIndex, new CodeInstruction(OpCodes.Beq_S, label));
                codeInstructionList.Insert(InsertIndex, new CodeInstruction(OpCodes.Ldc_I4, 18600000));
                codeInstructionList.Insert(InsertIndex, new CodeInstruction(OpCodes.Ldfld, typeof(ItemXmlData).GetField("id",AccessTools.all)));
                codeInstructionList.Insert(InsertIndex, new CodeInstruction(OpCodes.Callvirt, typeof(BookModel).GetMethod("get_ClassInfo", AccessTools.all)));
                codeInstructionList.Insert(InsertIndex, new CodeInstruction(OpCodes.Callvirt,typeof(BattleUnitModel).GetMethod("get_customBook", AccessTools.all)));
                codeInstructionList.Insert(InsertIndex, new CodeInstruction(OpCodes.Beq_S, label));
                codeInstructionList.Insert(InsertIndex, new CodeInstruction(OpCodes.Ldc_I4, 18600000));
                codeInstructionList.Insert(InsertIndex, new CodeInstruction(OpCodes.Callvirt, typeof(BookModel).GetMethod("GetBookClassInfoId", AccessTools.all)));
                codeInstructionList.Insert(InsertIndex, new CodeInstruction(OpCodes.Callvirt,typeof(BattleUnitModel).GetMethod("get_Book", AccessTools.all)));
            }        
            return (IEnumerable<CodeInstruction>)codeInstructionList;
        }
        public static bool CheckDuel(int stageId)
        {
            return stageId == 60002 || stageId == 70010 || stageId == 60007;
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
                        Debug.Error("XMl", ex);
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
        public static StageClassInfo CopyXml(StageClassInfo info)
        {
            StageClassInfo output = new StageClassInfo
            {
                id = info.id,
                waveList = new List<StageWaveInfo>(),
                stageType = info.stageType,
                mapInfo = new List<string>(),
                floorNum = info.floorNum,
                chapter = info.chapter,
                invitationInfo = CopyXml(info.invitationInfo),
                extraCondition = CopyXml(info.extraCondition),
                storyList = new List<StageStoryInfo>(),
                isChapterLast = info.isChapterLast,
                _storyType = info._storyType,
                isStageFixedNormal = info.isStageFixedNormal,
                floorOnlyList = new List<SephirahType>(),
                exceptFloorList = new List<SephirahType>(),
                rewardList = new List<BookDropItemInfo>()
            };
            foreach (StageWaveInfo wave in info.waveList)
                output.waveList.Add(CopyXml(wave));
            output.mapInfo.AddRange(info.mapInfo);
            foreach (StageStoryInfo story in info.storyList)
                output.storyList.Add(CopyXml(story));
            output.floorOnlyList.AddRange(info.floorOnlyList);
            output.exceptFloorList.AddRange(info.exceptFloorList);
            foreach (BookDropItemInfo reward in info.rewardList)
                output.rewardList.Add(CopyXml(reward));
            return output;
        }
        public static StageWaveInfo CopyXml(StageWaveInfo info)
        {
            StageWaveInfo output = new StageWaveInfo
            {
                enemyUnitIdList = new List<int>(),
                formationId = info.formationId,
                formationType = info.formationType,
                availableNumber = info.availableNumber,
                aggroScript = info.aggroScript,
                managerScript = info.managerScript
            };
            output.enemyUnitIdList.AddRange(info.enemyUnitIdList);
            return output;
        }
        public static StageInvitationInfo CopyXml(StageInvitationInfo info)
        {
            StageInvitationInfo output = new StageInvitationInfo
            {
                combine = info.combine,
                needsBooks = new List<int>(),
                bookNum = info.bookNum,
                bookValue = info.bookValue
            };
            output.needsBooks.AddRange(info.needsBooks);
            return output;
        }
        public static StageExtraCondition CopyXml(StageExtraCondition info)
        {
            StageExtraCondition output = new StageExtraCondition
            {
                needClearStageList = new List<int>(),
                needLevel = info.needLevel
            };
            output.needClearStageList.AddRange(info.needClearStageList);
            return output;
        }
        public static StageStoryInfo CopyXml(StageStoryInfo info)
        {
            StageStoryInfo output = new StageStoryInfo
            {
                cond = info.cond,
                story = info.story,
                valid = info.valid,
                chapter = info.chapter,
                group = info.group,
                episode = info.episode
            };
            return output;
        }
        public static BookDropItemInfo CopyXml(BookDropItemInfo info)
        {
            BookDropItemInfo output = new BookDropItemInfo
            {
                id = info.id,
                itemType = info.itemType
            };
            return output;
        }
    }
}
