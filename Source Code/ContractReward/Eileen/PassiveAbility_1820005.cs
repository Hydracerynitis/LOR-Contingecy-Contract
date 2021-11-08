using System;
using System.Collections.Generic;
using SummonLiberation;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class PassiveAbility_1820005 : PassiveAbility_240026
    {
        private static List<int> IdList => new List<int>(){ 18200006, 18200007, 18200008 };
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            foreach(int i in IdList)
            {
                if (this.owner.personalEgoDetail.GetHand().Exists(x => x.GetID().id == i))
                    return;
            }
            this.owner.personalEgoDetail.AddCard(Tools.MakeLorId(18200006));
            this.owner.personalEgoDetail.AddCard(Tools.MakeLorId(18200007));
            this.owner.personalEgoDetail.AddCard(Tools.MakeLorId(18200008));
        }
    }
}
