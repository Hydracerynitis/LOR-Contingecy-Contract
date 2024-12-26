using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    class HP_NewTrigger
    {
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnStartBattle))]
        [HarmonyPostfix]
        public static void OnStartBattleTrigger(BattleUnitModel __instance)
        {
            foreach (BattleUnitBuf buf in __instance.bufListDetail.GetActivatedBufList())
            {
                if (buf is StartBattleBuf)
                    (buf as StartBattleBuf).OnStartBattle();
            }
        }
        [HarmonyPatch(typeof(BattlePlayingCardDataInUnitModel), nameof(BattlePlayingCardDataInUnitModel.OnActivateResonance))]
        [HarmonyPostfix]
        public static void ResonatorPassive(BattlePlayingCardDataInUnitModel __instance)
        {
            if (__instance != null)
            {
                foreach (PassiveAbilityBase passive in __instance.owner.passiveDetail.PassiveList)
                {
                    if (passive is Resonator)
                        (passive as Resonator).ActiveResonate(__instance);
                }
            }
        }
        
        [HarmonyPatch(typeof(BattlePlayingCardDataInUnitModel), nameof(BattlePlayingCardDataInUnitModel.OnStandbyBehaviour))]
        [HarmonyPostfix]
        static void OnStandByAbility(BattlePlayingCardDataInUnitModel __instance, List<BattleDiceBehavior> __result)
        {
            if (__instance.card != null && __instance.card._script != null && __instance.card._script is OnStandBy)
                (__instance.card._script as OnStandBy).OnStandBy(__instance, __instance.owner, __result);
        }
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnRecoverHp))]
        [HarmonyPostfix]
        static void OnRecoverHpTrigger(BattleUnitModel __instance, int recoverAmount)
        {
            foreach (BattleUnitBuf buf in __instance.bufListDetail.GetActivatedBufList())
            {
                if (buf is RecoverHpBuf)
                    (buf as RecoverHpBuf).OnRecoverHp(recoverAmount);
            }
        }
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.RecoverHP))]
        [HarmonyPrefix]
        public static void RecoveryBonusPassive(BattleUnitModel __instance, ref int v)
        {
            foreach (PassiveAbilityBase passive in __instance.passiveDetail.PassiveList)
            {
                if (passive is GetRecovery)
                    v += (passive as GetRecovery).GetRecoveryBonus(v);
            }
        }
        [HarmonyPatch(typeof(BattleUnitEmotionDetail), nameof(BattleUnitEmotionDetail.DrawCardAdder))]
        [HarmonyPostfix]
        public static void CardDrawAdderPassive(BattleUnitEmotionDetail __instance, ref int __result, int previousUsedCount)
        {
            foreach (PassiveAbilityBase passive in __instance._self.passiveDetail.PassiveList)
            {
                if (passive is CardDrawer)
                    __result = __result + (passive as CardDrawer).getDrawCardAdder(previousUsedCount);
            }
        }
    }
}
