using BaseMod;
using Contingecy_Contract;
using ContractReward;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace ContractReward
{
    public class DiceCardSelfAbility_HanaJin : HexagramAbility
    {
        public override HexagramCardBuf EnhanceCardBuf()
        {
            return new JinBuf();
        }
        class JinBuf: HexagramCardBuf, StartBattleInHandBuf, OnAddToHandBuf, StartBattleBuf
        {
            private bool quickDraw = true;
            public override string keywordId => "HanaJin";
            public JinBuf()
            {
                quickDraw = true;
                SetBufIconSprite();
            }
            private void SetBufIconSprite()
            {
                string keywordIconId = "CardBuf_晋Buf";
                if (quickDraw)
                {
                    keywordIconId = "CardBuf_晋BufActive";
                }
                if (Harmony_Patch.ArtWorks.TryGetValue(keywordIconId, out Sprite sprite) && sprite != null)
                {
                    _bufIcon = sprite;
                    _iconInit = true;
                }
            }

            public void OnAddToHand(BattleUnitModel unit)
            {
                quickDraw = true;
                SetBufIconSprite();
            }

            public void OnStartBattle_inHand(BattleUnitModel unit)
            {
                quickDraw = false;
                SetBufIconSprite();
            }

            public void OnStartBattle(BattleUnitModel unit)
            {
                if (quickDraw)
                {
                    DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(_card.GetID());
                    List<BattleDiceBehavior> behaviourList = new List<BattleDiceBehavior>();
                    int num = 0;
                    foreach (DiceBehaviour diceBehaviour2 in cardItem.DiceBehaviourList)
                    {
                        DiceBehaviour counterDice = diceBehaviour2.Copy();
                        counterDice.Type = BehaviourType.Standby;
                        BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
                        battleDiceBehavior.behaviourInCard = counterDice;
                        battleDiceBehavior.SetIndex(num++);
                        string script = counterDice.Script;
                        if (script != string.Empty)
                        {
                            DiceCardAbilityBase instanceDiceCardAbility = Singleton<AssemblyManager>.Instance.CreateInstance_DiceCardAbility(script);
                            if (instanceDiceCardAbility != null)
                                battleDiceBehavior.AddAbility(instanceDiceCardAbility);
                        }
                        behaviourList.Add(battleDiceBehavior);
                    }
                    unit.cardSlotDetail.keepCard.AddBehaviours(cardItem, behaviourList);
                }
            }
        }
    }
}
