using System;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1890003 : PassiveAbilityBase
    {

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            //foreach (PassiveAbilityBase passive in this.owner.passiveDetail.PassiveList)
                //Contingecy_Contract.Debug.Log(passive.GetType().AssemblyQualifiedName);
            List<PassiveXmlInfo> list = new List<PassiveXmlInfo>();
            for (; list.Count<3;)
            {
                PassiveXmlInfo info = RandomUtil.SelectOne<PassiveXmlInfo>(Contingecy_Contract.Harmony_Patch.AvailablePassive);
                if (!list.Contains(info))
                    list.Add(info);
            }
            foreach (PassiveXmlInfo Info in list)
            {
                //Contingecy_Contract.Debug.Log("开始搜索被动");
                System.Type type = System.Type.GetType("PassiveAbility_" + Info.id.ToString()+ ", Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
                if (!(type == (System.Type)null))
                {
                    //Contingecy_Contract.Debug.Log("找到被动");
                    if (Activator.CreateInstance(type) is PassiveAbilityBase instance)
                    {
                        instance.name = Singleton<PassiveDescXmlList>.Instance.GetName(Info.id);
                        instance.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Info.id);
                        instance.rare = Info.rare;
                        this.owner.passiveDetail.AddPassive(instance);
                    }
                    //Contingecy_Contract.Debug.Log("获取被动");
                }
            }            
        }

    }
}
