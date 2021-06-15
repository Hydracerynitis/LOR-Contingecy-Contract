﻿using System;
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
        public bool bInit;
        private List<Contract> PassiveList;
        private List<Contract> StageList;
        public void Init()
        {
            PassiveList = new List<Contract>();
            StageList = new List<Contract>();
            Debug.PathDebug("/ContractLoader.txt", PathType.File);
            Debug.Log("Start Loading Contract");
            foreach (string readAllLine in File.ReadAllLines(Harmony_Patch.ModPath + "/ContractLoader.txt"))
            {
                int level = 0;
                string str = readAllLine.Trim();
                if (str == "")
                    continue;
                if (char.IsDigit(str[str.Length - 1]))
                {
                    level = int.Parse(str.Substring(str.Length - 1));
                    str = str.Substring(0, str.Length - 1);
                }
                Contract New = Singleton<ContractXmlList>.Instance.GetContract(str);
                if (New == null)
                {
                    Debug.Log("{0} is not found",str);
                    continue;
                }
                if(New.Variation>0 && level == 0)
                {
                    Debug.Log("{0}'s level can't be 0",str);
                    continue;
                }
                if (level > New.Variation)
                {
                    Debug.Log("{0}'s level excceed {0}'s maximun level", str);
                    continue;
                }
                New.level = New.BaseLevel + level*New.Step;
                string name = New.Type;
                if (New.Variation > 0)
                    name=string.Format("{0} {1}",name,level);
                if (New.contractType == ContractXmlType.Passive)
                {
                    if (PassiveList.Find(x => x.Conflict.Contains(New.Type)) != null)
                    {
                        Debug.Log("Conflict contract exist");
                        continue;
                    }
                    New.level = New.BaseLevel + level;
                    Contract Old = PassiveList.Find(x => x.Type == New.Type);
                    if (Old != null)
                    {
                        if (Old.level > New.level)
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
                        if (Old.level > New.level)
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
                    New.modifier = (StageModifier)Activator.CreateInstance(type, new object[] { New.level });
                    StageList.Add(New);
                    Debug.Log("Contract {0} Added to Stage", name);
                }

            }
            Debug.Log("End Loading Contract");
        }
        public int GetLevel(int id)
        {
            int i = 0;
            StageClassInfo info = Singleton<StageClassInfoList>.Instance.GetData(id);
            if (PassiveList.Count > 0)
            {
                foreach (Contract contract in PassiveList)
                {
                    if (contract.Enemy.Count>0)
                    {
                        bool HasEnemy = false;
                        foreach(StageWaveInfo wave in info.waveList)
                        {
                            if(wave.enemyUnitIdList.Exists((Predicate<int>)(x => contract.Enemy.Contains(x))))
                            {
                                HasEnemy = true;
                                continue;
                            }
                        }
                        if (HasEnemy)
                            i += contract.level;
                        continue;
                    }
                    i += contract.level;
                }
            }
            if (StageList.Count > 0)
            {
                foreach(Contract contract in StageList)
                {
                    if (contract.modifier == null)
                    {
                        Debug.Log("{0} does load modifier",contract.Type);
                        continue;
                    }
                    if (contract.modifier.IsValid(info))
                        i += contract.level;
                }
            }
            return i;
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
