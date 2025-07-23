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
        public bool Active = true;
        public HashSet<LorId> ClearList = new HashSet<LorId>();
        public List<string> UIs = new List<string>();
        public void CheckReward(StageClassInfo info)
        {
            if (!Active)
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
            UIs.Clear();
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
            foreach(RewardClearList RCL in StaticDataManager.rewardClearLists)
            {
                HashSet<LorId> test= new HashSet<LorId>();
                RCL.Requirements.ForEach(x => test.Add(new LorId(RCL.RequirementPid, x)));
                test.ExceptWith(ClearList);
                if (test.Count <= 0)
                {
                    ClearList.Add(new LorId(RCL.RewardPid, RCL.RewardId));
                }
            }
        }
        public void CheckRewardAchieved()
        {
            if (!Active)
                return;
            HashSet<LorId> Inventory = new HashSet<LorId>();
            foreach(BookModel book in BookInventoryModel.Instance.GetBookListAll().FindAll(x => StaticDataManager.Glossary.Contains(x.GetBookClassInfoId())))
            {
                LorId id = book.GetBookClassInfoId();
                Inventory.Add(id);
                if (!ClearList.Contains(id))
                    ClearList.Add(id);
            }
            ContractSaveManager.Save("RewardList_V2");
            HashSet<LorId> ExceptWith = new HashSet<LorId>(ClearList);
            ExceptWith.ExceptWith(Inventory);
            foreach (LorId i in ExceptWith)
                GiveEquipBook(i);
        }
        public void GetContractCondition(StageClassInfo info)
        {
            foreach(RewardConfig RC in StaticDataManager.ExtraCondition)
            {
                if (info.id != RC.StageId)
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
                    ClearList.Add(RC.rewardId);
            }
        }
        public void GiveEquipBook(LorId bookid)
        {
            if (!Active)
                return;
            List<BookModel> all = Singleton<BookInventoryModel>.Instance.GetBookListAll().FindAll(x => x.ClassInfo.id == bookid);
            BookXmlInfo data2 = Singleton<BookXmlList>.Instance.GetData(bookid);
            if (data2 == null || all.Count >= data2.Limit)
                return;
            int difference = data2.Limit - all.Count;
            for(int i = 0; i < difference; i++)
            {
                Singleton<BookInventoryModel>.Instance.CreateBook(bookid);
            }
            UIs.Add(TextDataModel.GetText("ui_popup_getequippage", (object)Singleton<BookDescXmlList>.Instance.GetBookName(bookid), (object)difference));
        }
    }
}
