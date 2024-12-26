using BaseMod;
using UnityEngine;
using System.Collections.Generic;
using Contingecy_Contract;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700033 : PassiveAbilityBase
    {
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1700033));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1700033));
            this.rare = Rarity.Unique;
        }
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            base.OnDieOtherUnit(unit);
            PassiveAbility_1700031 Synchroniz = owner.passiveDetail.FindPassive<PassiveAbility_1700031>();
            if (Synchroniz == null)
                return;
            if (unit== Synchroniz.Angelica)
            {
                owner.allyCardDetail = Synchroniz.solo.Deck;
                owner.personalEgoDetail = Synchroniz.solo.EGO;
                owner.passiveDetail = Synchroniz.solo.Passive;
                owner.bufListDetail.AddBuf(new Enrage());
                owner.breakDetail.LoseBreakLife();
                owner.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_SoulLink));
            }
        }
        class Enrage: BattleUnitBuf
        {
            private GameObject aura;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                GameObject gameObject = Object.Instantiate<GameObject>(StaticDataManager.VanilaGameObject["BlackSilence4Aura"]);
                gameObject.transform.SetParent(owner.view.gameObject.transform);
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localScale = Vector3.one;
                gameObject.transform.localRotation = Quaternion.identity;
                gameObject.SetActive(true);
                aura = gameObject;
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = 100, breakRate = 100 });
            }
            public override void Destroy()
            {
                base.Destroy();
                DestroyAura();
            }
            public override void OnDie()
            {
                base.OnDie();
                DestroyAura();
            }
            public void DestroyAura()
            {
                if(aura != null)
                {
                    Object.Destroy(this.aura);
                    this.aura = null;
                }

            }
        }
    }
}
