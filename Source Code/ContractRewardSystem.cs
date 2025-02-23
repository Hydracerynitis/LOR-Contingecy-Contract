﻿using System;
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
        public static HashSet<int> ClearList = new HashSet<int>();
        public List<string> UIs = new List<string>();
        public void CheckReward(StageClassInfo info)
        {
            if (!SupportedPid.Contains(info.id.packageId))
                return;
            if (Singleton<ContractLoader>.Instance.GetLevel(info.id) < getLevelRequirement(info.chapter))
                return;
            if (StaticDataManager.RewardDic.ContainsKey(info.id))
                ClearList.Add(StaticDataManager.RewardDic[info.id]);
            UIs.Add(TextDataModel.GetText("ui_RewardStage", getLevelRequirement(info.chapter), Singleton<StageNameXmlList>.Instance.GetName(info.id)));
            GetContractCondition(info);
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
        public int getLevelRequirement(int chapter)
        {
            if (chapter >= 6)
                return 16;
            else if (chapter == 5)
                return 24;
            else if (chapter == 4)
                return 32;
            else if (chapter == 3)
                return 40;
            return 48;
        }
        public void CheckSpecialCondition(StageClassInfo info)
        {
            if (EnsembleComplete)
                ClearList.Add(18000000);
            if (RolandComplete)
                ClearList.Add(17000005);
        }
        public void CheckRewardAchieved()
        {
            HashSet<int> Inventory = new HashSet<int>();
            foreach(BookModel book in BookInventoryModel.Instance.GetBookListAll().FindAll(x => x.GetBookClassInfoId().packageId== "ContingencyConract"))
            {
                int id = book.GetBookClassInfoId().id;
                Inventory.Add(id);
                if (!ClearList.Contains(id))
                    ClearList.Add(id);
            }
            ContractSaveManager.Save(new List<int>(ClearList), "RewardList");
            HashSet<int> ExceptWith = new HashSet<int>(ClearList);
            ExceptWith.ExceptWith(Inventory);
            foreach (int i in ExceptWith)
                GiveEquipBook(i);
        }
        public void GetContractCondition(StageClassInfo info)
        {
            foreach(RewardConfig RC in StaticDataManager.ExtraCondition)
            {
                if (info.id != RC.Id)
                    continue;
                List<Contract> contracts = new List<Contract>();
                contracts.AddRange(Singleton<ContractLoader>.Instance.GetPassiveList());
                contracts.AddRange(Singleton<ContractLoader>.Instance.GetStageList());
                bool pass = true;
                foreach (List<ContractCondition> conditions in RC.Condition)
                {
                    bool check= false;
                    foreach(ContractCondition condition in conditions)
                    {
                        Contract contract = contracts.Find(x => x.Type == condition.Type);
                        if (contract != null)
                        {
                            if (Singleton<ContractLoader>.Instance.CheckActivate(contract, info) && contract.Variant >= condition.Variation)
                            {
                                check = true;
                                break;
                            }                         
                        }
                    }
                    if (!check)
                    {
                        pass = false;
                    }                 
                }
                if(pass)
                    ClearList.Add(RC.RewardId);
            }
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
                HashSet<int> Test = new HashSet<int>() { 18100000, 18200000, 18300000, 18400000, 18500000, 18600000, 18700000, 18800000, 18900000 };
                Test.ExceptWith(ClearList);
                return Test.Count <= 0;
            }
        }
        public static bool RolandComplete
        {
            get
            {
                HashSet<int> Test = new HashSet<int>() { 17000001, 17000002, 17000003, 17000004 };
                Test.ExceptWith(ClearList);
                return Test.Count <= 0;
            }
        }
    }
}
//废案：给章节战斗书页的奖励
//UIs.Add(TextDataModel.GetText("ui_RewardChapter", (object)GetChpaterParams(info)));
//Singleton<InventoryModel>.Instance.AddCard(id);
//Debug.Log(string.Format("Achieved Chapter Reward: {0}", (object)GetChpaterParams(info)));
//UIs.Add(TextDataModel.GetText("ui_popup_getstorycard", (object)ItemXmlDataList.instance.GetCardItem(id).Name, (object)1));
