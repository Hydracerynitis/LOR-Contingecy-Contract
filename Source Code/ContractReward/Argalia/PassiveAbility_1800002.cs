using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1800002 : PassiveAbilityBase
    {
        public int count;
        private BattleDiceCardModel model;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            model=this.owner.allyCardDetail.AddNewCard(18000021);
            count = 0;
        }
        public void Upgrade()
        {
            if(model.XmlData.id!= 18000024)
            {
                model.exhaust = true;
                model=this.owner.allyCardDetail.AddNewCard(model.XmlData.id + 1);
            }
        }
    }
}
