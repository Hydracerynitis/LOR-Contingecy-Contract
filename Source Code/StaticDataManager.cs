using BaseMod;
using MyJsonTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Contingecy_Contract
{
    public class StaticDataManager
    {
        public static List<Contract> JsonList = new List<Contract>();
        public static Dictionary<LorId, int> ThumbPathDictionary = new Dictionary<LorId, int>();
        public static Dictionary<LorId, Sprite> NonThumbSprite = new Dictionary<LorId, Sprite>();
        public static Dictionary<LorId, int> RewardDic = new Dictionary<LorId, int>();
        public static List<RewardConfition> ExtraCondition=new List<RewardConfition>();
        public static void LoadStaticData()
        {
            LoadContract();
            LoadThumb();
            LoadReward();
            LoadExtraCondition();
        }
        public static void LoadContract()
        {

            Debug.PathDebug("/Contracts", PathType.Directory);
            Debug.XmlFileDebug("/Contracts");
            foreach (FileInfo file in new DirectoryInfo(Harmony_Patch.ModPath + "/Contracts").GetFiles())
            {
                try
                {
                    ContractBluePrintList list = File.ReadAllText(file.FullName).ToObject<ContractBluePrintList>();
                    foreach (ContractBluePrint bluePrint in list.CCs)
                    {
                        System.Type type = System.Type.GetType("Contingecy_Contract.ContingecyContract_" + bluePrint.Type);
                        if (type == null)
                            continue;
                        if (bluePrint.Variation <= 0)
                        {
                            JsonList.Add(new Contract() { Type = bluePrint.Type, Variant = 0, desc = bluePrint.desc, contractType = bluePrint.contractType, Faction = bluePrint.Faction, Level = bluePrint.BaseLevel, Bonus = bluePrint.BonusBaseLevel, Stageid = bluePrint.Stageid, Conflict = bluePrint.Conflict });
                            Debug.Log("XML: {0} Added", bluePrint.Type);
                        }
                        else
                        {
                            ContingecyContract contract = Activator.CreateInstance(type, new object[] { 0 }) as ContingecyContract;
                            for (int i = 1; i <= bluePrint.Variation; i++)
                            {
                                contract.Level = i;
                                Contract item = new Contract() { Type = bluePrint.Type, Variant = i, contractType = bluePrint.contractType, Faction = bluePrint.Faction, Level = bluePrint.BaseLevel + i * bluePrint.Step, Bonus = bluePrint.BonusBaseLevel + i * bluePrint.BonusStep, Stageid = bluePrint.Stageid, Conflict = bluePrint.Conflict };
                                bluePrint.desc.ForEach(x => item.desc.Add(new ContractDesc() { language = x.language, name = x.name + " " + i.ToString(), desc = string.Format(x.desc, contract.GetFormatParam) }));
                                JsonList.Add(item);
                                Debug.Log("XML: {0} Added", item.Id);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Error("JSON", ex);
                }
            }
            /*            try
                        {
                            foreach (NewContract bluePrint in ContractXmlList.JsonList)
                            {
                                Debug.ErrorLog("JsonItem", bluePrint.ToJson<NewContract>());
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Error("ToJSOn", ex);
                        }*/
        }
        public static void LoadThumb()
        {
            Debug.PathDebug("/Staticinfo/ThumbPath", PathType.Directory);
            Debug.XmlFileDebug("/Staticinfo/ThumbPath");
            foreach (FileInfo file in new DirectoryInfo(Harmony_Patch.ModPath + "/Staticinfo/ThumbPath").GetFiles())
            {
                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(File.ReadAllText(file.FullName));
                    foreach (XmlNode node in xml.SelectNodes("ThumbPath/Path"))
                    {
                        string str = string.Empty;
                        if (node.Attributes.GetNamedItem("id") != null)
                            str = node.Attributes.GetNamedItem("id").InnerText;
                        int key = Int32.Parse(str);
                        if (node.InnerText == "null")
                        {
                            NonThumbSprite[Tools.MakeLorId(key)] = null;
                            continue;
                        }
                        int value = Int32.Parse(node.InnerText);
                        ThumbPathDictionary[Tools.MakeLorId(key)] = value;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Error("ThumbLoadError", ex);
                }
            }
        }
        public static void LoadReward()
        {
            Debug.PathDebug("/Staticinfo/RewardList", PathType.Directory);
            Debug.XmlFileDebug("/Staticinfo/RewardList");
            foreach (FileInfo file in new DirectoryInfo(Harmony_Patch.ModPath + "/Staticinfo/RewardList").GetFiles())
            {
                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(File.ReadAllText(file.FullName));
                    foreach (XmlNode node in xml.SelectNodes("RewardList/Reward"))
                    {
                        string Pid = "";
                        if (node.Attributes.GetNamedItem("Pid") != null)
                            Pid = node.Attributes.GetNamedItem("Pid").InnerText;
                        string id = string.Empty;
                        if (node.Attributes.GetNamedItem("id") != null)
                            id = node.Attributes.GetNamedItem("id").InnerText;
                        LorId key = new LorId(Pid, Int32.Parse(id));
                        int value = Int32.Parse(node.InnerText);
                        RewardDic[key] = value;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Error("RewardError", ex);
                }
            }
        }
        public static void LoadExtraCondition()
        {
            Debug.PathDebug("/Staticinfo/ExtraCondition", PathType.Directory);
            Debug.XmlFileDebug("/Staticinfo/ExtraCondition");
            foreach (FileInfo file in new DirectoryInfo(Harmony_Patch.ModPath + "/Staticinfo/ExtraCondition").GetFiles())
            {
                try
                {
                    RewardConditionList list = File.ReadAllText(file.FullName).ToObject<RewardConditionList>();
                    ExtraCondition.AddRange(list.RCs);
                }
                catch (Exception ex)
                {
                    Debug.Error("RewardCondition", ex);
                }
            }
        }
    }
}
