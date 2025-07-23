using BaseMod;
using Contingecy_Contract;
using ContractReward;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ContractReward
{
    public class DiceCardSelfAbility_HanaDaYou : HexagramAbility
    {
        public override HexagramCardBuf EnhanceCardBuf()
        {
            return new DaYouBuf();
        }
        class DaYouBuf: HexagramCardBuf, StartBattleInHandBuf, OnAddToHandBuf
        {
            private bool quickDraw = true;
            public override string keywordId => "HanaDaYou";
            public DaYouBuf()
            {
                quickDraw = true;
                SetBufIconSprite();
            }
            private void SetBufIconSprite()
            {
                string keywordIconId = "CardBuf_大有Buf";
                if (quickDraw)
                {
                    keywordIconId = "CardBuf_大有BufActive";
                }
                if (Harmony_Patch.ArtWorks.TryGetValue(keywordIconId, out Sprite sprite) && sprite != null)
                {
                    _iconInit = true;
                    _bufIcon = sprite;
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
            public override void OnUseCard(BattleUnitModel owner, BattlePlayingCardDataInUnitModel playingCard)
            {
                base.OnUseCard(owner, playingCard);
                if(quickDraw)
                {
                    playingCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = 2 });
                }
            }
        }
    }
}
