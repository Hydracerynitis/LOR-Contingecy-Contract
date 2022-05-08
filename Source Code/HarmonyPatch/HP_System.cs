using System;
using System.Collections.Generic;
using System.Linq;
using Mod;
using HarmonyLib;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    class HP_System
    {
        [HarmonyPatch(typeof(EntryScene),nameof(EntryScene.CheckModError))]
        [HarmonyPrefix]
        static void EntryScene_CheckModError_Pre()
        {
            ModContentManager.Instance._logs.RemoveAll(x => x.Contains("energy3") || x.Contains("drawCards3"));
        }
    }
}
