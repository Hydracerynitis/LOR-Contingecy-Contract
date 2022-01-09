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
    public class ContractBluePrintList
    {
        public List<ContractBluePrint> CCs = new List<ContractBluePrint>();
    }
    public class ContractBluePrint
    {
        public string Type;
        public List<ContractDesc> desc;
        public ContractXmlType contractType = ContractXmlType.Passive;
        public Faction Faction;
        public int Variation = 0;
        public int BaseLevel = 0;
        public int Step = 1;
        public int BonusBaseLevel = 0;
        public int BonusStep = 0;
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
        public int Bonus = 1;
        public int Stageid = -1;
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
        Passive,
        Stage
    }
}
