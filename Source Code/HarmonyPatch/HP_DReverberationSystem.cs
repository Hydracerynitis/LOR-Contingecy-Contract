using System;
using System.Collections.Generic;
using System.Linq;
using ContractReward;
using HarmonyLib;
using LOR_DiceSystem;
using UI;
using UnityEngine;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    class HP_DReverberationSystem
    {
        //Oswald
        [HarmonyPatch(typeof(PassiveAbility_1405041),nameof(PassiveAbility_1405041.OnRoundStart))]
        [HarmonyPrefix]
        static bool PassiveAbility_1405041_OnRoundStart_Pre(PassiveAbility_1405041 __instance)
        {
            if (__instance._init)
                return false;
            __instance._init = true;
            if (__instance.Stage != null)
                __instance._linkedUnits.AddRange(__instance.Stage.GetLinkedUnits());
            foreach (BattleUnitModel linkedUnit in __instance._linkedUnits)
            {
                if (linkedUnit.faction == Faction.Player)
                {
                    foreach (PassiveAbilityBase passive in linkedUnit.passiveDetail.PassiveList)
                    {
                        if (passive.InnerTypeId != 1)
                        {
                            try
                            {
                                PassiveAbilityBase instance = Activator.CreateInstance(passive.GetType()) as PassiveAbilityBase;
                                instance.rare = passive.rare;
                                instance.name = passive.name;
                                instance.desc = passive.desc;
                                if (!__instance.owner.passiveDetail.PassiveList.Exists(x => x.GetType()==instance.GetType()))
                                    __instance.owner.passiveDetail.AddPassive(instance);
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            __instance.owner.RecoverHP(__instance._linkedUnits.Count * 20);
            return false;
        }
        [HarmonyPatch(typeof(EnemyTeamStageManager_TwistedReverberationBand_Middle),nameof(EnemyTeamStageManager_TwistedReverberationBand_Middle.AddOswald))]
        [HarmonyPrefix]
        static bool EnemyTeamStageManager_TwistedReverberationBand_Middle_AddOswald_Pre(EnemyTeamStageManager_TwistedReverberationBand_Middle __instance)
        {
            __instance.Map?.SetFilterOff();
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
            return false;
        }
        //Tanya
        [HarmonyPatch(typeof(PassiveAbility_1406011),nameof(PassiveAbility_1406011.RoundStartDmg),MethodType.Getter)]
        [HarmonyPostfix]
        static void PassiveAbility_1406011_get_RoundStartDmg_Post(ref int __result)
        {
            if (ContractLoader.Instance.GetPassiveList().Exists(x => x.Type == "DTanya_Sand"))
                __result = RandomUtil.Range(6, 10);
        }
        //Pluto
        [HarmonyPatch(typeof(PassiveAbility_1409022),nameof(PassiveAbility_1409022.CopyUnit))]
        [HarmonyPostfix]
        static void PassiveAbility_1409022_CopyUnit_Post(PassiveAbility_1409022 __instance)
        {
            if(ContractLoader.Instance.GetPassiveList().Exists(x => x.Type== "DPluto_Shade"))
            {
                foreach (PassiveAbilityBase passive in __instance._target.passiveDetail.PassiveList)
                {
                    if (passive is ContingecyContract)
                        continue;
                    try
                    {
                        PassiveAbilityBase instance = Activator.CreateInstance(passive.GetType()) as PassiveAbilityBase;
                        if(instance.SpeedDiceNumAdder() == 0 && instance.SpeedDiceBreakedAdder() == 0)
                        {
                            instance.desc = passive.desc;
                            instance.name = passive.name;
                            instance.rare = passive.rare;
                            __instance.owner.passiveDetail.AddPassive(instance);
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
