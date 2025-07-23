using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.UI.CanvasScaler;

namespace ContractReward
{
    public class HexagramAbility : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Tooltip_Hexagram" };
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            BattleDiceCardModel targetCard = owner.cardSlotDetail.cardAry[owner.cardOrder]?.card;
            if (targetCard == null)
                return false;
            if (targetCard._bufList.Exists(x => x is HexagramCardBuf))
                return false;
            DiceCardSpec cardSpec = targetCard.GetSpec();
            return cardSpec.affection == CardAffection.One && (cardSpec.Ranged == CardRange.Near ||
                cardSpec.Ranged == CardRange.Far || cardSpec.Ranged == CardRange.Special) && !targetCard.XmlData.IsPersonal() && !targetCard.XmlData.IsEgo();
        }
        public virtual HexagramCardBuf EnhanceCardBuf()
        {
            return null;
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            BattleDiceCardModel targetCard = unit.cardSlotDetail.cardAry[unit.cardOrder]?.card;
            if (targetCard ==null)
                return;
            BattleDiceCardBuf hanabuf = EnhanceCardBuf();
            if (hanabuf == null) 
                return;
            targetCard.AddBuf(hanabuf);
            unit.view.speedDiceSetterUI.GetSpeedDiceByIndex(unit.cardOrder)?.DeselectCard();
        }
        public class HexagramCardBuf: BattleDiceCardBuf
        {

        }
    }
}
