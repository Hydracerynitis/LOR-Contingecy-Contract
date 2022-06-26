using System.Collections.Generic;
using HarmonyLib;
using UI;
using UnityEngine;
using BaseMod;
using LOR_DiceSystem;
using System;
using UC = UI.UIController;
using Mod;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    static class HP_SplitDeck
    {
        static UIInvenCardListScroll cardListScroll;
        static int modified;
        [HarmonyPatch(typeof(UIEquipDeckCardList),nameof(UIEquipDeckCardList.SetDeckLayout))]
        [HarmonyPostfix]
        static void UIEquipDeckCardList_SetDeckLayout(UIEquipDeckCardList __instance)
        {
            BookModel book = __instance.currentunit.bookItem;
            UICustomTabsController CTC = __instance.multiDeckLayout.transform.GetChild(0).GetComponent<UICustomTabsController>();
            if (book.ClassInfo.workshopID == "ContingencyConract" && book.IsMultiDeck())
            {
                
                if (modified != book.ClassInfo._id)
                    RestoreDefault(CTC);
                if(book.ClassInfo._id== 18100000)
                {
                    
                    CTC.CustomTabs[0].TabName.text = PassiveDescXmlList.Instance.GetName(Tools.MakeLorId(1810003));
                    CTC.CustomTabs[1].TabName.text = PassiveDescXmlList.Instance.GetName(Tools.MakeLorId(1810004));
                    CTC.CustomTabs[2].gameObject.SetActive(false);
                    CTC.CustomTabs[3].gameObject.SetActive(false);
                }
                else if(book.ClassInfo._id == 18200000)
                {
                    CTC.CustomTabs[0].TabName.text = PassiveDescXmlList.Instance.GetName(250214);
                    CTC.CustomTabs[1].TabName.text = PassiveDescXmlList.Instance.GetName(250114);
                    CTC.CustomTabs[2].gameObject.SetActive(false);
                    CTC.CustomTabs[3].gameObject.SetActive(false);
                }
                else if (book.ClassInfo._id == 18500000)
                {
                    CTC.CustomTabs[0].TabName.text = CharactersNameXmlList.Instance.GetName(251);
                    CTC.CustomTabs[1].TabName.text = CharactersNameXmlList.Instance.GetName(252);
                    CTC.CustomTabs[2].TabName.text = CharactersNameXmlList.Instance.GetName(253);
                    CTC.CustomTabs[3].gameObject.SetActive(false);
                }
                else if (book.ClassInfo._id == 18700000)
                {
                    CTC.CustomTabs[0].TabName.text = CharactersNameXmlList.Instance.GetName(271);
                    CTC.CustomTabs[1].TabName.text = TextDataModel.GetText("ui_Jeaheon_counter");
                    CTC.CustomTabs[2].TabName.text = CharactersNameXmlList.Instance.GetName(272);
                    CTC.CustomTabs[3].gameObject.SetActive(false);
                }
                else if (book.ClassInfo._id == 17000003)
                {
                    CTC.CustomTabs[0].TabName.text = PassiveDescXmlList.Instance.GetName(230008);
                    CTC.CustomTabs[1].TabName.text = BookDescXmlList.Instance.GetBookName(Tools.MakeLorId(17000003));
                    CTC.CustomTabs[2].TabName.text = BookDescXmlList.Instance.GetBookName(Tools.MakeLorId(17000103));
                    CTC.CustomTabs[3].gameObject.SetActive(false);
                }
                modified = book.ClassInfo._id;
                return;
            }
            else if (modified!=-1)
                RestoreDefault(CTC);
        }
        static void RestoreDefault(UICustomTabsController CTC)
        {
            foreach (UICustomTabButton CTB in CTC.CustomTabs)
            {
                CTB.gameObject.SetActive(true);
            }
            CTC.CustomTabs[0].TabName.text = TextDataModel.GetText("ui_slash_form");
            CTC.CustomTabs[1].TabName.text = TextDataModel.GetText("ui_penetrate_form");
            CTC.CustomTabs[2].TabName.text = TextDataModel.GetText("ui_hit_form");
            CTC.CustomTabs[3].TabName.text = TextDataModel.GetText("ui_defense_form");
            modified = -1;
        }
        [HarmonyPatch(typeof(BookModel),nameof(BookModel.GetOnlyCards))]
        [HarmonyPostfix]
        static void BookModel_GetOnlyCards(BookModel __instance,ref List<DiceCardXmlInfo> __result)
        {
            if(__instance.ClassInfo.workshopID == "ContingencyConract" && __instance.IsMultiDeck())
            {
                List<DiceCardXmlInfo> only= GetDeckOnly(__result, __instance.GetCurrentDeckIndex());
                if (only != null)
                    __result = only;

            }
        }
        [HarmonyPatch(typeof(UIEquipDeckCardList),nameof(UIEquipDeckCardList.OnChangeDeckTab))]
        [HarmonyPostfix]
        static void UIEquipDeckCardList_OnChangeDeckTab()
        {
            if (modified != -1 && cardListScroll != null)
                cardListScroll.ApplyFilterAll();
        }
        static List<DiceCardXmlInfo> GetDeckOnly(List<DiceCardXmlInfo> __result, int deckIndex)
        {
            List<List<DiceCardXmlInfo>> split = new List<List<DiceCardXmlInfo>>();
            List<DiceCardXmlInfo> next = new List<DiceCardXmlInfo>();
            foreach(DiceCardXmlInfo card in __result)
            {
                if (!card.isError)
                    next.Add(card);
                else
                {
                    split.Add(next);
                    next= new List<DiceCardXmlInfo>();
                }
            }
            split.Add(next);
            if (deckIndex < 0 || deckIndex >= split.Count)
                return null;
            return split[deckIndex];
        }
        static bool EileenCond(BookModel book) => book.ClassInfo._id == 18200000 && book.GetCurrentDeckIndex() == 1;
        static bool OswaldCond(BookModel book) => book.ClassInfo._id == 18500000 && book.GetCurrentDeckIndex() == 2;
        static bool JaeheonCond(BookModel book) => book.ClassInfo._id == 18700000 && book.GetCurrentDeckIndex() == 1;
        //static bool EileenCond(int id, int index) => id == 18200000 && index == 1;
        [HarmonyPatch(typeof(UIInvenCardListScroll),nameof(UIInvenCardListScroll.ApplyFilterAll))]
        [HarmonyPostfix]
        static void UIInvenCardListScroll_ApplyFilterAll(UIInvenCardListScroll __instance)
        {
            if (cardListScroll == null)
                cardListScroll = __instance;
            if (__instance._unitdata == null)
                return;
            BookModel book = __instance._unitdata._bookItem;
            if (book == null)
                return;
            if (book.ClassInfo.workshopID != "ContingencyConract")
                return;
            if (!EileenCond(book) && !OswaldCond(book) && !JaeheonCond(book))
                return;
            __instance._currentCardListForFilter.Clear();
            List<DiceCardItemModel> byDetailFilterUi = __instance.GetCardsByDetailFilterUI(__instance.GetCardBySearchFilterUI(__instance.GetCardsByCostFilterUI(__instance.GetCardsByGradeFilterUI(__instance._originCardList))));
            byDetailFilterUi.Sort(ModCardItemSort);
            Predicate<DiceCardItemModel> cond1 = x => true;
            if (book.ClassInfo._id == 18200000)
                cond1 = x => x.GetSpec().Ranged == CardRange.Near;
            else if (book.ClassInfo._id == 18500000)
                cond1 = x => x.GetSpec().Ranged == CardRange.Far;
            else if (book.ClassInfo._id == 18700000)
                cond1 = x => !x.GetBehaviourList().Exists(y => y.Type != BehaviourType.Standby);
            List<DiceCardXmlInfo> onlyCards = book.GetOnlyCards();
            Predicate<DiceCardItemModel> cond2 = x => onlyCards.Exists(y => y.id == x.GetID());
            foreach (DiceCardItemModel diceCardItemModel in byDetailFilterUi.FindAll(x => x.ClassInfo.optionList.Contains(CardOption.OnlyPage) && !cond2(x)))
                byDetailFilterUi.Remove(diceCardItemModel);
            __instance._currentCardListForFilter.AddRange(byDetailFilterUi.FindAll(x => !x.ClassInfo.optionList.Contains(CardOption.OnlyPage) ? cond1(x) : cond2(x)));
            __instance._currentCardListForFilter.AddRange(byDetailFilterUi.FindAll(x => (x.ClassInfo.optionList.Contains(CardOption.OnlyPage) ? (cond2(x) ? 1 : 0) : (cond1(x) ? 1 : 0)) == 0));
            __instance.scrollBar.SetScrollRectSize(__instance.column * __instance.slotWidth, (__instance.GetMaxRow() + __instance.row - 1) * __instance.slotHeight);
            __instance.scrollBar.SetWindowPosition(0.0f, 0.0f);
            __instance.selectablePanel.ChildSelectable = __instance.slotList[0].selectable;
            __instance.SetCardsData(__instance.GetCurrentPageList());
        }
        //BaseMod Private
        private static int ModCardItemSort(DiceCardItemModel a, DiceCardItemModel b)
        {
            int num = b.ClassInfo.optionList.Contains(CardOption.OnlyPage) ? 1 : 0;
            int num2 = a.ClassInfo.optionList.Contains(CardOption.OnlyPage) ? 1 : 0;
            int num3 = (b.ClassInfo.isError ? -1 : num) - (a.ClassInfo.isError ? -1 : num2);
            int result;
            if (num3 != 0)
            {
                result = num3;
            }
            else
            {
                num3 = a.GetSpec().Cost - b.GetSpec().Cost;
                if (num3 != 0)
                {
                    result = num3;
                }
                else
                {
                    num3 = a.ClassInfo.workshopID.CompareTo(b.ClassInfo.workshopID);
                    result = ((num3 != 0) ? num3 : (a.GetID().id - b.GetID().id));
                }
            }
            return result;
        }
        [HarmonyPatch(typeof(UIInvenCardSlot),nameof(UIInvenCardSlot.SetSlotState))]
        [HarmonyPostfix]
        static void UIInvenCardSlot_SetSlotState(UIInvenCardSlot __instance)
        {
            UnitDataModel currentUnit = UC.Instance.CurrentUnit;
            if (currentUnit != null)
            {
                BookModel book = currentUnit.bookItem;
                if (book.ClassInfo.workshopID != "ContingencyConract")
                    return;
                if (!EileenCond(book) && !OswaldCond(book) && !JaeheonCond(book))
                    return;
                if (book.ClassInfo._id == 18200000)
                {
                    if (__instance.slotState == UIINVENCARD_STATE.MeleeCard)
                        __instance.slotState = UIINVENCARD_STATE.None;
                    else if (__instance.slotState == UIINVENCARD_STATE.None && __instance.CardModel.GetSpec().Ranged == CardRange.Far)
                        __instance.slotState = UIINVENCARD_STATE.RangeCard;
                }
                if (book.ClassInfo._id == 18500000)
                {
                    if (__instance.slotState == UIINVENCARD_STATE.RangeCard)
                        __instance.slotState = UIINVENCARD_STATE.None;
                    else if (__instance.slotState == UIINVENCARD_STATE.None && __instance.CardModel.GetSpec().Ranged == CardRange.Near && !__instance.CardModel.ClassInfo.IsOnlyPage())
                        __instance.slotState = UIINVENCARD_STATE.MeleeCard;
                }
                if (book.ClassInfo._id == 18700000)
                {
                    if(__instance.slotState == UIINVENCARD_STATE.None || __instance.slotState==UIINVENCARD_STATE.RangeCard)
                        if (__instance.CardModel.GetBehaviourList().Exists(y => y.Type != BehaviourType.Standby))
                            __instance.slotState = UIINVENCARD_STATE.OnlyPage;
                        
                }
                RefreshSlotState(__instance);
            }
        }
        static void RefreshSlotState(UIInvenCardSlot __instance)
        {
            __instance.deckLimitRoot.gameObject.SetActive(__instance.slotState > 0);
            __instance.SetGrayScale(__instance.slotState > 0);
            switch (__instance.slotState)
            {
                case UIINVENCARD_STATE.LimitedDeck:
                    __instance.txt_deckLimit.text = TextDataModel.GetText("ui_card_equipstate_overcardlimit");
                    break;
                case UIINVENCARD_STATE.LimitedFloor:
                    __instance.txt_deckLimit.text = TextDataModel.GetText("ui_card_equipstate_overfloorlimit");
                    break;
                case UIINVENCARD_STATE.NumberZero:
                    __instance.txt_deckLimit.text = TextDataModel.GetText("ui_card_equipstate_lackofcards");
                    break;
                case UIINVENCARD_STATE.OnlyPage:
                    __instance.txt_deckLimit.text = TextDataModel.GetText("ui_card_equipstate_onlypagelimit");
                    break;
                case UIINVENCARD_STATE.RangeCard:
                    __instance.txt_deckLimit.text = TextDataModel.GetText("ui_card_equipstate_fartypelimit");
                    break;
                case UIINVENCARD_STATE.MeleeCard:
                    __instance.txt_deckLimit.text = TextDataModel.GetText("ui_card_equipstate_neartypelimit");
                    break;
            }
        }
        [HarmonyPatch(typeof(BookModel),nameof(BookModel.AddCardFromInventoryToCurrentDeck))]
        [HarmonyPostfix]
        static void BookModel_AddCardFromInventoryToCurrentDeck(LorId cardId, BookModel __instance,ref CardEquipState __result)
        {
            if (__instance.ClassInfo.workshopID != "ContingencyConract")
                return;
            if (!EileenCond(__instance) && !OswaldCond(__instance) && !JaeheonCond(__instance))
                return;
            if (__instance.ClassInfo._id == 18200000)
            {
                if (__result == CardEquipState.NearTypeLimit)
                    __result = __instance._deck.AddCardFromInventory(cardId);
                else if(__result==CardEquipState.Equippable)
                {
                    DiceCardXmlInfo cardXmlInfo = ItemXmlDataList.instance.GetCardItem(cardId);
                    if (cardXmlInfo.Spec.Ranged == CardRange.Far)
                        __result = CardEquipState.FarTypeLimit;
                }
            }
            if(__instance.ClassInfo._id == 18500000)
            {
                if (__result == CardEquipState.FarTypeLimit)
                    __result = __instance._deck.AddCardFromInventory(cardId);
                else if (__result == CardEquipState.Equippable)
                {
                    DiceCardXmlInfo cardXmlInfo = ItemXmlDataList.instance.GetCardItem(cardId);
                    if (cardXmlInfo.Spec.Ranged == CardRange.Near && !cardXmlInfo.IsOnlyPage())
                        __result = CardEquipState.NearTypeLimit;
                }
            }
            if (__instance.ClassInfo._id == 18700000)
            {
                if (__result == CardEquipState.Equippable || __result==CardEquipState.FarTypeLimit)
                {
                    DiceCardXmlInfo cardXmlInfo = ItemXmlDataList.instance.GetCardItem(cardId);
                    if (cardXmlInfo.DiceBehaviourList.Exists(y => y.Type != BehaviourType.Standby))
                        __result = CardEquipState.OnlyPageLimit;
                    else
                        __result = CardEquipState.Equippable;
                    
                }
            }
        }
    }
}