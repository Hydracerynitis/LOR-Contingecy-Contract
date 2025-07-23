using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ContingecyContract_LoveTown : ContingecyContract
    {
        private Dictionary<BehaviourDetail, int> HitDic = new Dictionary<BehaviourDetail, int>();
        private BehaviourDetail resist = BehaviourDetail.None;
        private bool IsTomerry => owner.UnitData.unitData.EnemyUnitId == 30016;
        private int cap => IsTomerry ? 5 : 2;
        public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
        {
            if (detail == resist)
                return AtkResist.Resist;
            return base.GetResistHP(origin, detail);
        }
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            HitDic.Add(BehaviourDetail.Slash, 0);
            HitDic.Add(BehaviourDetail.Penetrate, 0);
            HitDic.Add(BehaviourDetail.Hit, 0);
        }
        public override int GetDamageReduction(BattleDiceBehavior behavior)
        {
            BehaviourDetail detail = behavior.behaviourInCard.Detail;
            if (HitDic.ContainsKey(detail))
            {
                HitDic[detail] += 1;
                return Math.Min(cap, HitDic[detail] * 2);
            }           
            return base.GetDamageReduction(behavior);
        }
        public override void OnRoundEnd()
        {
            if (IsTomerry)
            {
                BehaviourDetail detail = BehaviourDetail.Slash;
                if (HitDic[BehaviourDetail.Slash] < HitDic[BehaviourDetail.Penetrate])
                    detail = BehaviourDetail.Penetrate;
                else if (HitDic[BehaviourDetail.Slash] == HitDic[BehaviourDetail.Penetrate])
                    detail = RandomUtil.SelectOne(BehaviourDetail.Slash, BehaviourDetail.Penetrate);
                if (HitDic[BehaviourDetail.Hit] > HitDic[detail])
                    detail = BehaviourDetail.Hit;
                else if (HitDic[BehaviourDetail.Hit] == HitDic[detail])
                    detail = RandomUtil.SelectOne(detail, BehaviourDetail.Hit);
                resist = detail;
            }
            HitDic[BehaviourDetail.Slash] = 0;
            HitDic[BehaviourDetail.Penetrate] = 0;
            HitDic[BehaviourDetail.Hit] = 0;
            base.OnRoundEnd();
        }
    }
}
