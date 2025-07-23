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
    public class DiceCardSelfAbility_HanaWeiJi : HexagramAbility
    {
        public override HexagramCardBuf EnhanceCardBuf()
        {
            return new WeiJiBuf();
        }
        class WeiJiBuf: HexagramCardBuf, StartBattleInHandBuf, OnAddToHandBuf
        {
            private bool quickDraw;
            public override string keywordId => "HanaWeiJi";
            public WeiJiBuf()
            {
                quickDraw = true;
                SetBufIconSprite();
            }
            private void SetBufIconSprite()
            {
                string keywordIconId = "CardBuf_未济Buf";
                if (quickDraw)
                {
                    keywordIconId= "CardBuf_未济BufActive";
                }
                if (Harmony_Patch.ArtWorks.TryGetValue(keywordIconId, out Sprite sprite) && sprite!=null)
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
            public override void OnUseCard(BattleUnitModel owner, BattlePlayingCardDataInUnitModel playingCard)
            {
                base.OnUseCard(owner, playingCard);
                if(quickDraw)
                {
                    owner.bufListDetail.AddBuf(new ReturnToHand(_card));
                }
            }
            public class ReturnToHand: BattleUnitBuf
            {
                private BattleDiceCardModel card;
                public ReturnToHand(BattleDiceCardModel card)
                {
                    this.card = card;
                }

                public override void OnRoundStart()
                {
                    base.OnRoundStart();
                    _owner.allyCardDetail.DrawCardSpecified(x => x == card);
                    Destroy();
                }
            }
        }
    }
}
