using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700042 : PassiveAbilityBase
    {
        private int _patternCount = 0;
        private int _strCnt = 0;
        private BehaviourDetail _currentBuf=BehaviourDetail.Slash;
        public override void OnWaveStart()
        {
            base.OnWaveStart();     
            this.owner.passiveDetail.AddPassive(new PassiveAbility_170303());//Scar
            owner.passiveDetail._readyPassiveList.Add(new PassiveAbility_1700141(owner));//Aspiration
            this.owner.passiveDetail.OnCreated();
        }
        public override void OnRoundStartAfter()
        {
            switch (_patternCount)
            {
                case 0:
                    this.owner.passiveDetail.AddPassive(new PassiveAbility_170304());//Blood
                    if (owner.emotionDetail.EmotionLevel >= 2)
                        owner.passiveDetail._readyPassiveList.Add(new PassiveAbility_1700142(owner));//Lie+Learn
                    if (owner.emotionDetail.EmotionLevel >= 4)
                        owner.passiveDetail._readyPassiveList.Add(new PassiveAbility_1700144(owner));//Guilt
                    this.owner.passiveDetail.OnCreated();
                    break;
                case 1: 
                    ++this._strCnt;
                    this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, this._strCnt, this.owner);//Leer
                    this.owner.passiveDetail.AddPassive(new PassiveAbility_170305());
                    if(owner.emotionDetail.EmotionLevel >= 2)
                    {
                        foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList().FindAll(x => x != owner))//Blizzard
                            unit.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Binding, 3);
                        owner.passiveDetail._readyPassiveList.Add(new PassiveAbility_1700145(owner));
                    }
                    if (owner.emotionDetail.EmotionLevel >= 4)
                        owner.passiveDetail._readyPassiveList.Add(new PassiveAbility_1700149(owner));//Pale Hand
                    this.owner.passiveDetail.OnCreated();
                    break;
                case 2:
                    this.owner.passiveDetail.AddPassive(new PassiveAbility_170306());//Curiosity
                    if (owner.emotionDetail.EmotionLevel >= 2)
                        owner.passiveDetail._readyPassiveList.Add(new PassiveAbility_1700147(owner));//Fervent Beats
                    if (owner.emotionDetail.EmotionLevel >= 4)
                        owner.passiveDetail._readyPassiveList.Add(new PassiveAbility_1700148(owner));//Frost Sword
                    this.owner.passiveDetail.OnCreated();
                    break;
                case 3:
                    int DestroyPassive = -1;
                    if (owner.emotionDetail.EmotionLevel >= 2)
                        DestroyPassive = RandomUtil.Range(0, owner.emotionDetail.EmotionLevel >= 4 ? 2 : 1);
                    if(DestroyPassive!=0)
                        this.owner.passiveDetail.AddPassive(new PassiveAbility_170307());//Kiss
                    if (owner.emotionDetail.EmotionLevel >= 2 && DestroyPassive!=1)
                        owner.passiveDetail._readyPassiveList.Add(new PassiveAbility_1700146(owner));//Pulsation
                    if (owner.emotionDetail.EmotionLevel >= 4 && DestroyPassive!=2)
                        owner.passiveDetail._readyPassiveList.Add(new PassiveAbility_1700143(owner));//Nail and Hammer
                    this.owner.passiveDetail.OnCreated();
                    owner.OnRoundStartOnlyUI();
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
    }
}
