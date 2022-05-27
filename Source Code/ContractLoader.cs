using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ContractLoader: Singleton<ContractLoader>
    {
        private List<Contract> PassiveList = new List<Contract>();
        private List<Contract> StageList = new List<Contract>();
        public void Init()
        {
            PassiveList.Clear();
            StageList.Clear();
            Debug.PathDebug("/ContractLoader.txt", PathType.File);
            Debug.Log("----- Start Loading Contract -----");
            foreach (string readAllLine in File.ReadAllLines(CCInitializer.ModPath + "/ContractLoader.txt"))
            {
                string str = readAllLine.Trim();
                if (str == "")
                    continue;
                Contract New = StaticDataManager.JsonList.Find(x => x.Id==str);
                if (New == null)
                {
                    Debug.Log("{0} is not found",str);
                    continue;
                }
                string name = New.Type;
                if (New.contractType == ContractXmlType.Passive)
                {
                    if (PassiveList.Find(x => x.Conflict.Contains(New.Type)) != null)
                    {
                        Debug.Log("Conflict contract exist");
                        continue;
                    }
                    Contract Old = PassiveList.Find(x => x.Type == New.Type);
                    if (Old != null)
                    {
                        if (Old.Variant > New.Variant)
                        {
                            Debug.Log("Larger level of {0} exist", str);
                            continue;
                        }
                        else
                        {
                            PassiveList.Remove(Old);
                        }
                    }
                    PassiveList.Add(New);
                    Debug.Log("Contract {0} Added to Passive", name);
                }
                else if (New.contractType == ContractXmlType.Stage)
                {
                    if (StageList.Find(x => x.Conflict.Contains(New.Type)) != null)
                    {
                        Debug.Log("Conflict contract exist");
                        continue;
                    }
                    Contract Old = StageList.Find(x => x.Type == New.Type);
                    if (Old != null)
                    {
                        if (Old.Variant > New.Variant)
                        {
                            Debug.Log("Larger level of {0} exist", str);
                            continue;
                        }
                        else
                        {
                            StageList.Remove(Old);
                        }
                    }
                    System.Type type = System.Type.GetType("Contingecy_Contract.StageModifier_" + New.Type);
                    if (type == (System.Type)null)
                    {
                        Debug.Log(type.Name+ " is not found");
                        continue;
                    }
                    Debug.Log(type.Name+ " is found");
                    New.modifier = (StageModifier)Activator.CreateInstance(type, new object[] { New.Variant });
                    StageList.Add(New);
                    Debug.Log("Contract {0} Added to Stage", name);
                }

            }
            Debug.Log("----- End Loading Contract -----");
        }
        public bool CheckActivate(Contract contract, StageClassInfo info)
        {
            if (PassiveList.Contains(contract))
            {
                if (contract.Stageid != -1 && info.id != contract.Stageid)
                    return false;
                return true;
            }
            if (StageList.Contains(contract))
            {
                if (contract.modifier == null)
                {
                    Debug.Log("{0} does load modifier", contract.Type);
                    return false;
                }
                if (contract.modifier.IsValid(info))
                    return true;
                return false;
            }
            return false;
        }
        public int GetLevel(StageClassInfo info)
        {
            int i = 0;
            int b = 0;
            if (PassiveList.Count > 0)
            {
                foreach (Contract contract in PassiveList)
                {
                    if (CheckActivate(contract, info))
                    {
                        i += contract.Level;
                        b += contract.Bonus;
                    }
                    continue;
                }
            }
            if (StageList.Count > 0)
            {
                foreach (Contract contract in StageList)
                {
                    if (CheckActivate(contract, info))
                    {
                        i += contract.Level;
                        b += contract.Bonus;
                    }
                    continue;
                }
            }
            Debug.Log("Base Level: {0}", i.ToString());
            Debug.Log("Base Bonus: {0}", b.ToString());
            return (int)Math.Floor(i * (1 + b * 0.01));
        }
        public int GetLevel(LorId id)
        {
            return GetLevel(Singleton<StageClassInfoList>.Instance.GetData(id));
        }      
        public List<Contract> GetPassiveList()
        {
            List<Contract> list = new List<Contract>();
            list.AddRange(PassiveList);
            return list;
        }
        public List<Contract> GetStageList()
        {
            List<Contract> list = new List<Contract>();
            list.AddRange(StageList);
            return list;
        }
    }
}
