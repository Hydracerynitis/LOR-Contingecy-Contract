using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using HarmonyLib;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public static class DeepCopyUtil
    {
        public static StageClassInfo CopyXml(StageClassInfo info)
        {
            StageClassInfo output = new StageClassInfo
            {
                _id = info._id,
                workshopID = info.workshopID,
                waveList = new List<StageWaveInfo>(),
                stageType = info.stageType,
                mapInfo = new List<string>(),
                floorNum = info.floorNum,
                stageName = info.stageName,
                chapter = info.chapter,
                invitationInfo = CopyXml(info.invitationInfo),
                extraCondition = CopyXml(info.extraCondition),
                storyList = new List<StageStoryInfo>(),
                isChapterLast = info.isChapterLast,
                _storyType = info._storyType,
                isStageFixedNormal = info.isStageFixedNormal,
                floorOnlyList = new List<SephirahType>(),
                exceptFloorList = new List<SephirahType>(),
                _rewardList = new List<BookDropItemXmlInfo>()
            };
            foreach (StageWaveInfo wave in info.waveList)
                output.waveList.Add(CopyXml(wave));
            output.mapInfo.AddRange(info.mapInfo);
            foreach (StageStoryInfo story in info.storyList)
                output.storyList.Add(CopyXml(story));
            output.floorOnlyList.AddRange(info.floorOnlyList);
            output.exceptFloorList.AddRange(info.exceptFloorList);
            foreach (BookDropItemXmlInfo reward in info._rewardList)
                output._rewardList.Add(CopyXml(reward));
            output.InitializeIds(output.workshopID);
            return output;
        }
        public static StageWaveInfo CopyXml(StageWaveInfo info)
        {
            StageWaveInfo output = new StageWaveInfo
            {
                _enemyUnitIdList = new List<LorIdXml>(),
                formationId = info.formationId,
                formationType = info.formationType,
                availableNumber = info.availableNumber,
                aggroScript = info.aggroScript,
                managerScript = info.managerScript
            };
            foreach (LorIdXml Lorid in info._enemyUnitIdList)
                output._enemyUnitIdList.Add(CopyXml(Lorid));
            output.enemyUnitIdList.AddRange(info.enemyUnitIdList);
            return output;
        }
        public static StageInvitationInfo CopyXml(StageInvitationInfo info)
        {
            StageInvitationInfo output = new StageInvitationInfo
            {
                combine = info.combine,
                _needsBooks = new List<LorIdXml>(),
                bookNum = info.bookNum,
                bookValue = info.bookValue
            };
            output.needsBooks.AddRange(info.needsBooks);
            return output;
        }
        public static StageExtraCondition CopyXml(StageExtraCondition info)
        {
            StageExtraCondition output = new StageExtraCondition
            {
                needClearStageList = new List<int>(),
                needLevel = info.needLevel
            };
            output.needClearStageList.AddRange(info.needClearStageList);
            return output;
        }
        public static StageStoryInfo CopyXml(StageStoryInfo info)
        {
            StageStoryInfo output = new StageStoryInfo
            {
                cond = info.cond,
                story = info.story,
                packageId=info.packageId,
                valid = info.valid,
                chapter = info.chapter,
                group = info.group,
                episode = info.episode
            };
            return output;
        }
        public static BookDropItemXmlInfo CopyXml(BookDropItemXmlInfo info)
        {
            BookDropItemXmlInfo output = new BookDropItemXmlInfo
            {
                id = info.id,
                pid=info.pid,
                itemType = info.itemType
            };
            return output;
        }
        public static LorIdXml CopyXml(LorIdXml info)
        {
            LorIdXml output = new LorIdXml()
            {
                pid=info.pid,
                xmlId=info.xmlId
            };
            return output;
        }
        public static FormationPositionXmlData CopyVector(FormationPositionXmlData info)
        {
            XmlVector2 vector = info.vector;
            FormationPositionXmlData output = new FormationPositionXmlData();
            XmlVector2 v = new XmlVector2() { x = vector.x, y = vector.y };
            output.vector= v;
            return output;
        }
        public static void EnhanceCard(BattleDiceCardModel card, int min = 0, int dice = 0, bool ignoreStandby=false)
        {
            DiceCardXmlInfo xml = card.XmlData.Copy(true);
            foreach (DiceBehaviour Dice in xml.DiceBehaviourList)
            {
                if (ignoreStandby && Dice.Type == BehaviourType.Standby)
                    continue;
                Dice.Dice += dice;
                Dice.Min += min;
            }
            card._xmlData = xml;
        }
        public static void EnhanceCard(int index, BattleDiceCardModel card, int min = 0, int dice = 0)
        {
            DiceCardXmlInfo xml = card.XmlData.Copy(true);
            DiceBehaviour Dice = xml.DiceBehaviourList[index];
            Dice.Dice += dice;
            Dice.Min += min;
            card._xmlData = xml;
        }
    }
}
