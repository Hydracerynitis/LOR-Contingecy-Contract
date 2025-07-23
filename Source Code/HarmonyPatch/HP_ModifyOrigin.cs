using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using BaseMod;
using ContractReward;
using HarmonyLib;
using UI;
using UnityEngine;
using static System.Reflection.Emit.OpCodes;
using static HarmonyLib.AccessTools;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    class HP_ModifyOrigin
    {
        [HarmonyPatch(typeof(AssemblyManager), nameof(AssemblyManager.CreateInstance_DiceCardSelfAbility))]
        [HarmonyPostfix]
        public static void ReplaceDiceCardSelfAbility(ref DiceCardSelfAbilityBase __result)
        {
            if (ContractLoader.Instance.ExistContract("Roland3rd_Unity") && __result is DiceCardSelfAbility_atkcombo_allas)
                __result = new Contingecy_Contract.DiceCardSelfAbility_atkcombo_allas_New();
            else if (ContractLoader.Instance.ExistContract("Roland3rd_Unity") && __result is DiceCardSelfAbility_atkcombo_logic)
                __result = new Contingecy_Contract.DiceCardSelfAbility_atkcombo_logic_New();
            else if (ContractLoader.Instance.ExistContract("Roland3rd_Unity") && __result is DiceCardSelfAbility_atkcombo_zelkova)
                __result = new Contingecy_Contract.DiceCardSelfAbility_atkcombo_zelkova_New();
        }
        [HarmonyPatch(typeof(BookModel), nameof(BookModel.CreatePassiveList))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> BookModel_CreatePassiveList(IEnumerable<CodeInstruction> instructions, ILGenerator IL)
        {
            List<CodeInstruction> codes = instructions.ToList();
            int index = codes.FindIndex(c => c.opcode == Callvirt && (c.operand as MethodInfo)?.Name == "CreateInstance_PassiveAbility");
            if (index >= 0)
            {
                codes.InsertRange(index + 1, new CodeInstruction[]
                {
                    new CodeInstruction(Ldarg_0),
                    new CodeInstruction(Call, Method(typeof(HP_ModifyOrigin), nameof(ChangePassive)))
                });
            }
            return codes;
        }
        public static PassiveAbilityBase ChangePassive(PassiveAbilityBase passive, BookModel unitBook)
        {
            UnitDataModel unit = unitBook.owner;
            if (passive is PassiveAbility_1302013)
                return new PassiveAbility_1302013_New();
            else if (passive is PassiveAbility_1303012)
                return new CC_Greta_Phase1();
            else if (passive is PassiveAbility_1303013)
                return new CC_Greta_Phase2();
            else if (ContractLoader.Instance.ExistContract("Shi") && (passive is PassiveAbility_241001 || passive is PassiveAbility_241301) && IsEnemy(unit))
                return new ContingecyContract_Shi.Enhanced_passive_241301();
            else if (ContractLoader.Instance.ExistContract("Hana") && passive is PassiveAbility_260001 && IsEnemy(unit))
                return new ContingecyContract_Hana.New_Passive_260001();
            else if (ContractLoader.Instance.FindContract("Hana_Web", out Contract contract))
            {
                if (passive is PassiveAbility_260002)
                    return new ContingecyContract_Hana_Web.New_Passive_260002();
                if (passive is PassiveAbility_260003 && unit.EnemyUnitId == 60003)
                {
                    ContingecyContract_Hana_Web.New_Passive_260003 new_passive = new ContingecyContract_Hana_Web.New_Passive_260003();
                    new_passive.Initialise(contract.Variant);
                    return new_passive;
                }
            }
            else if (ContractLoader.Instance.ExistContract("Roland1st") && passive is PassiveAbility_170003 && !(passive is PassiveAbility_1700013))
                return new ContingecyContract_Roland1st.Enhanced_Passive_170003();
            else if (ContractLoader.Instance.ExistContract("Roland4th_BlackSilence") && passive is PassiveAbility_170301)
                return new ContingecyContract_Roland4th_BlackSilence.PassiveAbility_170301_New();
            else if (ContractLoader.Instance.ExistContract("DBremen_Self") && passive is PassiveAbility_1404013)
                return new ContingecyContract_DBremen_Self.PassiveAbility_1404013_New();
            else if (passive is PassiveAbility_1405011)
                return new PassiveAbility_1405011_New();
            else if (ContractLoader.Instance.ExistContract("DArgalia_Sonata") && passive is PassiveAbility_1410013)
                return new ContingecyContract_DArgalia_Sonata.PassiveAbility_1410013_New();
            return passive;
        }
        public static bool IsEnemy(UnitDataModel unit)
        {
            return unit.EnemyUnitId != LorId.None;
        }
        [HarmonyPatch(typeof(DiceCardSelfAbility_Jaeheon_AreaDt),nameof(DiceCardSelfAbility_Jaeheon_AreaDt.OnUseCard))]
        [HarmonyPostfix]
        public static void AddThreadToPuppet(DiceCardSelfAbility_Jaeheon_AreaDt __instance)
        {
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(__instance.owner.faction))
            {
                LorId enemyUnitId = alive.UnitData.unitData.EnemyUnitId;
                if (enemyUnitId == 1307021 || enemyUnitId == 1307031 || enemyUnitId == 1307041 || enemyUnitId == 1307051)
                {
                    BattleUnitBuf_Jaeheon_PuppetThread buf = new BattleUnitBuf_Jaeheon_PuppetThread();
                    buf.Init(alive);
                    buf.stack = 1;
                    alive.bufListDetail.AddReadyBuf(buf);
                }
            }
        }
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
        [HarmonyPatch(typeof(BehaviourAction_HanaSpecial), nameof(BehaviourAction_HanaSpecial.GetMovingAction))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> AddNewBookForHanaSpecial(IEnumerable<CodeInstruction> instructions, ILGenerator IL)
        {
            List<CodeInstruction> codes = instructions.ToList();
            int index = codes.FindIndex(c => c.opcode == Stfld);
            if (index >= 0)
            {
                int brIndex = codes.GetRange(index, codes.Count-index).FindIndex(c => c.opcode == Brtrue);
                CodeInstruction brtrue5= codes[index+brIndex];
                FieldInfo BookId = codes[index].operand as FieldInfo;
                codes.InsertRange(index + brIndex+1, new CodeInstruction[]
                {
                    new CodeInstruction(Ldloc_1),
                    new CodeInstruction(Ldfld,BookId),
                    new CodeInstruction(Call, Method(typeof(HP_ModifyOrigin), nameof(CheckHanaContractReward))),
                    brtrue5
                });
            }
            return codes;
        }
        public static bool CheckHanaContractReward(LorId BookId)
        {
            return BookId == Tools.MakeLorId(17100000) || BookId == Tools.MakeLorId(17100001) || BookId == Tools.MakeLorId(17100002);
        }
        [HarmonyPatch(typeof(BehaviourAction_TwistedTanyaSpecialAtk), nameof(BehaviourAction_TwistedTanyaSpecialAtk.GetMovingAction))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> AddNewBookForTwistedTanyaSpecial(IEnumerable<CodeInstruction> instructions, ILGenerator IL)
        {
            List<CodeInstruction> codes = instructions.ToList();
            int index = codes.FindIndex(c => c.operand is 1406011);
            if (index != 0)
            {
                int brIndex= codes.GetRange(index, codes.Count - index).FindIndex(c => c.opcode == Brtrue);
                CodeInstruction brtrue6 = codes[index + brIndex];
                int secondCheckIndex= codes.GetRange(index+1, codes.Count - index-1).FindIndex(c => c.operand is 1406011);
                codes.InsertRange(index+secondCheckIndex + 3, new CodeInstruction[]
                {
                    brtrue6,
                    new CodeInstruction(Ldloc_1),
                    new CodeInstruction(Call, Method(typeof(HP_ModifyOrigin), nameof(CheckTwistedTanyaContractReward)))
                });
            }
            return codes;
        }
        [HarmonyPatch(typeof(BehaviourAction_TwistedTanyaSuperAtk1),nameof(BehaviourAction_TwistedTanyaSuperAtk1.GetMovingAction))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> AddNewBookForTwistedTanyaSuper1(IEnumerable<CodeInstruction> instructions, ILGenerator IL)
        {
            List<CodeInstruction> codes = instructions.ToList();
            int index = codes.FindIndex(c => c.operand is 1406011);
            if (index != 0)
            {
                int brIndex = codes.GetRange(index, codes.Count - index).FindIndex(c => c.opcode == Brtrue);
                CodeInstruction brtrue6 = codes[index + brIndex];
                int secondCheckIndex = codes.GetRange(index + 1, codes.Count - index - 1).FindIndex(c => c.operand is 1406011);
                codes.InsertRange(index + secondCheckIndex + 3, new CodeInstruction[]
                {
                    brtrue6,
                    new CodeInstruction(Ldloc_1),
                    new CodeInstruction(Call, Method(typeof(HP_ModifyOrigin), nameof(CheckTwistedTanyaContractReward)))
                });
            }
            return codes;
        }
        [HarmonyPatch(typeof(BehaviourAction_TwistedTanyaSuperAtk2), nameof(BehaviourAction_TwistedTanyaSuperAtk2.GetMovingAction))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> AddNewBookForTwistedTanyaSuper2(IEnumerable<CodeInstruction> instructions, ILGenerator IL)
        {
            List<CodeInstruction> codes = instructions.ToList();
            int index = codes.FindIndex(c => c.operand is 1406011);
            if (index != 0)
            {
                int brIndex = codes.GetRange(index, codes.Count - index).FindIndex(c => c.opcode == Brtrue);
                CodeInstruction brtrue6 = codes[index + brIndex];
                int secondCheckIndex = codes.GetRange(index + 1, codes.Count - index - 1).FindIndex(c => c.operand is 1406011);
                codes.InsertRange(index + secondCheckIndex + 3, new CodeInstruction[]
                {
                    brtrue6,
                    new CodeInstruction(Ldloc_1),
                    new CodeInstruction(Call, Method(typeof(HP_ModifyOrigin), nameof(CheckTwistedTanyaContractReward)))
                });
            }
            return codes;
        }
        [HarmonyPatch(typeof(BehaviourAction_TanyaSpecialAtk),nameof(BehaviourAction_TanyaSpecialAtk.GetMovingAction))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> AddNewBookForTanyaSpecial(IEnumerable<CodeInstruction> instructions, ILGenerator IL)
        {
            List<CodeInstruction> codes = instructions.ToList();
            int index = codes.FindIndex(c => c.operand is 260011);
            if (index != 0)
            {
                int brIndex = codes.GetRange(index, codes.Count - index).FindIndex(c => c.opcode == Brtrue);
                CodeInstruction brtrue4 = codes[index + brIndex];
                int secondCheckIndex = codes.GetRange(index + 1, codes.Count - index - 1).FindIndex(c => c.operand is 260011);
                codes.InsertRange(index + secondCheckIndex + 3, new CodeInstruction[]
                {
                    brtrue4,
                    new CodeInstruction(Ldloc_1),
                    new CodeInstruction(Call, Method(typeof(HP_ModifyOrigin), nameof(CheckTanyaContractReward)))
                });
            }
            return codes;
        }
        public static bool CheckTwistedTanyaContractReward(BattleUnitModel model)
        {
            return model.Book.GetBookClassInfoId() == Tools.MakeLorId(19600000) || model.customBook.ClassInfo.id == Tools.MakeLorId(19600000);
        }
        public static bool CheckTanyaContractReward(BattleUnitModel model)
        {
            return model.Book.GetBookClassInfoId() == Tools.MakeLorId(18600000) || model.customBook.ClassInfo.id == Tools.MakeLorId(18600000);
        }
    }
}
