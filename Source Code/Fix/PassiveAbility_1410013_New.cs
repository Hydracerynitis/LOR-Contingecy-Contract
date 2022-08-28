using System;
using System.Collections.Generic;

namespace Fix
{
    public class PassiveAbility_1410013_New : PassiveAbilityBase
    {
        private readonly List<int> _idList = new List<int>(){ 1408011, 1409011, 1410011 };
        private int _elapsedRound;
        public override void OnWaveStart() => this._elapsedRound = 0;
        public override void OnRoundStart()
        {
            if (PassiveAbility_1410014.IsBattleEnd())
                return;
            ++this._elapsedRound;
            if (this._elapsedRound % 2 != 0)
                return;
            this._elapsedRound = 0;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.owner.faction))
            {
                BattleUnitModel u = alive;
                if (this._idList.Exists((Predicate<int>)(x => u.UnitData.unitData.EnemyUnitId == x)))
                    u.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.UpSurge, 1, this.owner);
            }
        }

        public override void OnRoundEnd_before()
        {
            if (PassiveAbility_1410014.IsBattleEnd())
                return;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (alive.emotionDetail.EmotionLevel < alive.emotionDetail.MaximumEmotionLevel)
                    alive.emotionDetail.LevelUp_Forcely(2);
            }
        }
    }
}