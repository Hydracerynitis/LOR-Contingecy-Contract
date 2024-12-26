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
    public class Data<T>
    {
        public T data;
    }
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

        public static void Save<T>(T value, string savename)
        {
            if (!Directory.Exists(Saveroot))
                Directory.CreateDirectory(Saveroot);
            string path = string.Concat(Saveroot, savename, ".json");
            File.WriteAllText(path, new Data<T>
            {
                data = value
            }.ToJson());
        }

        public static T Load<T>(string savename)
        {
            if (!Directory.Exists(Saveroot))
            {
                Directory.CreateDirectory(Saveroot);
                return default(T);
            }

            string path = string.Concat(Saveroot, savename, ".json");
            if (!File.Exists(path))
            {
                return default(T);
            }
            return File.ReadAllText(path).ToObject<Data<T>>().data;
        }

        public static string Saveroot => SaveManager.savePath + "/CCSave/";
    }
}
