using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using BH = BaseMod.Harmony_Patch;

namespace Contingecy_Contract
{
	public class BaseUI : MonoBehaviour
	{
		public static Dictionary<string, Action> Updates = new Dictionary<string, Action>();
		public void Update()
		{
			foreach (Action action in Updates.Values)
			{
				action();
			}
		}
	}
	public static class MyUITool
	{
		public static Vector2 smallElementSize = new Vector2(25f, 25f);
		public static readonly Font DefaultFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
		public static readonly Color disabledButtonColor = new Color(0.25f, 0.25f, 0.25f);
		public static GameObject CreateUIObject(string name, GameObject parent, Vector2 size = default(Vector2))
		{
			GameObject gameObject = new GameObject(name)
			{
				layer = 5,
				hideFlags = HideFlags.HideAndDontSave
			};
			if (parent)
			{
				gameObject.transform.SetParent(parent.transform, false);
			}
			gameObject.AddComponent<RectTransform>().sizeDelta = size;
			return gameObject;
		}
		public static GameObject CanvasRoot { get; private set; }
		private static void CreateRootCanvas()
		{
			CanvasRoot = new GameObject("CCUGUILibCanvas");
			CanvasRoot.AddComponent<BaseUI>();
			UnityEngine.Object.DontDestroyOnLoad(CanvasRoot);
			CanvasRoot.hideFlags |= HideFlags.HideAndDontSave;
			CanvasRoot.layer = 5;
			CanvasRoot.transform.position = new Vector3(0f, 0f, 1f);
			CanvasRoot.SetActive(false);
			CanvasRoot.AddComponent<EventSystem>().enabled = false;
			CanvasRoot.SetActive(true);
		}
		static MyUITool()
		{
			CreateRootCanvas();
		}
		public static GameObject RegisterUI(string id, Action updateMethod = null)
		{
			GameObject gameObject = CreateUIObject(id + "_Root", CanvasRoot, default(Vector2));
			gameObject.SetActive(false);
			Canvas canvas = gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceCamera;
			canvas.referencePixelsPerUnit = 100f;
			canvas.sortingOrder = 999;
			CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
			canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
			canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
			gameObject.AddComponent<GraphicRaycaster>();
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.pivot = new Vector2(0.5f, 0.5f);
			if (updateMethod != null)
			{
				BaseUI.Updates[id] = updateMethod;
			}
			gameObject.SetActive(true);
			gameObject.transform.SetParent(CanvasRoot.transform, false);
			return gameObject;
		}
		public static LayoutElement SetLayoutElement(GameObject gameObject, int? minWidth = null, int? minHeight = null, int? flexibleWidth = null, int? flexibleHeight = null, int? preferredWidth = null, int? preferredHeight = null, bool? ignoreLayout = null)
		{
			LayoutElement layoutElement = gameObject.GetComponent<LayoutElement>();
			if (!layoutElement)
			{
				layoutElement = gameObject.AddComponent<LayoutElement>();
			}
			if (minWidth != null)
			{
				layoutElement.minWidth = (float)minWidth.Value;
			}
			if (minHeight != null)
			{
				layoutElement.minHeight = (float)minHeight.Value;
			}
			if (flexibleWidth != null)
			{
				layoutElement.flexibleWidth = (float)flexibleWidth.Value;
			}
			if (flexibleHeight != null)
			{
				layoutElement.flexibleHeight = (float)flexibleHeight.Value;
			}
			if (preferredWidth != null)
			{
				layoutElement.preferredWidth = (float)preferredWidth.Value;
			}
			if (preferredHeight != null)
			{
				layoutElement.preferredHeight = (float)preferredHeight.Value;
			}
			if (ignoreLayout != null)
			{
				layoutElement.ignoreLayout = ignoreLayout.Value;
			}
			return layoutElement;
		}
		public static T SetLayoutGroup<T>(GameObject gameObject, bool? forceWidth = null, bool? forceHeight = null, bool? childControlWidth = null, bool? childControlHeight = null, int? spacing = null, int? padTop = null, int? padBottom = null, int? padLeft = null, int? padRight = null, TextAnchor? childAlignment = null) where T : HorizontalOrVerticalLayoutGroup
		{
			T t = gameObject.GetComponent<T>();
			if (!t)
			{
				t = gameObject.AddComponent<T>();
			}
			return SetLayoutGroup<T>(t, forceWidth, forceHeight, childControlWidth, childControlHeight, spacing, padTop, padBottom, padLeft, padRight, childAlignment);
		}
		public static T SetLayoutGroup<T>(T group, bool? forceWidth = null, bool? forceHeight = null, bool? childControlWidth = null, bool? childControlHeight = null, int? spacing = null, int? padTop = null, int? padBottom = null, int? padLeft = null, int? padRight = null, TextAnchor? childAlignment = null) where T : HorizontalOrVerticalLayoutGroup
		{
			if (forceWidth != null)
			{
				group.childForceExpandWidth = forceWidth.Value;
			}
			if (forceHeight != null)
			{
				group.childForceExpandHeight = forceHeight.Value;
			}
			if (childControlWidth != null)
			{
				group.childControlWidth = childControlWidth.Value;
			}
			if (childControlHeight != null)
			{
				group.childControlHeight = childControlHeight.Value;
			}
			if (spacing != null)
			{
				group.spacing = (float)spacing.Value;
			}
			if (padTop != null)
			{
				group.padding.top = padTop.Value;
			}
			if (padBottom != null)
			{
				group.padding.bottom = padBottom.Value;
			}
			if (padLeft != null)
			{
				group.padding.left = padLeft.Value;
			}
			if (padRight != null)
			{
				group.padding.right = padRight.Value;
			}
			if (childAlignment != null)
			{
				group.childAlignment = childAlignment.Value;
			}
			return group;
		}
		public static void SetColorBlock(Selectable selectable, Color? normal = null, Color? highlighted = null, Color? pressed = null, Color? disabled = null)
		{
			ColorBlock colors = selectable.colors;
			if (normal != null)
			{
				colors.normalColor = normal.Value;
			}
			if (highlighted != null)
			{
				colors.highlightedColor = highlighted.Value;
			}
			if (pressed != null)
			{
				colors.pressedColor = pressed.Value;
			}
			if (disabled != null)
			{
				colors.disabledColor = disabled.Value;
			}
			selectable.colors = colors;
		}
		public static void SetDefaultTextValues(Text text)
		{
			text.color = Color.white;
			text.font = DefaultFont;
			text.fontSize = 14;
		}
		public static InputField CreateInputField(this GameObject parent, string name = "InputField", string placeHolderText = "...")
		{
			GameObject gameObject = CreateUIObject(name, parent, default(Vector2));
			Image image = gameObject.AddComponent<Image>();
			image.type = Image.Type.Sliced;
			image.color = new Color(0f, 0f, 0f, 0.5f);
			InputField inputField = gameObject.AddComponent<InputField>();
			Navigation navigation = inputField.navigation;
			navigation.mode = Navigation.Mode.None;
			inputField.navigation = navigation;
			inputField.lineType = InputField.LineType.SingleLine;
			inputField.interactable = true;
			inputField.transition = Selectable.Transition.ColorTint;
			inputField.targetGraphic = image;
			SetColorBlock(inputField, new Color?(new Color(1f, 1f, 1f, 1f)), new Color?(new Color(0.95f, 0.95f, 0.95f, 1f)), new Color?(new Color(0.78f, 0.78f, 0.78f, 1f)), null);
			GameObject gameObject2 = CreateUIObject("TextArea", gameObject, default(Vector2));
			gameObject2.AddComponent<RectMask2D>();
			RectTransform component = gameObject2.GetComponent<RectTransform>();
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.offsetMin = Vector2.zero;
			component.offsetMax = Vector2.zero;
			GameObject gameObject3 = CreateUIObject("Placeholder", gameObject2, default(Vector2));
			Text text = gameObject3.AddComponent<Text>();
			SetDefaultTextValues(text);
			text.text = placeHolderText ?? "...";
			text.color = new Color(0.5f, 0.5f, 0.5f, 1f);
			text.horizontalOverflow = HorizontalWrapMode.Wrap;
			text.alignment = TextAnchor.MiddleLeft;
			text.fontSize = 14;
			RectTransform component2 = gameObject3.GetComponent<RectTransform>();
			component2.anchorMin = Vector2.zero;
			component2.anchorMax = Vector2.one;
			component2.offsetMin = Vector2.zero;
			component2.offsetMax = Vector2.zero;
			inputField.placeholder = text;
			GameObject gameObject4 = CreateUIObject("Text", gameObject2, default(Vector2));
			Text text2 = gameObject4.AddComponent<Text>();
			SetDefaultTextValues(text2);
			text2.text = "";
			text2.color = new Color(1f, 1f, 1f, 1f);
			text2.horizontalOverflow = HorizontalWrapMode.Wrap;
			text2.alignment = TextAnchor.MiddleLeft;
			text2.fontSize = 14;
			RectTransform component3 = gameObject4.GetComponent<RectTransform>();
			component3.anchorMin = Vector2.zero;
			component3.anchorMax = Vector2.one;
			component3.offsetMin = Vector2.zero;
			component3.offsetMax = Vector2.zero;
			inputField.textComponent = text2;
			inputField.characterLimit = 16000;
			ContentSizeFitter contentSizeFitter = inputField.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			inputField.lineType = InputField.LineType.MultiLineNewline;
			int? num = new int?(25);
			int? num2 = new int?(200);
			int? num3 = num;
			int? num4 = null;
			int? num5 = null;
			int? num6 = null;
			int? num7 = null;
			SetLayoutElement(inputField.gameObject, num2, num3, num4, num5, num6, num7, null);
			return inputField;
		}

		public static Button CreateButton(this GameObject parent, string name, string text, Action OnClick = null, Color? normalColor = null)
		{
			ColorBlock colorBlock = default(ColorBlock);
			normalColor = new Color?(normalColor ?? new Color(0.25f, 0.25f, 0.25f));
			GameObject gameObject = CreateUIObject(name, parent, new Vector2(25f, 25f));
			GameObject gameObject2 = CreateUIObject("Text", gameObject, default(Vector2));
			Image image = gameObject.AddComponent<Image>();
			image.type = Image.Type.Sliced;
			image.color = new Color(1f, 1f, 1f, 1f);
			Button button = gameObject.AddComponent<Button>();
			Navigation navigation = button.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			button.navigation = navigation;
			SetColorBlock(button, new Color?(new Color(0.2f, 0.2f, 0.2f)), new Color?(new Color(0.3f, 0.3f, 0.3f)), new Color?(new Color(0.15f, 0.15f, 0.15f)), null);
			colorBlock.colorMultiplier = 1f;
			button.colors = colorBlock;
			Text text2 = gameObject2.AddComponent<Text>();
			text2.text = text;
			SetDefaultTextValues(text2);
			text2.alignment = TextAnchor.MiddleCenter;
			RectTransform component = gameObject2.GetComponent<RectTransform>();
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.sizeDelta = Vector2.zero;
			SetColorBlock(button, normalColor, normalColor * 1.2f, normalColor * 0.7f, null);
			Button button2 = button;
			gameObject = button2.gameObject;
			gameObject.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			SetLayoutGroup<HorizontalLayoutGroup>(gameObject, new bool?(false), new bool?(true), new bool?(true), new bool?(true), new int?(0), new int?(0), new int?(0), new int?(5), new int?(5), new TextAnchor?(TextAnchor.MiddleCenter));
			gameObject2 = gameObject;
			int? num = new int?(80);
			int? num2 = null;
			int? num3 = null;
			int? num4 = null;
			int? num5 = null;
			int? num6 = null;
			SetLayoutElement(gameObject2, num, num2, num3, num4, num5, num6, null);
			SetColorBlock(button2, new Color?(disabledButtonColor), new Color?(disabledButtonColor * 1.2f), null, null);
			button2.onClick.RemoveAllListeners();
			if (OnClick != null)
			{
				button2.onClick.AddListener(delegate()
				{
					OnClick();
				});
			}
			gameObject.transform.Find("Text").gameObject.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			return button2;
		}
		public static Image CreateCCIcon(this GameObject parent, string Id)
		{
			GameObject gameObject = new GameObject(Id);
			Image image = gameObject.AddComponent<Image>();
			image.transform.SetParent(parent.transform);
			gameObject.transform.localScale = new Vector2(1f, 1f);
			gameObject.transform.localPosition = new Vector2(0f, 0f);
			if (BH.ArtWorks.TryGetValue("CC_" + Id, out Sprite value))
				image.sprite = value;
			else
				image.sprite = CCUGUI.CC_Error;
			image.rectTransform.sizeDelta = new Vector2((float)image.sprite.texture.width, (float)image.sprite.texture.height);
			return image;
		}
		public static Image CreateImage(this GameObject parent, string ArtWork)
		{
			GameObject gameObject = new GameObject(ArtWork);
			Image image = gameObject.AddComponent<Image>();
			image.transform.SetParent(parent.transform);
			gameObject.transform.localScale = new Vector2(1f, 1f);
			gameObject.transform.localPosition = new Vector2(0f, 0f);
			image.sprite = BH.ArtWorks[ArtWork];
			image.rectTransform.sizeDelta = new Vector2((float)image.sprite.texture.width, (float)image.sprite.texture.height);
			return image;
		}
		public static Image CreateImage(this GameObject parent, Sprite ArtWork)
		{
			GameObject gameObject = new GameObject(ArtWork.name);
			Image image = gameObject.AddComponent<Image>();
			image.transform.SetParent(parent.transform);
			gameObject.transform.localScale = new Vector2(1f, 1f);
			gameObject.transform.localPosition = new Vector2(0f, 0f);
			image.sprite = ArtWork;
			image.rectTransform.sizeDelta = new Vector2((float)image.sprite.texture.width, (float)image.sprite.texture.height);
			return image;
		}
		public static GameObject CreateScrollbar(this GameObject parent, string name, out Scrollbar scrollbar)
		{
			GameObject self = CreateUIObject(name, parent, new Vector2(25f, 25f));
			GameObject slideArea = CreateUIObject("Sliding Area", self, default(Vector2));
			GameObject handle = CreateUIObject("Handle", slideArea, default(Vector2));
			Image image = self.AddComponent<Image>();
			image.type = Image.Type.Sliced;
			image.color = new Color(0.1f, 0.1f, 0.1f);
			Image image2 = handle.AddComponent<Image>();
			image2.type = Image.Type.Sliced;
			image2.color = new Color(0.4f, 0.4f, 0.4f);
			RectTransform rt_slideArea = slideArea.GetComponent<RectTransform>();
			rt_slideArea.sizeDelta = new Vector2(-20f, -20f);
			rt_slideArea.anchorMin = Vector2.zero;
			rt_slideArea.anchorMax = Vector2.one;
			RectTransform rt_handle = handle.GetComponent<RectTransform>();
			rt_handle.sizeDelta = new Vector2(20f, 20f);
			scrollbar = self.AddComponent<Scrollbar>();
			scrollbar.handleRect = rt_handle;
			scrollbar.targetGraphic = image2;
			Navigation navigation = scrollbar.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			scrollbar.navigation = navigation;
			SetColorBlock(scrollbar, new Color?(new Color(0.2f, 0.2f, 0.2f)), new Color?(new Color(0.3f, 0.3f, 0.3f)), new Color?(new Color(0.15f, 0.15f, 0.15f)), null);
			return self;
		}

		public static void AddListener<T>(this UnityEvent<T> _event, Action<T> listener)
		{
			_event.AddListener(new UnityAction<T>(listener.Invoke));
		}

		public static Dropdown CreateDropdown(this GameObject parent, string defaultItemText, int itemFontSize, Action<int> onValueChanged, string[] defaultOptions = null)
		{
			GameObject gameObject = CreateUIObject("Dropdown", parent, new Vector2(100f, 30f));
			GameObject gameObject2 = CreateUIObject("Label", gameObject, default(Vector2));
			GameObject gameObject3 = CreateUIObject("Arrow", gameObject, default(Vector2));
			GameObject gameObject4 = CreateUIObject("Template", gameObject, default(Vector2));
			GameObject gameObject5 = CreateUIObject("Viewport", gameObject4, default(Vector2));
			GameObject gameObject6 = CreateUIObject("Content", gameObject5, default(Vector2));
			GameObject gameObject7 = CreateUIObject("Item", gameObject6, default(Vector2));
			GameObject gameObject8 = CreateUIObject("Item Background", gameObject7, default(Vector2));
			CreateUIObject("Item Checkmark", gameObject7, default(Vector2));
			GameObject gameObject9 = CreateUIObject("Item Label", gameObject7, default(Vector2));
			Scrollbar scrollbar;
			GameObject gameObject10 = gameObject4.CreateScrollbar("DropdownScroll", out scrollbar);
			scrollbar.SetDirection(Scrollbar.Direction.BottomToTop, true);
			SetColorBlock(scrollbar, new Color?(new Color(0.45f, 0.45f, 0.45f)), new Color?(new Color(0.6f, 0.6f, 0.6f)), new Color?(new Color(0.4f, 0.4f, 0.4f)), null);
			RectTransform component = gameObject10.GetComponent<RectTransform>();
			component.anchorMin = Vector2.right;
			component.anchorMax = Vector2.one;
			component.pivot = Vector2.one;
			component.sizeDelta = new Vector2(component.sizeDelta.x, 0f);
			Text text = gameObject9.AddComponent<Text>();
			SetDefaultTextValues(text);
			text.alignment = TextAnchor.MiddleLeft;
			text.text = defaultItemText;
			text.fontSize = itemFontSize;
			Text text2 = gameObject3.AddComponent<Text>();
			SetDefaultTextValues(text2);
			text2.text = "▼";
			RectTransform component2 = gameObject3.GetComponent<RectTransform>();
			component2.anchorMin = new Vector2(1f, 0.5f);
			component2.anchorMax = new Vector2(1f, 0.5f);
			component2.sizeDelta = new Vector2(20f, 20f);
			component2.anchoredPosition = new Vector2(-15f, 0f);
			Image image = gameObject8.AddComponent<Image>();
			image.color = new Color(0.25f, 0.35f, 0.25f, 1f);
			Toggle itemToggle = gameObject7.AddComponent<Toggle>();
			itemToggle.targetGraphic = image;
			itemToggle.isOn = true;
			SetColorBlock(itemToggle, new Color?(new Color(0.35f, 0.35f, 0.35f, 1f)), new Color?(new Color(0.25f, 0.55f, 0.25f, 1f)), null, null);
			itemToggle.onValueChanged.AddListener(delegate(bool val)
			{
				itemToggle.OnDeselect(null);
			});
			Image image2 = gameObject4.AddComponent<Image>();
			image2.type = Image.Type.Sliced;
			image2.color = Color.black;
			ScrollRect scrollRect = gameObject4.AddComponent<ScrollRect>();
			scrollRect.scrollSensitivity = 35f;
			scrollRect.content = gameObject6.GetComponent<RectTransform>();
			scrollRect.viewport = gameObject5.GetComponent<RectTransform>();
			scrollRect.horizontal = false;
			scrollRect.movementType = ScrollRect.MovementType.Clamped;
			scrollRect.verticalScrollbar = scrollbar;
			scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
			scrollRect.verticalScrollbarSpacing = -3f;
			gameObject5.AddComponent<Mask>().showMaskGraphic = false;
			gameObject5.AddComponent<Image>().type = Image.Type.Sliced;
			Text text3 = gameObject2.AddComponent<Text>();
			SetDefaultTextValues(text3);
			text3.alignment = TextAnchor.MiddleLeft;
			Image image3 = gameObject.AddComponent<Image>();
			image3.color = new Color(0.04f, 0.04f, 0.04f, 0.75f);
			image3.type = Image.Type.Sliced;
			Dropdown dropdown = gameObject.AddComponent<Dropdown>();
			dropdown.targetGraphic = image3;
			dropdown.template = gameObject4.GetComponent<RectTransform>();
			dropdown.captionText = text3;
			dropdown.itemText = text;
			dropdown.RefreshShownValue();
			RectTransform component3 = gameObject2.GetComponent<RectTransform>();
			component3.anchorMin = Vector2.zero;
			component3.anchorMax = Vector2.one;
			component3.offsetMin = new Vector2(10f, 2f);
			component3.offsetMax = new Vector2(-28f, -2f);
			RectTransform component4 = gameObject4.GetComponent<RectTransform>();
			component4.anchorMin = new Vector2(0f, 0f);
			component4.anchorMax = new Vector2(1f, 0f);
			component4.pivot = new Vector2(0.5f, 1f);
			component4.anchoredPosition = new Vector2(0f, 2f);
			component4.sizeDelta = new Vector2(0f, 150f);
			RectTransform component5 = gameObject5.GetComponent<RectTransform>();
			component5.anchorMin = new Vector2(0f, 0f);
			component5.anchorMax = new Vector2(1f, 1f);
			component5.sizeDelta = new Vector2(-18f, 0f);
			component5.pivot = new Vector2(0f, 1f);
			RectTransform component6 = gameObject6.GetComponent<RectTransform>();
			component6.anchorMin = new Vector2(0f, 1f);
			component6.anchorMax = new Vector2(1f, 1f);
			component6.pivot = new Vector2(0.5f, 1f);
			component6.anchoredPosition = new Vector2(0f, 0f);
			component6.sizeDelta = new Vector2(0f, 28f);
			RectTransform component7 = gameObject7.GetComponent<RectTransform>();
			component7.anchorMin = new Vector2(0f, 0.5f);
			component7.anchorMax = new Vector2(1f, 0.5f);
			component7.sizeDelta = new Vector2(0f, 25f);
			RectTransform component8 = gameObject8.GetComponent<RectTransform>();
			component8.anchorMin = Vector2.zero;
			component8.anchorMax = Vector2.one;
			component8.sizeDelta = Vector2.zero;
			RectTransform component9 = gameObject9.GetComponent<RectTransform>();
			component9.anchorMin = Vector2.zero;
			component9.anchorMax = Vector2.one;
			component9.offsetMin = new Vector2(20f, 1f);
			component9.offsetMax = new Vector2(-10f, -2f);
			gameObject4.SetActive(false);
			if (onValueChanged != null)
			{
				dropdown.onValueChanged.AddListener(onValueChanged);
			}
			if (defaultOptions != null)
			{
				foreach (string text4 in defaultOptions)
				{
					dropdown.options.Add(new Dropdown.OptionData(text4));
				}
			}
			int? num = new int?(25);
			int? num2 = new int?(110);
			int? num3 = num;
			int? num4 = new int?(999);
			int? num5 = null;
			int? num6 = null;
			int? num7 = null;
			SetLayoutElement(gameObject, num2, num3, num4, num5, num6, num7, null);
			dropdown.captionText.color = new Color(0.57f, 0.76f, 0.43f);
			dropdown.value = 0;
			dropdown.RefreshShownValue();
			return dropdown;
		}

		public static void SetDefaultSelectableValues(Selectable selectable)
		{
			Navigation navigation = selectable.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			selectable.navigation = navigation;
			SetColorBlock(selectable, new Color?(new Color(0.2f, 0.2f, 0.2f)), new Color?(new Color(0.3f, 0.3f, 0.3f)), new Color?(new Color(0.15f, 0.15f, 0.15f)), null);
		}

		public static GameObject CreateToggle(this GameObject parent, string name, out Toggle toggle, out Text text, Color bgColor = default(Color), int checkWidth = 20, int checkHeight = 20)
		{
			GameObject gameObject = CreateUIObject(name, parent, new Vector2(25f, 25f));
			SetLayoutGroup<HorizontalLayoutGroup>(gameObject, new bool?(false), new bool?(false), new bool?(true), new bool?(true), new int?(5), new int?(0), new int?(0), new int?(0), new int?(0), new TextAnchor?(TextAnchor.MiddleLeft));
			toggle = gameObject.AddComponent<Toggle>();
			toggle.isOn = true;
			SetDefaultSelectableValues(toggle);
			Toggle t2 = toggle;
			toggle.onValueChanged.AddListener(delegate(bool _)
			{
				t2.OnDeselect(null);
			});
			GameObject gameObject2 = CreateUIObject("Background", gameObject, default(Vector2));
			Image image = gameObject2.AddComponent<Image>();
			image.color = ((bgColor == default(Color)) ? new Color(0.04f, 0.04f, 0.04f, 0.75f) : bgColor);
			SetLayoutGroup<HorizontalLayoutGroup>(gameObject2, new bool?(true), new bool?(true), new bool?(true), new bool?(true), new int?(0), new int?(2), new int?(2), new int?(2), new int?(2), null);
			GameObject gameObject3 = gameObject2;
			int? num = new int?(checkWidth);
			int? num2 = new int?(0);
			SetLayoutElement(gameObject3, num, new int?(checkHeight), num2, new int?(0), null, null, null);
			Image image2 = CreateUIObject("Checkmark", gameObject2, default(Vector2)).AddComponent<Image>();
			image2.color = new Color(0.8f, 1f, 0.8f, 0.3f);
			GameObject gameObject4 = CreateUIObject("Label", gameObject, default(Vector2));
			text = gameObject4.AddComponent<Text>();
			text.text = "";
			text.alignment = TextAnchor.MiddleLeft;
			SetDefaultTextValues(text);
			GameObject gameObject5 = gameObject4;
			int? num3 = new int?(0);
			num2 = new int?(0);
			SetLayoutElement(gameObject5, num3, new int?(checkHeight), num2, new int?(0), null, null, null);
			toggle.graphic = image2;
			toggle.targetGraphic = image;
			SetLayoutElement(gameObject, new int?(17), new int?(17), null, new int?(0), null, null, null);
			return gameObject;
		}
		public static GridLayoutGroup CreateGridGroup(this GameObject parent, string name, Vector2 cellSize, Vector2 spacing, Color bgColor = default(Color))
		{
			GameObject gameObject = CreateUIObject(name, parent, default(Vector2));
			GridLayoutGroup gridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();
			gridLayoutGroup.childAlignment = TextAnchor.UpperLeft;
			gridLayoutGroup.cellSize = cellSize;
			gridLayoutGroup.spacing = spacing;
			gameObject.AddComponent<Image>().color = ((bgColor == default(Color)) ? new Color(0.17f, 0.17f, 0.17f) : bgColor);
			return gridLayoutGroup;
		}
		public static Button CreateButton(this Image image)
		{
			GameObject gameObject = new GameObject("[Button]" + image.name);
			Button button = gameObject.AddComponent<Button>();
			image.transform.SetParent(image.transform);
			gameObject.transform.localScale = new Vector2(1f, 1f);
			gameObject.transform.localPosition = new Vector2(0f, 0f);
			button.image = image;
			return button;
		}
	}
}
