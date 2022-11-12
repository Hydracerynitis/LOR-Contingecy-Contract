using LOR_DiceSystem;
using Contingecy_Contract;
using System.Collections.Generic;
using HP = SummonLiberation.Harmony_Patch;
using System.Text;
using System.Threading.Tasks;
using BaseMod;
using System;
using UnityEngine;

namespace ContractReward
{
    public class BattleUnitBuf_JaeheonControl: BattleUnitBuf
    {
        public override string keywordIconId => "Jaeheon_String";
        public override string keywordId => "JaeheonControl";
        public override void OnAfterRollSpeedDice()
        {
            base.OnAfterRollSpeedDice();
            int count = this._owner.speedDiceResult.Count;
            int num1 = Mathf.Min(this.stack, count);
            for (int index1 = 0; index1 < num1; ++index1)
            {
                int num2 = -1;
                int num3 = -1;
                for (int index2 = 0; index2 < count; ++index2)
                {
                    if (this._owner.speedDiceResult[index2].value >= 1 && this._owner.speedDiceResult[index2].isControlable)
                    {
                        if (num3 < 0)
                        {
                            num2 = index2;
                            num3 = this._owner.speedDiceResult[index2].value;
                        }
                        else if (this._owner.speedDiceResult[index2].value < num3)
                        {
                            num2 = index2;
                            num3 = this._owner.speedDiceResult[index2].value;
                        }
                    }
                }
                if (num2 >= 0)
                {
                    this._owner.SetCurrentOrder(num2);
                    this._owner.speedDiceResult[num2].isControlable = false;
                    int cardId = 707710;
                    if ((double)RandomUtil.valueForProb < 0.5)
                        cardId = 707711;
                    BattleDiceCardModel card = this._owner.allyCardDetail.AddTempCard(cardId);
                    BattleUnitModel target = this.GetTarget();
                    if (target != null)
                    {
                        int targetSlot = UnityEngine.Random.Range(0, target.speedDiceResult.Count);
                        this._owner.cardSlotDetail.AddCard(card, target, targetSlot);
                    }
                }
                else
                    break;
            }
            SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.UpdateTargetList();
        }

        private BattleUnitModel GetTarget()
        {
            BattleUnitModel target = (BattleUnitModel)null;
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this._owner.faction);
            aliveList.Remove(this._owner);
            aliveList.RemoveAll((Predicate<BattleUnitModel>)(x => !x.IsTargetable(this._owner)));
            if (aliveList.Count <= 0)
            {
                aliveList = BattleObjectManager.instance.GetAliveList();
                aliveList.Remove(this._owner);
                aliveList.RemoveAll((Predicate<BattleUnitModel>)(x => !x.IsTargetable(this._owner)));
            }
            if (aliveList.Count > 0)
                target = RandomUtil.SelectOne<BattleUnitModel>(aliveList);
            return target;
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            --stack;
            if (this.stack > 0)
                return;
            this.Destroy();
        }
    }
}
