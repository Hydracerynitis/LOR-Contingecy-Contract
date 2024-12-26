using System;
using UI;
using BaseMod;
using UnityEngine;
using UnityEngine.UI;
using BH = BaseMod.Harmony_Patch;
using System.IO;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Contingecy_Contract
{
	public class CCUGUI : MonoBehaviour,IDragHandler
	{
		private int alllevel;
		public string language;
		private static CCUGUI _instance;
		private Color color = new Color(0f, 0f, 0f, 0.01f);
		public static Sprite CC_Null;
		public static Sprite CC_On;
		public static Sprite CC_Conflict;
		public static Sprite CC_Error;
		//ScrollList
		private Scrollbar scrollList;
		//Tag列表
		public static GameObject CCItemViewport;
		//主界面
		public static GameObject CCGUI_Background;
        //描述
        public static GameObject DescUI;
        //更新贴图
        private static Image img_background;
		private static Image img_CCLevel;
		private static List<CCLevelUGUI> listeners = new List<CCLevelUGUI>();
		public static InputField search;
		private Text AllLevelUI;
		private string nowdesc;
		private Text NowDescUI;
		public string NowDesc
		{
			get
			{
				return nowdesc;
			}
			set
			{
				nowdesc = value;
				if (NowDescUI)
				{
					NowDescUI.text = nowdesc;
				}
			}
		}
		public static CCUGUI Instance
		{
			get
			{
				if (!_instance)
				{
					_instance = MyUITool.RegisterUI("CCUGUI", null).AddComponent<CCUGUI>();
				}
				return _instance;
			}
			set
			{
				if (_instance)
				{
					Destroy(_instance.gameObject);
				}
				_instance = value;
			}
		}
		public static Font DefFont
		{
			get
			{
				if (!UtilTools.DefFont)
				{
					UtilTools.DefFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
				}
				return UtilTools.DefFont;
			}
		}
		static CCUGUI()
		{
			CCUGUI instance = Instance;
		}
		public void OnDrag(PointerEventData eventData){
			img_background.rectTransform.anchoredPosition += eventData.delta;
        }
		public void Start()
		{
		}
        private bool IsBattleSetting()
        {
			if (UI.UIController.Instance == null)
				return true;
            UIPhase CurrentPhase = UI.UIController.Instance.CurrentUIPhase;
            return CurrentPhase == UIPhase.DUMMY || CurrentPhase == UIPhase.BattleSetting || CurrentPhase == UIPhase.Sepiroth;
        }
        public void Update()
		{
			if (!IsBattleSetting() && !UIPopupWindow.IsOpened())
			{
				if (Input.GetKeyDown(KeyCode.F9))
				{
					CCGUI_Background.SetActive(!CCGUI_Background.activeSelf);
					OnValueChanged();
				}
				else if (CCGUI_Background.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
						CCGUI_Background.SetActive(false);
						OnValueChanged();
					}
					else if (Input.GetAxis("Mouse ScrollWheel")!=0)
                    {
						scrollList.value += Input.GetAxis("Mouse ScrollWheel");

					}
					else if (Input.GetKeyDown(KeyCode.F7))
                    {
						CCManager.Clear();
                    }
                }
			}
			else
			{
				CCGUI_Background.SetActive(false);
			}
		}
		public void OnValueChanged()
		{
			if (CCGUI_Background.activeSelf)
            {
				string lang= TextDataModel.CurrentLanguage.EndsWith("cn") ? "cn" : "en";
                if (language != lang)
                {
					language = lang;
					img_background.sprite = BH.ArtWorks[language + "_CCGUI_Background"];
					img_CCLevel.sprite = BH.ArtWorks[language + "_CCGUI_Level"];
					listeners.ForEach(x => x.UpdateLanguage(language));
				}			
				CCManager.nowshowlevels.Clear();
				listeners.ForEach(x => x.OnEnable());
				NowDesc = "";
			}
            else
            {
				Debug.Log("Saving");
				File.WriteAllText(CCInitializer.ModPath + "/ContractLoader.txt", "");
				foreach (Contract cc in CCManager.OnContract)
					File.AppendAllText(CCInitializer.ModPath + "/ContractLoader.txt", cc.Id + "\n");
				Debug.Log("SaveComplete");
				Singleton<ContractLoader>.Instance.Init();
			}
        }
		public void SetGetLevel()
		{
            alllevel = 0;
			foreach (Contract contract in CCManager.OnContract)
			{
                alllevel += contract.Level;
			}
			if (AllLevelUI)
			{
				AllLevelUI.text = alllevel.ToString();
			}
		}
		public void MakeContractPanel()
		{
			//背景
			language = "cn";
			GameObject background = new GameObject("background");
			Image img_bg = background.AddComponent<Image>();
            img_bg.sprite = BH.ArtWorks[language+ "_CCGUI_Background"];
			img_bg.transform.SetParent(transform);
			img_bg.rectTransform.sizeDelta = new Vector2(960f, 540f);
			background.transform.localPosition = new Vector2(0f, 0f);
			img_background = img_bg;
			CCGUI_Background = background;
			//合约等级
			Image img_level = background.CreateImage(language+"_CCGUI_Level");
			img_level.transform.localScale = new Vector2(1.5f, 1.5f);
			img_level.transform.localPosition = new Vector2(369f, 233f);
			img_CCLevel = img_level;
			Text txt_level = new GameObject("CCGUI_Level_Text").AddComponent<Text>();
			txt_level.rectTransform.SetParent(img_level.transform);
			txt_level.font = DefFont;
			txt_level.color = Color.white;
			txt_level.transform.localPosition = new Vector2(66.5f, -28.6f);
			txt_level.text = alllevel.ToString();
			AllLevelUI = txt_level;
			//顶端栏
			//你应该为以下几个添加点击事件
			Image img_1level = CCGUI_Background.CreateImage(language+"_CCGUI_1stLevel");
			img_1level.transform.localPosition = new Vector2(-412f, 163f);
			Image img_1level_isOn = img_1level.gameObject.CreateImage(CC_On);//选中
			img_1level_isOn.transform.localScale = new Vector2(0.6f, 0.6f);
			img_1level_isOn.transform.localPosition = new Vector2(-42.8f, 0.6f);
			CCLevelUGUI listener = img_1level.gameObject.AddComponent<CCLevelUGUI>();
			listener.Init(img_1level, img_1level_isOn, 1);
			listeners.Add(listener);
			Image img_2level = CCGUI_Background.CreateImage(language+"_CCGUI_2ndLevel");
			img_2level.transform.localPosition = new Vector2(-293f, 163f);
			Image img_2level_isOn = img_2level.gameObject.CreateImage(CC_On);
			img_2level_isOn.transform.localScale = new Vector2(0.6f, 0.6f);
			img_2level_isOn.transform.localPosition = new Vector2(-42.8f, 0.6f);
			listener = img_2level.gameObject.AddComponent<CCLevelUGUI>();
			listener.Init(img_2level, img_2level_isOn, 2);
			listeners.Add(listener);
			Image img_3level = CCGUI_Background.CreateImage(language+"_CCGUI_3rdLevel");
			img_3level.transform.localPosition = new Vector2(-177f, 163f);
			Image img_3level_isOn = img_3level.gameObject.CreateImage(CC_On);
			img_3level_isOn.transform.localScale = new Vector2(0.6f, 0.6f);
			img_3level_isOn.transform.localPosition = new Vector2(-42.8f, 0.6f);
			listener = img_3level.gameObject.AddComponent<CCLevelUGUI>();
			listener.Init(img_3level, img_3level_isOn, 3);
			listeners.Add(listener);
			Image img_4level = CCGUI_Background.CreateImage(language+"_CCGUI_4thLevel");
			img_4level.transform.localPosition = new Vector2(-59f, 163f);
			Image img_4level_isOn = img_4level.gameObject.CreateImage(CC_On);
			img_4level_isOn.transform.localScale = new Vector2(0.6f, 0.6f);
			img_4level_isOn.transform.localPosition = new Vector2(-42.8f, 0.6f);
			listener = img_4level.gameObject.AddComponent<CCLevelUGUI>();
			listener.Init(img_4level, img_4level_isOn, 4);
			listeners.Add(listener);
			//搜索框
			Image img_search = CCGUI_Background.CreateImage("CCGUI_Search");
            img_search.transform.localPosition = new Vector2(45.5f, 164f);
            InputField inputField = img_search.gameObject.CreateInputField("InputField", "...");
			inputField.placeholder.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            inputField.transform.localPosition = new Vector2(79f, 0.6f);
            inputField.targetGraphic.enabled = false;
			inputField.lineType = InputField.LineType.SingleLine;
			inputField.onValueChanged.AddListener(delegate { CCManager.FilterList(); });
			search = inputField;
			//描述栏
			DescUI = new GameObject("DescUI");
			DescUI.transform.SetParent(CCGUI_Background.transform);
			DescUI.transform.localPosition = new Vector2(0f, -370f);
            Image img_desc = DescUI.AddComponent<Image>();
            img_desc.sprite = BH.ArtWorks["Desc_Background"];
            img_desc.transform.SetParent(DescUI.transform);
            img_desc.rectTransform.sizeDelta = new Vector2(960f, 200f);

			GameObject DescText = new GameObject("DescText");
            DescText.transform.SetParent(DescUI.transform);
            Text txt_desc = DescText.AddComponent<Text>();
			txt_desc.rectTransform.sizeDelta = Vector2.zero;
			txt_desc.rectTransform.anchorMin = new Vector2(0.05f, 0.05f);
			txt_desc.rectTransform.anchorMax = new Vector2(0.95f, 0.95f);
			txt_desc.rectTransform.anchoredPosition = new Vector2(0f, 0f);
			txt_desc.text = NowDesc;
			txt_desc.font = DefFont;
			txt_desc.fontSize = 20;
			txt_desc.color = Color.red; //0.25 1 0.65
			txt_desc.alignment = TextAnchor.UpperCenter;
			NowDescUI = txt_desc;
			//内容框-本体
			GameObject ItemT = new GameObject("CCItemTable");
			Image img_tablebg = ItemT.AddComponent<Image>();
			img_tablebg.rectTransform.sizeDelta = new Vector2(950f, 378f);
			img_tablebg.transform.SetParent(CCGUI_Background.transform);
			img_tablebg.color = color;
			ItemT.transform.localPosition = new Vector2(0f, -63f);
			ItemT.AddComponent<Mask>();
			//内容框-Scroll Bar
			ItemT.CreateScrollbar("CCbar", out Scrollbar scrollbar);
			scrollbar.SetDirection(Scrollbar.Direction.BottomToTop, true);
			MyUITool.SetColorBlock(scrollbar, new Color?(new Color(0.45f, 0.45f, 0.45f)), new Color?(new Color(0.6f, 0.6f, 0.6f)), new Color?(new Color(0.4f, 0.4f, 0.4f)), null);
			scrollbar.value = 0f;
			scrollbar.size = 0.1f;
			(scrollbar.transform as RectTransform).sizeDelta = new Vector2(20f, 400f);
			scrollbar.transform.localPosition = new Vector2(440f, 0f);
			scrollList = scrollbar;
			//内容框-填充栏
			GameObject ItemViewPort = new GameObject("CCItemViewport");
			Image img_ivp = ItemViewPort.AddComponent<Image>();
			img_ivp.transform.SetParent(ItemT.transform);
			img_ivp.rectTransform.sizeDelta = new Vector2(890f, 1000f);
			ItemViewPort.transform.localPosition = new Vector2(0f, -321f);
			img_ivp.color = color;
			//内容框-填充栏-Grid Layout
			GridLayoutGroup gridLayoutGroup = ItemViewPort.AddComponent<GridLayoutGroup>();
			gridLayoutGroup.cellSize = new Vector2(50f, 50f);
			gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
			gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
			gridLayoutGroup.childAlignment = TextAnchor.UpperLeft;
			gridLayoutGroup.constraint = GridLayoutGroup.Constraint.Flexible;
			gridLayoutGroup.spacing = new Vector2(10f, 10f);
			gridLayoutGroup.SetLayoutVertical();
			//内容框-填充栏-Scroll Rect
			ScrollRect scrollRect = ItemViewPort.AddComponent<ScrollRect>();
            scrollRect.content = img_ivp.rectTransform;
            scrollRect.viewport = img_tablebg.rectTransform;
            scrollRect.movementType = ScrollRect.MovementType.Elastic;
            scrollRect.elasticity = 0.1f;
            scrollRect.inertia = true;
            scrollRect.scrollSensitivity = 5f;
            scrollRect.verticalScrollbar = scrollbar;
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.verticalScrollbarSpacing = -3f;
            CCItemViewport = ItemViewPort;
		}
	}
}
