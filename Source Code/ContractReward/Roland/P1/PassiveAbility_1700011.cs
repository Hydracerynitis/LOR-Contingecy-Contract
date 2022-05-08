using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700011 : PassiveAbility_10012
    {
        private int _count;

        public override void OnWaveStart()
        {
            this.owner.allyCardDetail.DrawCards(2);
            base.OnWaveStart();
        }

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (this._count == 2)
            {
                curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                {
                    power = 2
                });
                this._count = 0;
            }
            else
                ++this._count;
            this.owner.bufListDetail.RemoveBufAll(typeof(PassiveAbility_10013.BattleUnitBuf_blackSilenceCardCount));
            if (this._count <= 0)
                return;
            BattleUnitBufListDetail bufListDetail = this.owner.bufListDetail;
            PassiveAbility_10013.BattleUnitBuf_blackSilenceCardCount buf = new PassiveAbility_10013.BattleUnitBuf_blackSilenceCardCount();
            buf.stack = _count;
            bufListDetail.AddBuf(buf);
        }
        
    }
}
