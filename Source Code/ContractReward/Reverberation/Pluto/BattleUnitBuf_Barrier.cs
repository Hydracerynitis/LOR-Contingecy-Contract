using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class BattleUnitBuf_Barrier: BattleUnitBuf
    {
        private GameObject _auraEffect;
        public override string keywordId => "CC_Barrier";
        public override string keywordIconId => "Pluto_Barrier";
        public override bool IsActionable() => false;
        public override bool IsTargetable() => false;
        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            this.stack = 0;
            UnityEngine.Object original = Resources.Load("Prefabs/Battle/DiceAttackEffects/New/FX/Mon/Pluto/FX_Mon_Pluto_Lock");
            if (original == null)
                return;
            GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
            if (gameObject == null)
                return;
            if (_auraEffect != null)
                UnityEngine.Object.Destroy(_auraEffect);
            this._auraEffect = gameObject;
            Pluto1st_BarrierAura component = gameObject.GetComponent<Pluto1st_BarrierAura>();
            if (component == null)
                return;
            component.Init(owner.view);
            owner.view.speedDiceSetterUI.DeselectAll();
            owner.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
            owner.view.speedDiceSetterUI.BlockDiceAll(true);
            owner.view.speedDiceSetterUI.BreakDiceAll(true);
            List<BattleUnitModel> actionableEnemyList = BattleObjectManager.instance.GetAliveList_opponent(owner.faction).FindAll(x => x.passiveDetail.IsActionable() && x.bufListDetail.IsActionable());
            for (int index1 = 0; index1 < actionableEnemyList.Count; ++index1)
            {
                BattleUnitModel actor = actionableEnemyList[index1];
                if (actor.turnState != BattleUnitTurnState.BREAK)
                    actor.turnState = BattleUnitTurnState.WAIT_CARD;
                try
                {
                    for (int index2 = 0; index2 < actor.speedDiceResult.Count; ++index2)
                    {
                        if (!actor.speedDiceResult[index2].breaked && index2 < actor.cardSlotDetail.cardAry.Count)
                        {
                            BattlePlayingCardDataInUnitModel cardDataInUnitModel = actor.cardSlotDetail.cardAry[index2];
                            if (cardDataInUnitModel != null && cardDataInUnitModel.card != null)
                            {
                                if (cardDataInUnitModel.card.GetSpec().Ranged == CardRange.FarArea || cardDataInUnitModel.card.GetSpec().Ranged == CardRange.FarAreaEach)
                                {
                                    if (cardDataInUnitModel.subTargets.Exists(x => x.target == owner))
                                        cardDataInUnitModel.subTargets.RemoveAll(x => x.target == owner);
                                    else if (cardDataInUnitModel.target == owner)
                                    {
                                        if (cardDataInUnitModel.subTargets.Count > 0)
                                        {
                                            BattlePlayingCardDataInUnitModel.SubTarget subTarget = RandomUtil.SelectOne(cardDataInUnitModel.subTargets);
                                            cardDataInUnitModel.target = subTarget.target;
                                            cardDataInUnitModel.targetSlotOrder = subTarget.targetSlotOrder;
                                            cardDataInUnitModel.earlyTarget = subTarget.target;
                                            cardDataInUnitModel.earlyTargetOrder = subTarget.targetSlotOrder;
                                        }
                                        else
                                        {
                                            actor.allyCardDetail.ReturnCardToHand(actor.cardSlotDetail.cardAry[index2].card);
                                            actor.cardSlotDetail.cardAry[index2] = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (cardDataInUnitModel.subTargets.Exists(x => x.target == owner))
                                        cardDataInUnitModel.subTargets.RemoveAll(x => x.target == owner);
                                    if (cardDataInUnitModel.target == owner)
                                    {
                                        BattleUnitModel targetByCard = BattleObjectManager.instance.GetTargetByCard(actor, cardDataInUnitModel.card, index2, actor.TeamKill());
                                        if (targetByCard != null)
                                        {
                                            int targetSlot = UnityEngine.Random.Range(0, targetByCard.speedDiceResult.Count);
                                            int num = actor.ChangeTargetSlot(cardDataInUnitModel.card, targetByCard, index2, targetSlot, actor.TeamKill());
                                            cardDataInUnitModel.target = targetByCard;
                                            cardDataInUnitModel.targetSlotOrder = num;
                                            cardDataInUnitModel.earlyTarget = targetByCard;
                                            cardDataInUnitModel.earlyTargetOrder = num;
                                        }
                                        else
                                        {
                                            actor.allyCardDetail.ReturnCardToHand(actor.cardSlotDetail.cardAry[index2].card);
                                            actor.cardSlotDetail.cardAry[index2] = null;
                                        }
                                    }
                                    else if (cardDataInUnitModel.earlyTarget == owner)
                                    {
                                        BattleUnitModel targetByCard = BattleObjectManager.instance.GetTargetByCard(actor, cardDataInUnitModel.card, index2, actor.TeamKill());
                                        if (targetByCard != null)
                                        {
                                            int targetSlot = UnityEngine.Random.Range(0, targetByCard.speedDiceResult.Count);
                                            int num = actor.ChangeTargetSlot(cardDataInUnitModel.card, targetByCard, index2, targetSlot, actor.TeamKill());
                                            cardDataInUnitModel.earlyTarget = targetByCard;
                                            cardDataInUnitModel.earlyTargetOrder = num;
                                        }
                                        else
                                        {
                                            cardDataInUnitModel.earlyTarget = cardDataInUnitModel.target;
                                            cardDataInUnitModel.earlyTargetOrder = cardDataInUnitModel.targetSlotOrder;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Contingecy_Contract.Debug.Error("target change error",ex);
                }
            }
            SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.UpdateTargetList();
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEnd();
            this.Destroy();
        }
        public override void OnDie()
        {
            base.OnDie();
            if (_owner?.view == null)
                return;
            this._owner.view.deadEvent += new BattleUnitView.DeadEvent(this.OnDeadEvent);
        }

        private void OnDeadEvent(BattleUnitView view)
        {
            if (_auraEffect == null)
                return;
            UnityEngine.Object.Destroy(_auraEffect);
        }

        public override void Destroy()
        {
            base.Destroy();
            if (_auraEffect == null)
                return;
            UnityEngine.Object.Destroy(_auraEffect);
        }
    }
}
