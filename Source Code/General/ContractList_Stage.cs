using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LOR_DiceSystem;
using System.Text;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class StageModifier_Duplicate : StageModifier
    {
        public StageModifier_Duplicate(int Level)
        {
            this.Level = Level;
        }
        public override void Modify(ref StageClassInfo info)
        {
            int index = UnityEngine.Random.Range(0, info.waveList.Count);
            StageWaveInfo reinforce = info.waveList[index];
            info.waveList.Insert(index,DeepCopyUtil.CopyXml(reinforce));
        }
        public override bool IsValid(StageClassInfo info)
        {
            return info.stageType == StageType.Invitation;
        }
    }
    public class ContingecyContract_Duplicate : ContingecyContract
    {
        public ContingecyContract_Duplicate(int level)
        {
            this.Level = level;
        }
    }
    public class StageModifier_Last: StageModifier
    {
        public StageModifier_Last(int Level)
        {
            this.Level = Level;
        }
        public override bool IsValid(StageClassInfo info)
        {
            return info.floorNum!=1;
        }
        public override void Modify(ref StageClassInfo info)
        {
            info.floorNum = 1;
        }
    }
    public class ContingecyContract_Last : ContingecyContract
    {
        public ContingecyContract_Last(int level)
        {
            this.Level = level;
        }
    }
    public class StageModifier_LowFloor : StageModifier
    {
        public StageModifier_LowFloor(int Level)
        {
            this.Level = Level;
        }
        public override bool IsValid(StageClassInfo info)
        {
            if (Level == 1)
                return info.exceptFloorList.Count < 5 && info.floorOnlyList.Count == 0;
            else
                return info.floorOnlyList.Count == 0;
        }
        public override void Modify(ref StageClassInfo info)
        {
            if (Level == 1)
            {
                while (info.exceptFloorList.Count < 5)
                {
                    SephirahType sephirah = RandomUtil.SelectOne(SephirahType.Malkuth, SephirahType.Yesod, SephirahType.Hod, SephirahType.Netzach, SephirahType.Tiphereth, SephirahType.Gebura, SephirahType.Chesed, SephirahType.Binah, SephirahType.Hokma, SephirahType.Keter);
                    if (!info.exceptFloorList.Contains(sephirah))
                        info.exceptFloorList.Add(sephirah);
                }
                return;
            }
            else
            {
                info.floorOnlyList.Add(RandomUtil.SelectOne(SephirahType.Malkuth, SephirahType.Yesod, SephirahType.Hod, SephirahType.Netzach, SephirahType.Tiphereth, SephirahType.Gebura, SephirahType.Chesed, SephirahType.Binah, SephirahType.Hokma, SephirahType.Keter));
            }
        }
    }
    public class ContingecyContract_LowFloor : ContingecyContract
    {
        public ContingecyContract_LowFloor(int level)
        {
            this.Level = level;
        }
        public override string[] GetFormatParam(string language) => Level==1? new string[] {"5"} : new string[] { "9"};
    }
    public class StageModifier_LowPeople: StageModifier
    {
        public StageModifier_LowPeople(int Level)
        {
            this.Level = Level;
        }
        public override bool IsValid(StageClassInfo info)
        {
            foreach(StageWaveInfo wave in info.waveList)
            {
                if (wave.availableNumber > 1)
                    return true;
            }
            return false;
        }
        public override void Modify(ref StageClassInfo info)
        {
            foreach (StageWaveInfo wave in info.waveList)
            {
                if (wave.availableNumber > 1)
                    wave.availableNumber -= 1;
            }
        }
    }
    public class ContingecyContract_LowPeople: ContingecyContract
    {
        public ContingecyContract_LowPeople(int level)
        {
            this.Level = level;
        }
    }
}
