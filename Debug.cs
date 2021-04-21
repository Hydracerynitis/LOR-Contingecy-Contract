using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class Debug
    {
        public static int LoadNum;
        public static void Log(string message)
        {
            PathDebug("/Debug", PathType.Directory);
            File.AppendAllText(Harmony_Patch.ModPath + "/Debug/Log.txt", message + "\n");
        }
        public static void ModPatchDebug()
        {
            PathDebug("/Debug", PathType.Directory);
            File.WriteAllText(Harmony_Patch.ModPath + "/Debug/Log.txt", "ModPath: "+Harmony_Patch.ModPath+"\n");
        }
        public static void XmlFileDebug()
        {
            PathDebug("/Debug", PathType.Directory);
            foreach (FileInfo file in new DirectoryInfo(Harmony_Patch.ModPath + "/Contracts").GetFiles())
                File.AppendAllText(Harmony_Patch.ModPath + "/Debug/Log.txt","XmlFile: " +file.Name+" Found\n");
        }
        public static void XMlDebug(string ID)
        {
            PathDebug("/Debug", PathType.Directory);
            File.AppendAllText(Harmony_Patch.ModPath + "/Debug/Log.txt", "XML: " + ID + " Added\n");
        }
        public static void HPDebug(string name)
        {
            PathDebug("/Debug", PathType.Directory);
            File.AppendAllText(Harmony_Patch.ModPath + "/Debug/Log.txt", "Patch: " + name + " Succeed\n");
        }
        public static void LoadDebug()
        {
            PathDebug("/Debug", PathType.Directory);
            LoadNum += 1;
            File.AppendAllText(Harmony_Patch.ModPath + "/Debug/Log.txt", "ContractLoad"+LoadNum.ToString()+" Start\n");
            foreach(Contract contract in Singleton<ContractLoader>.Instance.GetList())
            {
                File.AppendAllText(Harmony_Patch.ModPath + "/Debug/Log.txt", "Contract " + contract.Type + " Added\n");
            }
        }
        public static void InstanceDebug(params string[] args)
        {
            PathDebug("/Debug", PathType.Directory);
            File.AppendAllText(Harmony_Patch.ModPath + "/Debug/Log.txt", string.Format("Instance of {0} {1} for {2}\n",args));
        }
        public static void Error(string type,Exception ex)
        {
            PathDebug("/Debug", PathType.Directory);
            File.WriteAllText(Harmony_Patch.ModPath + "/Debug/"+type+"Error.txt", ex.Message + Environment.NewLine + ex.StackTrace);
        }
        public static void PathDebug(string path,PathType type)
        {
            if (type == PathType.Directory)
            {
                if (!Directory.Exists(Harmony_Patch.ModPath + path))
                {
                    File.WriteAllText(Application.dataPath + "/BaseMods/ContingecyContractModPathError.txt", Harmony_Patch.ModPath + path + " not found");
                }
            }
            if (type == PathType.File)
            {
                if (!File.Exists(Harmony_Patch.ModPath + path))
                {
                    File.WriteAllText(Application.dataPath + "/BaseMods/ContingecyContractModPathError.txt", Harmony_Patch.ModPath + path + " not found");
                }
            }
        }
    }
    public enum PathType
    {
        Directory,
        File
    }
}
