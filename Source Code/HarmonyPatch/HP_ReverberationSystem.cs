using System;
using System.Collections.Generic;
using System.Linq;
using ContractReward;
using HarmonyLib;
using LOR_DiceSystem;
using UnityEngine;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    class HP_ReverberationSystem
    {
        //Argalia
        [HarmonyPatch(typeof(BattlePlayingCardDataInUnitModel),nameof(BattlePlayingCardDataInUnitModel.OnActivateResonance))]
        [HarmonyPostfix]
        public static void BattlePlayingCardDataInUnitModel_OnActivateResonance(BattlePlayingCardDataInUnitModel __instance)
        {
            if (__instance != null)
            {
                foreach(PassiveAbilityBase passive in __instance.owner.passiveDetail.PassiveList)
                {
                    if (passive is Resonator)
                        (passive as Resonator).ActiveResonate(__instance);
                }
            }
        }
        //Philip
        [HarmonyPatch(typeof(BattleUnitBuf_Philip_OverHeat),nameof(BattleUnitBuf_Philip_OverHeat.Init))]
        [HarmonyPrefix]
        public static bool BattleUnitBuf_Philip_OverHeat_Init(BattleUnitModel owner, BattleUnitBuf_Philip_OverHeat __instance)
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
        [HarmonyPatch(typeof(DiceCardSelfAbility_greta_trample),nameof(DiceCardSelfAbility_greta_trample.OnSucceedAttack), new Type[] { })]
        [HarmonyPostfix]
        public static void DiceCardSelfAbility_greta_trample_OnSucceedAttack(DiceCardSelfAbility_greta_trample __instance)
        {
            __instance.card.target.bufListDetail.AddBuf(new BattleUnitBuf_Greta_Trampled());
        }
        [HarmonyPatch(typeof(BattleUnitBuf_Greta_Meat_Librarian),nameof(BattleUnitBuf_Greta_Meat_Librarian.OnBreakState))]
        [HarmonyPostfix]
        public static void BattleUnitBuf_Greta_Meat_Librarian_OnBreakState(ref float ___hp, BattleUnitModel ____origin, BattleUnitModel ____owner)
        {
            if (____origin == null)
                return;
            double ratio = ____owner.hp / ____owner.MaxHp;
            double hp = ____origin.hp - (1 - ratio) * ____origin.MaxHp;
            ___hp = Mathf.Max(1f, (float)hp);
        }
        [HarmonyPatch(typeof(BattleUnitBuf_Greta_Meat_Librarian),nameof(BattleUnitBuf_Greta_Meat_Librarian.OnDie))]
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
                ____origin.breakDetail.RecoverBreak(Mathf.RoundToInt((float)____origin.breakDetail.GetDefaultBreakGauge() * 0.75f));
                ____origin.view.EnableView(true);
                ____origin.Extinct(false);
            }
            else
                ____origin.Die();
            return false;
        }
        [HarmonyPatch(typeof(BattleUnitBuf_Greta_Meat),nameof(BattleUnitBuf_Greta_Meat.OnTakeDamageByAttack))]
        [HarmonyPrefix]
        public static bool BattleUnitBuf_Greta_Meat_OnTakeDamageByAttack(BattleDiceBehavior atkDice)
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
        //Bremen
        [HarmonyPatch(typeof(BattleUnitModel),nameof(BattleUnitModel.CheckCardAvailable))]
        [HarmonyPostfix]
        public static void BattleUnitModel_CheckCardAvailable(ref bool __result, BattleUnitModel __instance)
        {
            if (__result && BattleManagerUI.Instance.selectedAllyDice != null)
            {
                int index = BattleManagerUI.Instance.selectedAllyDice._speedDiceIndex;
                if (!__instance.speedDiceResult[index].isControlable)
                    __result = false;
            }
        }
        //Tanya
        [HarmonyPatch(typeof(StageController),nameof(StageController.StartAction))]
        [HarmonyPrefix]
        public static bool StageController_StartAction(BattlePlayingCardDataInUnitModel card)
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
        //Jeaheon
        [HarmonyPatch(typeof(PassiveAbility_1307012),nameof(PassiveAbility_1307012.AddThread))]
        [HarmonyPrefix]
        public static bool PassiveAbility_1307012_AddThread(int round, BattleUnitModel ___owner)
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
        [HarmonyPatch(typeof(DiceBehaviour),nameof(DiceBehaviour.Copy))]
        [HarmonyPostfix]
        public static void DiceBehaviour_Copy(DiceBehaviour __instance, DiceBehaviour __result)
        {
            if (CCInitializer.passive18900002_Makred.Contains(__instance))
                CCInitializer.passive18900002_Makred.Add(__result);
        }
        [HarmonyPatch(typeof(PassiveAbility_1309021),nameof(PassiveAbility_1309021.ChangeSpec))]
        [HarmonyPrefix]
        public static bool PassiveAbility_1309021_ChangeSpec(PassiveAbility_1309021 __instance)
        {
            if (__instance._copyUnit == null)
                return false;
            __instance.owner.UnitData.unitData.customizeData.SetCustomData(true);
            __instance.owner.view.ChangeSkin(__instance._copyUnit.Book.GetCharacterName());
            foreach (PassiveAbilityBase passive in __instance._copyUnit.passiveDetail.PassiveList)
            {
                if (passive is ContingecyContract)
                    continue;
                PassiveAbilityBase instance = Activator.CreateInstance(passive.GetType()) as PassiveAbilityBase;
                instance.Init(__instance.owner);
                if (instance.SpeedDiceNumAdder() == 0 && instance.SpeedDiceBreakedAdder() == 0)
                {
                    instance.rare = passive.rare;
                    __instance.owner.passiveDetail.AddPassive(instance);
                }
            }
            BookModel book = __instance._copyUnit.Book;
            book.GetStartPlayPoint();
            book.GetMaxPlayPoint();
            int speedDiceNum = book.SpeedDiceNum;
            int speedMin = book.SpeedMin;
            int speedMax = book.SpeedMax;
            int hp = book.HP;
            int maxBp = book.Break;
            __instance.owner.Book.SetCharacterName(book.GetCharacterName());
            __instance.owner.Book.SetSpeedDiceMin(speedMin);
            __instance.owner.Book.SetSpeedDiceMax(speedMax);
            __instance.owner.Book.SetHp(hp);
            __instance.owner.Book.SetBp(maxBp);
            __instance.owner.RecoverHP(__instance.owner.MaxHp);
            __instance.owner.RecoverBreakLife(__instance.owner.MaxBreakLife);
            __instance.owner.breakDetail.nextTurnBreak = false;
            __instance.owner.breakDetail.RecoverBreak(__instance.owner.breakDetail.GetDefaultBreakGauge());
            AtkResist resistHp1 = book.GetResistHP(BehaviourDetail.Slash);
            AtkResist resistHp2 = book.GetResistHP(BehaviourDetail.Penetrate);
            AtkResist resistHp3 = book.GetResistHP(BehaviourDetail.Hit);
            AtkResist resistBp1 = book.GetResistBP(BehaviourDetail.Slash);
            AtkResist resistBp2 = book.GetResistBP(BehaviourDetail.Penetrate);
            AtkResist resistBp3 = book.GetResistBP(BehaviourDetail.Hit);
            __instance.owner.Book.SetResistHP(BehaviourDetail.Slash, resistHp1);
            __instance.owner.Book.SetResistHP(BehaviourDetail.Penetrate, resistHp2);
            __instance.owner.Book.SetResistHP(BehaviourDetail.Hit, resistHp3);
            __instance.owner.Book.SetResistBP(BehaviourDetail.Slash, resistBp1);
            __instance.owner.Book.SetResistBP(BehaviourDetail.Penetrate, resistBp2);
            __instance.owner.Book.SetResistBP(BehaviourDetail.Hit, resistBp3);
            __instance.owner.view.charAppearance.ChangeMotion(ActionDetail.Standing);
            BattleUnitModel owner = __instance.owner;
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(owner, owner.faction, owner.hp, owner.breakDetail.breakGauge, owner.bufListDetail.GetBufUIDataList());
            return false;
        }
        //Elena
        [HarmonyPatch(typeof(DiceCardSelfAbility_elenaMinionStrong),nameof(DiceCardSelfAbility_elenaMinionStrong.OnSucceedAttack), new Type[] { })]
        [HarmonyPostfix]
        public static void DiceCardSelfAbility_elenaMinionStrong_OnSucceedAttack(DiceCardSelfAbility_elenaMinionStrong __instance)
        {
            __instance.card.target.bufListDetail.AddBuf(new DiceCardSelfAbility_elenaMinionStrong.BattleUnitBuf_elenaStrongOnce());
        }
        [HarmonyPatch(typeof(BattleUnitModel),nameof(BattleUnitModel.RecoverHP))]
        [HarmonyPrefix]
        public static void BattleUnitModel_RecoverHP(BattleUnitModel __instance, ref int v)
        {
            foreach (PassiveAbilityBase passive in __instance.passiveDetail.PassiveList)
            {
                if (passive is GetRecovery)
                    v += (passive as GetRecovery).GetRecoveryBonus(v);
            }
        }
        [HarmonyPatch(typeof(BattleUnitModel),nameof(BattleUnitModel.CanChangeAttackTarget))]
        [HarmonyPostfix]
        public static void BattleUnitModel_CanChangeAttackTarget(BattleUnitModel __instance, ref bool __result,  int myIndex)
        {
            if (__result)
            {
                if(myIndex>=0 && myIndex<__instance.cardSlotDetail.cardAry.Count && __instance.cardSlotDetail.cardAry[myIndex] != null)
                {
                    if (__instance.cardSlotDetail.cardAry[myIndex].cardAbility !=null && __instance.cardSlotDetail.cardAry[myIndex].cardAbility is DiceCardSelfAbility_DirectAttack)
                        __result = false;
                }
            }
        }
    }
}
