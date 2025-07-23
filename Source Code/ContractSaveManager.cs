using GameSave;
using MyJsonTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static BaseMod.Tools;

namespace Contingecy_Contract
{
    static class ContractSaveManager
    {
        public static void RemoveData(string savename)
        {
            if (!Directory.Exists(Saveroot))
                Directory.CreateDirectory(Saveroot);
            if (!File.Exists(Saveroot  + savename))
                return;
            File.Delete(Saveroot + savename);
        }

        public static SaveCollection EncryClearList(HashSet<LorId> ClearList)
        {
            SaveCollection SC=new SaveCollection();
            SC.Active = ContractRewardSystem.Instance.Active;
            foreach (LorId id in ClearList)
            {
                SavedReward sr = SC.GetSavedReward(id.packageId);
                if (sr == null)
                    SC.SRs.Add(new SavedReward() { Pid = id.packageId, data = new List<int>() { id.id } });
                else
                    sr.data.Add(id.id);
            }
            return SC;
        }
        public static HashSet<LorId> DecryptSave(SaveCollection saves)
        {
            HashSet<LorId> clearList= new HashSet<LorId>();
            ContractRewardSystem.Instance.Active = saves.Active;
            saves.SRs.ForEach(x => x.data.ForEach(y => clearList.Add(new LorId(x.Pid, y) ) ) );
            return clearList;
        }

        public static void Save(string savename)
        {
            if (!Directory.Exists(Saveroot))
                Directory.CreateDirectory(Saveroot);
            string path = string.Concat(Saveroot, savename, ".json");
            SaveCollection SC = EncryClearList(ContractRewardSystem.Instance.ClearList);
            File.WriteAllText(path, SC.ToJson());
        }

        public static void Load(string savename)
        {
            HashSet<LorId> bmSave = new HashSet<LorId>();
            if (Directory.Exists(Saveroot))
            {
                string path = string.Concat(Saveroot, savename, ".json");
                if (File.Exists(path))
                {
                    bmSave = DecryptSave(File.ReadAllText(path).ToObject<SaveCollection>());
                }
            }
            if (bmSave.Count > 0)
            {
                foreach(LorId id in bmSave)
                    ContractRewardSystem.Instance.ClearList.Add(id);
            }
        }

        public static string Saveroot => SaveManager.savePath + "/CCSave/";
    }
}
