﻿using System;
using UI;
using LOR_BattleUnit_UI;
using System.Collections.Generic;
using GameSave;
using LOR_DiceSystem;
using System.Reflection;
using UnityEngine;
using System.IO;
using HarmonyLib;
using BaseMod;
using TMPro;
using AutoKeywordUtil;
using ContractReward;

namespace Contingecy_Contract
{

    public class CCInitializer: ModInitializer
    {
        public static readonly string Pid = "ContingencyConract";
        public static int PatchNum = 0;
        public static List<DiceBehaviour> passive18900002_Makred = new List<DiceBehaviour>();
        public static Dictionary<UnitBattleDataModel, int> CombaltData = new Dictionary<UnitBattleDataModel, int>();
        public static Dictionary<BattleUnitModel, LorId> UnitBookId = new Dictionary<BattleUnitModel, LorId>();
        public static string ModPath;
        public override void OnInitializeMod()
        {
            base.OnInitializeMod();
            Harmony harmony = new Harmony("Hydracerynitis.ContingecyContract");
            ModPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            AutoKeywordUtils.RegisterKeywordsFromAssembly(Assembly.GetExecutingAssembly());
            Debug.ModPatchDebug();
            StaticDataManager.LoadStaticData(ModPath);
            StaticDataManager.LoadGameObject();
            StaticDataManager.InitNonThumbDic();
            ModifyEnsemble();
            Singleton<ContractLoader>.Instance.Init();
            harmony.PatchAll(typeof(HP_StageController));
            Debug.Log("Patch Class: StageController succeed");
            harmony.PatchAll(typeof(HP_Miscellaneous));
            Debug.Log("Patch Class: Miscellaneous succeed");
            harmony.PatchAll(typeof(HP_NewTrigger));
            Debug.Log("Patch Class: New_Trigger succeed");
            harmony.PatchAll(typeof(HP_RewardList));
            Debug.Log("Patch Class: Reward List succeed");
            harmony.PatchAll(typeof(HP_SplitDeck));
            Debug.Log("Patch Class: Split Deck succeed");
            harmony.PatchAll(typeof(HP_ModifyOrigin));
            Debug.Log("Patch Class: ModifyOrigin succeed");
            harmony.PatchAll(typeof(HP_ContractSpecific));
            Debug.Log("Patch Class: ContractSpecific) succeed");
            harmony.PatchAll(typeof(HP_RewardPage));
            Debug.Log("Patch Class: RewardPage succeed");       
            new BattleUnitBuf().GetBufIcon(); //cope BaseMod Late loading artwork for CCGUI Artworks
            CCManager.InitializeUI();
        }
        public static bool IsRoland(UnitDataModel __instance) => __instance.OwnerSephirah == SephirahType.Keter && __instance.isSephirah;
        public static bool IsRolandEmotion(EmotionCardXmlInfo card)
        {
            return card.Sephirah == SephirahType.ETC && card.id >= 18001 && card.id <= 18009;
        }
        public static bool CheckTanya(RencounterManager.ActionAfterBehaviour self, int id)
        {
            return CheckTanya(self, new LorId(id));
        }
        public static bool CheckTanya(RencounterManager.ActionAfterBehaviour self, LorId id)
        {
            return self.view.model.Book.GetBookClassInfoId() == id || self.view.model.customBook.ClassInfo.id == id;
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
        public static List<int> NoThumbPage = new List<int>() { 18810000, 17000002, 17000003, 17000004, 17000005, 19100000 };
        public static List<int> NonHeadEquipPage = new List<int>() { 18810000, 17000002 };
    }
}
