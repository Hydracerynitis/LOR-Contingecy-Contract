using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Text;
using Sound;
using LOR_DiceSystem;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class PassiveAbility_1810002: PassiveAbilityBase
    {
        private BattleAllyCardDetail Hot;
        private BattleAllyCardDetail Cold;
        public static SoundEffectPlayer _loopSound;
        private PhilipPhase phase;
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            phase = PhilipPhase.Cold;
            Cold = this.owner.allyCardDetail;
            Hot = new BattleAllyCardDetail(owner);
            Hot.Init(owner.UnitData.unitData.GetDeckForBattle(1));
            if(_loopSound==null)
                _loopSound = SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Philip_StrongLoop", true);
        }
        public override void OnBattleEnd()
        {
            base.OnBattleEnd();
            if (_loopSound == null)
                return;
            _loopSound.source.Stop();
            _loopSound = null;
        }
        public override void OnRoundStartAfter()
        {
            base.OnRoundStartAfter();
            if(phase==PhilipPhase.Hot)
                SoundEffectPlayer.PlaySound("Battle/Philip_FilterOn");
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            BattleUnitBuf activatedBuf = this.owner.bufListDetail.GetActivatedBuf(KeywordBuf.Burn);
            if (activatedBuf == null || activatedBuf.stack < 10)
            {
                if (phase == PhilipPhase.Hot)
                {
                    Hot = this.owner.allyCardDetail;
                    this.owner.allyCardDetail = Cold;
                    this.owner.allyCardDetail.DrawCards(4);
                    this.owner.view.ChangeSkin("Blue_Philip");
                    this.owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
                    List<PassiveAbilityBase> passive = this.owner.passiveDetail.PassiveList;
                    PassiveAbility_1810004 hot = passive.Find(x => x is PassiveAbility_1810004) as PassiveAbility_1810004;
                    hot.Destroy();
                    PassiveAbilityBase cold = new PassiveAbility_1810003(this.owner);
                    passive.Remove(hot);
                    passive.Add(cold);
                    owner.passiveDetail._passiveList = passive;
                    this.phase = PhilipPhase.Cold;
                    if(this.owner.cardSlotDetail.PlayPoint<4)
                        this.owner.cardSlotDetail.RecoverPlayPoint(4- this.owner.cardSlotDetail.PlayPoint);
                }
                return;
            }
            if (phase == PhilipPhase.Cold)
            {
                Cold = this.owner.allyCardDetail;
                this.owner.allyCardDetail = Hot;
                this.owner.allyCardDetail.DrawCards(4);
                SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.SetRunningState(true);
                SoundEffectPlayer.PlaySound("Battle/Cry_MapChange_One");
                new UnityEngine.GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/PhilipFilter", false, 2f);
                this.owner.view.ChangeSkin("Blue_Philip_Burn");
                this.owner.view.charAppearance.ChangeMotion(ActionDetail.S5);
                this.owner.view.StartCoroutine(this.Transformation());
                List<PassiveAbilityBase> passive = this.owner.passiveDetail.PassiveList;
                PassiveAbilityBase cold = passive.Find(x => x is PassiveAbility_1810003);
                PassiveAbilityBase hot = new PassiveAbility_1810004(this.owner);
                passive.Remove(cold);
                passive.Add(hot);
                owner.passiveDetail._passiveList = passive;
                this.phase = PhilipPhase.Hot;
                if (this.owner.cardSlotDetail.PlayPoint < 4)
                    this.owner.cardSlotDetail.RecoverPlayPoint(4 - this.owner.cardSlotDetail.PlayPoint);
            }

        }
        public override int OnGiveKeywordBufByCard(BattleUnitBuf buf, int stack, BattleUnitModel target)
        {
            if (buf.bufType != KeywordBuf.Burn)
                return 0;
            this.owner.battleCardResultLog?.SetPassiveAbility(this);
            return 1;
        }
        private enum PhilipPhase
        {
            Hot,
            Cold
        }

        public IEnumerator<object> Transformation()
        {
            yield return (object)YieldCache.WaitForSeconds(0.65f);
            owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
            SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("Philip/Philip_Aura_Start", 1f, owner.view, owner.view, 1f);
            SoundEffectPlayer.PlaySound("Battle/Xiao_Vert");
            yield return (object)YieldCache.WaitForSeconds(0.8f);
            SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.SetRunningState(false);
        }
    }
}
