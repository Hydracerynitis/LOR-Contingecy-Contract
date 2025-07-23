using System;
using System.Collections.Generic;
using BaseMod;
using HarmonyLib;
using UI;
using UnityEngine;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    class HP_Miscellaneous
    {
        [HarmonyPatch(typeof(DropBookInventoryModel), nameof(DropBookInventoryModel.GetBookList_invitationBookList))]
        [HarmonyPostfix]
        public static void AddEnsembleRematchPages(ref List<LorId> __result)
        {
            if (LibraryModel.Instance.PlayHistory.Clear_EndcontentsAllStage == 1)
            {
                __result.Add(Tools.MakeLorId(70001));
                __result.Add(Tools.MakeLorId(70002));
                __result.Add(Tools.MakeLorId(70003));
                __result.Add(Tools.MakeLorId(70004));
                __result.Add(Tools.MakeLorId(70005));
                __result.Add(Tools.MakeLorId(70006));
                __result.Add(Tools.MakeLorId(70007));
                __result.Add(Tools.MakeLorId(70008));
                __result.Add(Tools.MakeLorId(70009));
                __result.Add(Tools.MakeLorId(70010));
            }
        }
        [HarmonyPatch(typeof(BookModel),nameof(BookModel.GetThumbSprite))]
        [HarmonyPostfix]
        public static void BookModel_AddThumbSprite(ref Sprite __result, BookModel __instance)
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
        public static void BookXmlInfo_AddThumbSprite(ref Sprite __result, BookXmlInfo __instance)
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
        [HarmonyPatch(typeof(UICharacterRenderer),nameof(UICharacterRenderer.SetCharacter))]
        [HarmonyPostfix]
        public static void UICharacterRenderer_RemoveHead(UnitDataModel unit, int index)
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
        public static void SdCharacterUtil_RemoveHead(UnitDataModel unit, CharacterAppearance __result)
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
        public static void RenewAvailableFloorCount(UIBattleSettingPanel __instance)
        {
            __instance.txt_FloorText.text= TextDataModel.GetText("ui_battlesetting_possiblefloor") + " " + 
                (object)(StageController.Instance.GetStageModel().ClassInfo.floorNum-StageController.Instance.GetStageModel().floorList.FindAll(x => x.IsUnavailable()).Count);
        }
    }
}
