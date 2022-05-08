using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700041 : PassiveAbility_10013
    {
        public List<LorId> _usedCount = new List<LorId>();

        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            LorId id1 = curCard.card.GetID();
            if (id1.packageId != "ContingencyConract")
                return;
            int id2 = id1.id;
            if (this._usedCount.Contains(id1) || (id2 < 17000041 || id2 > 17000049))
                return;
            this._usedCount.Add(id1);
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            this.owner.personalEgoDetail.AddCard(Tools.MakeLorId(17000040));
        }

        public override void OnRoundStart()
        {
            foreach (BattleDiceCardModel battleDiceCardModel in this.owner.allyCardDetail.GetAllDeck())
                battleDiceCardModel.RemoveBuf<PassiveAbility_10012.BattleDiceCardBuf_blackSilenceEgoCount>();
            int count = this._usedCount.Count;
            foreach (BattleDiceCardModel battleDiceCardModel in this.owner.allyCardDetail.GetAllDeck())
            {
                LorId id1 = battleDiceCardModel.GetID();
                if (id1.packageId== "ContingencyConract")
                {
                    int id2 = id1.id;
                    if (!this._usedCount.Contains(id1) && (id2 >= 17000041 || id2 <= 17000049))
                        battleDiceCardModel.AddBuf(new PassiveAbility_10012.BattleDiceCardBuf_blackSilenceEgoCount());
                }
            }
            this.owner.bufListDetail.RemoveBufAll(typeof(PassiveAbility_10012.BattleUnitBuf_blackSilenceSpecialCount));
            BattleUnitBufListDetail bufListDetail = this.owner.bufListDetail;
            PassiveAbility_10012.BattleUnitBuf_blackSilenceSpecialCount buf = new PassiveAbility_10012.BattleUnitBuf_blackSilenceSpecialCount();
            buf.stack = this._usedCount.Count;
            bufListDetail.AddBuf(buf);
        }

        public class BattleDiceCardBuf_blackSilenceEgoCount : BattleDiceCardBuf
        {
            public override string keywordIconId => "BlackSilenceSpecialCard";
        }

        public class BattleUnitBuf_blackSilenceSpecialCount : BattleUnitBuf
        {
            public override string keywordId => "BlackSilenceSpecial";

            public override string keywordIconId => "BlackSilenceSpecialCard";
        }
    }
}
