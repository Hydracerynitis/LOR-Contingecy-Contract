using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class RewardConditionList
    {
        public List<RewardConfition> RCs;
    }
    public class RewardConfition
    {
        public string Pid = "";
        public int Stageid;
        public List<ContractCondition> Condition = new List<ContractCondition>();
        public int RewardId;
        public LorId Id => new LorId(Pid, Stageid);
    }
    public class ContractCondition
    {
        public string Type;
        public int Variation;
    }
}
