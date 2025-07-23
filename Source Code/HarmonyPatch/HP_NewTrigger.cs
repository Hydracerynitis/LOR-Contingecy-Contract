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
        [HarmonyPatch(typeof(BattleAllyCardDetail),nameof(BattleAllyCardDetail.AddCardToHand))]
        [HarmonyPostfix]
        public static void OnAddToHandTrigger(BattleAllyCardDetail __instance, BattleDiceCardModel card)
        {
            foreach(BattleDiceCardBuf cardBuf in card._bufList)
            {
                if (cardBuf is OnAddToHandBuf)
                    (cardBuf as OnAddToHandBuf).OnAddToHand(__instance._self);
            }
        }
        [HarmonyPatch(typeof(BattleUnitModel),nameof(BattleUnitModel.GetBreakDamageReductionAll))]
        [HarmonyPostfix]
        public static void StaggerDamageReductionAllTrigger(BattleUnitModel __instance, ref int __result, int dmg, DamageType dmgType, BattleUnitModel attacker)
        {
            foreach(BattleUnitBuf buf in __instance.bufListDetail._bufList)
            {
                if (buf is StaggerDamageReductionAllBuf)
                    __result += (buf as StaggerDamageReductionAllBuf).GetBreakDamageReductionAll(dmg, dmgType, attacker);
            }
        }
        [HarmonyPatch(typeof(BattleDiceCardModel),nameof(BattleDiceCardModel.OnUseOtherCard_inHand))]
        [HarmonyPostfix]
        public static void OnUseOtherCardTrigger(BattleDiceCardModel __instance, BattleUnitModel unit, BattlePlayingCardDataInUnitModel curCard)
        {
            foreach(BattleDiceCardBuf cardBuf in __instance._bufList)
            {
                if (cardBuf is OnUseOtherCardInHand)
                    (cardBuf as OnUseOtherCardInHand).OnUseOtherCardInHand(unit, curCard);
            }
        }
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnStartBattle))]
        [HarmonyPostfix]
        public static void OnStartBattleTrigger(BattleUnitModel __instance)
        {
            List<StartBattleBuf> new_triggers= new List<StartBattleBuf>();
            List<StartBattleInHandBuf> new_triggers_inHand= new List<StartBattleInHandBuf>();
            foreach (BattleUnitBuf buf in __instance.bufListDetail.GetActivatedBufList())
            {
                if (buf is StartBattleBuf)
                    new_triggers.Add((StartBattleBuf)buf);

            }
            foreach(BattlePlayingCardDataInUnitModel pages in __instance.cardSlotDetail.cardAry)
            {
                if(pages != null && pages.card!=null)
                {
                    foreach(BattleDiceCardBuf cardBuf in pages.card._bufList)
                    {
                        if (cardBuf is StartBattleBuf)
                            new_triggers.Add((StartBattleBuf)cardBuf);
                    }
                }
            }
            foreach (BattleDiceCardModel cards in __instance.allyCardDetail.GetHand())
            {
                if (cards != null)
                {
                    foreach (BattleDiceCardBuf cardBuf in cards._bufList)
                    {
                        if (cardBuf is StartBattleInHandBuf)
                            new_triggers_inHand.Add((StartBattleInHandBuf)cardBuf);
                    }
                }
            }
            try
            {
                new_triggers.ForEach(x => x.OnStartBattle(__instance));
                new_triggers_inHand.ForEach(x => x.OnStartBattle_inHand(__instance));
            }
            catch(Exception ex)
            {
                Debug.Error("OnStartBattle", ex);
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
