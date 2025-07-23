using BaseMod;
using LitJson;
using MyJsonTool;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
        public static List<Contract> ContractList = new List<Contract>();
        public static Dictionary<LorId , Sprite> NonThumbSprite = new Dictionary<LorId, Sprite>();
        public static Dictionary<LorId, LorId> RewardDic = new Dictionary<LorId, LorId>();
        public static List<RewardConfig> ExtraCondition=new List<RewardConfig>();
        public static Dictionary<string,Dictionary<LorId,string>> RewardCondition= new Dictionary<string, Dictionary<LorId, string>>();
        public static Dictionary<(string, string), string> ContractParam = new Dictionary<(string, string), string>();
        public static List<RewardClearList> rewardClearLists = new List<RewardClearList>();
        public static AudioClip[] reverberation;
        public static List<LorId> Glossary
        {
            get
            {
                if(RewardCondition.ContainsKey(GetSupportedLanguage()))
                    return RewardCondition[GetSupportedLanguage()].Keys.ToList();
                else
                    return new List<LorId>();
            }
        }
        public static System.Type GetContingencyContract(string ContractType)
        {
            return System.Type.GetType("Contingecy_Contract.ContingecyContract_" + ContractType);
        }
        public static System.Type GetStageModifier(string ContractType)
        {
            return System.Type.GetType("Contingecy_Contract.StageModifier_" + ContractType);
        }
        public static string GetSupportedLanguage()
        {
            return TextDataModel.CurrentLanguage.EndsWith("cn") ? "cn" : "en";
        }
        public static void LoadStaticData(string dllPath)
        {
            LoadContractParam(dllPath);
            LoadContract(dllPath);
            LoadReward(dllPath);
            LoadExtraCondition(dllPath);
            LoadClearList(dllPath);
        }
        public static void LoadContract(string dllPath)
        {
            foreach (FileInfo file in new DirectoryInfo(dllPath + "/Contracts").GetFiles())
            {
                try
                {
                    ContractBluePrintCollection list = File.ReadAllText(file.FullName).ToObject<ContractBluePrintCollection>();
                    foreach (ContractBluePrint bluePrint in list.CCs)
                    {
                        System.Type type = GetContingencyContract(bluePrint.Type);
                        if (type == null)
                            continue;
                        ContingecyContract contract = Activator.CreateInstance(type, new object[] { }) as ContingecyContract;
                        contract.Level = 0;
                        if (bluePrint.Variation <= 0)
                        {
                            
                            Contract item = new Contract()
                            {
                                Type = bluePrint.Type,
                                Variant = 0,
                                contractType = bluePrint.contractType,
                                Faction = bluePrint.Faction,
                                Level = bluePrint.BaseLevel,
                                Pid = bluePrint.Pid,
                                Stageid = bluePrint.Stageid,
                                Conflict = bluePrint.Conflict
                            };
                            bluePrint.desc.ForEach(x => item.desc.Add(new ContractDesc() { language = x.language, name = x.name, desc = string.Format(x.desc, contract.GetFormatParam(x.language)) }));
                            ContractList.Add(item);
                            Debug.Log("Contract: {0} Added", bluePrint.Type);
                        }
                        {
                            for (int i = 1; i <= bluePrint.Variation; i++)
                            {
                                contract.Level = i;
                                Contract item = new Contract()
                                {
                                    Type = bluePrint.Type,
                                    Variant = i,
                                    contractType = bluePrint.contractType,
                                    Faction = bluePrint.Faction,
                                    Level = bluePrint.BaseLevel + i * bluePrint.Step,
                                    Stageid = bluePrint.Stageid,
                                    Pid = bluePrint.Pid,
                                    Conflict = bluePrint.Conflict
                                };
                                bluePrint.desc.ForEach(x => item.desc.Add(new ContractDesc() { language = x.language, name = x.name + " " + i.ToString(), desc = string.Format(x.desc, contract.GetFormatParam(x.language)) }));
                                ContractList.Add(item);
                                Debug.Log("Contract: {0} Added", item.Id);
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
        public static void LoadReward(string dllPath)
        {
            foreach (FileInfo file in new DirectoryInfo(dllPath + "/Contracts/ContractReward/RewardList").GetFiles())
            {
                try
                {
                    ContractRewardCollection list = JsonMapper.ToObject<ContractRewardCollection>(File.ReadAllText(file.FullName));
                    foreach (ContractReward CR in list.CRs)
                        RewardDic.Add(new LorId(CR.stagePid,CR.stageId), new LorId(CR.rewardPid,CR.rewardId));
                }
                catch (Exception ex)
                {
                    Debug.Error("Reward", ex);
                }
            }
            foreach (string lang in TextDataModel._supported)
            {
                RewardCondition[lang] = new Dictionary<LorId, string>();
                string path = dllPath + "/Contracts/ContractReward/RewardCondition/"+lang;
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
                            string Pid = "";
                            if (node.Attributes.GetNamedItem("Pid") != null)
                                Pid = node.Attributes.GetNamedItem("Pid").InnerText;
                            string id = string.Empty;
                            if (node.Attributes.GetNamedItem("id") != null)
                                id = node.Attributes.GetNamedItem("id").InnerText;
                            LorId key = new LorId(Pid, Int32.Parse(id));
                            RewardCondition[lang][key] = node.InnerText;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Error("RewardConditionError", ex);
                    }
                }
            }     
        }
        public static void LoadExtraCondition(string dllPath)
        {
            foreach (FileInfo file in new DirectoryInfo(dllPath + "/Contracts/ContractReward/ExtraCondition").GetFiles())
            {
                try
                {
                    RewardConditionCollection list = File.ReadAllText(file.FullName).ToObject<RewardConditionCollection>();
                    ExtraCondition.AddRange(list.RCs);
                }
                catch (Exception ex)
                {
                    Debug.Error("RewardCondition", ex);
                }
            }
        }
        public static void LoadContractParam(string dllPath)
        {
            foreach (FileInfo file in new DirectoryInfo(dllPath + "/Contracts/ContractParams").GetFiles())
            {
                try
                {
                    ParamCollection list = File.ReadAllText(file.FullName).ToObject<ParamCollection>();
                    foreach(Params p in list.Ps)
                        p.Desc.ForEach(x => ContractParam.Add((p.Id, x.Language), x.Content));
                }
                catch (Exception ex)
                {
                    Debug.Error("ContractParam", ex);
                }
            }
        }
        public static void LoadClearList(string dllPath)
        {
            foreach (FileInfo file in new DirectoryInfo(dllPath + "/Contracts/ContractReward/ClearList").GetFiles())
            {
                try
                {
                    ClearListCollection list = JsonMapper.ToObject<ClearListCollection>(File.ReadAllText(file.FullName));
                    rewardClearLists.AddRange(list.RCLs);
                }
                catch (Exception ex)
                {
                    Debug.Error("ClearList", ex);
                }
            }
        }
        public static void LoadGameObject()
        {
            GameObject BSA = Util.LoadPrefab("InvitationMaps/InvitationMap_BlackSilence4");
            VanilaGameObject.Add("BlackSilence4Aura", ((BlackSilence4thMapManager)BSA.GetComponent<MapManager>()).areaAuraEffect);
            VanilaGameObject.Add("BlackSilence4Boom", ((BlackSilence4thMapManager)BSA.GetComponent<MapManager>()).areaBoomEffect);
            GameObject.Destroy(BSA);
            GameObject FAEO = Util.LoadPrefab("Battle/SpecialEffect/FarAreaEffect_Oswald1st");
            VanilaAudio.Add("ClownClip", FAEO.GetComponent<FarAreaEffect_Oswald>()._curtainUpSound);
            GameObject.Destroy(FAEO);
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
