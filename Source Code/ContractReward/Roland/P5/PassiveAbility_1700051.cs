using BaseMod;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using Contingecy_Contract;

namespace ContractReward
{
    public class PassiveAbility_1700051 : PassiveAbilityBase
    {
        public override void OnRoundEnd()
        {
            base.OnBattleEnd();
            if(owner.faction==Faction.Player)
                StageController.Instance.GetCurrentStageFloorModel()._selectedList.RemoveAll(x => x.Sephirah == SephirahType.ETC);
        }
        public override void OnRoundEndTheLast()
        {
            if (BattleObjectManager.instance.GetAliveList_opponent(owner.faction).Count==0 && StageController.Instance.EnemyStageManager.IsStageFinishable() || owner.faction==Faction.Player)
                return;
            List<EmotionCardXmlInfo> RolandEmotion = new List<EmotionCardXmlInfo>();
            for (int i = 18001; i <= 18009; i++)
                RolandEmotion.Add(EmotionCardXmlList.Instance.GetData(i, SephirahType.ETC).Copy());
            List<BattleEmotionCardModel> selected = owner.UnitData.emotionDetail.PassiveList;
            RolandEmotion.RemoveAll(x => selected.Exists(y => y.XmlInfo.id == x.id));
            while (RolandEmotion.Count > 3)
                RolandEmotion.RemoveAt(Random.Range(0, RolandEmotion.Count));
            owner.emotionDetail.ApplyEmotionCard(RandomUtil.SelectOne(RolandEmotion));
        }
    }
}
