using HarmonyLib;
using UI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using BaseMod;
using UnityEngine.UI;
using UC = UI.UIController;
using UnityEngine;
using System.Collections.Generic;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    static class HP_RewardList
    {
        [HarmonyPatch(typeof(BookInventoryModel), nameof(BookInventoryModel.GetBookList_equip))]
        [HarmonyPrefix]
        static void ResetRewardSystem()
        {
            ContractRewardSystem.Instance.CheckRewardAchieved();
            ContractRewardSystem.Instance.UIs.Clear();
        }
        [HarmonyPatch(typeof(BookInventoryModel),nameof(BookInventoryModel.GetBookList_equip))]
        [HarmonyPostfix]
        static void AppendDemonstrationPage(List<BookModel> __result)
        {
            if (!ContractRewardSystem.Instance.Active)
                return;
            foreach (LorId rewardPage in StaticDataManager.Glossary)
            {
                if (!ContractRewardSystem.Instance.ClearList.Contains(rewardPage))
                {
                    BookModel demo = new BookModel(BookXmlList.Instance.GetData(rewardPage).CopyForDemo());
                    DeckXmlInfo deck = DeckXmlList.Instance.GetData(demo.GetBookClassInfoId());
                    if(deck != null)
                        foreach (LorId id in deck.cardIdList)
                            demo._deck.AddCardForLoading(id);
                    __result.Add(demo);
                }
            }
        }
        [HarmonyPatch(typeof(BookInventoryModel),nameof(BookInventoryModel.GetBookList_PassiveEquip))]
        [HarmonyPostfix]
        static void RemoveRewardPageFromPassiveAttribute(List<BookModel> __result)
        {
            __result.RemoveAll(x => StaticDataManager.Glossary.Contains(x.GetBookClassInfoId()));
        }
        [HarmonyPatch(typeof(UISettingInvenEquipPageSlot), nameof(UISettingInvenEquipPageSlot.SetOperatingPanel))]
        [HarmonyPostfix]
        static void DemonstrationEquipPageUI_SIEPS(UISettingInvenEquipPageSlot __instance, BookModel ____bookDataModel, UICustomGraphicObject ___button_Equip, TextMeshProUGUI ___txt_equipButton, Image ___img_equipbuttonIcon)
        {
            if(____bookDataModel.ClassInfo.workshopID == "ContingencyConract" && ____bookDataModel.ClassInfo.isError)
            {
                string id1 = "ui_ForDemo";
                __instance.button_BookMark.interactable = false;
                __instance.button_EmptyDeck.interactable = false;
                __instance.txt_bookmarkButton.text = TextDataModel.GetText("ui_equippage_notequip");
                __instance.img_bookmarkbuttonIcon.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                ___button_Equip.interactable = true;
                ___txt_equipButton.text = TextDataModel.GetText(id1);
                ___img_equipbuttonIcon.color = Color.white;
                return;
            }      
            UnitDataModel currentUnit = UC.Instance.CurrentUnit;
            if (____bookDataModel != null && (____bookDataModel.ClassInfo.id == Tools.MakeLorId(17000001) || ____bookDataModel.ClassInfo.id == Tools.MakeLorId(17000004)))//Rpland Special
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
        [HarmonyPatch(typeof(UISettingInvenEquipPageLeftSlot), nameof(UISettingInvenEquipPageLeftSlot.SetOperatingPanel))]
        [HarmonyPostfix]
        static void DemonstrationEquipPageUI_SIEPLS(UISettingInvenEquipPageLeftSlot __instance,BookModel ____bookDataModel, UICustomGraphicObject ___button_Equip, TextMeshProUGUI ___txt_equipButton, Image ___img_equipbuttonIcon)
        {
            if (____bookDataModel.ClassInfo.workshopID == "ContingencyConract" && ____bookDataModel.ClassInfo.isError)
            {
                string id1 = "ui_ForDemo";
                __instance.button_BookMark.interactable = false;
                __instance.button_EmptyDeck.interactable = false;
                __instance.txt_bookmarkButton.text = TextDataModel.GetText("ui_equippage_notequip");
                __instance.img_bookmarkbuttonIcon.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                ___button_Equip.interactable = true;
                ___txt_equipButton.text = TextDataModel.GetText(id1);
                ___img_equipbuttonIcon.color = Color.white;
                return;
            }
            UnitDataModel currentUnit = UC.Instance.CurrentUnit;
            if (____bookDataModel != null && (____bookDataModel.ClassInfo.id == Tools.MakeLorId(17000001) || ____bookDataModel.ClassInfo.id == Tools.MakeLorId(17000004))) //Roland Special
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
        static void DemonstrationEquipPageUI_IEPS(UIInvenEquipPageSlot __instance, BookModel ____bookDataModel, UICustomGraphicObject ___button_Equip, TextMeshProUGUI ___txt_equipButton, Image ___img_equipbuttonIcon)
        {
            if (____bookDataModel.ClassInfo.workshopID == "ContingencyConract" && ____bookDataModel.ClassInfo.isError)
            {
                string id1 = "ui_ForDemo";
                __instance.button_BookMark.interactable = false;
                __instance.button_EmptyDeck.interactable = false;
                __instance.txt_bookmarkButton.text = TextDataModel.GetText("ui_equippage_notequip");
                __instance.img_bookmarkbuttonIcon.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                __instance.button_PassiveSuccession.interactable = false;
                ___button_Equip.interactable = true;
                ___txt_equipButton.text = TextDataModel.GetText(id1);
                ___img_equipbuttonIcon.color = Color.white;
                return;
            }
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
        static void DemonstrationEquipPageUI_ILEPS(UIInvenLeftEquipPageSlot __instance, BookModel ____bookDataModel, UICustomGraphicObject ___button_Equip, TextMeshProUGUI ___txt_equipButton, Image ___img_equipbuttonIcon)
        {
            if (____bookDataModel.ClassInfo.workshopID == "ContingencyConract" && ____bookDataModel.ClassInfo.isError)
            {
                string id1 = "ui_ForDemo";
                __instance.button_BookMark.interactable = false;
                __instance.button_EmptyDeck.interactable = false;
                __instance.txt_bookmarkButton.text = TextDataModel.GetText("ui_equippage_notequip");
                __instance.img_bookmarkbuttonIcon.color = UIColorManager.Manager.GetUIColor(UIColor.Disabled);
                __instance.button_PassiveSuccession.interactable = false;
                ___button_Equip.interactable = true;
                ___txt_equipButton.text = TextDataModel.GetText(id1);
                ___img_equipbuttonIcon.color = Color.white;
                return;
            }
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
        [HarmonyPatch(typeof(UIInvenLeftEquipPageSlot),nameof(UIInvenLeftEquipPageSlot.OnClickEquipButton))]
        [HarmonyPrefix]
        static bool DisplayRewardCondition_ILEPS(UIInvenLeftEquipPageSlot __instance)
        {
            if(__instance.BookDataModel.ClassInfo.isError)
            {
                string lanagugae = StaticDataManager.GetSupportedLanguage();
                if (StaticDataManager.RewardCondition[lanagugae].ContainsKey(__instance.BookDataModel.ClassInfo.id))
                {
                    UIAlarmPopup.instance.SetAlarmText(StaticDataManager.RewardCondition[lanagugae][__instance.BookDataModel.ClassInfo.id]);
                    return false;
                }
            }
            return true;
        }
        [HarmonyPatch(typeof(UIInvenEquipPageSlot),nameof(UIInvenEquipPageSlot.OnClickEquipButton))]
        [HarmonyPrefix]
        static bool DisplayRewardCondition_IEPS(UIInvenEquipPageSlot __instance)
        {
            if (__instance.BookDataModel.ClassInfo.isError)
            {
                string lanagugae = StaticDataManager.GetSupportedLanguage();
                if (StaticDataManager.RewardCondition[lanagugae].ContainsKey(__instance.BookDataModel.ClassInfo.id))
                {
                    UIAlarmPopup.instance.SetAlarmText(StaticDataManager.RewardCondition[lanagugae][__instance.BookDataModel.ClassInfo.id]);
                    return false;
                }
            }
            return true;
        }
        [HarmonyPatch(typeof(UISettingInvenEquipPageLeftSlot),nameof(UISettingInvenEquipPageLeftSlot.OnClickEquipButton))]
        [HarmonyPrefix]
        static bool DisplayRewardCondition_SIEPLS(UISettingInvenEquipPageLeftSlot __instance)
        {
            if (__instance.BookDataModel.ClassInfo.isError)
            {
                string lanagugae = StaticDataManager.GetSupportedLanguage();
                if (StaticDataManager.RewardCondition[lanagugae].ContainsKey(__instance.BookDataModel.ClassInfo.id))
                {
                    UIAlarmPopup.instance.SetAlarmText(StaticDataManager.RewardCondition[lanagugae][__instance.BookDataModel.ClassInfo.id]);
                    return false;
                }
            }
            return true;
        }
        [HarmonyPatch(typeof(UISettingInvenEquipPageSlot),nameof(UISettingInvenEquipPageSlot.OnClickEquipButton))]
        [HarmonyPrefix]
        static bool DisplayRewardCondition_ILEPS_SIEPS(UISettingInvenEquipPageSlot __instance)
        {
            if (__instance.BookDataModel.ClassInfo.isError)
            {
                string lanagugae = StaticDataManager.GetSupportedLanguage();
                if (StaticDataManager.RewardCondition[lanagugae].ContainsKey(__instance.BookDataModel.ClassInfo.id))
                {
                    UIAlarmPopup.instance.SetAlarmText(StaticDataManager.RewardCondition[lanagugae][__instance.BookDataModel.ClassInfo.id]);
                    return false;
                }
            }
            return true;
        }
    }
}
