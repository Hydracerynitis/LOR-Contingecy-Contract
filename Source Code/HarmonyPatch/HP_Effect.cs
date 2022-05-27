using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HarmonyLib;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    class HP_Effect
    {
        [HarmonyPatch(typeof(Roland2_FarArea_SmokeArea),nameof(Roland2_FarArea_SmokeArea.OnEffectEnd))]
        [HarmonyPrefix]
        static bool Roland2_FarArea_SmokeArea_OnEffectEnd_Pre(Roland2_FarArea_SmokeArea __instance)
        {
            __instance.state= FarAreaEffect.EffectState.End;
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
    }
}
