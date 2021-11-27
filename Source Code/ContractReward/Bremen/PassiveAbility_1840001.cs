using System;
using Sound;
using System.Collections.Generic;
using SummonLiberation;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class PassiveAbility_1840001 : PassiveAbilityBase
    {
        private List<Head> AvailableHead = new List<Head>() { Head.Donkey, Head.Chicken, Head.Dog};
        private Head currentHead = Head.Chicken;
        public override int SpeedDiceNumAdder() => 1;
        public override bool AllowTargetChanging(BattleUnitModel attacker, int myCardSlotIdx) => myCardSlotIdx != owner.speedDiceCount - 1;
        public override void OnAfterRollSpeedDice()
        {
            int index = owner.speedDiceCount-1;
            owner.SetCurrentOrder(index);
            owner.speedDiceResult[index].isControlable = false;
            do
                currentHead = GetHead(currentHead);
            while (!AvailableHead.Contains(currentHead));
            int cardId = GetCard(out BattleUnitModel target);
            if (cardId == -1)
                return;
            LorId newId = Tools.MakeLorId(cardId);
            BattleDiceCardModel card = owner.allyCardDetail.AddTempCard(newId);
            if (target != null)
            {
                int targetSlot = UnityEngine.Random.Range(0, target.speedDiceResult.Count);
                owner.cardSlotDetail.AddCard(card, target, targetSlot);
            }
            SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(owner, owner.faction, owner.hp, owner.breakDetail.breakGauge);
            SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.UpdateTargetList();
        }
        private Head GetHead(Head head)
        {
            switch (head)
            {
                case Head.Donkey:
                    return Head.Dog;
                case Head.Chicken:
                    return Head.Donkey;
                case Head.Dog:
                    return Head.Chicken;
            }
            return head;
        }
        private int GetCard(out BattleUnitModel target)
        {
            target = RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList_opponent(owner.faction));
            switch (currentHead)
            {
                case Head.Donkey:
                    this.owner.bufListDetail.AddBuf(new HeadDonkey());
                    SoundEffectPlayer.PlaySound("Battle/Bremen_Horse");                 
                    return RandomUtil.SelectOne<int>(18400031, 18400032);
                case Head.Chicken:
                    this.owner.bufListDetail.AddBuf(new HeadChicken());
                    SoundEffectPlayer.PlaySound("Battle/Bremen_Chicken");
                    if(BattleObjectManager.instance.GetAliveList(owner.faction).Exists(x => x != owner) && RandomUtil.valueForProb < 0.5)
                    {
                        target= RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(owner.faction).Find(x=> x!=owner));
                        return 18400021;
                    }
                    else
                        return 18400022;
                case Head.Dog:
                    this.owner.bufListDetail.AddBuf(new HeadDog());
                    SoundEffectPlayer.PlaySound("Battle/Bremen_Dog");
                    return RandomUtil.SelectOne<int>(18400011, 18400012);
            }
            return -1;
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            if (this.owner.IsBreakLifeZero())
            {
                this.owner.breakDetail.RecoverBreakLife(this.owner.MaxBreakLife);
                this.owner.breakDetail.nextTurnBreak = false;
                this.owner.breakDetail.RecoverBreak(this.owner.breakDetail.GetDefaultBreakGauge());
                this.owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
                this.AvailableHead.Remove(this.currentHead);
                if (this.AvailableHead.Count <= 0)
                {
                    Util.LoadPrefab("Battle/CreatureEffect/Bremen/Bremen_Filter", SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    AvailableHead = new List<Head>() { Head.Donkey, Head.Chicken, Head.Dog };
                    owner.allyCardDetail.AddNewCard(Tools.MakeLorId(18400006));
                }
            }
        }
        public enum Head
        {
            Donkey,
            Chicken,
            Dog
        }
        private class HeadChicken: BattleUnitBuf_Bremen_Head
        {
            protected override string keywordIconId => "Bremen_Head_Chicken";
            protected override string keywordId => "HeadChicken";
        }
        private class HeadDonkey : BattleUnitBuf_Bremen_Head
        {
            protected override string keywordIconId => "Bremen_Head_Donkey";
            protected override string keywordId => "HeadDonkey";
        }
        private class HeadDog: BattleUnitBuf_Bremen_Head
        {
            protected override string keywordIconId => "Bremen_Head_Dog";
            protected override string keywordId => "HeadDog";
        }
    }
}
