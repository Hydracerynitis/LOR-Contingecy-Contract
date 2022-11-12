using System;
using System.Collections.Generic;
using Sound;
using Contingecy_Contract;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;
using BaseMod;
using static UnityEngine.GraphicsBuffer;

namespace ContractReward
{
    public class DiceCardSelfAbility_OswaldJaeheonCombo : DiceCardSelfAbilityBase
    {
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            BattleUnitModel puppeteer = BattleObjectManager.instance.GetAliveList(owner.faction).Find(x => x.Book.ClassInfo.id == Tools.MakeLorId(19700000));
            BattleUnitModel bunny = BattleObjectManager.instance.GetAliveList(owner.faction).Find(x => x.Book.ClassInfo.id == Tools.MakeLorId(19510000));
            return puppeteer != null && puppeteer.IsActionable() && puppeteer.IsControlable() && GetUnUsedDice(puppeteer) != -1;
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            BattleUnitModel puppeteer = BattleObjectManager.instance.GetAliveList(owner.faction).Find(x => x.Book.ClassInfo.id == Tools.MakeLorId(19700000));
            if (puppeteer == null || !puppeteer.IsActionable() || !puppeteer.IsControlable())
                return;
            int index = GetUnUsedDice(puppeteer);
            if (index == -1)
                return;
            puppeteer.speedDiceResult[index].isControlable = false; 
            BattleDiceCardModel card = puppeteer.allyCardDetail.AddTempCard(Tools.MakeLorId(19500103));
            if (targetUnit != null)
            {
                int targetSlot = UnityEngine.Random.Range(0, targetUnit.speedDiceResult.Count);
                puppeteer.cardOrder = index;
                puppeteer.cardSlotDetail.AddCard(card, targetUnit, targetSlot);
            }
            base.OnUseInstance(unit, self, targetUnit);
        }
        private int GetUnUsedDice(BattleUnitModel puppeteer)
        {
            int result = -1;
            if (puppeteer.IsBreakLifeZero())
                return result;
            for(int i=0; i<puppeteer.speedDiceResult.Count; i++)
            {
                if (puppeteer.speedDiceResult[i].isControlable && !puppeteer.speedDiceResult[i].breaked && puppeteer.cardSlotDetail.cardAry[i] == null)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }
    }
}
