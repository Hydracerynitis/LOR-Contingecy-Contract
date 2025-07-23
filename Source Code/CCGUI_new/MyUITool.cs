using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using BH = BaseMod.Harmony_Patch;
using UI;
using static TubeLightShadowPlane;

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
		public static void SetYesNoAlert(this UIAlarmPopup popup,string text, ConfirmEvent confirmFunc)
		{
            if (popup.IsOpened())
                popup.Close();
            if (popup.ob_blue.activeSelf)
                popup.ob_blue.gameObject.SetActive(false);
            if (!popup.ob_normal.activeSelf)
                popup.ob_normal.gameObject.SetActive(true);
            if (popup.ob_Reward.activeSelf)
                popup.ob_Reward.SetActive(false);
            if (popup.ob_BlackBg.activeSelf)
                popup.ob_BlackBg.SetActive(false);
            foreach (GameObject buttonRoot in popup.ButtonRoots)
                buttonRoot.gameObject.SetActive(false);
            popup.currentAlarmType = UIAlarmType.Default;
            popup.currentAnimState = UIAlarmPopup.UIAlarmAnimState.Normal;
            popup.buttonNumberType = UIAlarmButtonType.YesNo;
			popup.txt_alarm.text = text;
            popup.currentmode = AnimatorUpdateMode.Normal;
            popup.anim.updateMode = AnimatorUpdateMode.Normal;
            popup._confirmEvent = confirmFunc;
            popup.ButtonRoots[(int)popup.buttonNumberType].gameObject.SetActive(true);
            popup.Open();
            UIControlManager.Instance.SelectSelectableForcely(popup.yesButton);
        }
	}
}
