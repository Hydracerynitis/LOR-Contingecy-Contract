using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using BH = BaseMod.Harmony_Patch;

namespace Contingecy_Contract
{
	public static class CCManager
	{
		public static readonly string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		public static List<CCTagUGUI> tagUGUIs;
		public static List<int> nowshowlevels = new List<int>();
		public static List<Contract> OnContract = new List<Contract>();
		public static void InitializeUI()
		{
			CCUGUI.Instance.gameObject.SetActive(false);
			CCUGUI.CC_Conflict = BH.ArtWorks["CC_Conflict"];
			CCUGUI.CC_Error = BH.ArtWorks["CC_Default"];
			CCUGUI.CC_Null = BH.ArtWorks["CCGUI_null"];
			CCUGUI.CC_On = BH.ArtWorks["CC_On"];
			CCUGUI.Instance.MakeContractPanel();
			LoadandInitCCTag();
			CCUGUI.Instance.gameObject.SetActive(true);
			CCUGUI.CCGUI_Background.SetActive(false);
		}
		public static void LoadandInitCCTag()
		{
			tagUGUIs = new List<CCTagUGUI>();
			foreach (Contract contract in StaticDataManager.JsonList)
			{
				tagUGUIs.Add(CCTagUGUI.Add(CCUGUI.CCItemViewport, contract));
			}
			LoadCC();
			UpdateCCTagUI();
		}
		public static void LoadCC()
        {	
			foreach (Contract cc in StaticDataManager.JsonList)
			{
				cc.isConflict = false;
				cc.isOn = false;
			}
			ContractLoader.Instance.Init();
			List<Contract> active = new List<Contract>(ContractLoader.Instance.GetPassiveList());
			active.AddRange(ContractLoader.Instance.GetStageList());
			foreach (Contract cc in active)
            {
				cc.isOn = true;
				OnContract.Add(cc);
				BanTag(cc);
            }
			CCUGUI.Instance.SetGetLevel();
		}
		public static void UpdateCCTagUI()
		{
			foreach (CCTagUGUI cctagUGUI in tagUGUIs)
			{
				cctagUGUI.UpdateUI();
			}
		}
		public static void FilterList()
        {
			Predicate<CCTagUGUI> True = x => true;
			Predicate<CCTagUGUI> filterLevel = nowshowlevels.Count <= 0 ? True : x => x.CC.Level >= 4 ? nowshowlevels.Contains(4) : nowshowlevels.Contains(x.CC.Level);
			string s = CCUGUI.search.text;
			Predicate<CCTagUGUI> filterText = string.IsNullOrWhiteSpace(s) ? True : x => x.CC.Id.ToLower().Contains(s.ToLower());
			foreach(CCTagUGUI tag in tagUGUIs)
				tag.gameObject.SetActive(false);
			foreach (CCTagUGUI tag in tagUGUIs.FindAll(filterLevel).FindAll(filterText))
				tag.gameObject.SetActive(true);
		}
		public static void Clear()
        {
			OnContract.Clear();
			foreach(CCTagUGUI ui in tagUGUIs)
            {
				ui.CC.isOn = false;
				ui.CC.isConflict = false;
            }
			CCUGUI.Instance.SetGetLevel();
			UpdateCCTagUI();
		}
		public static void UpdateConflict()
		{
			tagUGUIs.ForEach(x => x.CC.isConflict = false);
			List<CCTagUGUI> On = tagUGUIs.FindAll(x => x.CC.isOn);
			foreach(CCTagUGUI off in tagUGUIs.FindAll(x => !On.Contains(x)))
			{
				if (On.Exists(x => off.CC.Type==x.CC.Type || x.CC.Conflict.Contains(off.CC.Type)))
					off.CC.isConflict = true;
			}
			tagUGUIs.ForEach(x => x.UpdateUI());
		}
		public static void BanTag(Contract contract)
		{
			bool isOn = contract.isOn;
			if (!string.IsNullOrEmpty(contract.Type))
			{
				List<CCTagUGUI> list = tagUGUIs;
				Predicate<CCTagUGUI> ban = x => (x.CC.Type == contract.Type || contract.Conflict.Contains(x.CC.Type)) && x.CC!=contract;
				foreach (CCTagUGUI cctagUGUI in list.FindAll(ban))
				{
					OnContract.Remove(cctagUGUI.CC);
					cctagUGUI.CC.isOn = false;
					cctagUGUI.CC.isConflict = isOn;
					cctagUGUI.UpdateUI();
				}
			}
		}
	}
}
