using System;
using System.Collections.Generic;
using System.Linq;
using BaseMod;
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
            if (Harmony_Patch.CheckDuel(info.id) || Harmony_Patch.CheckPlaceHolder(info.id))
                return;
            if (!SupportedPid.Contains(info.id.packageId))
                return;
            if (Singleton<ContractLoader>.Instance.GetLevel(info.id) < 16)
                return;
            UIs = new List<string>();
            switch (info.id.id)
            {
                case 70001:
                    Harmony_Patch.ClearList.Add(18100000);
                    break;
                case (70002):
                    Harmony_Patch.ClearList.Add(18200000);
                    break;
                case (70003):
                    Harmony_Patch.ClearList.Add(18300000);
                    break;
                case (70004):
                    Harmony_Patch.ClearList.Add(18400000);
                    break;
                case (70005):
                    Harmony_Patch.ClearList.Add(18500000);
                    break;
                case (70006):
                    Harmony_Patch.ClearList.Add(18600000);
                    break;
                case (70007):
                    Harmony_Patch.ClearList.Add(18700000);
                    break;
                case (70008):
                    Harmony_Patch.ClearList.Add(18800000);
                    break;
                case (70009):
                    Harmony_Patch.ClearList.Add(18900000);
                    break;
            }
            UIs.Add(TextDataModel.GetText("ui_RewardStage", Singleton<StageNameXmlList>.Instance.GetName(info.id)));
            CheckSpecialCondition(info);
            CheckRewardAchieved();
            if (UIs.Count > 0)
            {
                string uis = string.Join("\n", UIs.GetRange(0, Math.Min(UIs.Count, 4)));
                if (UIs.Count > 4)
                    uis += TextDataModel.GetText("ui_MoreEquipPage");
                UIAlarmPopup.instance.SetAlarmText(uis);
            }

        }
        public void CheckSpecialCondition(StageClassInfo info)
        {
            if (GetContractCondition(OrangeCrossCondition, info))
                Harmony_Patch.ClearList.Add(18810000);
            if (EnsembleComplete)
                Harmony_Patch.ClearList.Add(18000000);
        }
        public void CheckRewardAchieved()
        {
            HashSet<int> Inventory = new HashSet<int>();
            foreach(BookModel book in Singleton<BookInventoryModel>.Instance.GetBookListAll().FindAll(x => x.GetBookClassInfoId().packageId== "ContingencyConract"))
            {
                int id = book.GetBookClassInfoId().id;
                Inventory.Add(id);
                if (!Harmony_Patch.ClearList.Contains(id))
                    Harmony_Patch.ClearList.Add(id);
            }
            new List<int>(Harmony_Patch.ClearList).Save<List<int>>("ContingecyContract_Save");
            HashSet<int> ExceptWith = new HashSet<int>(Harmony_Patch.ClearList);
            ExceptWith.ExceptWith(Inventory);
            foreach (int i in ExceptWith)
                GiveEquipBook(i);
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
            LorId newId = Tools.MakeLorId(bookid);
            List<BookModel> all = Singleton<BookInventoryModel>.Instance.GetBookListAll().FindAll(x => x.ClassInfo.id == newId);
            BookXmlInfo data2 = Singleton<BookXmlList>.Instance.GetData(newId);
            if (data2 == null || all.Count >= data2.Limit)
                return;
            int difference = data2.Limit - all.Count;
            for(int i = 0; i < difference; i++)
            {
                Singleton<BookInventoryModel>.Instance.CreateBook(newId);
            }
            UIs.Add(TextDataModel.GetText("ui_popup_getequippage", (object)Singleton<BookDescXmlList>.Instance.GetBookName(newId), (object)difference));
        }
        public static List<string> SupportedPid = new List<string>() { ""};
        public static bool EnsembleComplete
        {
            get
            {
                HashSet<int> Test = new HashSet<int>() { 18100000, 18200000, 18300000, 18400000, 18600000, 18700000, 18800000, 18900000 };
                Test.ExceptWith(Harmony_Patch.ClearList);
                return Test.Count <= 0;
            }
        }

        public static List<(string, int)> OrangeCrossCondition => new List<(string, int)>() { ("Elena_Cross", 4), ("Elena", 4), ("Damage", 4) };
    }
}
//废案：给章节战斗书页的奖励
//UIs.Add(TextDataModel.GetText("ui_RewardChapter", (object)GetChpaterParams(info)));
//Singleton<InventoryModel>.Instance.AddCard(id);
//Debug.Log(string.Format("Achieved Chapter Reward: {0}", (object)GetChpaterParams(info)));
//UIs.Add(TextDataModel.GetText("ui_popup_getstorycard", (object)ItemXmlDataList.instance.GetCardItem(id).Name, (object)1));
