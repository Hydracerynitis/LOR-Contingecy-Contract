﻿using System;
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
        private readonly Dictionary<string, Contract> XmlList = new Dictionary<string, Contract>();
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
                Debug.Log("XML: {0} Added",contract.Type);
            }
        }
        public Contract GetContract(string Type)
        {
            if (XmlList.ContainsKey(Type))
                return XmlList[Type].Copy();
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
        public List<ContractDesc> desc;
        [XmlElement("ContractType")]
        public ContractXmlType contractType = ContractXmlType.Passive;
        [XmlElement("Faction")]
        public Faction Faction;
        [XmlElement("Variation")]
        public int Variation = 0;
        [XmlElement("BaseLevel")]
        public int BaseLevel = 0;
        [XmlElement("Step")]
        public int Step = 1;
        [XmlElement("BonusBase")]
        public int BonusBaseLevel = 0;
        [XmlElement("BonusStep")]
        public int BonusStep = 0;
        [XmlElement("Stage")]
        public int Stageid = -1;
        [XmlElement("Conflict")]
        public List<string> Conflict=new List<string>();
        [XmlIgnore]
        public int level = -1;
        public int Level = 0;
        public int Bonus = 0;
        public StageModifier modifier;
        public ContractDesc GetDesc(string language)
        {
            return this.desc.Find(x => x.language == language);
        }
        public Contract Copy()
        {
            Contract copy = new Contract
            {
                Type = Type,
                desc = new List<ContractDesc>(),
                contractType = contractType,
                Variation = Variation,
                BaseLevel = BaseLevel,
                Faction = Faction,
                Step = Step,
                BonusBaseLevel=BonusBaseLevel,
                BonusStep=BonusStep,
                Stageid=Stageid,
                Conflict = new List<string>()
            };
            foreach (ContractDesc desc in desc)
                copy.desc.Add(desc.Copy());
            copy.Conflict.AddRange(Conflict);
            return copy;
        }
    }
    public class ContractDesc
    {
        [XmlAttribute("language")]
        public string language;
        [XmlAttribute("name")]
        public string name;
        [XmlAttribute("desc")]
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
