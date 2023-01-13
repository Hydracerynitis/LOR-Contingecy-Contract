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
        public static Dictionary<string, GameObject> VanilaGameObject=new Dictionary<string, GameObject>();
        public static Dictionary<string, AudioClip> VanilaAudio = new Dictionary<string, AudioClip>();
        public static List<Contract> JsonList = new List<Contract>();
        public static Dictionary<LorId , Sprite> NonThumbSprite = new Dictionary<LorId, Sprite>();
        public static Dictionary<LorId, int> RewardDic = new Dictionary<LorId, int>();
        public static List<RewardConfition> ExtraCondition=new List<RewardConfition>();
        public static Dictionary<(string,int), string> RewardCondition= new Dictionary<(string, int), string>();
        public static Dictionary<(string, string), string> ContractParam = new Dictionary<(string, string), string>();
        public static AudioClip[] reverberation;
        public static void LoadStaticData()
        {
            LoadContractParam();
            LoadContract();
            InitNonThumbDic();
            LoadReward();
            LoadExtraCondition();
            LoadGameObject();
        }
        public static void LoadContract()
        {

            Debug.PathDebug("/Contracts", PathType.Directory);
            Debug.XmlFileDebug("/Contracts");
            foreach (FileInfo file in new DirectoryInfo(CCInitializer.ModPath + "/Contracts").GetFiles())
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
                                bluePrint.desc.ForEach(x => item.desc.Add(new ContractDesc() { language = x.language, name = x.name + " " + i.ToString(), desc = string.Format(x.desc, contract.GetFormatParam(x.language)) }));
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
        }
        public static void InitNonThumbDic()
        {
            foreach (int i in CCInitializer.NoThumbPage)
                NonThumbSprite.Add(Tools.MakeLorId(i), null);
        }
        public static void LoadReward()
        {
            Debug.PathDebug("/Staticinfo/RewardList", PathType.Directory);
            Debug.XmlFileDebug("/Staticinfo/RewardList");
            foreach (FileInfo file in new DirectoryInfo(CCInitializer.ModPath + "/Staticinfo/RewardList").GetFiles())
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
            foreach (string lang in TextDataModel._supported)
            {
                string path = CCInitializer.ModPath + "/Localize/" + lang + "/RewardCondition";
                if (!Directory.Exists(path))
                    continue;
                foreach (FileInfo file in new DirectoryInfo(path).GetFiles())
                {
                    try
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(File.ReadAllText(file.FullName));
                        foreach (XmlNode node in xml.SelectNodes("RewardCondition/Reward"))
                        {
                            string id = string.Empty;
                            if (node.Attributes.GetNamedItem("id") != null)
                                id = node.Attributes.GetNamedItem("id").InnerText;
                            int key = Int32.Parse(id);
                            RewardCondition[(lang,key)] = node.InnerText;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Error("RewardConditionError", ex);
                    }
                }
            }     
        }
        public static void LoadExtraCondition()
        {
            Debug.PathDebug("/Staticinfo/ExtraCondition", PathType.Directory);
            Debug.XmlFileDebug("/Staticinfo/ExtraCondition");
            foreach (FileInfo file in new DirectoryInfo(CCInitializer.ModPath + "/Staticinfo/ExtraCondition").GetFiles())
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
        public static void LoadContractParam()
        {
            Debug.PathDebug("/Contracts/ContractParams", PathType.Directory);
            Debug.XmlFileDebug("/Contracts/ContractParams");
            foreach (FileInfo file in new DirectoryInfo(CCInitializer.ModPath + "/Contracts/ContractParams").GetFiles())
            {
                try
                {
                    ParamList list = File.ReadAllText(file.FullName).ToObject<ParamList>();
                    foreach(Params p in list.Ps)
                        p.Desc.ForEach(x => ContractParam.Add((p.Id, x.Language), x.Content));
                }
                catch (Exception ex)
                {
                    Debug.Error("ContractParam", ex);
                }
            }
        }
        public static void LoadGameObject()
        {
            GameObject BSA = Resources.Load<GameObject>("Prefabs/InvitationMaps/InvitationMap_BlackSilence4");
            VanilaGameObject.Add("BlackSilence4Aura", ((BlackSilence4thMapManager)BSA.GetComponent<MapManager>()).areaAuraEffect);
            VanilaGameObject.Add("BlackSilence4Boom", ((BlackSilence4thMapManager)BSA.GetComponent<MapManager>()).areaBoomEffect);
            UnityEngine.Object.Destroy(BSA);
            GameObject FAEO = Util.LoadPrefab("Battle/SpecialEffect/FarAreaEffect_Oswald1st");
            VanilaAudio.Add("ClownClip", FAEO.GetComponent<FarAreaEffect_Oswald>()._curtainUpSound);
            UnityEngine.Object.Destroy(FAEO);
            reverberation = new AudioClip[4] { Resources.Load<AudioClip>("Sounds/Battle/Reverberation1st_Asiyah"),
            Resources.Load<AudioClip>("Sounds/Battle/Reverberation1st_Briah"),Resources.Load<AudioClip>("Sounds/Battle/Reverberation1st_Atziluth"),
            Resources.Load<AudioClip>("Sounds/Battle/Reverberation1st_Argalia")};
        }
        public static string GetParam(string ID, string Language)
        {
            if (Language == "en" || Language == "kr" || Language == "jp")
                return ContractParam[(ID, "en")];
            else if (Language == "cn" || Language == "trcn")
                return ContractParam[(ID, "cn")];
            return "";
        }
    }
}
