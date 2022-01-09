using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UI;
using UnityEngine;

namespace Contingecy_Contract
{
    public class ContingecyContractGUI : MonoBehaviour
    {
        public bool GUIbool;
        public int Level;
        public string Itemname;
        public string language;
        private Rect CCTitleRect;
        private Rect CCTableRect;
        private Rect windowRect;
        private Rect CCDescRect;
        private Rect CCItemTableRect;
        private Vector2 _Position;
        public List<int> nowlevel = new List<int>();
        public List<Contract> ContractList = new List<Contract>();
        public string lastDesc;
        public void Start()
        {
            ComputeRect();
            lastDesc = "";
            Itemname = string.Empty;
        }
        public void Update()
        {
            if (UI.UIController.Instance.CurrentUIPhase == UIPhase.Invitation || UI.UIController.Instance.CurrentUIPhase == UIPhase.Sephirah || !UIPopupWindow.IsOpened())
            {
                if (!( Input.GetKeyDown(KeyCode.F9) || ( GUIbool && Input.GetKeyDown(KeyCode.Escape) ) ) )
                {
                    return;
                }
                if (GUIbool)
                    OnClose();
                else
                    OnOpen();
                GUIbool = !GUIbool;
            }
            else
                GUIbool = false;
        }
        private void OnOpen()
        {
            language = TextDataModel.CurrentLanguage.EndsWith("cn") ? "cn": "en";
            ContractList.Clear();
            foreach (Contract cc in StaticDataManager.JsonList)
            {
                cc.isConflict = false;
                cc.isOn = false;
            }
            foreach (Contract cc in Singleton<ContractLoader>.Instance.GetPassiveList())
                SetCC(cc.Id, cc.Type);
            foreach (Contract cc in Singleton<ContractLoader>.Instance.GetStageList())
                SetCC(cc.Id, cc.Type);
            nowlevel.Clear();
            lastDesc = "";
            Itemname = string.Empty;
            Level = GetLevel();
        }
        private void OnClose()
        {
            Debug.Log("Saving");
            File.WriteAllText(Harmony_Patch.ModPath + "/ContractLoader.txt", "");
            foreach (Contract cc in ContractList)
                File.AppendAllText(Harmony_Patch.ModPath + "/ContractLoader.txt", cc.Id + "\n");
            Debug.Log("SaveComplete");
            Singleton<ContractLoader>.Instance.Init();
        }
        private void OnGUI()
        {
            if (GUIbool)
                windowRect = GUI.Window(20210219, windowRect, new GUI.WindowFunction(DoMyWindow), "", new GUIStyle()
                {
                    normal = new GUIStyleState()
                    {
                        textColor = new Color32(47, 53, 66, 1),
                        background = BaseMod.Harmony_Patch.ArtWorks[language+ "_CCGUI_Background"].texture
                    },
                    wordWrap = true,
                    alignment = TextAnchor.UpperCenter
                }) ;
            ChangeToGame();
        }
        private void ComputeRect()
        {
            int num1 = Math.Min(Screen.width, 1100);
            int num2 = Math.Min(Screen.height, 700);
            windowRect = new Rect(Mathf.RoundToInt((Screen.width - num1) / 2f), Mathf.RoundToInt((Screen.height - num2) / 2f), num1, (float)num2);
            CCTitleRect = windowRect;
            CCTitleRect.position = Vector2.zero;
            CCTableRect = new Rect(0.0f, 120f, 1100f, 60f);;
            CCItemTableRect = new Rect(0.0f, 180f, 1100f, 360f);
            CCDescRect = new Rect(0.0f, 550f, 1100f, 150f);
        }
        private void CCTitle(Rect HeaderTitleRect)
        {
            GUIStyle style = new GUIStyle()
            {
                normal = new GUIStyleState()
                {
                    textColor = Color.red,
                    background = BaseMod.Harmony_Patch.ArtWorks["CCGUI_null"].texture
                },
                wordWrap = true,
                alignment = TextAnchor.MiddleCenter,
                fontSize = 20,
                margin = new RectOffset(10, 10, 10, 0)
            };
            GUILayout.BeginHorizontal();
            GUILayout.BeginArea(HeaderTitleRect);
            GUILayout.Label(lastDesc, style);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        private void CCTable(Rect HeaderTableRect)
        {
            GUIStyle style1 = new GUIStyle()
            {
                normal = new GUIStyleState()
                {
                    textColor = Color.white,
                    background = BaseMod.Harmony_Patch.ArtWorks["CCGUI_null"].texture
                },
                wordWrap = true,
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = 60f,
                fixedWidth = 150f,
                margin = new RectOffset(5, 5, 0, 0),
                border = new RectOffset(5, 5, 5, 5)
            };
            GUILayout.BeginArea(HeaderTableRect);
            GUILayout.BeginHorizontal(new GUIStyle()
            {
                alignment = TextAnchor.UpperLeft
            });
            if (GUILayout.Button(BaseMod.Harmony_Patch.ArtWorks[language + "_CCGUI_1stLevel"].texture, style1))
            {
                if (nowlevel.Contains(1))
                    nowlevel.Remove(1);
                else
                    nowlevel.Add(1);
            }
            if (GUILayout.Button(BaseMod.Harmony_Patch.ArtWorks[language + "_CCGUI_2ndLevel"].texture, style1))
            {
                if (nowlevel.Contains(2))
                    nowlevel.Remove(2);
                else
                    nowlevel.Add(2);
            }
            if (GUILayout.Button(BaseMod.Harmony_Patch.ArtWorks[language + "_CCGUI_3rdLevel"].texture, style1))
            {
                if (nowlevel.Contains(3))
                    nowlevel.Remove(3);
                else
                    nowlevel.Add(3);
            }
            if (GUILayout.Button(BaseMod.Harmony_Patch.ArtWorks[language + "_CCGUI_4thLevel"].texture, style1))
            {
                if (nowlevel.Contains(4))
                    nowlevel.Remove(4);
                else
                    nowlevel.Add(4);
            }
            string text = Level.ToString();
            GUIStyle style2 = new GUIStyle
            {
                fixedWidth = 225f,
                fixedHeight = 60f,
                alignment = TextAnchor.MiddleRight,
                margin = new RectOffset(5, 5, 0, 0),
                border = new RectOffset(5, 5, 5, 5),
                normal = new GUIStyleState()
                {
                    textColor = Color.white,
                    background = BaseMod.Harmony_Patch.ArtWorks[language + "_CCGUI_Level"].texture
                },
                fontSize = 24
            };
            GUILayoutOption[] guiLayoutOptionArray1 = new GUILayoutOption[0];
            GUILayout.Label(text, style2, guiLayoutOptionArray1);
            string itemname = Itemname;
            GUIStyle style3 = new GUIStyle
            {
                fixedWidth = 225f,
                fixedHeight = 60f,
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(5, 5, 0, 0),
                border = new RectOffset(5, 5, 5, 5),
                normal = new GUIStyleState()
                {
                    textColor = Color.white,
                    background = BaseMod.Harmony_Patch.ArtWorks["CCGUI_Search"].texture
                },
                fontSize = 24
            };
            GUILayoutOption[] guiLayoutOptionArray2 = new GUILayoutOption[0];
            Itemname = GUILayout.TextField(itemname, style3, guiLayoutOptionArray2);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        private void CCItemTable(Rect AddItemTableRect)
        {
            GUILayout.BeginArea(AddItemTableRect);
            _Position = GUILayout.BeginScrollView(_Position, false, false, GUILayout.Width(1050f), GUILayout.Height(360f));
            GUILayout.BeginHorizontal(new GUIStyle()
            {
                alignment = TextAnchor.UpperCenter
            });
            int num1 = 0;
            int num2 = 17;
            foreach (Contract cc in GetFiltered())
            {
                GUIStyle style = new GUIStyle
                {
                    normal = new GUIStyleState()
                    {
                        textColor = Color.white,
                        background = GetArtwork(cc.Id)
                    },
                    fixedHeight = 60f,
                    fixedWidth = 60f
                };
                if (GUILayout.Button(BaseMod.Harmony_Patch.ArtWorks[cc.isConflict ? "CC_Conflict" : (cc.isOn ? "CC_On" : "CCGUI_null")].texture, style))
                {
                    SetCC(cc.Id, cc.Type);
                    lastDesc = cc.isOn? cc.GetDesc().name + "\n" + cc.GetDesc().desc : "";
                    Level = GetLevel();
                }
                if ((num1 + 1) % num2 == 0)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(new GUIStyle()
                    {
                        alignment = TextAnchor.UpperLeft
                    });
                }
                ++num1;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        private void ChangeToGame()
        {
        }
        private void DoMyWindow(int winId)
        {
            CCTitle(CCDescRect);
            CCTable(CCTableRect);
            CCItemTable(CCItemTableRect);
            GUI.DragWindow();
        }
        public List<Contract> GetFiltered()
        {
            List<Contract> Filtered;
            if (nowlevel.Count <= 0)
                Filtered = new List<Contract>(StaticDataManager.JsonList);
            else
                Filtered = StaticDataManager.JsonList.FindAll(x => x.Level > 4 ? nowlevel.Contains(4) : nowlevel.Contains(x.Level));
            if (!string.IsNullOrEmpty(Itemname))
                Filtered = Filtered.FindAll(x => x.Id.Contains(Itemname) || x.GetDesc().name.Contains(Itemname));
            return Filtered;
        }
        public Texture2D GetArtwork(string id)
        {
            if (BaseMod.Harmony_Patch.ArtWorks.ContainsKey("CC_" + id))
                return BaseMod.Harmony_Patch.ArtWorks["CC_" + id].texture;
            else
                return BaseMod.Harmony_Patch.ArtWorks["CC_Default"].texture;
        }
        public void SetCC(string name, string type)
        {
            Contract cc = StaticDataManager.JsonList.Find(x => x.Id == name);
            cc.isOn = !cc.isOn;
            cc.isConflict = false;
            if (cc.isOn)
                ContractList.Add(cc);
            else
                ContractList.Remove(cc);
            List<Contract> all = StaticDataManager.JsonList.FindAll(y => y.Type == type || cc.Conflict.Contains(y.Type));
            for (int index = 0; index < all.Count; ++index)
            {
                if (all[index].Id != name)
                {
                    ContractList.Remove(all[index]);
                    all[index].isOn = false;
                    all[index].isConflict = cc.isOn;
                }
            }
        }
        public int GetLevel()
        {
            int i = 0;
            int b = 0;
            foreach (Contract contract in ContractList)
            {
                i += contract.Level;
                b += contract.Bonus;
            }
            return (int)Math.Floor(i * (1 + b * 0.01));
        }
    }
}
