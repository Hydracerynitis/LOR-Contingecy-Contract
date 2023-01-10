using LOR_DiceSystem;
using Contingecy_Contract;
using System.Collections.Generic;
using HP = SummonLiberation.Harmony_Patch;
using System.Text;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class BattleUnitBuf_RedString: BattleUnitBuf, StartBattleBuf
    {
        public override string keywordIconId => "Oswald_String";
        public override string keywordId => "RedString";
        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            stack = 2;
        }
        public override void OnRoundEnd()
        {
            if (_destroyed)
                return;
            stack--;
            if (stack <= 0)
            {
                List<BattleUnitModel> conjoined = new List<BattleUnitModel>();
                foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(_owner.faction))
                {
                    BattleUnitBuf_RedString redString = unit.bufListDetail.FindBuf<BattleUnitBuf_RedString>();
                    if (redString == null)
                        redString = unit.bufListDetail.FindBuf<BattleUnitBuf_RedString>(BufReadyType.NextRound);
                    if (redString != null)
                    {
                        conjoined.Add(unit);
                        redString.Destroy();
                    }
                }
                BattleUnitModel Bunny = HP.SummonUnit(_owner.faction == Faction.Enemy ? Faction.Player : Faction.Enemy, Tools.MakeLorId(19510000), Tools.MakeLorId(19510000), PlayerUnitName: CharactersNameXmlList.Instance.GetName(352));
                PassiveAbility_1950101 amalgamation = Bunny.passiveDetail.FindPassive<PassiveAbility_1950101>();
                if (amalgamation != null && conjoined.Count>=2)
                    amalgamation.Conjoin(conjoined);
                Destroy();
            }
            base.OnRoundEnd();
        }
        public void OnStartBattle()
        {
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(_owner.Book.ClassInfo.RangeType==EquipRangeType.Range? 19500202: 19500201));
            List<BattleDiceBehavior> behaviourList = new List<BattleDiceBehavior>();
            int num = 0;
            foreach (DiceBehaviour diceBehaviour2 in cardItem.DiceBehaviourList)
            {
                BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
                battleDiceBehavior.behaviourInCard = diceBehaviour2.Copy();
                battleDiceBehavior.SetIndex(num++);
                battleDiceBehavior.AddAbility(new DiceCardAbility_RemoveRedString());
                behaviourList.Add(battleDiceBehavior);
            }
            _owner.cardSlotDetail.keepCard.AddBehaviours(cardItem, behaviourList);
        }
    }
}
