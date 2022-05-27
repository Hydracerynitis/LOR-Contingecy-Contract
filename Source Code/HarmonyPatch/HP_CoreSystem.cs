using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using UI;
using BaseMod;
using GameSave;
using LOR_XML;
using Mod;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    class HP_CoreSystem
    {
        [HarmonyPatch(typeof(StageNameXmlList),nameof(StageNameXmlList.GetName),new Type[] { typeof(int) })]
        [HarmonyPostfix]
        public static void StageNameXmlList_GetName_int(ref string __result, int id)
        {
            if (UI.UIController.Instance.CurrentUIPhase == UIPhase.Invitation || UI.UIController.Instance.CurrentUIPhase == UIPhase.BattleSetting || UI.UIController.Instance.CurrentUIPhase == UIPhase.DUMMY)
            {
                LorId newId = new LorId(id);
                if (CheckDuel(newId) || CheckPlaceHolder(newId))
                    return;
                if (Singleton<StageController>.Instance.battleState == StageController.BattleState.None)
                    Singleton<ContractLoader>.Instance.Init();
                __result = TextDataModel.GetText("ui_ContingecyLevel", (object)Singleton<ContractLoader>.Instance.GetLevel(newId), (object)__result);
            }
        }
        [HarmonyPatch(typeof(StageNameXmlList), nameof(StageNameXmlList.GetName), new Type[] { typeof(StageClassInfo) })]
        [HarmonyPostfix]
        public static void StageNameXmlList_GetName_xml(ref string __result, StageClassInfo stageInfo)
        {
            if (UI.UIController.Instance.CurrentUIPhase == UIPhase.Invitation || UI.UIController.Instance.CurrentUIPhase == UIPhase.BattleSetting || UI.UIController.Instance.CurrentUIPhase == UIPhase.DUMMY)
            {
                if (CheckDuel(stageInfo.id) || CheckPlaceHolder(stageInfo.id))
                    return;
                if (Singleton<StageController>.Instance.battleState == StageController.BattleState.None)
                    Singleton<ContractLoader>.Instance.Init();
                __result = TextDataModel.GetText("ui_ContingecyLevel", (object)Singleton<ContractLoader>.Instance.GetLevel(stageInfo.id), (object)__result);
            }
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.RoundStartPhase_System))]
        [HarmonyPrefix]
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
        [HarmonyPatch(typeof(StageController), nameof(StageController.InitStageByInvitation))]
        [HarmonyPrefix]
        public static void StageController_InitStageByInvitation(StageClassInfo stage)
        {
            Duel = false;
            if (CheckDuel(stage.id))
                Duel = true;
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.InitStageByEndContentsStage))]
        [HarmonyPrefix]
        public static void StageController_InitStageByEndContentsStage(StageClassInfo stage)
        {
            Duel = false;
            if (CheckDuel(stage.id))
                Duel = true;
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.InitCommon))]
        [HarmonyPrefix]
        public static void StageController_InitCommon(ref StageClassInfo stage)
        {
            if (Duel)
                return;
            stage = DeepCopyUtil.CopyXml(stage);
            ContractModification.Init(stage);
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.EndBattlePhase))]
        [HarmonyPrefix]
        public static bool StageController_EndBattlePhase()
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
            {
                if (unit.bufListDetail.GetActivatedBufList().Exists(x => x is ContractStatBonus))
                {
                    if (!CCInitializer.CombaltData.ContainsKey(unit.UnitData))
                    {
                        CCInitializer.CombaltData.Add(unit.UnitData, (int)unit.hp);
                    }
                    else
                    {
                        CCInitializer.CombaltData[unit.UnitData] = (int)unit.hp;
                    }
                }
            }
            ContractAttribution.Inition.Clear();
            return true;
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.GameOver))]
        [HarmonyPostfix]
        public static void StageController_GameOver()
        {
            CCInitializer.CombaltData.Clear();
        }
        [HarmonyPatch(typeof(LibraryModel), nameof(LibraryModel.OnClearStage))]
        [HarmonyPostfix]
        public static void LibraryModel_OnClearStage(LorId stageId)
        {
            if (!CheckDuel(stageId) && !CheckPlaceHolder(stageId))
                Singleton<ContractRewardSystem>.Instance.CheckReward(Singleton<StageClassInfoList>.Instance.GetData(stageId));
            if (LibraryModel.Instance.PlayHistory.Clear_EndcontentsAllStage == 1 && stageId == 70010)
                LibraryModel.Instance.PlayHistory.Start_TheBlueReverberationPrimaryBattle = 0;
            if (stageId.id >= 70001 && stageId.id <= 70010)
            {
                LatestDataModel data1 = new LatestDataModel();
                Singleton<SaveManager>.Instance.LoadLatestData(data1);
                data1.LatestStorychapter = 7;
                data1.LatestStorygroup = 4;
                data1.LatestStoryepisode = 1;
                Singleton<SaveManager>.Instance.SaveLatestData(data1);
            }
        }
        [HarmonyPatch(typeof(PlayHistoryModel), nameof(PlayHistoryModel.LoadFromSaveData))]
        [HarmonyPostfix]
        public static void PlayHistoryModel_LoadFromSaveData(SaveData data)
        {
            ModifyLocalize();
            List<int> bmSave = Tools.Load<List<int>>("ContingecyContract_Save");
            if (bmSave != null && bmSave.Count > 0)
            {
                bmSave.ForEach(x => ContractRewardSystem.ClearList.Add(x));
                return;
            }
            SaveData save = data.GetData("ContingecyContract_ChallengeProgress");
            if (save == null)
                return;
            if (save.GetInt("Philiph_Risk") >= 1)
                ContractRewardSystem.ClearList.Add(18100000);
            if (save.GetInt("Eileen_Risk") >= 1)
                ContractRewardSystem.ClearList.Add(18200000);
            if (save.GetInt("Greta_Risk") >= 1)
                ContractRewardSystem.ClearList.Add(18300000);
            if (save.GetInt("Bremen_Risk") >= 1)
                ContractRewardSystem.ClearList.Add(18400000);
            if (save.GetInt("Tanya_Risk") >= 1)
                ContractRewardSystem.ClearList.Add(18600000);
            if (save.GetInt("Jaeheon_Risk") >= 1)
                ContractRewardSystem.ClearList.Add(18700000);
            if (save.GetInt("Elena_Risk") >= 1)
                ContractRewardSystem.ClearList.Add(18800000);
            if (save.GetInt("Orange_Path") >= 1)
                ContractRewardSystem.ClearList.Add(18810000);
            if (save.GetInt("Pluto_Risk") >= 1)
                ContractRewardSystem.ClearList.Add(18900000);
            if (save.GetInt("Ensemble_Complete") >= 1)
                ContractRewardSystem.ClearList.Add(18000000);
            new List<int>(ContractRewardSystem.ClearList).Save<List<int>>("ContingecyContract_Save");
        }
        [HarmonyPatch(typeof(BattleObjectManager), nameof(BattleObjectManager.Clear))]
        [HarmonyPrefix]
        public static bool BattleObjectManager_Clear()
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetList())
            {
                unit.Book.SetHp(unit.Book.ClassInfo.EquipEffect.Hp);
                unit.Book.SetBp(unit.Book.ClassInfo.EquipEffect.Break);
                unit.Book.SetSpeedDiceMax(unit.Book.ClassInfo.EquipEffect.Speed);
                unit.Book.SetSpeedDiceMin(unit.Book.ClassInfo.EquipEffect.SpeedMin);
                unit.Book._maxPlayPoint = unit.Book.ClassInfo.EquipEffect.MaxPlayPoint;
                if (CCInitializer.UnitBookId.ContainsKey(unit))
                {
                    unit.Book.ClassInfo._id = CCInitializer.UnitBookId[unit].id;
                    unit.Book.ClassInfo.workshopID = CCInitializer.UnitBookId[unit].packageId;
                }

            }
            CCInitializer.UnitBookId.Clear();
            return true;
        }
        [HarmonyPatch(typeof(DropBookInventoryModel), nameof(DropBookInventoryModel.GetBookList_invitationBookList))]
        [HarmonyPostfix]
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
        [HarmonyPatch(typeof(StageController), nameof(StageController.CheckStoryBeforeBattle))]
        [HarmonyPrefix]
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
        [HarmonyPatch(typeof(UI.UIController), nameof(UI.UIController.Initialize))]
        [HarmonyPostfix]
        public static void UI_UIController_Initialize()
        {
            if (!UIInit)
            {
                UI.UIController.Instance.gameObject.AddComponent<ContingecyContractGUI>();
                UIInit = true;
            }
        }
        [HarmonyPatch(typeof(EntryScene), nameof(EntryScene.CheckModError))]
        [HarmonyPrefix]
        static void EntryScene_CheckModError_Pre()
        {
            ModContentManager.Instance._logs.RemoveAll(x => x.Contains("energy3") || x.Contains("drawCards3"));
        }
        public static bool UIInit = false;
        public static bool Duel = false;
        public static bool CheckDuel(LorId stageId)
        {
            return stageId == 60002 || stageId == 70010 || stageId == 60007;
        }
        public static bool CheckPlaceHolder(LorId stageId)
        {
            return stageId == Tools.MakeLorId(1800000) || stageId == Tools.MakeLorId(1800007);
        }
        public static void ModifyLocalize()
        {
            Dictionary<LorId, PassiveDesc> _dictionary = typeof(PassiveDescXmlList).GetField("_dictionary", AccessTools.all).GetValue(Singleton<PassiveDescXmlList>.Instance) as Dictionary<LorId, PassiveDesc>;
            if (_dictionary.ContainsKey(Tools.MakeLorId(1)))
                _dictionary[new LorId(1302017)].desc = _dictionary[Tools.MakeLorId(1)].desc;
        }
    }
}
