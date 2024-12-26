using BaseMod;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contingecy_Contract;
using UnityEngine;

namespace ContractReward
{
    public class PassiveAbility_1800000 : PassiveAbilityBase
    {
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1800000));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1800000));
            this.rare = Rarity.Unique;
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            HashSet<int> input = new HashSet<int>();
            List<BattleUnitModel> user = new List<BattleUnitModel>();
            PassiveAbility_1840001 bremen = null;
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (unit.passiveDetail.HasPassive<PassiveAbility_1800000>())
                {
                    user.Add(unit);
                    int id = unit.Book.ClassInfo._id;
                    if (id % 100000 != 0)
                        continue;
                    id /= 100000;
                    id -= 180;
                    input.Add(id);
                    if (id == 4)
                        bremen = unit.passiveDetail.FindPassive<PassiveAbility_1840001>();
                }
            }
            owner.bufListDetail.AddBuf(new SynphonyOrchestra(input, user,bremen));
        }
    }
    public class SynphonyOrchestra: BattleUnitBuf
    {
        public static AudioClip[] _oldEnemytheme;
        public override string keywordId => "CC_Reverberation";
        public override string keywordIconId => "Reverberation";
        public override string bufActivatedText => BattleEffectTextsXmlList.Instance.GetEffectTextDesc(keywordId) + GetText();
        private string GetText()
        {
            List<string> desc= new List<string>();
            if (atttendee.Contains(0))
                desc.Add(TextDataModel.GetText("Reverberation_Conductor"));
            if (atttendee.Contains(1))
                desc.Add(TextDataModel.GetText("Reverberation_Cello"));
            if (atttendee.Contains(2))
                desc.Add(TextDataModel.GetText("Reverberation_Harp"));
            if (atttendee.Contains(3))
                desc.Add(TextDataModel.GetText("Reverberation_Drum"));
            if (atttendee.Contains(4) && Bremen!=null)
            {
                switch (Bremen.currentHead)
                {
                    case PassiveAbility_1840001.Head.Donkey:
                        desc.Add(TextDataModel.GetText("Reverberation_Horn"));
                        break;
                    case PassiveAbility_1840001.Head.Dog:
                        desc.Add(TextDataModel.GetText("Reverberation_Tuba"));
                        break;
                    case PassiveAbility_1840001.Head.Chicken:
                        desc.Add(TextDataModel.GetText("Reverberation_Trombone"));
                        break;
                    default:
                        break;
                }
            }  
            if (atttendee.Contains(5))
                desc.Add(TextDataModel.GetText("Reverberation_Clarinet"));
            if (atttendee.Contains(6))
                desc.Add(TextDataModel.GetText("Reverberation_Viola"));
            if (atttendee.Contains(7))
                desc.Add(TextDataModel.GetText("Reverberation_S_Violin"));
            if (atttendee.Contains(8))
                desc.Add(TextDataModel.GetText("Reverberation_F_Violin"));
            if (atttendee.Contains(9))
                desc.Add(TextDataModel.GetText("Reverberation_Organ"));
            return string.Join("\n",desc);
        }
        private PassiveAbility_1840001 Bremen;
        private int drumCount = 0;
        private bool violaActive = false;
        private HashSet<int> atttendee = new HashSet<int>();
        private List<BattleUnitModel> unit = new List<BattleUnitModel>();
        public SynphonyOrchestra(HashSet<int> input, List<BattleUnitModel> user, PassiveAbility_1840001 bremen=null)
        {
            atttendee = input;
            unit = user;
            Bremen = bremen;
            stack = 0;
        }
        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            if (atttendee.Count >= 5)
                owner.personalEgoDetail.AddCard(Tools.MakeLorId(101));
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            int dmg = 0;
            int bdmg = 0;
            int min = 0;
            int max = 0;
            int pow = 0;
            if (behavior.Detail == BehaviourDetail.Hit)
            {
                if (atttendee.Contains(3))
                {
                    if (drumCount == 2)
                    {
                        pow += 1;
                        drumCount = 0;
                    }
                    else
                        drumCount++;
                }
                if (atttendee.Contains(9))
                    behavior.SetDamageRedution(2);
            }
            if (behavior.Detail == BehaviourDetail.Penetrate)
            {
                if (atttendee.Contains(4) && Bremen!=null)
                {
                    switch (Bremen.currentHead)
                    {
                        case PassiveAbility_1840001.Head.Donkey:
                            dmg += 2;
                            break;
                        case PassiveAbility_1840001.Head.Dog:
                            bdmg += 2;
                            break;
                        case PassiveAbility_1840001.Head.Chicken:
                            pow += 1;
                            break;
                        default:
                            break;
                    }
                }
            }
            if (behavior.Detail == BehaviourDetail.Slash)
            {
                if (atttendee.Contains(8))
                    dmg += 2;
                if (atttendee.Contains(7))
                    bdmg += 2;
                if (atttendee.Contains(6))
                {
                    if (violaActive)
                        min += 2;
                }
                violaActive = true;
            }
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = dmg, breakDmg = bdmg, max = max, min = min, power = pow });
        }
        public override void OnSuccessAttack(BattleDiceBehavior behavior)
        {
            if (behavior.Detail == BehaviourDetail.Hit)
            {
                if (atttendee.Contains(2))
                    _owner.breakDetail.RecoverBreak(2);
            }
            if (behavior.Detail == BehaviourDetail.Penetrate)
            {
                if (atttendee.Contains(5))
                    behavior.card.ApplyDiceStatBonus(DiceMatch.NextDice, new DiceStatBonus() { power = 1 });
            }
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_oldEnemytheme != null)
            {
                int emotionTotalCoinNumber = Singleton<StageController>.Instance.GetCurrentStageFloorModel().team.emotionTotalCoinNumber;
                Singleton<StageController>.Instance.GetCurrentWaveModel().team.emotionTotalBonus = emotionTotalCoinNumber + 1;
                Singleton<StageController>.Instance.GetStageModel().SetCurrentMapInfo(0);
            }
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEnd();
            if (atttendee.Contains(0))
            {
                int el = _owner.emotionDetail.EmotionLevel;
                foreach(BattleUnitModel unit in unit)
                {
                    if (unit.emotionDetail.EmotionLevel < el)
                    {
                        unit.emotionDetail.SetEmotionLevel(el);
                        unit.passiveDetail.OnLevelUpEmotion();
                    }
                        
                }
            }
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
        {
            base.OnUseCard(card);
            if (atttendee.Contains(1))
            {
                if (card.GetOriginalDiceBehaviorList().Exists(x => x.Detail == BehaviourDetail.Slash))
                {
                    card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { max = 2 });
                }
            }
        }
        public void SwitchMusic()
        {
            AudioClip[] synphony = new AudioClip[3] { StaticDataManager.reverberation[0], StaticDataManager.reverberation[1], StaticDataManager.reverberation[2] };
            _oldEnemytheme = BattleSoundManager.Instance.enemyThemeSound;
            BattleSoundManager.Instance.SetEnemyTheme(synphony);
            EmotionBattleTeamModel team = StageController.Instance.GetCurrentStageFloorModel().team;
            BattleSoundManager.Instance.ChangeEnemyTheme(team.emotionLevel >= 5 ? 2 : team.emotionLevel >= 3 ? 1 : 0);
            foreach(BattleUnitModel unit in unit)
            {
                unit.personalEgoDetail.RemoveCard(Tools.MakeLorId(101));
                unit.personalEgoDetail.AddCard(Tools.MakeLorId(102));
                unit.personalEgoDetail.AddCard(Tools.MakeLorId(103));
                unit.personalEgoDetail.AddCard(Tools.MakeLorId(104));
                unit.personalEgoDetail.AddCard(Tools.MakeLorId(105));
                unit.personalEgoDetail.AddCard(Tools.MakeLorId(106));
            }
        }
        public void SwitchChapter1()
        {
            AudioClip[] synphony = new AudioClip[3] { StaticDataManager.reverberation[0], StaticDataManager.reverberation[0], StaticDataManager.reverberation[0] };
            BattleSoundManager.Instance.SetEnemyTheme(synphony);
            BattleSoundManager.Instance.ChangeEnemyTheme(0);
            foreach (BattleUnitModel unit in unit)
            {
                foreach (int id in new int[] { 102, 103, 104, 105, 106 })
                    unit.personalEgoDetail.RemoveCard(Tools.MakeLorId(id));
                foreach (int id in new int[] { 103, 104, 105, 106 })
                    unit.personalEgoDetail.AddCard(Tools.MakeLorId(id));
            }      
        }
        public void SwitchChapter2()
        {
            AudioClip[] synphony = new AudioClip[3] { StaticDataManager.reverberation[1], StaticDataManager.reverberation[1], StaticDataManager.reverberation[1] };
            BattleSoundManager.Instance.SetEnemyTheme(synphony);
            BattleSoundManager.Instance.ChangeEnemyTheme(1);
            foreach (BattleUnitModel unit in unit)
            {
                foreach (int id in new int[] { 102, 103, 104, 105, 106 })
                    unit.personalEgoDetail.RemoveCard(Tools.MakeLorId(id));
                foreach (int id in new int[] { 102, 104, 105, 106 })
                    unit.personalEgoDetail.AddCard(Tools.MakeLorId(id));
            }
        }
        public void SwitchChapter3()
        {
            AudioClip[] synphony = new AudioClip[3] { StaticDataManager.reverberation[2], StaticDataManager.reverberation[2], StaticDataManager.reverberation[2] };
            BattleSoundManager.Instance.SetEnemyTheme(synphony);
            BattleSoundManager.Instance.ChangeEnemyTheme(2);
            foreach (BattleUnitModel unit in unit)
            {
                foreach (int id in new int[] { 102, 103, 104, 105, 106 })
                    unit.personalEgoDetail.RemoveCard(Tools.MakeLorId(id));
                foreach (int id in new int[] { 102, 103, 105, 106 })
                    unit.personalEgoDetail.AddCard(Tools.MakeLorId(id));
            }
        }
        public void SwitchChapter4()
        {
            AudioClip[] synphony = new AudioClip[3] { StaticDataManager.reverberation[3], StaticDataManager.reverberation[3], StaticDataManager.reverberation[3] };
            BattleSoundManager.Instance.SetEnemyTheme(synphony);
            BattleSoundManager.Instance.ChangeEnemyTheme(0);
            foreach (BattleUnitModel unit in unit)
            {
                foreach (int id in new int[] { 102, 103, 104, 105, 106 })
                    unit.personalEgoDetail.RemoveCard(Tools.MakeLorId(id));
                foreach (int id in new int[] { 102, 103, 104, 106 })
                    unit.personalEgoDetail.AddCard(Tools.MakeLorId(id));
            }
        }
        public void EndMusic()
        {
            if (_oldEnemytheme != null)
            {
                BattleSoundManager.Instance.SetEnemyTheme(_oldEnemytheme);
                EmotionBattleTeamModel team = StageController.Instance.GetCurrentStageFloorModel().team;
                BattleSoundManager.Instance.ChangeAllyTheme(team.emotionLevel >= 5 ? 2 : team.emotionLevel >= 3 ? 1 : 0);
            }
            _oldEnemytheme = null;
            foreach (BattleUnitModel unit in unit)
            {
                unit.personalEgoDetail.AddCard(Tools.MakeLorId(101));
                unit.personalEgoDetail.RemoveCard(Tools.MakeLorId(102));
                unit.personalEgoDetail.RemoveCard(Tools.MakeLorId(103));
                unit.personalEgoDetail.RemoveCard(Tools.MakeLorId(104));
                unit.personalEgoDetail.RemoveCard(Tools.MakeLorId(105));
                unit.personalEgoDetail.RemoveCard(Tools.MakeLorId(106));
            }
        }
    }
    public class DiceCardSelfAbility_SwitchMusic : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            if (unit.bufListDetail.FindBuf<SynphonyOrchestra>() is SynphonyOrchestra SO)
                SO.SwitchMusic();
        }
    }
    public class DiceCardSelfAbility_SwitchChapter1 : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            if (unit.bufListDetail.FindBuf<SynphonyOrchestra>() is SynphonyOrchestra SO)
                SO.SwitchChapter1();
        }
    }
    public class DiceCardSelfAbility_SwitchChapter2 : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            if (unit.bufListDetail.FindBuf<SynphonyOrchestra>() is SynphonyOrchestra SO)
                SO.SwitchChapter2();
        }
    }
    public class DiceCardSelfAbility_SwitchChapter3 : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            if (unit.bufListDetail.FindBuf<SynphonyOrchestra>() is SynphonyOrchestra SO)
                SO.SwitchChapter3();
        }
    }
    public class DiceCardSelfAbility_SwitchChapter4 : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            if (unit.bufListDetail.FindBuf<SynphonyOrchestra>() is SynphonyOrchestra SO)
                SO.SwitchChapter4();
        }
    }
    public class DiceCardSelfAbility_EndMusic : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            if (unit.bufListDetail.FindBuf<SynphonyOrchestra>() is SynphonyOrchestra SO)
                SO.EndMusic();
        }
    }
}
