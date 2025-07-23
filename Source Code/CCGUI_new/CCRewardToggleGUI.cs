using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BH=BaseMod.Harmony_Patch;
using UI;
using GameSave;

namespace Contingecy_Contract
{
    public class CCRewardToggleGUI : EventTrigger
    {
        private Image background;
        private Image isOn;
        public void Init(Image bg, Image on)
        {
            background = bg;
            isOn = on;
            isOn.enabled = true;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            bool active = isOn.gameObject.activeSelf;
            UIAlarmPopup.instance.SetYesNoAlert(active? TextDataModel.GetText("ui_disable_reward") : TextDataModel.GetText("ui_enable_reward")
                ,  (bool yesno) =>  { 
                    Debug.Log("YesNO Alert Test: " + yesno.ToString());
                    if (yesno)
                    {
                        List<LorId> glossry = StaticDataManager.Glossary;
                        isOn.gameObject.SetActive(!active);
                        if (active)
                        {
                            ContractRewardSystem.Instance.Active =false;
                            BookInventoryModel.Instance.GetBookListAll()
                                .FindAll(b => glossry.Contains(b.GetBookClassInfoId()))
                                    .ForEach(b => BookInventoryModel.Instance.RemoveBookWithUnequip(b));
                            LibraryModel.Instance.GetFloor(UI.UIController.Instance.CurrentSephirah).GetUnitDataList()
                                .ForEach(unit => UICharacterRenderer.Instance.ReloadCharacter(unit));
                            ContractSaveManager.Save("RewardList_V2");
                        }
                        else
                        {
                            ContractRewardSystem.Instance.Active = true;
                            ContractRewardSystem.Instance.CheckRewardAchieved();
                        }
                        SaveManager.Instance.SavePlayData(1);
                    } 
                    CCUGUI.CCGUI_Background.SetActive(true);
                });
            
        }
        public void UpdateLanguage(string language)
        {
            background.sprite = BH.ArtWorks[language + "_CCGUI_Reward"];
        }
        public void OnValueChange()
        {
            isOn.gameObject.SetActive(ContractRewardSystem.Instance.Active);
        }
    }
}
