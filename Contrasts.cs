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
    public class ContractXmlList : Singleton<ContractXmlList>
    {
        private Dictionary<string, Contract> XmlList = new Dictionary<string, Contract>();
        public void Init()
        {
            XmlList.Clear();
        }
        public void Add(List<Contract> list)
        {
            foreach (Contract contract in list)
            {
                if (XmlList.ContainsKey(contract.Type ))
                    XmlList[contract.Type] = contract;
                else
                    XmlList.Add(contract.Type, contract);
                Debug.XMlDebug(contract.Type);
            }
        }
        public Contract GetContract(string Type)
        {
            if (XmlList.ContainsKey(Type))
                return XmlList[Type];
            return null;
        }
    }
    public class ContractList
    {
        [XmlElement("Contract")]
        public List<Contract> ContractsList;
    }
    public class Contract
    {
        [XmlAttribute("Type")]
        public string Type;
        [XmlArray("DescList")]
        [XmlArrayItem("Desc")]
        public List<ContracDesc> desc;
        [XmlElement("Faction")]
        public Faction Faction;
        [XmlElement("Variation")]
        public int Variation = 0;
        [XmlElement("BaseLevel")]
        public int BaseLevel = 0;
        [XmlElement("Enemy")]
        public List<int> Enemy = new List<int>();
        [XmlElement("Conflict")]
        public List<string> Conflict=new List<string>();
        [XmlIgnore]
        public int level = -1;
        public ContracDesc GetDesc(string language)
        {
            return this.desc.Find((Predicate<ContracDesc>)(x => x.language == language));
        }
    }
    public class ContracDesc
    {
        [XmlAttribute("language")]
        public string language;
        [XmlAttribute("name")]
        public string name;
        [XmlAttribute("desc")]
        public string desc;
    }
}
