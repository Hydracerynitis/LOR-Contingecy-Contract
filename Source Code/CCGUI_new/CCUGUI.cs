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
		private static CCRewardToggleGUI rewardButton;
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
        private bool IsAppropiate()
        {
			if (UI.UIController.Instance == null)
				return true;
            UIPhase CurrentPhase = UI.UIController.Instance.CurrentUIPhase;
            return CurrentPhase == UIPhase.DUMMY || CurrentPhase == UIPhase.BattleSetting || CurrentPhase == UIPhase.Sepiroth
				|| CurrentPhase==UIPhase.Main_ItemList || CurrentPhase==UIPhase.Librarian_CardList || CurrentPhase==UIPhase.Librarian;
        }
		public void UpdateRewardState()
		{
			if(rewardButton != null)
			{
                rewardButton.OnValueChange();
            }
		}
        public void Update()
		{
			if (!IsAppropiate() && !UIAlarmPopup.instance.IsOpened())
			{
				if (Input.GetKeyDown(KeyCode.F9))
				{
					CCGUI_Background.SetActive(!CCGUI_Background.activeSelf);
					if(CCGUI_Background.activeSelf)
						OnValueChanged();
				}
				else if (CCGUI_Background.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
						CCGUI_Background.SetActive(false);
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
				string lang= StaticDataManager.GetSupportedLanguage();
                if (language != lang)
                {
					language = lang;
					img_background.sprite = BH.ArtWorks[language + "_CCGUI_Background"];
					img_CCLevel.sprite = BH.ArtWorks[language + "_CCGUI_Level"];
					listeners.ForEach(x => x.UpdateLanguage(language));
					rewardButton.UpdateLanguage(language);
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
		private void AddLevelFilterUI(string backgroundImage,float x_displacement,int FilterVariant)
		{
            Image background = CCGUI_Background.CreateImage(language + backgroundImage);
            background.transform.localPosition = new Vector2(x_displacement, 163f);
            Image background_isOn = background.gameObject.CreateImage(CC_On);//选中
            background_isOn.transform.localScale = new Vector2(0.6f, 0.6f);
            background_isOn.transform.localPosition = new Vector2(-42.8f, 0.6f);
            CCLevelUGUI listener = background.gameObject.AddComponent<CCLevelUGUI>();
            listener.Init(background, background_isOn, FilterVariant);
            listeners.Add(listener);
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
            //合约奖励开关
            Image img_reward = background.CreateImage(language + "_CCGUI_Reward");
            img_reward.transform.localScale = new Vector2(1.5f, 1.5f);
            img_reward.transform.localPosition = new Vector2(205f, 233f);
			Image img_reward_on = img_reward.gameObject.CreateImage("tick");
			img_reward_on.transform.localScale = new Vector2(0.8f, 0.8f);
			img_reward_on.transform.localPosition = new Vector2(37.5f, 0f);
			CCRewardToggleGUI rewardToggle= img_reward.gameObject.AddComponent<CCRewardToggleGUI>();
			rewardToggle.Init(img_reward,img_reward_on);
            rewardButton = rewardToggle;
            //合约等级
            Image img_level = background.CreateImage(language+"_CCGUI_Level");
			img_level.transform.localScale = new Vector2(1.5f, 1.5f);
			img_level.transform.localPosition = new Vector2(369f, 233f);
			img_CCLevel = img_level;
			Text txt_level = new GameObject("CCGUI_Level_Text").AddComponent<Text>();
			txt_level.rectTransform.SetParent(img_level.transform);
			txt_level.alignment = TextAnchor.MiddleRight;
			txt_level.font = DefFont;
			txt_level.color = new Color(1f, 0.9333334f, 0.5490196f, 1f);
            txt_level.transform.localPosition = new Vector2(12f, -1f);
			txt_level.fontSize = 30;
			txt_level.text = alllevel.ToString();
			AllLevelUI = txt_level;
			//顶端栏
			//你应该为以下几个添加点击事件
			AddLevelFilterUI("_CCGUI_1stLevel", -412f, 1);
            AddLevelFilterUI("_CCGUI_2ndLevel", -292f, 2);
            AddLevelFilterUI("_CCGUI_3rdLevel", -172f, 3);
            AddLevelFilterUI("_CCGUI_4thLevel", -52f, 4);
            AddLevelFilterUI("_CCGUI_0thLevel", 68f, 0);
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
