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
        private string GetChpaterParams(StageClassInfo info) => TextDataModel.GetText("ui_maintitle_citystate_" + info.chapter.ToString());
        public void CheckReward(StageClassInfo info)
        {
            if (Harmony_Patch.CheckDuel(info.id))
                return;
            if (Singleton<ContractLoader>.Instance.GetLevel(info.id) < 12)
                return;
            UIs = new List<string>();
            int id = -1;
            switch (info.id)
            {
                case (70001):
                    id = 18100000;
                    break;
                case (70007):
                    id = 18700000;
                    break;
                case (70008):
                    id = 18800000;
                    break;
                case (70009):
                    id = 18900000;
                    break;
            }
            if (id != -1)
            {
                UIs.Add(TextDataModel.GetText("ui_RewardStage", Singleton<StageNameXmlList>.Instance.GetName(info.id)));
                if (Singleton<BookInventoryModel>.Instance.GetBookListAll().Find((Predicate<BookModel>)(x => x.GetBookClassInfoId() == id)) == null)
                {
                    Singleton<BookInventoryModel>.Instance.CreateBook(id);
                    Debug.Log(string.Format("Achieved Stage Reward: {0}", id));
                    UIs.Add(TextDataModel.GetText("ui_popup_getequippage", (object)Singleton<BookDescXmlList>.Instance.GetBookName(id)));
                }
            }
            CheckRevebrateReward();
            if (UIs.Count > 0)
                UIAlarmPopup.instance.SetAlarmText(string.Join("\n", UIs));
        }
        public void CheckRevebrateReward()
        {
            if (Singleton<BookInventoryModel>.Instance.GetBookListAll().Find((Predicate<BookModel>)(x => x.GetBookClassInfoId() == 18100000)) == null)
                return;
            if (Singleton<BookInventoryModel>.Instance.GetBookListAll().Find((Predicate<BookModel>)(x => x.GetBookClassInfoId() == 18700000)) == null)
                return;
            if (Singleton<BookInventoryModel>.Instance.GetBookListAll().Find((Predicate<BookModel>)(x => x.GetBookClassInfoId() == 18800000)) == null)
                return;
            if (Singleton<BookInventoryModel>.Instance.GetBookListAll().Find((Predicate<BookModel>)(x => x.GetBookClassInfoId() == 18900000)) == null)
                return;
            UIs.Add(TextDataModel.GetText("ui_RewardRevebrate"));
            if (Singleton<BookInventoryModel>.Instance.GetBookListAll().Find((Predicate<BookModel>)(x => x.GetBookClassInfoId() == 18000000)) == null)
            {
                Singleton<BookInventoryModel>.Instance.CreateBook(18000000);
                Debug.Log(string.Format("Achieved Reverberation Reward: {0}", 18000000));
                UIs.Add(TextDataModel.GetText("ui_popup_getequippage", (object)Singleton<BookDescXmlList>.Instance.GetBookName(18000000)));
            }
        }
    }
}
//废案：给章节战斗书页的奖励
//UIs.Add(TextDataModel.GetText("ui_RewardChapter", (object)GetChpaterParams(info)));
//Singleton<InventoryModel>.Instance.AddCard(id);
//Debug.Log(string.Format("Achieved Chapter Reward: {0}", (object)GetChpaterParams(info)));
//UIs.Add(TextDataModel.GetText("ui_popup_getstorycard", (object)ItemXmlDataList.instance.GetCardItem(id).Name, (object)1));
