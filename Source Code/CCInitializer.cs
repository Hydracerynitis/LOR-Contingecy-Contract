using System;
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
using LOR_XML;
using ContractReward;

namespace Contingecy_Contract
{

    public class CCInitializer: ModInitializer
    {
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
            Debug.ModPatchDebug();
            StaticDataManager.LoadStaticData();
            ModifyEnsemble();
            Singleton<ContractLoader>.Instance.Init();
            harmony.PatchAll(typeof(HP_CoreSystem));
            Debug.Log("Patch Class: CoreSystem succeed");
            harmony.PatchAll(typeof(HP_SubSystem));
            Debug.Log("Patch Class: SubSystem succeed");
            harmony.PatchAll(typeof(HP_ReverberationSystem));
            Debug.Log("Patch Class: ReverberationSystem succeed");
            harmony.PatchAll(typeof(HP_ReverberationUI));
            Debug.Log("Patch Class: ReverberationUI succeed");
            harmony.PatchAll(typeof(HP_RolandUI));
            Debug.Log("Patch Class: RolandUI succeed");
            harmony.PatchAll(typeof(HP_RolandSystem));
            Debug.Log("Patch Class: RolandSystem succeed");
            harmony.PatchAll(typeof(HP_SetCard));
            Debug.Log("Patch Class: SetCard succeed");
            harmony.PatchAll(typeof(HP_Effect));
            Debug.Log("Patch Class: Effect succeed");
            harmony.PatchAll(typeof(HP_RewardList));
            Debug.Log("Patch Class: Reward List succeed");
            harmony.PatchAll(typeof(HP_SplitDeck));
            Debug.Log("Patch Class: Split Deck succeed");
        }
        public static bool IsRoland(UnitDataModel __instance) => __instance.OwnerSephirah == SephirahType.Keter && __instance.isSephirah;
        public static bool IsRolandEmotion(EmotionCardXmlInfo card)
        {
            return card.Sephirah == SephirahType.ETC && card.id >= 18001 && card.id <= 18009;
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
        public static List<int> NoThumbPage = new List<int>() { 18810000, 17000002, 17000003, 17000004, 17000005 };
        public static List<int> NonHeadEquipPage = new List<int>() { 18810000, 17000002 };
    }
}
