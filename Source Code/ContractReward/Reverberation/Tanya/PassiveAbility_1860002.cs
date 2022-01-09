using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1860002 : PassiveAbilityBase
    {
        private bool _activate=false;
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (_activate)
                return true;
            else if (this.owner.UnitData.floorBattleData.param1 == 0 && this.owner.hp - dmg <= 1)
            {
                this.owner.UnitData.floorBattleData.param1 = 1;
                _activate = true;
                return true;
            }           
            return base.BeforeTakeDamage(attacker, dmg);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_activate)
            {
                this.owner.RecoverBreakLife(1);
                this.owner.ResetBreakGauge();
                this.owner.turnState = BattleUnitTurnState.WAIT_CARD;
                this.owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
                this.owner.SetHp(this.owner.MaxHp);
                this.owner.bufListDetail.RemoveBufAll(BufPositiveType.Negative);
                _activate = false;
            }
            if (this.owner.UnitData.floorBattleData.param1==1)
            {
                if (this.owner.allyCardDetail.GetAllDeck().Find(x => x.GetID() == Tools.MakeLorId(18600006)) == null)
                    this.owner.allyCardDetail.AddNewCardToDeck(Tools.MakeLorId(18600006));
                this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2);
                this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 2);
            }
        }
    }
}
