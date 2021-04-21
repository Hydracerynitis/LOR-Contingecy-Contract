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
        public bool bInit;
        private List<Contract> ChosenList;
        public void Init()
        {
            ChosenList=new List<Contract>();
            Debug.PathDebug("/ContractLoader.txt", PathType.File);
            Debug.Log("Start Loading Contract");
            if (Harmony_Patch.Duel)
            {
                Contract Duel = Singleton<ContractXmlList>.Instance.GetContract("Duel");
                Duel.level = 0;
                ChosenList.Add(Duel);
            }
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
                    Debug.Log(string.Format("{0} is not found", str));
                    continue;
                }
                if (ChosenList.Find((Predicate<Contract>)(x => x.Conflict.Contains(New.Type))) != null)
                {
                    Debug.Log("Conflict contract exist");
                    continue;
                }
                if (level > New.Variation)
                {
                    Debug.Log(string.Format("{0}'s {1} excceed {0}'s maximun level", str, level));
                    continue;
                }
                New.level = New.BaseLevel + level;
                Contract Old = ChosenList.Find((Predicate<Contract>)(x => x.Type == New.Type));
                if (Old != null)
                {
                    if (Old.level > New.level)
                    {
                        Debug.Log(string.Format("Larger level of {0} exist", str));
                        continue;
                    }
                    else
                    {
                        ChosenList.Remove(Old);
                    }
                }
                ChosenList.Add(New);
                Debug.Log(string.Format("Contract {0} Added", New.Type + level));
            }
            Debug.Log("End Loading Contract");
        }
        public int GetLevel(int id)
        {
            int i = 0;
            if (ChosenList.Count > 0)
            {
                foreach (Contract contract in ChosenList)
                {
                    if (contract.Enemy.Count>0)
                    {
                        bool HasEnemy = false;
                        foreach(StageWaveInfo wave in Singleton<StageClassInfoList>.Instance.GetData(id).waveList)
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
            return i;
        }      
        public List<Contract> GetList()
        {
            List<Contract> list = new List<Contract>();
            list.AddRange(ChosenList);
            return list;
        }
    }
}
