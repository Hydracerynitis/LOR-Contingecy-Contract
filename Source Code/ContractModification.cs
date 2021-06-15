using System;
using System.Collections.Generic;
using HarmonyLib;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ContractModification
    {
        public static StageClassInfo current;
        public static void Init(StageClassInfo info)
        {
            foreach (Contract contract in Singleton<ContractLoader>.Instance.GetStageList())
            {
                if (contract.modifier == null)
                {
                    Debug.Log("{0} doesn't load modifier", contract.Type);
                    continue;
                }
                if (!contract.modifier.IsValid(info))
                {
                    Debug.Log("{0} doesn't match {1}'s requirement", Singleton<StageNameXmlList>.Instance.GetName(info.id), contract.Type);
                    continue;
                }
                contract.modifier.Modify(ref info);
                Debug.Log("{0} finish modifier", contract.Type);
            }
            current = info;
        }
    }
    public class StageModifier
    {
        public int Level;
        public virtual bool IsValid(StageClassInfo info) => true;
        public virtual void Modify(ref StageClassInfo info)
        {

        }
    }
}
