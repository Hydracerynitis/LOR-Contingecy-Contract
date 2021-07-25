using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ContractRewardSystem : Singleton<ContractRewardSystem>
    {
        private List<string> UIs;
        public void CheckReward(StageClassInfo info)
        {
            if (Harmony_Patch.CheckDuel(info.id))
                return;
            if (Singleton<ContractLoader>.Instance.GetLevel(info.id) < 12)
                return;
            UIs = new List<string>();
            switch (info.id)
            {
                case (70001):
                    Harmony_Patch.Progess.Philiph_Risk = 1;
                    break;
                case (70002):
                    Harmony_Patch.Progess.Eileen_Risk = 1;
                    break;
                case (70003):
                    Harmony_Patch.Progess.Greta_Risk = 1;
                    break;
                case (70004):
                    Harmony_Patch.Progess.Bremen_Risk = 1;
                    break;
                case (70006):
                    Harmony_Patch.Progess.Tanya_Risk = 1;
                    break;
                case (70007):
                    Harmony_Patch.Progess.Jaeheon_Risk = 1;
                    break;
                case (70008):
                    Harmony_Patch.Progess.Elena_Risk = 1;
                    break;
                case (70009):
                    Harmony_Patch.Progess.Pluto_Risk = 1;
                    break;
            }
            UIs.Add(TextDataModel.GetText("ui_RewardStage", Singleton<StageNameXmlList>.Instance.GetName(info.id)));
            CheckSpecialCondition(info);
            CheckRewardAchieved();
            Debug.SaveDebug();
            if (UIs.Count > 0)
                UIAlarmPopup.instance.SetAlarmText(string.Join("\n", UIs));
        }
        public void CheckSpecialCondition(StageClassInfo info)
        {
            if (GetContractCondition(OrangeCrossCondition,info))
            {
                Harmony_Patch.Progess.Orange_Path = 1;
                UIs.Add(TextDataModel.GetText("ui_RewardSpecial", TextDataModel.GetText("Condition_OrangeCross")));
            }
            if (EnsembleComplete)
            {
                if(Harmony_Patch.Progess.Ensemble_Complete==0)
                    UIs.Add(TextDataModel.GetText("ui_RewardSpecial", TextDataModel.GetText("Condition_Ensemble")));
                Harmony_Patch.Progess.Ensemble_Complete = 1;
            }
        }
        public void CheckRewardAchieved()
        {
            if (Harmony_Patch.Progess.Ensemble_Complete == 1)
            {
                GiveEquipBook(18000000);
            }
            if (Harmony_Patch.Progess.Philiph_Risk == 1)
            {
                GiveEquipBook(18100000);
            }
            if (Harmony_Patch.Progess.Eileen_Risk == 1)
            {
                GiveEquipBook(18200000);
            }
            if (Harmony_Patch.Progess.Greta_Risk == 1)
            {
                GiveEquipBook(18300000);
            }
            if (Harmony_Patch.Progess.Bremen_Risk == 1)
            {
                GiveEquipBook(18400000);
            }
            if (Harmony_Patch.Progess.Tanya_Risk == 1)
            {
                GiveEquipBook(18600000);
            }
            if (Harmony_Patch.Progess.Jaeheon_Risk == 1)
            {
                GiveEquipBook(18700000);
            }
            if (Harmony_Patch.Progess.Elena_Risk == 1)
            {
                GiveEquipBook(18800000);
            }
            if (Harmony_Patch.Progess.Orange_Path == 1)
            {
                GiveEquipBook(18810000);
            }
            if (Harmony_Patch.Progess.Pluto_Risk == 1)
            {
                GiveEquipBook(18900000);
            }
        }
        public bool GetContractCondition(List<(string, int)> Condition,StageClassInfo info)
        {
            List<Contract> contracts = new List<Contract>();
            contracts.AddRange(Singleton<ContractLoader>.Instance.GetPassiveList());
            contracts.AddRange(Singleton<ContractLoader>.Instance.GetStageList());
            foreach ((string,int) condition in Condition)
            {
                Contract contract = contracts.Find(x => x.Type == condition.Item1);
                if (contract != null)
                {
                    if (Singleton<ContractLoader>.Instance.CheckActivate(contract,info) && contract.Level >= condition.Item2)
                        continue;
                }
                return false;
            }
            return true;
        }
        public void GiveEquipBook(int bookid)
        {
            List<BookModel> all = Singleton<BookInventoryModel>.Instance.GetBookListAll().FindAll(x => x.ClassInfo.id == bookid);
            BookXmlInfo data2 = Singleton<BookXmlList>.Instance.GetData(bookid);
            if (data2 == null || all.Count >= data2.Limit)
                return;
            int difference = data2.Limit - all.Count;
            for(int i = 0; i < difference; i++)
            {
                Singleton<BookInventoryModel>.Instance.CreateBook(bookid);
            }
            UIs.Add(TextDataModel.GetText("ui_popup_getequippage", (object)Singleton<BookDescXmlList>.Instance.GetBookName(bookid),(object)difference));
        }
        public static bool EnsembleComplete => Harmony_Patch.Progess.Philiph_Risk == 1 && Harmony_Patch.Progess.Eileen_Risk == 1 && Harmony_Patch.Progess.Greta_Risk ==1 &&
                                                  Harmony_Patch.Progess.Bremen_Risk == 1 &&  Harmony_Patch.Progess.Tanya_Risk == 1 && Harmony_Patch.Progess.Jaeheon_Risk == 1
                                                 && Harmony_Patch.Progess.Elena_Risk == 1 && Harmony_Patch.Progess.Pluto_Risk == 1;
        public static List<(string, int)> OrangeCrossCondition => new List<(string, int)>() { ("Elena_Cross", 4), ("Elena", 4), ("Damage", 4) };
    }
}
//废案：给章节战斗书页的奖励
//UIs.Add(TextDataModel.GetText("ui_RewardChapter", (object)GetChpaterParams(info)));
//Singleton<InventoryModel>.Instance.AddCard(id);
//Debug.Log(string.Format("Achieved Chapter Reward: {0}", (object)GetChpaterParams(info)));
//UIs.Add(TextDataModel.GetText("ui_popup_getstorycard", (object)ItemXmlDataList.instance.GetCardItem(id).Name, (object)1));
