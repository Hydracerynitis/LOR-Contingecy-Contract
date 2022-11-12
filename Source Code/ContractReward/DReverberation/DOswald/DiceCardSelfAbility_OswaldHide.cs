using System;
using System.Collections.Generic;
using Sound;
using Contingecy_Contract;
using HarmonyLib;
using System.Text;
using LOR_DiceSystem;
using System.Threading.Tasks;

namespace ContractReward
{
    public class DiceCardSelfAbility_OswaldHide : DiceCardSelfAbilityBase
    {
        public static BattleUnitModel HidingOswald;
        public static int HidingTime = 0;
        public static bool HasChecked = false;
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            HidingOswald = unit;
            HidingTime = 2;
            SoundEffectManager.Instance.PlayClip(StaticDataManager.VanilaAudio["ClownClip"]);
            foreach (BattleUnitModel other in BattleObjectManager.instance.GetAliveList())
            {
                for(int i=0; i<other.cardSlotDetail.cardAry.Count; i++)
                {
                    BattlePlayingCardDataInUnitModel card = other.cardSlotDetail.cardAry[i];
                    if (card == null)
                        continue;
                    if (card.target == unit)
                    {
                        if(card.subTargets.Exists(x => x.target == unit))
                        {
                            card.subTargets.RemoveAll(x => x.target == unit);
                            BattleManagerUI.Instance.ui_TargetArrow.UpdateTargetList();
                        }
                        else if (card.subTargets.Count > 0)
                        {
                            card.ChangeSubTargetToMainTarget();
                            BattleManagerUI.Instance.ui_TargetArrow.UpdateTargetList();
                        }                       
                        else
                        {
                            other.cardOrder = i;
                            other.cardSlotDetail.AddCard(null, null, 0);
                        }
                    }
                }
            }
            BattleObjectManager.instance.UnregisterUnit(unit);
        }
        public static void RecallOswald()
        {
            if (HidingOswald != null)
            {
                if (HidingOswald.IsBreakLifeZero())
                {
                    OswaldReappear();
                    return;
                }
                if (BattleObjectManager.instance.GetAliveList(HidingOswald.faction).Count <= 0)
                {
                    OswaldReappear();
                    return;
                }
                HidingTime-=1;
                if (HidingTime <= 0)
                    OswaldReappear();
            }
        }
        private static void OswaldReappear()
        {
            BattleObjectManager.instance.RegisterUnit(HidingOswald);
            HidingOswald.cardSlotDetail.RecoverPlayPoint(HidingOswald.cardSlotDetail.GetMaxPlayPoint());
            HidingOswald.allyCardDetail.DrawCards(6);
            HidingOswald.OnRoundStartOnlyUI();
            HidingOswald.cardSlotDetail.OnRoundStart();
            BattleManagerUI.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(HidingOswald, HidingOswald.faction, HidingOswald.hp, HidingOswald.breakDetail.breakGauge);
            if (HidingOswald.IsBreakLifeZero())
                HidingOswald.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
            HidingOswald.bufListDetail.AddBuf(new CoolDown());
            SoundEffectManager.Instance.PlayClip(StaticDataManager.VanilaAudio["ClownClip"]);
            HidingOswald = null;
        }
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            return owner.bufListDetail.GetActivatedBufList().Find((x => x is CoolDown)) == null;
        }
        public class CoolDown : BattleUnitBuf
        {
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 3;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                stack--;
                if (stack <= 0)
                    this.Destroy();
            }
        }
    }
}
