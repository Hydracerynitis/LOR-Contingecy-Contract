using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class RewardConditionList
    {
        public List<RewardConfig> RCs;
    }
    public class RewardConfig
    {
        public string Pid = "";
        public int Stageid;
        public List<List<ContractCondition>> Condition = new List<List<ContractCondition>>();
        public int RewardId;
        public LorId Id => new LorId(Pid, Stageid);
    }
    public class ContractCondition
    {
        public string Type;
        public int Variation;
    }
}
