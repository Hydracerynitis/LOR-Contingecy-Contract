using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BH=BaseMod.Harmony_Patch;

namespace Contingecy_Contract
{
    public class CCLevelUGUI: EventTrigger
    {
        private Image background;
        private Image isOn;
        private int level;
        public void Init(Image bg, Image on, int Level)
        {
            background = bg;
            isOn = on;
            level = Level;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            isOn.gameObject.SetActive(!isOn.gameObject.activeSelf);
            if (isOn.gameObject.activeSelf)
                CCManager.nowshowlevels.Add(level);
            else
                CCManager.nowshowlevels.Remove(level);
            CCManager.FilterList();
        }
        public void OnEnable()
        {
            isOn.gameObject.SetActive(false);
        }
        public void UpdateLanguage(string language)
        {
            string word = "";
            switch (level)
            {
                case 1:
                    word = "1st";
                    break;
                case 2:
                    word = "2nd";
                    break;
                case 3:
                    word = "3rd";
                    break;
                case 4:
                    word = "4th";
                    break;
                case 0:
                    word = "0th";
                    break;
            }
            background.sprite = BH.ArtWorks[language + "_CCGUI_" + word + "Level"];
        }
    }
}
