using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class DiceCardSelfAbility_Pluto_UC_Protect_New : DiceCardSelfAbilityBase
    {
        public override void OnUseCard() => card?.target?.bufListDetail.AddBuf(new BattleUnitBuf_AddCard_UC_Protect());

        public class BattleUnitBuf_AddCard_UC_Protect : BattleUnitBuf
        {
            public override void OnRoundStart()
            {
                if (!_owner.allyCardDetail.GetAllDeck().Exists(x => x.GetID() == 707911))
                {
                    _owner.allyCardDetail.AddNewCard(707911).AddCost(2);
                    _owner.bufListDetail.AddBuf(new BattleUnitBuf_UnfairProtect());
                }
                Destroy();
            }
        }
    }
    public class DiceCardSelfAbility_Pluto_UC_Atk_New : DiceCardSelfAbilityBase
    {
        public override void OnUseCard() => card?.target?.bufListDetail.AddBuf(new BattleUnitBuf_AddCard_UC_Atk());

        public class BattleUnitBuf_AddCard_UC_Atk : BattleUnitBuf
        {
            public override void OnRoundStart()
            {
                if (!_owner.allyCardDetail.GetAllDeck().Exists(x => x.GetID() == 707912))
                {
                    _owner.allyCardDetail.AddNewCard(707912).AddCost(2);
                    _owner.bufListDetail.AddBuf(new BattleUnitBuf_UnfairAtk());
                }
                Destroy();
            }
        }
    }
    public class DiceCardSelfAbility_Pluto_UC_Light_New : DiceCardSelfAbilityBase
    {
        public override void OnUseCard() => card?.target?.bufListDetail.AddBuf(new BattleUnitBuf_AddCard_UC_Light());

        public class BattleUnitBuf_AddCard_UC_Light : BattleUnitBuf
        {
            public override void OnRoundStart()
            {
                if (!_owner.allyCardDetail.GetAllDeck().Exists(x => x.GetID() == 707913))
                {
                    _owner.allyCardDetail.AddNewCard(707913).AddCost(2);
                    _owner.bufListDetail.AddBuf(new BattleUnitBuf_UnfairLight());
                }
                Destroy();
            }
        }
    }
}
