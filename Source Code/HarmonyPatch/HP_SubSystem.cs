using System;
using System.Collections.Generic;
using System.Linq;
using BaseMod;
using ContractReward;
using HarmonyLib;
using UI;
using UnityEngine;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    class HP_SubSystem
    {
        [HarmonyPatch(typeof(BookModel),nameof(BookModel.GetThumbSprite))]
        [HarmonyPostfix]
        public static void BookModel_GetThumbSprite(ref Sprite __result, BookModel __instance)
        {
            if (StaticDataManager.NonThumbSprite.ContainsKey(__instance.GetBookClassInfoId()))
            {
                if (StaticDataManager.NonThumbSprite[__instance.GetBookClassInfoId()] != null)
                    __result = StaticDataManager.NonThumbSprite[__instance.GetBookClassInfoId()];
                else
                {
                    try
                    {
                        GameObject prefab = Singleton<AssetBundleManagerRemake>.Instance.LoadCharacterPrefab(__instance.GetOriginalCharcterName(), "_N", out string resourcename);
                        if (prefab == null)
                            return;
                        CharacterAppearance character = prefab.GetComponent<CharacterAppearance>();
                        CharacterMotion Default = character.GetCharacterMotion(ActionDetail.Default);
                        if (Default == null)
                            return;
                        SpriteSet Body = Default.motionSpriteSet.Find(x => x.sprType == CharacterAppearanceType.Body);
                        if (Body != null)
                        {
                            StaticDataManager.NonThumbSprite[__instance.GetBookClassInfoId()] = Default.motionSpriteSet.Find(x => x.sprType == CharacterAppearanceType.Body).sprRenderer.sprite;
                            __result = StaticDataManager.NonThumbSprite[__instance.GetBookClassInfoId()];
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Error("PrefabThumbe", ex);
                    }
                }
            }

        }
        [HarmonyPatch(typeof(BookXmlInfo),nameof(BookXmlInfo.GetThumbSprite))]
        [HarmonyPostfix]
        public static void BookXmlInfo_GetThumbSprite(ref Sprite __result, BookXmlInfo __instance)
        {
            if (StaticDataManager.NonThumbSprite.ContainsKey(__instance.id))
            {
                if (StaticDataManager.NonThumbSprite[__instance.id] != null)
                    __result = StaticDataManager.NonThumbSprite[__instance.id];
                else
                {
                    try
                    {
                        GameObject prefab = Singleton<AssetBundleManagerRemake>.Instance.LoadCharacterPrefab(__instance.GetCharacterSkin(), "_N", out string resourcename);
                        if (prefab == null)
                            return;
                        CharacterAppearance character = prefab.GetComponent<CharacterAppearance>();
                        CharacterMotion Default = character.GetCharacterMotion(ActionDetail.Default);
                        if (Default == null)
                            return;
                        SpriteSet Body = Default.motionSpriteSet.Find(x => x.sprType == CharacterAppearanceType.Body);
                        if (Body != null)
                        {
                            StaticDataManager.NonThumbSprite[__instance.id] = Default.motionSpriteSet.Find(x => x.sprType == CharacterAppearanceType.Body).sprRenderer.sprite;
                            __result = StaticDataManager.NonThumbSprite[__instance.id];
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Error("PrefabThumbe", ex);
                    }
                }
            }

        }
        [HarmonyPatch(typeof(AssemblyManager),nameof(AssemblyManager.CreateInstance_DiceCardSelfAbility))]
        [HarmonyPostfix]
        public static void AssemblyManager_CreateInstance_DiceCardSelfAbility(ref DiceCardSelfAbilityBase __result)
        {
            if (__result is DiceCardSelfAbility_Jaeheon_AreaDt)
                __result = new Fix.DiceCardSelfAbility_Jaeheon_AreaDt_New();
            else if (ContractLoader.Instance.GetPassiveList().Exists(x => x.Type == "Roland3rd_Unity") && __result is DiceCardSelfAbility_atkcombo_allas)
                __result = new Fix.DiceCardSelfAbility_atkcombo_allas_New();
            else if (ContractLoader.Instance.GetPassiveList().Exists(x => x.Type == "Roland3rd_Unity") && __result is DiceCardSelfAbility_atkcombo_logic)
                __result = new Fix.DiceCardSelfAbility_atkcombo_logic_New();
            else if (ContractLoader.Instance.GetPassiveList().Exists(x => x.Type == "Roland3rd_Unity") && __result is DiceCardSelfAbility_atkcombo_zelkova)
                __result = new Fix.DiceCardSelfAbility_atkcombo_zelkova_New();
        }
        [HarmonyPatch(typeof(AssemblyManager),nameof(AssemblyManager.CreateInstance_PassiveAbility))]
        [HarmonyPostfix]
        public static void AssemblyManager_CreateInstance_PassiveAbility(ref PassiveAbilityBase __result)
        {
            if (__result is PassiveAbility_1302013)
                __result = new Fix.PassiveAbility_1302013_New();
            else if (__result is PassiveAbility_1303012)
                __result = new Fix.PassiveAbility_1303012_New();
            else if (__result is PassiveAbility_1303013)
                __result = new Fix.PassiveAbility_1303013_New();
            else if (ContractLoader.Instance.GetPassiveList().Exists(x => x.Type == "Roland1st") && __result is PassiveAbility_170003 && !(__result is PassiveAbility_1700013))
                __result = new Fix.PassiveAbility_170003_New();
            else if (ContractLoader.Instance.GetPassiveList().Exists(x => x.Type == "Roland4th_BlackSilence") && __result is PassiveAbility_170301)
                __result = new Fix.PassiveAbility_170301_New();
            else if (ContractLoader.Instance.GetPassiveList().Exists(x => x.Type == "DBremen_Self") && __result is PassiveAbility_1404013)
                __result = new Fix.PassiveAbility_1404013_New();
            else if (ContractLoader.Instance.GetPassiveList().Exists(x => x.Type == "DOswald_Friend" || x.Type== "DOswald_Show") && __result is PassiveAbility_1405011)
                __result = new Fix.PassiveAbility_1405011_New();
            else if (ContractLoader.Instance.GetPassiveList().Exists(x => x.Type == "DArgalia_Sonata") && __result is PassiveAbility_1410013)
                __result = new Fix.PassiveAbility_1410013_New();
        }
        [HarmonyPatch(typeof(UICharacterRenderer),nameof(UICharacterRenderer.SetCharacter))]
        [HarmonyPostfix]
        public static void UICharacterRenderer_SetCharacter(UnitDataModel unit, int index)
        {
            if (unit.GetCustomBookItemData() != null && CCInitializer.NonHeadEquipPage.Exists(x => unit.GetCustomBookItemData().GetBookClassInfoId() == Tools.MakeLorId(x)) || CCInitializer.NonHeadEquipPage.Exists(x => unit.bookItem.GetBookClassInfoId() == Tools.MakeLorId(x)) && unit.GetCustomBookItemData() == null)
            {
                UICharacter character = UICharacterRenderer.Instance.characterList[index];
                CustomizedAppearance appearance = character.unitAppearance._customAppearance;
                if (appearance != null)
                {
                    foreach (SpriteRenderer allSprite in appearance.allSpriteList)
                    {
                        SpriteMask spriteMask = allSprite.GetComponent<SpriteMask>();
                        if (spriteMask != null)
                            character.unitAppearance.maskControl.maskList.Remove(spriteMask);
                    }
                    if (appearance.gameObject != null)
                        UnityEngine.Object.Destroy(appearance.gameObject);
                    character.unitAppearance._customAppearance = null;
                }
            }
        }
        [HarmonyPatch(typeof(SdCharacterUtil),nameof(SdCharacterUtil.CreateSkin))]
        [HarmonyPostfix]
        public static void SdCharacterUtil_CreateSkin(UnitDataModel unit, CharacterAppearance __result)
        {
            if (unit.GetCustomBookItemData() != null && CCInitializer.NonHeadEquipPage.Exists(x => unit.GetCustomBookItemData().GetBookClassInfoId() == Tools.MakeLorId(x)) || CCInitializer.NonHeadEquipPage.Exists(x => unit.bookItem.GetBookClassInfoId() == Tools.MakeLorId(x)) && unit.GetCustomBookItemData() == null)
            {
                CustomizedAppearance appearance = __result._customAppearance;
                if (appearance != null)
                {
                    foreach (SpriteRenderer allSprite in appearance.allSpriteList)
                    {
                        SpriteMask spriteMask = allSprite.GetComponent<SpriteMask>();
                        if (spriteMask != null)
                            __result.maskControl.maskList.Remove(spriteMask);
                    }
                    if (appearance.gameObject != null)
                        UnityEngine.Object.Destroy(appearance.gameObject);
                    __result._customAppearance = null;
                }
            }
        }
        [HarmonyPatch(typeof(UIBattleSettingPanel),nameof(UIBattleSettingPanel.SetButtonText))]
        [HarmonyPostfix]
        public static void UIBattleSettingPanel_SetButtonText(UIBattleSettingPanel __instance)
        {
            __instance.txt_FloorText.text= TextDataModel.GetText("ui_battlesetting_possiblefloor") + " " + (object)(StageController.Instance.GetStageModel().ClassInfo.floorNum-StageController.Instance.GetStageModel().floorList.FindAll(x => x.IsUnavailable()).Count);
        }
        [HarmonyPatch(typeof(EmotionCardXmlList),nameof(EmotionCardXmlList.GetDataList), new Type[] { typeof(SephirahType), typeof(int), typeof(int) })]
        [HarmonyPostfix]
        public static void EmotionCardXmlList_GetDataList(List<EmotionCardXmlInfo> __result, int emotionLevel)
        {
            if (Singleton<ContractLoader>.Instance.GetPassiveList().Find(x => x.Type == "NoEmotion") is Contract NoEmotion && emotionLevel >= 4 - NoEmotion.Variant)
                __result.Clear();
        }
        [HarmonyPatch(typeof(BattleUnitModel),nameof(BattleUnitModel.OnStartBattle))]
        [HarmonyPostfix]
        public static void BattleUnitModel_OnStartBattle(BattleUnitModel __instance)
        {
            foreach(BattleUnitBuf buf in __instance.bufListDetail.GetActivatedBufList())
            {
                if (buf is BattleUnitBuf_Extention)
                    (buf as BattleUnitBuf_Extention).OnStartBattle();
            }
        }
    }
}
