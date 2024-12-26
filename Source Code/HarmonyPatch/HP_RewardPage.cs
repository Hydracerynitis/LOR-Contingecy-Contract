using System;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using BaseMod;
using ContractReward;
using HarmonyLib;
using UI;
using UnityEngine;
using LOR_BattleUnit_UI;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    class HP_RewardPage
    {
        //Greta
        [HarmonyPatch(typeof(BattleUnitBuf_Resistance), nameof(BattleUnitBuf_Resistance.keywordId), MethodType.Getter)]
        [HarmonyPostfix]
        public static void BattleUnitBuf_Resistance_get_keywordId(ref string __result, BattleUnitModel ____owner)
        {
            if (____owner != null && ____owner.Book != null && ____owner.Book.GetBookClassInfoId() == Tools.MakeLorId(18300000))
                __result = "Resistance";
        }
        //Bremen
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.CheckCardAvailable))]
        [HarmonyPostfix]
        public static void BattleUnitModel_CheckCardAvailable(ref bool __result, BattleUnitModel __instance)
        {
            if (__result && BattleManagerUI.Instance.selectedAllyDice != null)
            {
                int index = BattleManagerUI.Instance.selectedAllyDice._speedDiceIndex;
                if (!__instance.speedDiceResult[index].isControlable && __instance.faction == Faction.Player)
                    __result = false;
            }
        }
        //Pluto
        [HarmonyPatch(typeof(DiceBehaviour), nameof(DiceBehaviour.Copy))]
        [HarmonyPostfix]
        public static void DiceBehaviour_Copy(DiceBehaviour __instance, DiceBehaviour __result)
        {
            if (CCInitializer.passive18900002_Makred.Contains(__instance))
                CCInitializer.passive18900002_Makred.Add(__result);
        }
        [HarmonyPatch(typeof(BattleDiceCard_BehaviourDescUI), nameof(BattleDiceCard_BehaviourDescUI.SetBehaviourInfo))]
        [HarmonyPostfix]
        public static void BattleDiceCard_BehaviourDescUI_SetBehaviourInfo(BattleDiceCard_BehaviourDescUI __instance, DiceBehaviour behaviour)
        {
            if (CCInitializer.passive18900002_Makred.Contains(behaviour))
            {
                __instance.txt_ability.text = __instance.txt_ability.text.Insert(0, TextUtil.TransformConditionKeyword(TextDataModel.GetText("marked_dice_desc")));
                Debug.Log(behaviour.Detail.ToString() + " " + behaviour.Min.ToString() + "-" + behaviour.Dice.ToString());
            }
        }
        //Roland P1
        [HarmonyPatch(typeof(BattleUnitInfoManagerUI), nameof(BattleUnitInfoManagerUI.DisplayDlg))]
        [HarmonyPrefix]
        static bool BattleUnitInfoManagerUI_DisplayDlg_Pre(ref string str, BattleUnitModel unit)
        {
            if (unit.passiveDetail.HasPassive<PassiveAbility_1700000>())
                str = "..............";
            return true;
        }
        [HarmonyPatch(typeof(UnitDataModel), nameof(UnitDataModel.ResetForBlackSilence))]
        [HarmonyPrefix]
        public static bool UnitDataModel_ResetFor_BlackSilence_Pre(UnitDataModel __instance, BookModel ____bookItem)
        {
            if (CCInitializer.IsRoland(__instance) && ____bookItem != null && (____bookItem.ClassInfo.id == Tools.MakeLorId(17000001) || ____bookItem.ClassInfo.id == Tools.MakeLorId(17000004)))
                return false;
            return true;
        }
        [HarmonyPatch(typeof(UnitDataModel), nameof(UnitDataModel.EquipBook))]
        [HarmonyPrefix]
        public static bool UnitDataModel_EquipBook_Pre(UnitDataModel __instance, BookModel newBook, ref BookModel ____bookItem, bool isEnemySetting = false, bool force = false)
        {
            if (force || newBook == null || (newBook.ClassInfo.id != Tools.MakeLorId(17000001) && newBook.ClassInfo.id != Tools.MakeLorId(17000004)) || newBook.owner != null || !CCInitializer.IsRoland(__instance))
                return true;
            BookModel bookItem = ____bookItem;
            ____bookItem = newBook;
            newBook.SetOwner(__instance);
            if (!isEnemySetting)
                __instance.ReEquipDeck();
            bookItem?.SetOwner(null);
            return false;
        }
        [HarmonyPatch(typeof(BattleUnitEmotionDetail), nameof(BattleUnitEmotionDetail.ApplyEmotionCard))]
        [HarmonyPrefix]
        public static bool BattleUnitEmotionDetail_ApplyEmotionCard_Pre(BattleUnitModel ____self, EmotionCardXmlInfo card)
        {
            if (____self.passiveDetail.HasPassive<PassiveAbility_1700000>() && !CCInitializer.IsRolandEmotion(card))
                return false;
            if (CCInitializer.IsRolandEmotion(card))
                return ____self.passiveDetail.HasPassive<PassiveAbility_1700051>();
            return true;
        }
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.IsTargetable))]
        [HarmonyPostfix]
        public static void BattleUnitModel_IsTargetable_Post(BattleUnitModel __instance, ref bool __result)
        {
            if (__instance.speedDiceResult != null && __instance.speedDiceCount <= 1 && !__instance.IsTargetable_theLast())
                __result = false;
        }
        [HarmonyPatch(typeof(BattlePlayingCardSlotDetail), nameof(BattlePlayingCardSlotDetail.AddCard))]
        [HarmonyPrefix]
        static bool BattlePlayingCardSlotDetail_AddCard_Pre(BattlePlayingCardSlotDetail __instance, BattleUnitModel target, ref int targetSlot, bool isEnemyAuto = false)
        {
            if (target != null && !target.IsTargetable_theLast() && targetSlot == target.speedDiceResult.Count - 1)
                targetSlot = RandomUtil.Range(0, target.speedDiceResult.Count - 2);
            return true;
        }
        //Roland P4
        static Color grey = new Color(0.75f, 0.75f, 0.75f);
        [HarmonyPatch(typeof(BattleDiceCardUI), nameof(BattleDiceCardUI.SetCard))]
        [HarmonyPostfix]
        static void BattleDiceCardUI_SetCard(BattleDiceCardUI __instance)
        {
            if (__instance.CardModel != null && __instance.CardModel.GetID().id >= 17000040 && __instance.CardModel.GetID().id <= 17000049 && __instance.CardModel.GetID().packageId == "ContingencyConract")
            {
                __instance.colorFrame = grey;
                __instance.colorLineardodge = grey;
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
        //Roalnd P5
        [HarmonyPatch(typeof(EmotionPassiveCardUI), nameof(EmotionPassiveCardUI.SetSprites))]
        [HarmonyPostfix]
        static void EmotionPassiveCardUI_SetSprites(EmotionPassiveCardUI __instance)
        {
            if (CCInitializer.IsRolandEmotion(__instance.Card))
            {
                __instance._hOverImg.color = new Color(0f, grey.g, grey.b);
                __instance._rootImageBg.color = new Color(0f, grey.g, grey.b, 0.25f);
                __instance._artwork.sprite = AssetBundleManagerRemake.Instance.LoadCardSprite(__instance.Card._artwork);
                __instance._flavorText.fontMaterial.SetColor("_UnderlayColor", grey);
                __instance._abilityDesc.fontMaterial.SetColor("_UnderlayColor", grey);
            }
        }
        [HarmonyPatch(typeof(Roland2_FarArea_SmokeArea), nameof(Roland2_FarArea_SmokeArea.OnEffectEnd))]
        [HarmonyPrefix]
        static bool Roland2_FarArea_SmokeArea_OnEffectEnd_Pre(Roland2_FarArea_SmokeArea __instance)
        {
            __instance.state = FarAreaEffect.EffectState.End;
            __instance._isDoneEffect = true;
            foreach (FarAreaEffect effect in __instance.effectList)
                try
                {
                    effect.OnEffectEnd();
                }
                catch
                {

                }
            UnityEngine.Object.Destroy(__instance.gameObject);
            return false;
        }
        //DBremen
        [HarmonyPatch(typeof(BattlePlayingCardSlotDetail), nameof(BattlePlayingCardSlotDetail.RecoverPlayPoint))]
        [HarmonyPrefix]
        static bool BattlePlayingCardSlotDetail_RecoverPlayPoint(BattlePlayingCardSlotDetail __instance)
        {
            return !__instance._self.passiveDetail.HasPassive<PassiveAbility_1940002>();
        }
        //DOswald
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.Die))]
        [HarmonyPostfix]
        static void BattleUnitModel_Die(BattleUnitModel __instance)
        {
            if (DiceCardSelfAbility_OswaldHide.HidingOswald != null && DiceCardSelfAbility_OswaldHide.HidingOswald.faction == __instance.faction)
                DiceCardSelfAbility_OswaldHide.HidingOswald.TakeBreakDamage(16);
        }
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnCheckEndBattle))]
        [HarmonyPostfix]
        static void BattleUnitModel_OnCheckEndBattle(ref bool librarianExists)
        {
            if (librarianExists == false && DiceCardSelfAbility_OswaldHide.HidingOswald != null)
                librarianExists = true;
        }
        [HarmonyPatch(typeof(BattlePersonalEgoCardDetail), nameof(BattlePersonalEgoCardDetail.UseCard))]
        [HarmonyPrefix]
        static bool BattlePersonalEgoCardDetail_UseCard(BattleDiceCardModel card)
        {
            if (card.GetID() == Tools.MakeLorId(19500102))
                return false;
            return true;
        }
        //DJaeheon Fix Can't Target Enemy UnControllable Dice
        [HarmonyPatch(typeof(SpeedDiceUI), nameof(SpeedDiceUI.OnClickSpeedDice))]
        [HarmonyPrefix]
        static bool SpeedDiceUI_OnClickSpeedDice(SpeedDiceUI __instance)
        {
            if (!__instance.view.model.speedDiceResult[__instance._speedDiceIndex].isControlable && __instance.view.model.faction == Faction.Enemy)
            {
                SingletonBehavior<BattleSoundManager>.Instance.PlaySound(EffectSoundType.UI_CLICK, __instance.transform.position);
                if (SingletonBehavior<BattleManagerUI>.Instance.ui_unitCardsInHand.IsCardSelected())
                {
                    BattleDiceCardUI selectedCard = SingletonBehavior<BattleManagerUI>.Instance.ui_unitCardsInHand.GetSelectedCard();
                    BattleUnitModel selectedModel = SingletonBehavior<BattleManagerUI>.Instance.ui_unitCardsInHand.SelectedModel;
                    int faction1 = (int)__instance._view.model.faction;
                    int speedDiceIndex = __instance._speedDiceIndex;
                    if (!BattleUnitModel.IsTargetableUnit(selectedCard.CardModel, selectedModel, __instance._view.model, speedDiceIndex) || __instance.CheckBlockDice())
                        return false;
                    BattlePlayingCardDataInUnitModel cardDataInUnitModel = __instance._view.model.cardSlotDetail.cardAry[speedDiceIndex];
                    SingletonBehavior<BattleManagerUI>.Instance.ui_unitCardsInHand.ApplySelectedCard(__instance._view.model, speedDiceIndex);
                    BattleUIInputController.Instance.ResetCharacterCursor(false);
                    __instance.playerinfo.ReleaseSelectedCard();
                    if (!SingletonBehavior<BattleTutorialManagerUI>.Instance.IsRunningTutorial || SingletonBehavior<BattleTutorialManagerUI>.Instance.selectEnemySpeedDiceFuncForTutorial == null)
                        return false;
                    SingletonBehavior<BattleTutorialManagerUI>.Instance.selectEnemySpeedDiceFuncForTutorial();
                }
                else
                {
                    SingletonBehavior<BattleSoundManager>.Instance.PlaySound(EffectSoundType.UI_CLICK, __instance.transform.position);
                    if ((UnityEngine.Object)SingletonBehavior<BattleManagerUI>.Instance.selectedEnemyDice != (UnityEngine.Object)__instance)
                    {
                        SingletonBehavior<BattleManagerUI>.Instance.selectedEnemyDice = __instance;
                        __instance.SetHighlightClicked();
                        __instance.enemyinfo.ReleaseSelectedCharacter();
                        __instance.enemyinfo.OpenUnitInformationByDice(__instance._view.model, true, __instance._speedDiceIndex);
                    }
                    else
                    {
                        SingletonBehavior<BattleManagerUI>.Instance.selectedEnemyDice = (SpeedDiceUI)null;
                        __instance.SetHighlight(false);
                    }
                }
                return false;
            }
            return true;
        }
    }
}
