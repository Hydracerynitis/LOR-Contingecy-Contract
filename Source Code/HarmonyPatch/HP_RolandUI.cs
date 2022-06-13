using System;
using UI;
using UnityEngine;
using HarmonyLib;
using TMPro;
using UC = UI.UIController;
using BaseMod;
using UnityEngine.UI;
using ContractReward;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    public class HP_RolandUI
    {
        static Color grey= new Color(0.75f, 0.75f, 0.75f);
        //P1
        [HarmonyPatch(typeof(UISettingInvenEquipPageSlot), nameof(UISettingInvenEquipPageSlot.SetOperatingPanel))]
        [HarmonyPostfix]
        static void UISettingInvenEquipPageSlot_SetOperatingPanel_Pre(BookModel ____bookDataModel, UICustomGraphicObject ___button_Equip, TextMeshProUGUI ___txt_equipButton, Image ___img_equipbuttonIcon)
        {
            UnitDataModel currentUnit = UC.Instance.CurrentUnit;
            if (____bookDataModel != null && (____bookDataModel.ClassInfo.id == Tools.MakeLorId(17000001) || ____bookDataModel.ClassInfo.id == Tools.MakeLorId(17000004)))
            {
                if (CCInitializer.IsRoland(currentUnit))
                {
                    string id1 = ____bookDataModel.owner == null ? "ui_bookinventory_equipbook" : "ui_book_bookname_unequip";
                    ___button_Equip.interactable = true;
                    ___txt_equipButton.text = TextDataModel.GetText(id1);
                    ___img_equipbuttonIcon.color = Color.white;
                }
                else
                {
                    string id1 = "ui_equippage_notequip";
                    ___button_Equip.interactable = false;
                    ___txt_equipButton.text = TextDataModel.GetText(id1);
                    ___img_equipbuttonIcon.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                }
            }
        }
        [HarmonyPatch(typeof(UISettingInvenEquipPageLeftSlot) ,nameof(UISettingInvenEquipPageLeftSlot.SetOperatingPanel))]
        [HarmonyPostfix]
        static void UISettingInvenEquipPageLeftSlot_SetOperatingPanel_Pre(BookModel ____bookDataModel, UICustomGraphicObject ___button_Equip, TextMeshProUGUI ___txt_equipButton, Image ___img_equipbuttonIcon)
        {
            UnitDataModel currentUnit = UC.Instance.CurrentUnit;
            if (____bookDataModel != null && (____bookDataModel.ClassInfo.id == Tools.MakeLorId(17000001) || ____bookDataModel.ClassInfo.id == Tools.MakeLorId(17000004)))
            {
                if (CCInitializer.IsRoland(currentUnit))
                {
                    string id1 = ____bookDataModel.owner == null ? "ui_bookinventory_equipbook" : "ui_book_bookname_unequip";
                    ___button_Equip.interactable = true;
                    ___txt_equipButton.text = TextDataModel.GetText(id1);
                    ___img_equipbuttonIcon.color = Color.white;
                }
                else
                {
                    string id1 = "ui_equippage_notequip";
                    ___button_Equip.interactable = false;
                    ___txt_equipButton.text = TextDataModel.GetText(id1);
                    ___img_equipbuttonIcon.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                }
            }
        }
        [HarmonyPatch(typeof(UIInvenEquipPageSlot), nameof(UIInvenEquipPageSlot.SetOperatingPanel))]
        [HarmonyPostfix]
        static void UIInvenEquipPageSlot_SetOperatingPanel_Pre(BookModel ____bookDataModel, UICustomGraphicObject ___button_Equip, TextMeshProUGUI ___txt_equipButton, Image ___img_equipbuttonIcon)
        {
            UnitDataModel currentUnit = UC.Instance.CurrentUnit;
            if (____bookDataModel != null && (____bookDataModel.ClassInfo.id == Tools.MakeLorId(17000001) || ____bookDataModel.ClassInfo.id == Tools.MakeLorId(17000004)))
            {
                if (CCInitializer.IsRoland(currentUnit))
                {
                    string id1 = ____bookDataModel.owner == null ? "ui_bookinventory_equipbook" : "ui_book_bookname_unequip";
                    ___button_Equip.interactable = true;
                    ___txt_equipButton.text = TextDataModel.GetText(id1);
                    ___img_equipbuttonIcon.color = Color.white;
                }
                else
                {
                    string id1 = "ui_equippage_notequip";
                    ___button_Equip.interactable = false;
                    ___txt_equipButton.text = TextDataModel.GetText(id1);
                    ___img_equipbuttonIcon.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                }
            }
        }
        [HarmonyPatch(typeof(UIInvenLeftEquipPageSlot), nameof(UIInvenLeftEquipPageSlot.SetOperatingPanel))]
        [HarmonyPostfix]
        static void UIInvenLeftEquipPageSlot_SetOperatingPanel_Pre(BookModel ____bookDataModel, UICustomGraphicObject ___button_Equip, TextMeshProUGUI ___txt_equipButton, Image ___img_equipbuttonIcon)
        {
            UnitDataModel currentUnit = UC.Instance.CurrentUnit;
            if (____bookDataModel != null && (____bookDataModel.ClassInfo.id == Tools.MakeLorId(17000001) || ____bookDataModel.ClassInfo.id == Tools.MakeLorId(17000004)))
            {
                if (CCInitializer.IsRoland(currentUnit))
                {
                    string id1 = ____bookDataModel.owner == null ? "ui_bookinventory_equipbook" : "ui_book_bookname_unequip";
                    ___button_Equip.interactable = true;
                    ___txt_equipButton.text = TextDataModel.GetText(id1);
                    ___img_equipbuttonIcon.color = Color.white;
                }
                else
                {
                    string id1 = "ui_equippage_notequip";
                    ___button_Equip.interactable = false;
                    ___txt_equipButton.text = TextDataModel.GetText(id1);
                    ___img_equipbuttonIcon.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                }
            }
        }
        [HarmonyPatch(typeof(BattleUnitInfoManagerUI),nameof(BattleUnitInfoManagerUI.DisplayDlg))]
        [HarmonyPrefix]
        static bool BattleUnitInfoManagerUI_DisplayDlg_Pre(ref string str, BattleUnitModel unit)
        {
            if (unit.passiveDetail.HasPassive<PassiveAbility_1700000>())
                str = "..............";
            return true;
        }
        //P4
        [HarmonyPatch(typeof(BattleDiceCardUI), nameof(BattleDiceCardUI.SetCard))]
        [HarmonyPostfix]
        static void BattleDiceCardUI_SetCard(BattleDiceCardUI __instance)
        {
            if(__instance.CardModel!=null && __instance.CardModel.GetID().id >= 17000040 && __instance.CardModel.GetID().id <= 17000049 && __instance.CardModel.GetID().packageId== "ContingencyConract")
            {
                __instance.colorFrame = grey;
                __instance.colorLineardodge= grey;
                __instance.colorLineardodge_deactive = __instance.colorLineardodge;
                __instance.colorLineardodge_deactive.a = 0;
                __instance.SetFrameColor(__instance.colorFrame);
                __instance.SetLinearDodgeColor(__instance.colorLineardodge);
                __instance.img_artwork.color = grey;
            }
        }
        [HarmonyPatch(typeof(UIOriginCardSlot), nameof(UIOriginCardSlot.SetData))]
        [HarmonyPostfix]
        static void UIOriginCardSlot_SetData(UIOriginCardSlot __instance)
        {
            if (__instance.CardModel != null && __instance.CardModel.GetID().id >= 17000040 && __instance.CardModel.GetID().id <= 17000049 && __instance.CardModel.GetID().packageId == "ContingencyConract")
            {
                __instance.colorFrame = grey;
                __instance.colorLineardodge = grey;
                __instance.SetFrameColor(__instance.colorFrame);
                __instance.SetLinearDodgeColor(__instance.colorLineardodge);
                __instance.img_Artwork.color = grey;
            }
        }
        //P5
        [HarmonyPatch(typeof(EmotionPassiveCardUI),nameof(EmotionPassiveCardUI.SetSprites))]
        [HarmonyPostfix]
        static void EmotionPassiveCardUI_SetSprites(EmotionPassiveCardUI __instance)
        {
            if (HP_RolandSystem.IsRolandEmotion(__instance.Card))
            {
                __instance._hOverImg.color = new Color(0f, grey.g, grey.b);
                __instance._rootImageBg.color = new Color(0f, grey.g, grey.b, 0.25f);
                __instance._artwork.sprite = AssetBundleManagerRemake.Instance.LoadCardSprite(__instance.Card._artwork);
                __instance._flavorText.fontMaterial.SetColor("_UnderlayColor", grey);
                __instance._abilityDesc.fontMaterial.SetColor("_UnderlayColor", grey);
            }
        }
    } 
}
