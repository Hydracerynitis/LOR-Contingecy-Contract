using System;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class DiceCardSelfAbility_ElenaZombie: DiceCardSelfAbilityBase
    {
        public override bool IsValidTarget(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            return targetUnit.hp<targetUnit.MaxHp*0.25;
        }
        public override void OnUseCard()
        {
            base.OnUseCard();
            this.card.target.bufListDetail.AddBuf(new ZombieInit());
            this.card.card.exhaust = true;
        }
        public class ZombieInit: BattleUnitBuf
        {
            private bool init;
            public override bool IsControllable => this._owner.faction==Faction.Player? false: base.IsControllable;
            public override bool IsImmortal() => !init;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                init = false;
            }
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                if (!init)
                {
                    this._owner.RecoverHP(this._owner.MaxHp);
                    this._owner.RecoverBreakLife(1);
                    this._owner.ResetBreakGauge();
                    this._owner.breakDetail.nextTurnBreak = false;
                    List<PassiveAbilityBase> list = this._owner.passiveDetail.PassiveList;
                    list.Insert(0, new Zombie(this._owner));
                    typeof(BattleUnitPassiveDetail).GetField("_passiveList", AccessTools.all).SetValue((object)this._owner.passiveDetail, (object)list);
                    this._owner.UnitData.unitData.SetTempName(TextDataModel.GetText("Zombie_name"));
                    init = true;
                }
            }
        }
    }
    public class Zombie : PassiveAbilityBase
    {
        public Zombie(BattleUnitModel unit)
        {
            this.owner = unit;
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1880003));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1880003));
            this.rare = Rarity.Common;
        }
        public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int currentSlot)
        {
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction).FindAll(x=> !x.passiveDetail.HasPassive<Zombie>());
            if (aliveList.Count <= 0)
            {
                this.owner.Die();
                Singleton<StageController>.Instance.CheckEndBattle();
            }
            Contingecy_Contract.Debug.Log("Zomebie try to move");
            return RandomUtil.SelectOne<BattleUnitModel>(aliveList);
        }
    }
}
