using System;
using System.Collections.Generic;
using UI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;
using UnityEngine;
using System.Collections;
using Sound;
using LOR_DiceSystem;

namespace Fix
{
    public class PassiveAbility_170301_New : PassiveAbilityBase
    {
        private int _cardCount;
        private int _patternCount;
        private int _strCnt;
        private BehaviourDetail _currentBuf;
        private BlackSilence4thMapManager _map;
        private bool _transformed;
        private int _dmgReduction;

        private BlackSilence4thMapManager Map
        {
            get
            {
                if ((UnityEngine.Object)this._map == (UnityEngine.Object)null)
                    this._map = SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject as BlackSilence4thMapManager;
                return this._map;
            }
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            BattleUnitModel battleUnitModel1 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60108, 1);
            BattleUnitModel battleUnitModel2 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60118, 2);
            BattleUnitModel battleUnitModel3 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60128, 3);
            if (!self.IsBreakLifeZero())
            {
                self.view.charAppearance.RemoveAltMotion(ActionDetail.Default);
                self.view.charAppearance.RemoveAltMotion(ActionDetail.Standing);
                self.view.charAppearance.ChangeMotion(ActionDetail.Default);
            }
            battleUnitModel1.SetDeadSceneBlock(false);
            battleUnitModel1.view.EnableView(false);
            battleUnitModel2.SetDeadSceneBlock(false);
            battleUnitModel2.view.EnableView(false);
            battleUnitModel3.SetDeadSceneBlock(false);
            battleUnitModel3.view.EnableView(false);
        }
        public override float DmgFactor(int dmg, DamageType type = DamageType.ETC, KeywordBuf keyword = KeywordBuf.None) => type == DamageType.Buf ? 0.5f : base.DmgFactor(dmg, type, keyword);

        public override void OnLoseHp(int dmg)
        {
            base.OnLoseHp(dmg);
            if ((double)this.owner.hp >= (double)this.owner.MaxHp * 0.4)
                return;
            this.owner.SetHp(Mathf.RoundToInt((float)this.owner.MaxHp * 0.4f));
        }

        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            this._dmgReduction = 0;
            if (!this._transformed && (double)this.owner.hp - (double)dmg <= (double)this.owner.MaxHp * 0.4)
            {
                this._transformed = true;
                this._dmgReduction = (int)((double)this.owner.MaxHp * 0.4 - ((double)this.owner.hp - (double)dmg));
            }
            return base.BeforeTakeDamage(attacker, dmg);
        }

        public override int GetDamageReductionAll() => (double)this.owner.hp > (double)this.owner.MaxHp * 0.4 ? this._dmgReduction : 9999;

        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            if ((double)this.owner.hp <= (double)this.owner.MaxHp * 0.4)
            {
                SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.SetRunningState(true);
                this.owner.view.StartCoroutine(this.Transformation());
            }
            else
            {
                switch (this._patternCount)
                {
                    case 0:
                        BattleUnitModel battleUnitModel1 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60108, 1);
                        BattleUnitModel battleUnitModel2 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60118, 2);
                        BattleUnitModel battleUnitModel3 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60128, 3);
                        if (!this.owner.IsBreakLifeZero())
                        {
                            this.owner.view.charAppearance.RemoveAltMotion(ActionDetail.Default);
                            this.owner.view.charAppearance.RemoveAltMotion(ActionDetail.Standing);
                            this.owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
                        }
                        battleUnitModel1.SetDeadSceneBlock(false);
                        battleUnitModel1.view.EnableView(false);
                        battleUnitModel2.SetDeadSceneBlock(false);
                        battleUnitModel2.view.EnableView(false);
                        battleUnitModel3.SetDeadSceneBlock(false);
                        battleUnitModel3.view.EnableView(false);
                        break;
                    case 1:
                        BattleUnitModel battleUnitModel4 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60138, 1);
                        BattleUnitModel battleUnitModel5 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60148, 2);
                        BattleUnitModel battleUnitModel6 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60158, 3);
                        if (!this.owner.IsBreakLifeZero())
                        {
                            this.owner.view.charAppearance.RemoveAltMotion(ActionDetail.Default);
                            this.owner.view.charAppearance.RemoveAltMotion(ActionDetail.Standing);
                            this.owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
                        }
                        battleUnitModel4.SetDeadSceneBlock(false);
                        battleUnitModel4.view.EnableView(false);
                        battleUnitModel5.SetDeadSceneBlock(false);
                        battleUnitModel5.view.EnableView(false);
                        battleUnitModel6.SetDeadSceneBlock(false);
                        battleUnitModel6.view.EnableView(false);
                        break;
                    case 2:
                        BattleUnitModel battleUnitModel7 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60168, 1);
                        BattleUnitModel battleUnitModel8 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60178, 2);
                        BattleUnitModel battleUnitModel9 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60188, 3);
                        if (!this.owner.IsBreakLifeZero())
                        {
                            this.owner.view.charAppearance.RemoveAltMotion(ActionDetail.Default);
                            this.owner.view.charAppearance.RemoveAltMotion(ActionDetail.Standing);
                            this.owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
                        }
                        battleUnitModel7.SetDeadSceneBlock(false);
                        battleUnitModel7.view.EnableView(false);
                        battleUnitModel8.SetDeadSceneBlock(false);
                        battleUnitModel8.view.EnableView(false);
                        battleUnitModel9.SetDeadSceneBlock(false);
                        battleUnitModel9.view.EnableView(false);
                        break;
                    case 3:
                        if (!this.owner.IsBreakLifeZero())
                        {
                            this.owner.view.charAppearance.ChangeMotion(ActionDetail.S12);
                            break;
                        }
                        break;
                }
                int num = 0;
                foreach (BattleUnitModel battleUnitModel10 in BattleObjectManager.instance.GetList())
                    SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnitModel10.UnitData.unitData, num++, true);
                BattleObjectManager.instance.InitUI();
            }
        }

        public override void OnRoundStartAfter()
        {
            if (this.owner.IsBreakLifeZero())
                return;
            this.SetCards();
            switch (this._patternCount)
            {
                case 0:
                    this.owner.passiveDetail.AddPassive((PassiveAbilityBase)new PassiveAbility_170304());
                    this.owner.passiveDetail.OnCreated();
                    break;
                case 1:
                    ++this._strCnt;
                    this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, this._strCnt, this.owner);
                    this.owner.passiveDetail.AddPassive((PassiveAbilityBase)new PassiveAbility_170305());
                    this.owner.passiveDetail.OnCreated();
                    break;
                case 2:
                    this.owner.passiveDetail.AddPassive((PassiveAbilityBase)new PassiveAbility_170306());
                    this.owner.passiveDetail.OnCreated();
                    break;
                case 3:
                    this.owner.passiveDetail.AddPassive((PassiveAbilityBase)new PassiveAbility_170307());
                    this.owner.passiveDetail.OnCreated();
                    break;
            }
            ++this._patternCount;
            this._patternCount %= 4;
            switch (this._currentBuf)
            {
                case BehaviourDetail.Slash:
                    this.owner.bufListDetail.AddBuf(new BattleUnitBuf_Roland_4th_DmgReduction_Slash());
                    this._currentBuf = BehaviourDetail.Penetrate;
                    break;
                case BehaviourDetail.Penetrate:
                    this.owner.bufListDetail.AddBuf(new BattleUnitBuf_Roland_4th_DmgReduction_Penetrate());
                    this._currentBuf = BehaviourDetail.Hit;
                    break;
                case BehaviourDetail.Hit:
                    this.owner.bufListDetail.AddBuf(new BattleUnitBuf_Roland_4th_DmgReduction_Hit());
                    this._currentBuf = BehaviourDetail.Slash;
                    break;
            }
        }

        private void SetCards()
        {
            this.owner.allyCardDetail.ExhaustAllCards();
            int num = this.owner.Book.GetSpeedDiceRule(this.owner).Roll(this.owner).Count - 3;
            this._cardCount = 0;
            switch (this._patternCount)
            {
                case 0:
                    this.AddNewCard(702301);
                    this.AddNewCard(702302);
                    this.AddNewCard(702303);
                    this.AddNewCard(702315);
                    this.AddNewCard(702316);
                    break;
                case 1:
                    this.AddNewCard(702304);
                    this.AddNewCard(702305);
                    this.AddNewCard(702306);
                    this.AddNewCard(702315);
                    this.AddNewCard(702316);
                    break;
                case 2:
                    this.owner.view.charAppearance.SetAltMotion(ActionDetail.Default, ActionDetail.S14);
                    this.owner.view.charAppearance.SetAltMotion(ActionDetail.Standing, ActionDetail.S14);
                    this.owner.view.charAppearance.ChangeMotion(ActionDetail.S14);
                    this.AddNewCard(702307);
                    this.AddNewCard(702308);
                    this.AddNewCard(702309);
                    this.AddNewCard(702315);
                    this.AddNewCard(702316);
                    break;
                case 3:
                    this.owner.view.charAppearance.SetAltMotion(ActionDetail.Default, ActionDetail.S14);
                    this.owner.view.charAppearance.SetAltMotion(ActionDetail.Standing, ActionDetail.S14);
                    this.owner.view.charAppearance.ChangeMotion(ActionDetail.S14);
                    this.Map?.AttatchAura();
                    this.AddNewCard(702313);
                    this.AddNewCard(702310);
                    this.AddNewCard(702312);
                    this.AddNewCard(702312);
                    this.AddNewCard(702315);
                    break;
            }
            for (int index = 0; index < num; ++index)
                this.AddNewCard(this.GetAddedDiceCard());
        }

        public override BattleUnitModel ChangeAttackTarget(
          BattleDiceCardModel card,
          int idx)
        {
            BattleUnitModel battleUnitModel = base.ChangeAttackTarget(card, idx);
            List<BattleUnitModel> list = new List<BattleUnitModel>();
            float num = -1f;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.owner.faction == Faction.Enemy ? Faction.Player : Faction.Enemy))
            {
                if (alive.IsTargetable(this.owner))
                {
                    if (list.Count <= 0)
                    {
                        list.Add(alive);
                        num = alive.hp;
                    }
                    else if ((double)alive.hp == (double)num)
                        list.Add(alive);
                    else if ((double)alive.hp < (double)num)
                    {
                        list.Clear();
                        list.Add(alive);
                        num = alive.hp;
                    }
                }
            }
            if (list.Count > 0)
                battleUnitModel = RandomUtil.SelectOne<BattleUnitModel>(list);
            return battleUnitModel;
        }
        private void AddNewCard(int id)
        {
            BattleDiceCardModel battleDiceCardModel = this.owner.allyCardDetail.AddTempCard(id);
            battleDiceCardModel.SetPriorityAdder(1000 - this._cardCount * 100);
            battleDiceCardModel.SetCostToZero();
            ++this._cardCount;
        }
        private int GetAddedDiceCard()
        {
            int[] numArray = new int[2] { 702315, 702316 };
            int index = UnityEngine.Random.Range(0, numArray.Length);
            return numArray[index];
        }
        public IEnumerator Transformation()
        {
            PassiveAbility_170301_New passiveAbility170301 = this;
            passiveAbility170301.Map?.PhaseChangeEffect();
            SoundEffectPlayer.PlaySound("Creature/Bossbird_Bossbird_Laser_Start");
            yield return (object)YieldCache.WaitForSeconds(1.55f);
            SoundEffectPlayer.PlaySound("Battle/Roland_Phase4_PhaseChangeDown");
            yield return (object)YieldCache.WaitForSeconds(0.45f);
            SoundEffectPlayer.PlaySound("Battle/Tanya_StronStart");
            passiveAbility170301.Map?.RemoveAllWeapons();
            passiveAbility170301.owner.passiveDetail.AddPassive(new PassiveAbility_170311());
            passiveAbility170301.owner.passiveDetail.AddPassive((PassiveAbilityBase)new PassiveAbility_170312());
            passiveAbility170301.owner.passiveDetail.DestroyPassive((PassiveAbilityBase)passiveAbility170301);
            passiveAbility170301.owner.passiveDetail.OnCreated();
            PassiveAbilityBase passive1 = passiveAbility170301.owner.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_170302));
            if (passive1 != null)
                passiveAbility170301.owner.passiveDetail.DestroyPassive(passive1);
            PassiveAbilityBase passive2 = passiveAbility170301.owner.passiveDetail.PassiveList.Find((Predicate<PassiveAbilityBase>)(x => x is PassiveAbility_170303));
            if (passive2 != null)
                passiveAbility170301.owner.passiveDetail.DestroyPassive(passive2);
            passiveAbility170301.owner.view.charAppearance.SetAltMotion(ActionDetail.Default, ActionDetail.S14);
            passiveAbility170301.owner.view.charAppearance.SetAltMotion(ActionDetail.Standing, ActionDetail.S14);
            passiveAbility170301.owner.view.charAppearance.ChangeMotion(ActionDetail.S14);
            Singleton<StageController>.Instance.GetCurrentWaveModel().SetFormationPosition(0, new Vector2Int(12, 0));
            Singleton<StageController>.Instance.GetCurrentWaveModel().SetFormationPosition(1, new Vector2Int(6, -12));
            Singleton<StageController>.Instance.GetCurrentWaveModel().SetFormationPosition(2, new Vector2Int(5, 12));
            Singleton<StageController>.Instance.GetCurrentWaveModel().SetFormationPosition(3, new Vector2Int(18, 12));
            Singleton<StageController>.Instance.GetCurrentWaveModel().SetFormationPosition(4, new Vector2Int(17, -12));
            BattleUnitModel battleUnitModel1 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60208, 1);
            BattleUnitModel battleUnitModel2 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60218, 2);
            BattleUnitModel battleUnitModel3 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60228, 3);
            BattleUnitModel battleUnitModel4 = Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 60238, 4);
            battleUnitModel1.SetDeadSceneBlock(false);
            battleUnitModel1.view.charAppearance.SetAltMotion(ActionDetail.Default, ActionDetail.S13);
            battleUnitModel1.view.charAppearance.SetAltMotion(ActionDetail.Standing, ActionDetail.S13);
            battleUnitModel2.SetDeadSceneBlock(false);
            battleUnitModel2.view.charAppearance.SetAltMotion(ActionDetail.Default, ActionDetail.S13);
            battleUnitModel2.view.charAppearance.SetAltMotion(ActionDetail.Standing, ActionDetail.S13);
            battleUnitModel3.SetDeadSceneBlock(false);
            battleUnitModel3.view.charAppearance.SetAltMotion(ActionDetail.Default, ActionDetail.S13);
            battleUnitModel3.view.charAppearance.SetAltMotion(ActionDetail.Standing, ActionDetail.S13);
            battleUnitModel4.SetDeadSceneBlock(false);
            battleUnitModel4.view.charAppearance.SetAltMotion(ActionDetail.Default, ActionDetail.S13);
            battleUnitModel4.view.charAppearance.SetAltMotion(ActionDetail.Standing, ActionDetail.S13);
            int num = 0;
            foreach (BattleUnitModel battleUnitModel5 in (IEnumerable<BattleUnitModel>)BattleObjectManager.instance.GetList())
                SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnitModel5.UnitData.unitData, num++, true);
            BattleObjectManager.instance.InitUI();
            yield return (object)YieldCache.WaitForSeconds(0.8f);
            SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject.SetRunningState(false);
        }
    }
}
