﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    class HP_SetCard
    {
        [HarmonyPatch(typeof(PassiveAbility_240008),nameof(PassiveAbility_240008.OnRoundStart))]
        [HarmonyPostfix]
        public static void PassiveAbility_240008_OnRoundStart(PassiveAbility_240008 __instance)
        {
            int num = __instance.owner.emotionDetail.SpeedDiceNumAdder() + __instance.owner.emotionDetail.GetSpeedDiceAdder(0);
            if (num <= 0)
                return;
            for (int index = 0; index < num; ++index)
                __instance.Owner.allyCardDetail.AddNewCard(503002).SetCostToZero();
        }
        [HarmonyPatch(typeof(PassiveAbility_1305012),nameof(PassiveAbility_1305012.SetCard))]
        [HarmonyPostfix]
        public static void PassiveAbility_1305012_SetCard(PassiveAbility_1305012 __instance)
        {
            if (__instance.Owner.passiveDetail.HasPassive<ContingecyContract_Oswald_Troll>() && __instance.cardPhase == PassiveAbility_1305012.CardPhase.DO_DAZE)
            {
                __instance.Owner.allyCardDetail.ExhaustAllCards();
                __instance.SetCardPatterNormal();
                int num = __instance.Owner.emotionDetail.SpeedDiceNumAdder() + __instance.Owner.emotionDetail.GetSpeedDiceAdder(0);
                if (num <= 0)
                    return;
                for (int index = 0; index < num; ++index)
                    __instance.Owner.allyCardDetail.AddNewCard(703501).SetPriorityAdder(0);
                __instance.cardPhase = PassiveAbility_1305012.CardPhase.DO_DAZE;
                __instance.Owner.bufListDetail.AddBuf(new ContingecyContract_Oswald_Troll.TrollIndicator());
            }
        }
        [HarmonyPatch(typeof(PassiveAbility_1400001),nameof(PassiveAbility_1400001.OnRoundStart))]
        [HarmonyPostfix]
        public static void PassiveAbility_1400001_OnRoundStart_Post(PassiveAbility_1400001 __instance)
        {
            if(__instance.owner.UnitData.unitData.EnemyUnitId== 1401011 && !__instance.owner.IsBreakLifeZero())
            {
                int num = __instance.owner.emotionDetail.SpeedDiceNumAdder() + __instance.owner.emotionDetail.GetSpeedDiceAdder(0);
                if (num <= 0)
                    return;
                for (int index = 0; index < num; ++index)
                    __instance.Owner.allyCardDetail.AddNewCard(RandomUtil.SelectOne(__instance.Philip_CardIds)).SetCostToZero();
            }
        }
        [HarmonyPatch(typeof(PassiveAbility_170311),nameof(PassiveAbility_170311.SetCards))]
        [HarmonyPostfix]
        public static void PassiveAbility_170311_SetCards(PassiveAbility_170311 __instance)
        {
            if (__instance.owner.emotionDetail.EmotionLevel >= 4)
                __instance.AddNewCard(702315);
        }
    }
}
