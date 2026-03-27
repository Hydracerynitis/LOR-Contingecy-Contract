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
        public override int GetPriority()
        {
            return -5;
        }
        public override void Modify(ref StageClassInfo info)
        {
            int index = info.waveList.Count-1;
            StageWaveInfo reinforce = info.waveList[index];
            info.waveList.Insert(index,DeepCopyUtil.CopyXml(reinforce));
        }
        public override bool IsValid(StageClassInfo info)
        {
            return info.stageType == StageType.Invitation;
        }
    }
    public class StageModifier_Last: StageModifier
    {
        public override bool IsValid(StageClassInfo info)
        {
            return info.floorNum!=1;
        }
        public override void Modify(ref StageClassInfo info)
        {
            info.floorNum = 1;
        }
    }
    public class StageModifier_LowFloor : StageModifier
    {
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
    public class StageModifier_LowPeople: StageModifier
    {
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
}
