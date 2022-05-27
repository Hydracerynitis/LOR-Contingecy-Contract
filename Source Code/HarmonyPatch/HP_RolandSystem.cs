using System;
using System.Collections.Generic;
using ContractReward;
using BaseMod;
using HarmonyLib;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    class HP_RolandSystem
    {
        //Contract
        [HarmonyPatch(typeof(EnemyTeamStageManager_BlackSilence),nameof(EnemyTeamStageManager_BlackSilence.OnWaveStart))]
        [HarmonyPrefix]
        public static void EnemyTeamStageManager_BlackSilence_OnWaveStart(EnemyTeamStageManager_BlackSilence __instance)
        {
            if (ContractLoader.Instance.GetStageList().Exists(x => x.Type == "Roland"))
                __instance.curPhase = EnemyTeamStageManager_BlackSilence.Phase.FOURTH;
        }
        //P1
        [HarmonyPatch(typeof(UnitDataModel),nameof(UnitDataModel.ResetForBlackSilence))]
        [HarmonyPrefix]
        public static bool UnitDataModel_ResetFor_BlackSilence_Pre(UnitDataModel __instance, BookModel ____bookItem)
        {
            if (CCInitializer.IsRoland(__instance) && ____bookItem!=null && ____bookItem.ClassInfo.id == Tools.MakeLorId(17000001))
                return false;
            return true;
        }
        [HarmonyPatch(typeof(UnitDataModel),nameof(UnitDataModel.EquipBook))]
        [HarmonyPrefix]
        public static bool UnitDataModel_EquipBook_Pre(UnitDataModel __instance, BookModel newBook, ref BookModel ____bookItem, bool isEnemySetting = false, bool force = false)
        {
            if (force || newBook == null || newBook.ClassInfo.id != Tools.MakeLorId(17000001) || newBook.ClassInfo.id != Tools.MakeLorId(17000004) || newBook.owner != null || !CCInitializer.IsRoland(__instance))
                return true;
            BookModel bookItem = ____bookItem;
            ____bookItem = newBook;
            newBook.SetOwner(__instance);
            if (!isEnemySetting)
                __instance.ReEquipDeck();
            bookItem?.SetOwner(null);
            return false;
        }
        [HarmonyPatch(typeof(BattleUnitEmotionDetail),nameof(BattleUnitEmotionDetail.ApplyEmotionCard))]
        [HarmonyPrefix]
        public static bool BattleUnitEmotionDetail_ApplyEmotionCard_Pre(BattleUnitModel ____self, EmotionCardXmlInfo card)
        {
            if (____self.passiveDetail.HasPassive<PassiveAbility_1700000>() && card.Sephirah != SephirahType.None)
                return false;
            return true;
        }
        [HarmonyPatch(typeof(BattleUnitModel),nameof(BattleUnitModel.IsTargetable))]
        [HarmonyPostfix]
        public static void BattleUnitModel_IsTargetable_Post(BattleUnitModel __instance, ref bool __result)
        {
            if (__instance.speedDiceResult!=null && __instance.speedDiceCount <= 1 && !__instance.IsTargetable_theLast())
                __result = false;
        }
        [HarmonyPatch(typeof(BattlePlayingCardSlotDetail), nameof(BattlePlayingCardSlotDetail.AddCard))]
        [HarmonyPrefix]
        static bool BattlePlayingCardSlotDetail_AddCard_Pre(BattlePlayingCardSlotDetail __instance, BattleUnitModel target, ref int targetSlot, bool isEnemyAuto = false)
        {
            if (target!=null && !target.IsTargetable_theLast() && targetSlot==target.speedDiceResult.Count-1)
                targetSlot = RandomUtil.Range(0, target.speedDiceResult.Count - 2);
            return true;
        }
    }
}
