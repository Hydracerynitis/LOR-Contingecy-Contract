using System;
using System.Collections.Generic;
using ContractReward;
using HarmonyLib;
using UI;
using BaseMod;
using GameSave;
using LOR_XML;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    class HP_StageController
    {
        [HarmonyPatch(typeof(StageNameXmlList),nameof(StageNameXmlList.GetName),new Type[] { typeof(int) })]
        [HarmonyPostfix]
        public static void AddCCLevelPrefixToStageName1(ref string __result, int id)
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
        public static void AddCCLevelPrefixToStageName2(ref string __result, StageClassInfo stageInfo)
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
        public static bool StageController_RoundStartPhase_Pre()
        {
            if (StageController.Instance._bCalledRoundStart_system)
                return true;
            ExtentionMethod.triggeredTagTeamCard.Clear();
            firstTime = true; //仅限刷新被控制的速度骰UI一次
            DiceCardSelfAbility_OswaldHide.RecallOswald(); //重招奥斯瓦尔德的中途离场
            if (Duel)
                return true;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (ContractAttribution.Inition.Contains(alive))
                    continue;
                ContractAttribution.Init(alive); //施加合约被动
            }
            return true;
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.ApplyLibrarianCardPhase))]
        [HarmonyPrefix]
        public static void FixUncontrollableSpeedDiceBug()
        {
            if (firstTime)
            {
                foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
                    unit.view?.speedDiceSetterUI?._speedDices.ForEach(x => x.isLightOn = false);
                firstTime = false;
            }
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.InitStageByInvitation))]
        [HarmonyPrefix]
        public static void StageController_InitStageByInvitation_Pre(StageClassInfo stage, ref List<LorId> books)
        {
            Duel = false;
            if (CheckDuel(stage.id))
            {
                Duel = true;
                return;
            }
            books = null; //清理邀请用的书，取消接待失败惩罚
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.InitStageByEndContentsStage))]
        [HarmonyPrefix]
        public static void StageController_InitStageByEndContentsStage_Pre(StageClassInfo stage)
        {
            Duel = false;
            if (CheckDuel(stage.id))
                Duel = true;
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.InitCommon))]
        [HarmonyPrefix]
        public static void StageController_InitCommon_Pre(ref StageClassInfo stage)
        {
            if (Duel)
                return;
            stage = DeepCopyUtil.CopyXml(stage);
            ContractModification.Init(stage); //施加合约对接待的改动
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.EndBattlePhase))]
        [HarmonyPrefix]
        public static bool StageController_EndBattlePhase_Pre() //战后更新所有单位的血量
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
            if (DiceCardSelfAbility_OswaldHide.HidingOswald != null)
                DiceCardSelfAbility_OswaldHide.HidingOswald.UpdateUnitData();
            DiceCardSelfAbility_OswaldHide.HidingOswald = null; 
            return true;
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.GameOver))]
        [HarmonyPostfix]
        public static void StageController_GameOver_Post(bool iswin)
        {
            CCInitializer.CombaltData.Clear();
            SynphonyOrchestra._oldEnemytheme = null;
            if(PassiveAbility_1810002._loopSound != null)
            {
                PassiveAbility_1810002._loopSound.source.Stop();
                PassiveAbility_1810002._loopSound = null;
            }
            
        }
        [HarmonyPatch(typeof(UIBattleResultLeftPanel),nameof(UIBattleResultLeftPanel.SetData))]
        [HarmonyPrefix]
        public static void ClearInvitationLostBooks(TestBattleResultData resultdata)
        {
            resultdata.loseinvitationbooks = new List<LorId>();
        }
        [HarmonyPatch(typeof(LibraryModel), nameof(LibraryModel.OnClearStage))]
        [HarmonyPostfix]
        public static void LibraryModel_OnClearStage_Post(LorId stageId)
        {
            if (!CheckDuel(stageId) && !CheckPlaceHolder(stageId))
                Singleton<ContractRewardSystem>.Instance.CheckReward(Singleton<StageClassInfoList>.Instance.GetData(stageId)); //提供合约奖励
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
            } //蓝人战后数据整理
        }
        [HarmonyPatch(typeof(PlayHistoryModel), nameof(PlayHistoryModel.LoadFromSaveData))]
        [HarmonyPostfix]
        public static void PlayHistoryModel_LoadFromSaveData(SaveData data)
        {
            ModifyLocalize();
            List<int> bmSave = ContractSaveManager.Load<List<int>>("RewardList");
            if (bmSave != null && bmSave.Count > 0)
            {
                bmSave.ForEach(x => ContractRewardSystem.ClearList.Add(x));
                return;
            }
            LegacySaveCompatible(data);
        }
        public static void LegacySaveCompatible(SaveData data)
        {
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
            ContractSaveManager.Save(new List<int>(ContractRewardSystem.ClearList), "RewardList");
        }
        [HarmonyPatch(typeof(BattleObjectManager), nameof(BattleObjectManager.Clear))]
        [HarmonyPrefix]
        public static bool BattleObjectManager_Clear() //清理对核心书页数据的修改
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
        [HarmonyPatch(typeof(StageController), nameof(StageController.StartAction))]
        [HarmonyPrefix]
        public static bool RetaliaterTrigger(BattlePlayingCardDataInUnitModel card)
        {
            BattlePlayingCardDataInUnitModel retaliate = null;
            foreach (PassiveAbilityBase passive in card.target.passiveDetail.PassiveList)
            {
                if (passive is Retaliater)
                    retaliate = (passive as Retaliater).Retaliate(card);
            }
            if (retaliate == null)
                return true;
            Singleton<StageController>.Instance.sp(card, (retaliate));
            return false;
        }
        [HarmonyPatch(typeof(StageController), nameof(StageController.RoundEndPhase_ChoiceEmotionCard))]
        [HarmonyPostfix]
        static void ChooseRolandP5Emotion(ref bool __result)
        {
            if (__result && BattleObjectManager.instance.GetAliveList(Faction.Player).Find(x => x.passiveDetail.HasPassive<PassiveAbility_1700051>()) is BattleUnitModel p5)
            {
                List<BattleEmotionCardModel> selected = p5.UnitData.emotionDetail.PassiveList;
                if (selected.Count >= p5.emotionDetail.EmotionLevel)
                    return;
                List<EmotionCardXmlInfo> RolandEmotion = new List<EmotionCardXmlInfo>();
                for (int i = 18001; i <= 18009; i++)
                    RolandEmotion.Add(EmotionCardXmlList.Instance.GetData(i, SephirahType.ETC).Copy());
                RolandEmotion.RemoveAll(x => selected.Exists(y => y.XmlInfo.id == x.id));
                while (RolandEmotion.Count > 3)
                    RolandEmotion.RemoveAt(RandomUtil.Range(0, RolandEmotion.Count-1));
                RolandEmotion.ForEach(x => x.EmotionLevel = selected.Count + 1);
                StageController.Instance.GetCurrentStageFloorModel().team.currentSelectEmotionLevel--;
                BattleManagerUI.Instance.ui_levelup.SetRootCanvas(true);
                BattleManagerUI.Instance.ui_levelup.Init(selected.Count, RolandEmotion);
                __result = false;
            }
        }
        static bool firstTime = false;
        public static bool UIInit = false;
        public static bool Duel = false;
        public static bool CheckDuel(LorId stageId)
        {
            return stageId == 60002 || stageId == 70010 || stageId == 60007;
        }
        public static bool CheckPlaceHolder(LorId stageId)
        {
            return stageId.packageId == CCInitializer.Pid && stageId.id >= 1800001 && stageId.id <= 1800007;
        }
        public static void ModifyLocalize()
        {
            Dictionary<LorId, PassiveDesc> _dictionary = PassiveDescXmlList.Instance._dictionary;
            if (_dictionary.ContainsKey(Tools.MakeLorId(1)))
                _dictionary[new LorId(1302017)].desc = _dictionary[Tools.MakeLorId(1)].desc;
        }
    }
}
