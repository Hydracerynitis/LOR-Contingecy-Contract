using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Contingecy_Contract
{
	public class CCTagUGUI : EventTrigger
	{
		public Image CCIcon;
		public Image CC_Conflict;
		public Image CC_On;
		public Image CCGUI_null;
		public Contract CC;
		public override void OnPointerDown(PointerEventData eventData)
		{
			CC.isOn = !CC.isOn;
			CC.isConflict = false;
			if (CC.isOn)
				CCManager.OnContract.Add(CC);
			else
				CCManager.OnContract.Remove(CC);
			CCManager.BanTag(CC);
			UpdateUI();
			CCUGUI.Instance.SetGetLevel();
		}
		public void UpdateUI()
		{
			CC_Conflict.gameObject.SetActive(!CC.isOn && CC.isConflict);
			CC_On.gameObject.SetActive(CC.isOn);
		}
		public void Destroy()
		{
			Destroy(gameObject);
		}
		public override void OnPointerEnter(PointerEventData eventData)
		{
			CCUGUI.Instance.NowDesc = CC.GetDesc().name+"\n"+ CC.GetDesc().desc;
		}
		public override void OnPointerExit(PointerEventData eventData)
		{
		}
		public static CCTagUGUI Add(GameObject parent, Contract contract)
		{
			GameObject gameObject = new GameObject("[CCTagUGUI]" + contract.Id);
			Image image = gameObject.AddComponent<Image>();
			image.sprite = CCUGUI.CC_Null;
			image.rectTransform.sizeDelta = new Vector2((float)image.sprite.texture.width, (float)image.sprite.texture.height);
			gameObject.transform.SetParent(parent.transform);
			gameObject.transform.localPosition = new Vector2(0f, 0f);
			CCTagUGUI cctagUGUI = gameObject.AddComponent<CCTagUGUI>();
			cctagUGUI.Show(contract);
			return cctagUGUI;
		}
		public void Show(Contract contract)
		{
			CC = contract;
			CCIcon = gameObject.CreateCCIcon(contract.Id);
			CCIcon.name = "CCIcon";
			CC_On = gameObject.CreateImage(CCUGUI.CC_On);
			CC_On.name = "CC_On";
			CC_Conflict = gameObject.CreateImage(CCUGUI.CC_Conflict);
			CC_Conflict.name = "CC_Conflict";
			CCGUI_null = gameObject.CreateImage(CCUGUI.CC_Null);
			CCGUI_null.name = "CC_Null";
		}
		
	}
}
