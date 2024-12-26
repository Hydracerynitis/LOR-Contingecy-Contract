using System;
using System.IO;
using UnityEngine;
using System.Reflection;
using BaseMod;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using System.Reflection.Emit;

namespace Contingecy_Contract
{
    public class Debug
    {
        public static void ModPatchDebug()
        {
            File.WriteAllText(CCInitializer.ModPath + "/Log.txt", "ModPath: " + CCInitializer.ModPath + "\n");
        }
        public static void Log(string message, params string[] Params)
        {
            File.AppendAllText(CCInitializer.ModPath + "/Log.txt", string.Format(message,Params) + "\n");
        }
        public static void XmlFileDebug(string path)
        {
            foreach (FileInfo file in new DirectoryInfo(CCInitializer.ModPath + path).GetFiles())
                File.AppendAllText(CCInitializer.ModPath + "/Log.txt","XmlFile: " +file.Name+" Found\n");
        }
        public static void Error(string type,Exception ex)
        {
            File.WriteAllText(CCInitializer.ModPath+"/"+type+"Error.txt", ex.ToString());
        }
        public static void PathDebug(string path,PathType type)
        {
            if (type == PathType.Directory)
            {
                if (!Directory.Exists(CCInitializer.ModPath + path))
                {
                    File.WriteAllText(Application.dataPath + "/Mods/ContingecyContractModPathError.txt", CCInitializer.ModPath + path + " not found");
                }
            }
            if (type == PathType.File)
            {
                if (!File.Exists(CCInitializer.ModPath + path))
                {
                    File.WriteAllText(Application.dataPath + "/Mods/ContingecyContractModPathError.txt", CCInitializer.ModPath + path + " not found");
                }
            }
        }
        public static void OutputIL(string name, List<CodeInstruction> codes)
        {
            File.WriteAllText(CCInitializer.ModPath + "/" + name + ".txt", "");
            for (int i = 0; i < codes.Count; i++)
            {
                string output = "Line " + i.ToString() + ": " + codes[i].opcode;
                try
                {
                    if (codes[i].operand is Label)
                        output += " [" + ((Label)codes[i].operand).GetHashCode().ToString()+"]";
                    else
                        output += " " + codes[i].operand.ToString();
                }
                catch (NullReferenceException ex)
                {
                    output += " null";
                }
                foreach(Label l in codes[i].labels)
                {
                    output +=" [" +l.GetHashCode().ToString()+"] ";
                }
                File.AppendAllText(CCInitializer.ModPath + "/"+name+".txt", output + "\n");
            }
        }
    }
    public enum PathType
    {
        Directory,
        File
    }
}
