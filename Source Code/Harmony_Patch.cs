﻿using System;
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
using MyJsonTool;
using TMPro;

namespace Contingecy_Contract
{
    public class Harmony_Patch
    {
        public static HashSet<int> ClearList = new HashSet<int>();
        public static List<Contract> JsonList = new List<Contract>();
/*        public static bool UIInit = false;*/
        public static int PatchNum = 0;
        public static Harmony harmony;
        public static List<DiceBehaviour> passive18900002_Makred = new List<DiceBehaviour>();
        public static Dictionary<UnitBattleDataModel, int> CombaltData = new Dictionary<UnitBattleDataModel, int>();
        public static Dictionary<BattleUnitModel, LorId> UnitBookId = new Dictionary<BattleUnitModel, LorId>();
        public static Dictionary<LorId, int> ThumbPathDictionary = new Dictionary<LorId, int>();
        public static Dictionary<LorId, Sprite> NonThumbSprite = new Dictionary<LorId, Sprite>();
        public static string ModPath;
        public static bool Duel = false;
        public Harmony_Patch()
        {
            harmony = new Harmony("Hydracerynitis.ContingecyContract");
            ModPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            Debug.ModPatchDebug();
            LoadContract();
            LoadThumb();
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
            MethodInfo Method13 = typeof(PlayHistoryModel).GetMethod("LoadFromSaveData", AccessTools.all);
            Patch(Method13, "PlayHistoryModel_LoadFromSaveData", false);
/*            MethodInfo Method14 = typeof(PlayHistoryModel).GetMethod("GetSaveData", AccessTools.all);
            Patch(Method14, "PlayHistoryModel_GetSaveData", false);*/
            MethodInfo Method15 = typeof(BattleCardAbilityDescXmlList).GetMethod("GetAbilityDesc", new Type[]{ typeof(DiceBehaviour)});
            Patch(Method15, "BattleCardAbilityDescXmlList_GetAbilityDesc", false);
            MethodInfo Method16 = typeof(StageController).GetMethod("StartAction",AccessTools.all);
            Patch(Method16, "StageController_StartAction", true);
            MethodInfo Method17 = typeof(BattleUnitBuf_Resistance).GetMethod("get_keywordId", AccessTools.all);
            Patch(Method17, "BattleUnitBuf_Resistance_get_keywordId", false);
            MethodInfo Method18 = typeof(DiceCardSelfAbility_elenaMinionStrong).GetMethod("OnSucceedAttack", new Type[] { });
            Patch(Method18, "DiceCardSelfAbility_elenaMinionStrong_OnSucceedAttack", false);
            MethodInfo Method19 = typeof(DiceCardSelfAbility_greta_trample).GetMethod("OnSucceedAttack", new Type[] { });
            Patch(Method19, "DiceCardSelfAbility_greta_trample_OnSucceedAttack", false);
            MethodInfo Method20 = typeof(BattleUnitBuf_Greta_Meat_Librarian).GetMethod("OnBreakState", AccessTools.all);
            Patch(Method20, "BattleUnitBuf_Greta_Meat_Librarian_OnBreakState", false);
            MethodInfo Method21 = typeof(BattleUnitBuf_Greta_Meat_Librarian).GetMethod("OnDie", AccessTools.all);
            Patch(Method21, "BattleUnitBuf_Greta_Meat_Librarian_OnDie", true);
            MethodInfo Method22 = typeof(BattleUnitBuf_Greta_Meat).GetMethod("OnTakeDamageByAttack", AccessTools.all);
            Patch(Method22, "BattleUnitBuf_Greta_Meat_OnTakeDamageByAttack", true);
            MethodInfo Method23 = typeof(BattleUnitModel).GetMethod("CheckCardAvailable", AccessTools.all);
            Patch(Method23, "BattleUnitModel_CheckCardAvailable", false);
            MethodInfo Method24 = typeof(AssemblyManager).GetMethod("CreateInstance_DiceCardSelfAbility", AccessTools.all);
            Patch(Method24, "AssemblyManager_CreateInstance_DiceCardSelfAbility", false);
            MethodInfo Method25 = typeof(AssemblyManager).GetMethod("CreateInstance_BehaviourAction", AccessTools.all);
            Patch(Method25, "AssemblyManager_CreateInstance_BehaviourAction", false);
            MethodInfo Method26 = typeof(AssemblyManager).GetMethod("CreateInstance_PassiveAbility", AccessTools.all);
            Patch(Method26, "AssemblyManager_CreateInstance_PassiveAbility", false);
            MethodInfo Method27 = typeof(DropBookInventoryModel).GetMethod("GetBookList_invitationBookList", AccessTools.all);
            Patch(Method27, "DropBookInventoryModel_GetBookList_invitationBookList", false);
            MethodInfo Method28 = typeof(UIInvitationDropBookSlot).GetMethod("SetData_DropBook", AccessTools.all);
            Patch(Method28, "UIInvitationDropBookSlot_SetData_DropBook", false);
            MethodInfo Method29 = typeof(StageController).GetMethod("CheckStoryBeforeBattle", AccessTools.all);
            Patch(Method29, "StageController_CheckStoryBeforeBattle", true);
            MethodInfo Method30 = typeof(DiceBehaviour).GetMethod("Copy", AccessTools.all);
            Patch(Method30, "DiceBehaviour_Copy", false);
            MethodInfo Method31 = typeof(BattleUnitModel).GetMethod("RecoverHP", AccessTools.all);
            Patch(Method31, "BattleUnitModel_RecoverHP", true);
            MethodInfo Method32 = typeof(BookModel).GetMethod("GetThumbSprite", AccessTools.all);
            Patch(Method32, "BookModel_GetThumbSprite", false);
            MethodInfo Method33 = typeof(BookXmlInfo).GetMethod("GetThumbSprite", AccessTools.all);
            Patch(Method33, "BookXmlInfo_GetThumbSprite", false);
            MethodInfo Method34 = typeof(UICharacterRenderer).GetMethod("SetCharacter", AccessTools.all);
            Patch(Method34, "UICharacterRenderer_SetCharacter", false);
            MethodInfo Method35 = typeof(SdCharacterUtil).GetMethod("CreateSkin", AccessTools.all);
            Patch(Method35, "SdCharacterUtil_CreateSkin", false);
/*            MethodInfo Method36 = typeof(UI.UIController).GetMethod("Initialize", AccessTools.all);
            Patch(Method36, "UI_UIController_Initialize", false);*/
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
        public static bool HasMethod(Type type, string methodName)
        {
            foreach (MemberInfo method in type.GetMethods())
            {
                if (method.Name == methodName)
                    return true;
            }
            return false;
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
            List<int> bmSave = Tools.Load<List<int>>("ContingecyContract_Save");
            if (bmSave != null && bmSave.Count>0)
            {
                bmSave.ForEach(x => ClearList.Add(x));
                return;
            }
            SaveData save=data.GetData("ContingecyContract_ChallengeProgress");
            if (save == null)
                return;
            if (save.GetInt("Philiph_Risk") >= 1)
                ClearList.Add(18100000);
            if (save.GetInt("Eileen_Risk") >= 1)
                ClearList.Add(18200000);
            if (save.GetInt("Greta_Risk") >= 1)
                ClearList.Add(18300000);
            if (save.GetInt("Bremen_Risk") >= 1)
                ClearList.Add(18400000);
            if (save.GetInt("Tanya_Risk") >= 1)
                ClearList.Add(18600000);
            if (save.GetInt("Jaeheon_Risk") >= 1)
                ClearList.Add(18700000);
            if (save.GetInt("Elena_Risk") >= 1)
                ClearList.Add(18800000);
            if (save.GetInt("Orange_Path") >= 1)
                ClearList.Add(18810000);
            if (save.GetInt("Pluto_Risk") >= 1)
                ClearList.Add(18900000);
            if (save.GetInt("Ensemble_Complete") >= 1)
                ClearList.Add(18000000);
            new List<int>(ClearList).Save<List<int>>("ContingecyContract_Save");
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
            BattlePlayingCardDataInUnitModel retaliate = null;
            foreach (PassiveAbilityBase passive in card.target.passiveDetail.PassiveList)
            {
                if (HasMethod(passive.GetType(), "Retaliate"))
                {
                    try
                    {
                        retaliate = (BattlePlayingCardDataInUnitModel)passive.GetType().GetMethod("Retaliate").Invoke(passive, new object[1] { card });
                    }
                    catch (Exception ex)
                    {
                        Debug.Error("RetaliateBug", ex);
                    }
                }
            }
            if (retaliate == null)
                return true;
            Singleton<StageController>.Instance.sp(card, (retaliate));
            return false;
        }
        public static void BattleUnitBuf_Resistance_get_keywordId(ref string __result,BattleUnitModel ____owner)
        {
            if (____owner != null && ____owner.Book != null && ____owner.Book.GetBookClassInfoId() == Tools.MakeLorId(18300000))
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
        public static void BattleUnitModel_RecoverHP(BattleUnitModel __instance, ref int v)
        {
            foreach (PassiveAbilityBase passive in __instance.passiveDetail.PassiveList)
            {
                if (HasMethod(passive.GetType(), "GetRecoveryBonus"))
                {
                    try
                    {
                        v += (int)passive.GetType().GetMethod("GetRecoveryBonus").Invoke(passive, new object[1] { v });
                    }
                    catch (Exception ex)
                    {
                        Debug.Error("RecoveryBug", ex);
                    }
                }
            }
        }
        public static void BookModel_GetThumbSprite(ref Sprite __result, BookModel __instance)
        {
            if (ThumbPathDictionary.ContainsKey(__instance.GetBookClassInfoId())) 
                __result= Resources.Load<Sprite>("Sprites/Books/Thumb/" + ThumbPathDictionary[__instance.GetBookClassInfoId()]);
            else if(NonThumbSprite.ContainsKey(__instance.GetBookClassInfoId()))
            {
                if (NonThumbSprite[__instance.GetBookClassInfoId()] != null)
                    __result = NonThumbSprite[__instance.GetBookClassInfoId()];
                else
                {
                    try
                    {
                        GameObject prefab = Singleton<AssetBundleManagerRemake>.Instance.LoadCharacterPrefab(__instance.GetOriginalCharcterName(), "_N", out string resourcename);
                        if (prefab == null)
                            return;
                        CharacterAppearance character = prefab.GetComponent<CharacterAppearance>();
                        CharacterMotion Default = character.GetCharacterMotion(ActionDetail.Default);
                        if (Default == null)
                            return;
                        SpriteSet Body = Default.motionSpriteSet.Find(x => x.sprType == CharacterAppearanceType.Body);
                        if (Body != null)
                        {
                            NonThumbSprite[__instance.GetBookClassInfoId()] = Default.motionSpriteSet.Find(x => x.sprType == CharacterAppearanceType.Body).sprRenderer.sprite;
                            __result = NonThumbSprite[__instance.GetBookClassInfoId()];
                        }

                    }
                    catch(Exception ex)
                    {
                        Debug.Error("PrefabThumbe", ex);
                    }
                }
            }

        }
        public static void BookXmlInfo_GetThumbSprite(ref Sprite __result, BookXmlInfo __instance)
        {
            if (ThumbPathDictionary.ContainsKey(__instance.id))
                __result = Resources.Load<Sprite>("Sprites/Books/Thumb/" + ThumbPathDictionary[__instance.id]);
            else if (NonThumbSprite.ContainsKey(__instance.id))
            {
                if (NonThumbSprite[__instance.id] != null)
                    __result = NonThumbSprite[__instance.id];
                else
                {
                    try
                    {
                        GameObject prefab = Singleton<AssetBundleManagerRemake>.Instance.LoadCharacterPrefab(__instance.GetCharacterSkin(), "_N", out string resourcename);
                        if (prefab == null)
                            return;
                        CharacterAppearance character = prefab.GetComponent<CharacterAppearance>();
                        CharacterMotion Default = character.GetCharacterMotion(ActionDetail.Default);
                        if (Default == null)
                            return;
                        SpriteSet Body = Default.motionSpriteSet.Find(x => x.sprType == CharacterAppearanceType.Body);
                        if (Body != null)
                        {
                            NonThumbSprite[__instance.id] = Default.motionSpriteSet.Find(x => x.sprType == CharacterAppearanceType.Body).sprRenderer.sprite;
                            __result = NonThumbSprite[__instance.id];
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.Error("PrefabThumbe", ex);
                    }
                }
            }

        }
        public static void UICharacterRenderer_SetCharacter(UnitDataModel unit, int index)
        {
            if (unit.GetCustomBookItemData() != null && NonHeadEquipPage.Exists(x => unit.GetCustomBookItemData().GetBookClassInfoId() == Tools.MakeLorId(x)) || NonHeadEquipPage.Exists(x => unit.bookItem.GetBookClassInfoId()==Tools.MakeLorId(x)) && unit.GetCustomBookItemData() == null)
            {
                UICharacter character = SingletonBehavior<UICharacterRenderer>.Instance.characterList[index];
                CustomizedAppearance appearance = typeof(CharacterAppearance).GetField("_customAppearance", AccessTools.all).GetValue(character.unitAppearance) as CustomizedAppearance;
                if (appearance != null)
                {
                    foreach (SpriteRenderer allSprite in appearance.allSpriteList)
                    {
                        SpriteMask spriteMask = allSprite.GetComponent<SpriteMask>();
                        if (spriteMask != null)
                            character.unitAppearance.maskControl.maskList.Remove(spriteMask);
                    }
                    if (appearance.gameObject != null)
                        UnityEngine.Object.Destroy(appearance.gameObject);
                    typeof(CharacterAppearance).GetField("_customAppearance", AccessTools.all).SetValue(character.unitAppearance,null);
                }
            }
        }
        public static void SdCharacterUtil_CreateSkin(UnitDataModel unit, CharacterAppearance __result)
        {
            if (unit.GetCustomBookItemData()!=null && NonHeadEquipPage.Exists(x => unit.GetCustomBookItemData().GetBookClassInfoId() == Tools.MakeLorId(x)) || NonHeadEquipPage.Exists(x => unit.bookItem.GetBookClassInfoId() == Tools.MakeLorId(x)) && unit.GetCustomBookItemData() == null)
            {
                CustomizedAppearance appearance = typeof(CharacterAppearance).GetField("_customAppearance", AccessTools.all).GetValue(__result) as CustomizedAppearance;
                if (appearance != null)
                {
                    foreach (SpriteRenderer allSprite in appearance.allSpriteList)
                    {
                        SpriteMask spriteMask = allSprite.GetComponent<SpriteMask>();
                        if (spriteMask != null)
                            __result.maskControl.maskList.Remove(spriteMask);
                    }
                    if (appearance.gameObject != null)
                        UnityEngine.Object.Destroy(appearance.gameObject);
                    typeof(CharacterAppearance).GetField("_customAppearance", AccessTools.all).SetValue(__result, null);
                }
            }
        }
/*        public static void UI_UIController_Initialize()
        {
            if (!UIInit)
            {
                UI.UIController.Instance.gameObject.AddComponent<CCGUI.CCGUI>();
                UIInit = true;
            }
        }*/
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

            Debug.PathDebug("/Contracts", PathType.Directory);
            Debug.XmlFileDebug("/Contracts");
            foreach (FileInfo file in new DirectoryInfo(ModPath + "/Contracts").GetFiles())
            {
                try
                {
                    ContractBluePrintList list = File.ReadAllText(file.FullName).ToObject<ContractBluePrintList>();
                    foreach (ContractBluePrint bluePrint in list.CCs)
                    {
                        System.Type type = System.Type.GetType("Contingecy_Contract.ContingecyContract_" + bluePrint.Type);
                        if (type == null)
                            continue;
                        if (bluePrint.Variation <= 0)
                        {
                            JsonList.Add(new Contract() { Type = bluePrint.Type, Variant = 0, desc = bluePrint.desc, contractType = bluePrint.contractType, Faction = bluePrint.Faction, Level = bluePrint.BaseLevel, Bonus = bluePrint.BonusBaseLevel, Stageid = bluePrint.Stageid, Conflict = bluePrint.Conflict });
                            Debug.Log("XML: {0} Added", bluePrint.Type);
                        }
                        else
                        {
                            ContingecyContract contract = Activator.CreateInstance(type, new object[] { 0 }) as ContingecyContract;
                            for (int i = 1; i <= bluePrint.Variation; i++)
                            {
                                contract.Level = i;
                                Contract item = new Contract() { Type = bluePrint.Type, Variant = i, contractType = bluePrint.contractType, Faction = bluePrint.Faction, Level = bluePrint.BaseLevel + i * bluePrint.Step, Bonus = bluePrint.BonusBaseLevel + i * bluePrint.BonusStep, Stageid = bluePrint.Stageid, Conflict = bluePrint.Conflict };
                                bluePrint.desc.ForEach(x => item.desc.Add(new ContractDesc() { language = x.language, name = x.name + " " + i.ToString(), desc = string.Format(x.desc, contract.GetFormatParam) }));
                                JsonList.Add(item);
                                Debug.Log("XML: {0} Added", item.Id);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Error("JSON", ex);
                }
            }
/*            try
            {
                foreach (NewContract bluePrint in ContractXmlList.JsonList)
                {
                    Debug.ErrorLog("JsonItem", bluePrint.ToJson<NewContract>());
                }
            }
            catch (Exception ex)
            {
                Debug.Error("ToJSOn", ex);
            }*/
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
                        if (node.InnerText == "null")
                        {
                            NonThumbSprite[Tools.MakeLorId(key)] = null;
                            continue;
                        }
                        int value = Int32.Parse(node.InnerText);
                        ThumbPathDictionary[Tools.MakeLorId(key)] = value;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Error("ThumbLoadError", ex);
                }
            }
        }
        public static List<int> NonHeadEquipPage = new List<int>() { 18810000 };
    }
}
