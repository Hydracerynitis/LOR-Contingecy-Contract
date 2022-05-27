using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using LOR_DiceSystem;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    class HP_ReverberationUI
    {
        //Greta
        [HarmonyPatch(typeof(BattleUnitBuf_Resistance),nameof(BattleUnitBuf_Resistance.keywordId), MethodType.Getter)]
        [HarmonyPostfix]
        public static void BattleUnitBuf_Resistance_get_keywordId(ref string __result, BattleUnitModel ____owner)
        {
            if (____owner != null && ____owner.Book != null && ____owner.Book.GetBookClassInfoId() == Tools.MakeLorId(18300000))
                __result = "Resistance";
        }
        //Pluto
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
    }
}
