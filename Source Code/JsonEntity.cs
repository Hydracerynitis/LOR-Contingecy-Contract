using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    //Contract
    public class ContractBluePrintCollection
    {
        public List<ContractBluePrint> CCs = new List<ContractBluePrint>();
    }
    public class ContractBluePrint
    {
        public string Type;
        public List<ContractDesc> desc = new List<ContractDesc>();
        public ContractXmlType contractType = ContractXmlType.Passive;
        public Faction Faction;
        public int Variation = 0;
        public int BaseLevel = 0;
        public int Step = 1;
        public string Pid = "";
        public int Stageid = -1;

        public List<string> Conflict = new List<string>();

    }
    public class Contract
    {
        public string Type;
        public int Variant;
        public List<ContractDesc> desc = new List<ContractDesc>();
        public ContractXmlType contractType = ContractXmlType.Passive;
        public Faction Faction;
        public int Level = 0;
        public int Stageid = -1;
        public string Pid = "";
        public List<string> Conflict = new List<string>();
        public StageModifier modifier;
        public string Id => Variant == 0 ? Type : Type + Variant.ToString();
        public bool isOn = false;
        public bool isConflict = false;
        public ContractDesc GetDesc()
        {
            if (desc.Find(x => x.language == TextDataModel.CurrentLanguage) is ContractDesc Desc)
                return Desc;
            else if(TextDataModel.CurrentLanguage=="trcn")
                return desc.Find(x => x.language == "cn");
            else
                return desc.Find(x => x.language == "en");
        }
    }
    public class ContractDesc
    {
        public string language;
        public string name;
        public string desc;
        public ContractDesc Copy()
        {
            ContractDesc copy = new ContractDesc
            {
                language = language,
                name = name,
                desc = desc
            };
            return copy;
        }
    }
    public enum ContractXmlType
    {
        Passive =0,
        Stage =1 
    }
    // ContractParam
    public class ParamCollection
    {
        public List<Params> Ps = new List<Params>();
    }
    public class Params
    {
        public string Id;
        public List<ParamDesc> Desc = new List<ParamDesc>();
    }
    public class ParamDesc
    {
        public string Language;
        public string Content;
    }
    //ContractReward
    public class ContractRewardCollection
    {
        public List<ContractReward> CRs = new List<ContractReward>();
    }
    public class ContractReward
    {
        public string stagePid = "";
        public int stageId;
        public string rewardPid = "";
        public int rewardId;
    }
    // ExtraCondition
    public class RewardConditionCollection
    {
        public List<RewardConfig> RCs = new List<RewardConfig>();
    }
    public class RewardConfig
    {
        public string StagePid = "";
        public int Stageid;
        public List<List<ContractCondition>> Condition = new List<List<ContractCondition>>();
        public string RewardPid = "";
        public int RewardId;
        public LorId StageId => new LorId(StagePid, Stageid);
        public LorId rewardId => new LorId(RewardPid, RewardId);
    }
    public class ContractCondition
    {
        public string Type;
        public int Variation;
    }
    // RewardClearList
    public class ClearListCollection
    {
        public List<RewardClearList> RCLs = new List<RewardClearList>();
    }
    public class RewardClearList
    {
        public string RewardPid="";
        public int RewardId;
        public string RequirementPid = "";
        public List<int> Requirements=new List<int>();
    }
    // Saved ClearList
    public class SaveCollection
    {
        public bool Active = true;
        public List<SavedReward> SRs =new List<SavedReward>();

        public SavedReward GetSavedReward(string Pid) => SRs.Find(x => x.Pid == Pid);
    }
    public class SavedReward
    {
        public string Pid = "";
        public List<int> data = new List<int>();
    }
}
