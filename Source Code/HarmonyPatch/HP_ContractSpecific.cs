using ContractReward;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UI;
using System.Reflection;
using LOR_DiceSystem;
using static System.Reflection.Emit.OpCodes;
using System.Reflection.Emit;
using UnityEngine;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    static class HP_ContractSpecific
    {
        //Enemy
        [HarmonyPatch(typeof(BookModel), nameof(BookModel.GetSpeedDiceRule))]
        [HarmonyPostfix]
        public static void Contract_SpeedDice(BattleUnitModel unit, ref SpeedDiceRule __result)
        {
            if (ContractLoader.Instance.GetPassiveList().Find(x => x.Type.StartsWith("SpeedDice")) is Contract contract && unit.faction==Faction.Enemy)
            {
                int breakNum = unit.passiveDetail.SpeedDiceBreakAdder() + unit.bufListDetail.SpeedDiceBreakAdder();
                int num = 2;
                if (contract.Variant >= 3 || (contract.Variant >= 2 && unit.emotionDetail.EmotionLevel >= 3))
                    num = 3;
                __result = new SpeedDiceRule(__result.diceMin, __result.diceFaces, Math.Max(num, __result.diceNum), breakNum);
            }
        }
        [HarmonyPatch(typeof(StageLibraryFloorModel),nameof(StageLibraryFloorModel.CreateSelectableList))]
        [HarmonyPostfix]
        public static void ClearEmotionForNoEmotionContract(List<EmotionCardXmlInfo> __result, int emotionLevel)
        {
            if (Singleton<ContractLoader>.Instance.GetPassiveList().Find(x => x.Type == "NoEmotion") is Contract NoEmotion)
            {
                int libraryLevelThreshold = 6;
                if(NoEmotion.Variant==3)
                    libraryLevelThreshold = 1;
                else if(NoEmotion.Variant==2)
                    libraryLevelThreshold = 3;
                else if (NoEmotion.Variant == 1)
                    libraryLevelThreshold = 5;
                if(emotionLevel >= libraryLevelThreshold)
                    __result.Clear();
            }
                
        }
        [HarmonyPatch(typeof(StageLibraryFloorModel), nameof(StageLibraryFloorModel.CreateSelectableEgoList))]
        [HarmonyPostfix]
        public static void ClearEgoForNoEgoContract(List<EmotionEgoXmlInfo> __result)
        {
            if (Singleton<ContractLoader>.Instance.GetPassiveList().Find(x => x.Type == "NoEGO") is Contract NoEGO)
                __result.Clear();
        }
        
        //Philip
        [HarmonyPatch(typeof(BattleUnitBuf_Philip_OverHeat), nameof(BattleUnitBuf_Philip_OverHeat.Init))]
        [HarmonyPrefix]
        public static bool Contract_Philip_Burn(BattleUnitModel owner, BattleUnitBuf_Philip_OverHeat __instance)
        {
            if (owner.passiveDetail.PassiveList.Find(x => x is ContingecyContract_Philip_Burn) is ContingecyContract_Philip_Burn burn)
            {
                owner.bufListDetail.AddBuf(new ContingecyContract_Philip_Burn.OverHeat_cc(burn.Level));
                __instance.Destroy();
                return false;
            }
            return true;
        }
        //Greta
        [HarmonyPatch(typeof(BattleUnitBuf_Greta_Meat_Librarian), nameof(BattleUnitBuf_Greta_Meat_Librarian.OnBreakState))]
        [HarmonyPostfix]
        public static void KeepPercentageHP(ref float ___hp, BattleUnitModel ____origin, BattleUnitModel ____owner)
        {
            if (____origin == null)
                return;
            double ratio = ____owner.hp / ____owner.MaxHp;
            double hp = ____origin.hp - (1 - ratio) * ____origin.MaxHp;
            ___hp = Mathf.Max(1f, (float)hp);
        }
        [HarmonyPatch(typeof(BattleUnitBuf_Greta_Meat_Librarian), nameof(BattleUnitBuf_Greta_Meat_Librarian.OnDie))]
        [HarmonyPrefix]
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
                ____origin.breakDetail.RecoverBreak(Mathf.RoundToInt((float)____origin.breakDetail.GetDefaultBreakGauge() * 0.5f));
                ____origin.view.EnableView(true);
                ____origin.Extinct(false);
            }
            else
                ____origin.Die();
            return false;
        }
        [HarmonyPatch(typeof(BattleUnitBuf_Greta_Meat), nameof(BattleUnitBuf_Greta_Meat.OnTakeDamageByAttack))]
        [HarmonyPrefix]
        public static bool Contract_Greta_Feast(BattleDiceBehavior atkDice)
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
        //Jeaheon
        [HarmonyPatch(typeof(PassiveAbility_1307012), nameof(PassiveAbility_1307012.AddThread))]
        [HarmonyPrefix]
        public static bool StackThreadBuf(int round, BattleUnitModel ___owner)
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
            if (owner.bufListDetail.GetActivatedBuf(KeywordBuf.JaeheonPuppetThread) is BattleUnitBuf_Jaeheon_PuppetThread thread)
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
        //Pluto
        [HarmonyPatch(typeof(PassiveAbility_1309021), nameof(PassiveAbility_1309021.ChangeSpec))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> FilterContractPassive(IEnumerable<CodeInstruction> instructions,ILGenerator IL)
        {
            List<CodeInstruction> codes = instructions.ToList();
            Label endofLoop = IL.DefineLabel();
            int endofLoopIndex = codes.FindIndex(c => c.opcode == Call && (c.operand as MethodInfo)?.Name == "MoveNext") - 1;
            codes[endofLoopIndex].labels.Add(endofLoop);
            int startIndex = codes.FindIndex(c => c.opcode == Call && (c.operand as MethodInfo)?.Name == "CreateInstance")-2;
            if (startIndex >= 0)
            {
                codes.InsertRange(startIndex + 1, new CodeInstruction[]
                {
                    new CodeInstruction(Isinst,typeof(ContingecyContract)),
                    new CodeInstruction(Brtrue,endofLoop),
                    codes[startIndex]
                });
            }
            return codes;
        }
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.CanChangeAttackTarget))]
        [HarmonyPostfix]
        public static void PreventClashwithAlly(BattleUnitModel __instance, ref bool __result, int myIndex)
        {
            if (__result)
            {
                if (myIndex >= 0 && myIndex < __instance.cardSlotDetail.cardAry.Count && __instance.cardSlotDetail.cardAry[myIndex] != null)
                {
                    if (__instance.cardSlotDetail.cardAry[myIndex].cardAbility != null && __instance.cardSlotDetail.cardAry[myIndex].cardAbility is DiceCardSelfAbility_DirectAttack)
                        __result = false;
                }
            }
        }
        //Roland
        [HarmonyPatch(typeof(EnemyTeamStageManager_BlackSilence), nameof(EnemyTeamStageManager_BlackSilence.OnWaveStart))]
        [HarmonyPostfix]
        public static void Contract_Roland(EnemyTeamStageManager_BlackSilence __instance)
        {
            if (ContractLoader.Instance.GetStageList().Exists(x => x.Type == "Roland") && __instance.curPhase!= EnemyTeamStageManager_BlackSilence.Phase.FOURTH)
            {
                __instance.curPhase = EnemyTeamStageManager_BlackSilence.Phase.FOURTH;
                __instance.SetFourthPhase_restart();
                __instance.SetBgm();
            }
                
        }
        //DOswald
        [HarmonyPatch(typeof(PassiveAbility_1405041), nameof(PassiveAbility_1405041.OnRoundStart))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> PassiveAbility_1405041_OnRoundStart(IEnumerable<CodeInstruction> instructions, ILGenerator IL)
        {
            List<CodeInstruction> codes = instructions.ToList();
            Label endofLoop = IL.DefineLabel();
            int endofLoopIndex = codes.FindIndex(c => c.opcode == Call && (c.operand as MethodInfo)?.Name == "MoveNext") - 1;
            codes[endofLoopIndex].labels.Add(endofLoop);
            int startIndex = codes.FindIndex(c => c.opcode == Beq)+1;
            if (startIndex >= 0)
            {
                codes.InsertRange(startIndex + 1, new CodeInstruction[]
                {
                    new CodeInstruction(Isinst,typeof(ContingecyContract)),
                    new CodeInstruction(Brtrue,endofLoop),
                    codes[startIndex],
                });
            }
            return codes;
        }
        [HarmonyPatch(typeof(EnemyTeamStageManager_TwistedReverberationBand_Middle), nameof(EnemyTeamStageManager_TwistedReverberationBand_Middle.AddOswald))]
        [HarmonyPostfix]
        static void Contract_DOswald_Friend(EnemyTeamStageManager_TwistedReverberationBand_Middle __instance)
        {
            __instance.Map?.SetFilterOff();
            List<BattleUnitModel> enemy = BattleObjectManager.instance.GetList(Faction.Enemy);
            if(enemy.Count>=5 && !enemy.Exists(x => x.UnitData.unitData.EnemyUnitId== 1405011))
            {
                for (int index = 0; index < 5; ++index)
                {
                    if (BattleObjectManager.instance.GetUnitWithIndex(Faction.Enemy, index) == null || BattleObjectManager.instance.GetUnitWithIndex(Faction.Enemy, index).IsDead())
                    {
                        BattleUnitModel battleUnitModel1 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 1405011, index);
                        if (battleUnitModel1 == null)
                            break;
                        battleUnitModel1.SetDeadSceneBlock(false);
                        int num = 0;
                        battleUnitModel1.SetHp(__instance._oswaldHp);
                        battleUnitModel1.breakDetail.breakGauge = __instance._oswaldBp;
                        if (__instance._oswaldBp <= 0)
                        {
                            battleUnitModel1.breakDetail.breakGauge = 0;
                            battleUnitModel1.breakDetail.breakLife = 0;
                            battleUnitModel1.breakDetail.DestroyBreakPoint();
                            battleUnitModel1.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
                        }
                        foreach (BattleUnitModel battleUnitModel2 in (IEnumerable<BattleUnitModel>)BattleObjectManager.instance.GetList())
                            SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnitModel2.UnitData.unitData, num++);
                        BattleObjectManager.instance.InitUI();
                        __instance.isOswaldHide = false;
                        break;
                    }
                }
            }
            BattleUnitModel newOswald = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(x => x.UnitData.unitData.EnemyUnitId == 1405011);
            if(newOswald != null)
            {
                if (!ContractAttribution.Inition.Contains(newOswald))
                    ContractAttribution.Init(newOswald);
            }
        }
        //DTanya
        [HarmonyPatch(typeof(PassiveAbility_1406011), nameof(PassiveAbility_1406011.RoundStartDmg), MethodType.Getter)]
        [HarmonyPostfix]
        static void Contract_DTanya_Sand(ref int __result)
        {
            if (ContractLoader.Instance.GetPassiveList().Exists(x => x.Type == "DTanya_Sand"))
                __result = RandomUtil.Range(6, 10);
        }
        //DPluto
        [HarmonyPatch(typeof(PassiveAbility_1409022), nameof(PassiveAbility_1409022.CopyUnit))]
        [HarmonyPostfix]
        static void Contract_DPluto_Shade(PassiveAbility_1409022 __instance)
        {
            if (ContractLoader.Instance.GetPassiveList().Exists(x => x.Type == "DPluto_Shade"))
            {
                foreach (PassiveAbilityBase passive in __instance._target.passiveDetail.PassiveList)
                {
                    if (passive is ContingecyContract)
                        continue;
                    try
                    {
                        PassiveAbilityBase new_passive = Activator.CreateInstance(passive.GetType()) as PassiveAbilityBase;
                        if (new_passive.SpeedDiceNumAdder() == 0 && new_passive.SpeedDiceBreakedAdder() == 0)
                        {
                            new_passive.desc = passive.desc;
                            new_passive.name = passive.name;
                            new_passive.rare = passive.rare;
                            new_passive.Init(__instance.owner);
                            __instance.owner.passiveDetail.PassiveList.Add(new_passive);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }
    }
}
