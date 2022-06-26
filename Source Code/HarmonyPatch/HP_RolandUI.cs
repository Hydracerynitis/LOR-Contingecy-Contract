using System;
using UI;
using UnityEngine;
using HarmonyLib;
using TMPro;
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
            if (CCInitializer.IsRolandEmotion(__instance.Card))
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
