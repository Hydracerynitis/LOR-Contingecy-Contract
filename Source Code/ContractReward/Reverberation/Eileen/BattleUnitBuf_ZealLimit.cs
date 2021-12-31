using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class BattleUnitBuf_ZealLimit: BattleUnitBuf
    {
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            this.Destroy();
        }
    }
}
