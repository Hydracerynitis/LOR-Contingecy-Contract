using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ContractRewardSystem: Singleton<ContractRewardSystem>
    {
        private List<string> UIs;
        private static bool IsRevebrate(int id) => id >= 70001 && id <= 70010;
        private string GetChpaterParams(StageClassInfo info) => TextDataModel.GetText("ui_maintitle_citystate_" + info.chapter.ToString());
        public void CheckReward(StageClassInfo info)
        {
            if (Singleton<BookInventoryModel>.Instance.GetBookListAll().Find((Predicate<BookModel>)(x => x.GetBookClassInfoId() == 18900000)) == null)
                Singleton<BookInventoryModel>.Instance.CreateBook(18900000);
            if (Singleton<ContractLoader>.Instance.GetLevel(info.id) >= 18)
            {
                if (Harmony_Patch.Cheat)
                {
                    if (!Harmony_Patch.Warn)
                    {
                        UIAlarmPopup.instance.SetAlarmText((TextDataModel.GetText("Cheat_Detect")));
                        Harmony_Patch.Warn = true;
                    }
                    Debug.Log("Reward Denied due to cheat");
                    return;
                }
                UIs = new List<string>();
                int id=-1;
                if (IsRevebrate(info.id))
                {
                    switch (info.id)
                    {
                        case (70001):
                            id = 18100000;
                            break;
                        case (70007):
                            id = 18700000;
                            break;
                        case (70010):
                            id = 18000000;
                            break;
                    }
                    if (id != -1)
                    {
                        if(Singleton<BookInventoryModel>.Instance.GetBookListAll().Find((Predicate<BookModel>)(x => x.GetBookClassInfoId()==id))==null)
                            Singleton<BookInventoryModel>.Instance.CreateBook(id);
                        Debug.Log(string.Format("Achieved Reverberation Reward: {0}",id ));
                        UIs.Add(TextDataModel.GetText("ui_RewardRevebrate", Singleton<StageNameXmlList>.Instance.GetName(info.id)));
                        UIs.Add(TextDataModel.GetText("ui_popup_getequippage", (object)Singleton<BookDescXmlList>.Instance.GetBookName(id)));
                    }
                }
                else
                {
                    switch (info.chapter)
                    {
                        case (1):
                            id = 185001;
                            break;
                    }
                    if (id != -1)
                    {
                        Singleton<InventoryModel>.Instance.AddCard(id);
                        Debug.Log(string.Format("Achieved Chapter Reward: {0}", (object)GetChpaterParams(info)));
                        UIs.Add(TextDataModel.GetText("ui_RewardChapter", (object)GetChpaterParams(info)));
                        UIs.Add(TextDataModel.GetText("ui_popup_getstorycard", (object)ItemXmlDataList.instance.GetCardItem(id).Name, (object)1));
                    }
                }
                if (UIs.Count > 0)
                    UIAlarmPopup.instance.SetAlarmText(string.Join("\n", UIs));
            }
        }
    }
}
