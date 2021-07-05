using System;
using System.IO;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class Debug
    {
        public static void ModPatchDebug()
        {
            PathDebug("/Debug", PathType.Directory);
            File.WriteAllText(Harmony_Patch.ModPath + "/Debug/Log.txt", "ModPath: " + Harmony_Patch.ModPath + "\n");
        }
        public static void Log(string message, params string[] Params)
        {
            PathDebug("/Debug", PathType.Directory);
            File.AppendAllText(Harmony_Patch.ModPath + "/Debug/Log.txt", string.Format(message,Params) + "\n");
        }
        public static void XmlFileDebug(string path)
        {
            PathDebug("/Debug", PathType.Directory);
            foreach (FileInfo file in new DirectoryInfo(Harmony_Patch.ModPath + path).GetFiles())
                File.AppendAllText(Harmony_Patch.ModPath + "/Debug/Log.txt","XmlFile: " +file.Name+" Found\n");
        }
        public static void Error(string type,Exception ex)
        {
            PathDebug("/Debug", PathType.Directory);
            File.WriteAllText(Harmony_Patch.ModPath + "/Debug/"+type+"Error.txt", ex.Message + Environment.NewLine + ex.StackTrace);
        }
        public static void ErrorLog(string type, string Log)
        {
            PathDebug("/Debug", PathType.Directory);
            File.AppendAllText(Harmony_Patch.ModPath + "/Debug/" + type + "Error.txt", Log+"\n'");
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
        public static void SaveDebug()
        {
            File.WriteAllText(Harmony_Patch.ModPath + "/Debug/Save.txt", "");
            FieldInfo[] values = typeof(ChallengeProgress).GetFields();
            for (int i= 0; i < values.Length; i++)
            {
                File.AppendAllText(Harmony_Patch.ModPath + "/Debug/Save.txt", values[i].Name+" "+values[i].GetValue(Harmony_Patch.Progess)+"\n");
            }           
        }
    }
    public enum PathType
    {
        Directory,
        File
    }
}
