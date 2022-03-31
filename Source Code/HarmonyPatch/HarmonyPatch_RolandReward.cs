using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;

namespace Contingecy_Contract
{
    [HarmonyPatch]
    class HarmonyPatch_RolandReward
    {
        public static bool UnitDataModel_EquipBook_Pre(UnitDataModel __instance, BookModel newBook, ref BookModel ____bookItem, bool isEnemySetting = false, bool force = false)
        {
            if (force || newBook == null || newBook.ClassInfo.id != Tools.MakeLorId(17000001) || newBook.owner != null || !IsRoland(__instance))
                return true;
            BookModel bookItem = ____bookItem;
            ____bookItem = newBook;
            newBook.SetOwner(__instance);
            if (!isEnemySetting)
                __instance.ReEquipDeck();
            bookItem?.SetOwner(null);
            return false;
        }
        public static bool BattleUnitEmotionDetail_ApplyEmotionCard_Pre(BattleUnitModel ____self, EmotionCardXmlInfo card)
        {
            if (____self.passiveDetail.HasPassive<PassiveAbility_1700000>() && card.Sephirah != SephirahType.None)
                return false;
            return true;
        }
    }
}
