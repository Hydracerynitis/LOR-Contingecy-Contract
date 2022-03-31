using System;
using UI;
using UnityEngine;
using HarmonyLib;
using TMPro;
using UC = UI.UIController;
using BaseMod;
using UnityEngine.UI;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    public class HarmonyPatch_UI
    {
        [HarmonyPatch(typeof(UISettingInvenEquipPageSlot), nameof(UISettingInvenEquipPageSlot.SetOperatingPanel))]
        [HarmonyPostfix]
        static void UISettingInvenEquipPageSlot_SetOperatingPanel_Pre(BookModel ____bookDataModel, UICustomGraphicObject ___button_Equip, TextMeshProUGUI ___txt_equipButton, Image ___img_equipbuttonIcon)
        {
            UnitDataModel currentUnit = UC.Instance.CurrentUnit;
            if (____bookDataModel != null && ____bookDataModel.ClassInfo.id == Tools.MakeLorId(17000001))
            {
                if (Harmony_Patch.IsRoland(currentUnit))
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
            if (____bookDataModel != null && ____bookDataModel.ClassInfo.id == Tools.MakeLorId(17000001))
            {
                if (Harmony_Patch.IsRoland(currentUnit))
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
            if (____bookDataModel != null && ____bookDataModel.ClassInfo.id == Tools.MakeLorId(17000001))
            {
                if (Harmony_Patch.IsRoland(currentUnit))
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
            if (____bookDataModel != null && ____bookDataModel.ClassInfo.id == Tools.MakeLorId(17000001))
            {
                if (Harmony_Patch.IsRoland(currentUnit))
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
    } 
}
