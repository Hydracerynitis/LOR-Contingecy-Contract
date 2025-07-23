using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1710001 : PassiveAbilityBase
    {
        private int hexagramCard = -1;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if(hexagramCard != -1)
            {
                owner.allyCardDetail.AddNewCard(Tools.MakeLorId(hexagramCard));
                if(owner.passiveDetail.HasPassive<PassiveAbility_1710002>())
                    RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList(owner.faction).FindAll(x => x!=owner)).allyCardDetail.AddNewCard(Tools.MakeLorId(hexagramCard));
                hexagramCard = -1;
            }
        }
        public override void OnRoundEnd_before()
        {
            if (owner.bufListDetail.FindBuf<TooltipBuf>() is TooltipBuf TTB)
            {
                if(owner.bufListDetail.FindBuf<BattleUnitBuf_hanaBufCommon>() is BattleUnitBuf_hanaBufCommon HBC)
                    hexagramCard = TTB.UseHana(HBC);
                return;
            }
            if (owner.bufListDetail.HasBuf<BattleUnitBuf_hana1>())
                owner.bufListDetail.AddReadyBuf(new TooltipQian());
            if (owner.bufListDetail.HasBuf<BattleUnitBuf_hana2>())
                owner.bufListDetail.AddReadyBuf(new TooltipKun());
            if (owner.bufListDetail.HasBuf<BattleUnitBuf_hana3>())
                owner.bufListDetail.AddReadyBuf(new TooltipKan());
            if (owner.bufListDetail.HasBuf<BattleUnitBuf_hana4>())
                owner.bufListDetail.AddReadyBuf(new TooltipLi());
        }
        abstract class TooltipBuf: BattleUnitBuf
        {
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public abstract int UseHana(BattleUnitBuf_hanaBufCommon buf);
        }
        class TooltipQian : TooltipBuf
        {
            public override string keywordIconId => "hanaQian";
            public override string keywordId => "Tooltip_Qian";
            public override int UseHana(BattleUnitBuf_hanaBufCommon buf)
            {
                int output = -1;
                if (buf is BattleUnitBuf_hana2)
                    output = 17100101;
                if (buf is BattleUnitBuf_hana3)
                    output = 17100102;
                if (buf is BattleUnitBuf_hana4)
                    output = 17100103;
                if (output != -1)
                    Destroy();
                 return output;
            }
        }
        class TooltipKun : TooltipBuf
        {
            public override string keywordIconId => "hanaKun";
            public override string keywordId => "Tooltip_Kun";
            public override int UseHana(BattleUnitBuf_hanaBufCommon buf)
            {
                int output = -1;
                if (buf is BattleUnitBuf_hana1)
                    output = 17100104;
                if (buf is BattleUnitBuf_hana3)
                    output = 17100105;
                if (buf is BattleUnitBuf_hana4)
                    output = 17100106;
                if (output != -1)
                    Destroy();
                return output;
            }
        }
        class TooltipKan: TooltipBuf
        {
            public override string keywordIconId => "hanaKan";
            public override string keywordId => "Tooltip_Kan";
            public override int UseHana(BattleUnitBuf_hanaBufCommon buf)
            {
                int output = -1;
                if (buf is BattleUnitBuf_hana1)
                    output = 17100107;
                if (buf is BattleUnitBuf_hana2)
                    output = 17100108;
                if (buf is BattleUnitBuf_hana4)
                    output = 17100109;
                if (output != -1)
                    Destroy();
                return output;
            }
        }
        class TooltipLi : TooltipBuf
        {
            public override string keywordIconId => "hanaLi";
            public override string keywordId => "Tooltip_Li";
            public override int UseHana(BattleUnitBuf_hanaBufCommon buf)
            {
                int output = -1;
                if (buf is BattleUnitBuf_hana1)
                    output = 17100110;
                if (buf is BattleUnitBuf_hana2)
                    output = 17100111;
                if (buf is BattleUnitBuf_hana3)
                    output = 17100112;
                if (output != -1)
                    Destroy();
                return output;
            }
        }
    }
    
}
