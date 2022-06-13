using UnityEngine;
using Contingecy_Contract;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;

namespace ContractReward
{
    public class BehaviourAction_BlackSilenceCry : BehaviourActionBase
    {
        public override FarAreaEffect SetFarAreaAtkEffect(BattleUnitModel self)
        {
            this._self = self;
            FarAreaEffect_BlackSilenceCry silence4thAreaStrong = new GameObject().AddComponent<FarAreaEffect_BlackSilenceCry> ();
            silence4thAreaStrong.Init(self);
            return silence4thAreaStrong;
        }
    }
    public class FarAreaEffect_BlackSilenceCry: FarAreaeffect_BlackSilence_4th_Area_Strong
    {
        public override void OnGiveDamage()
        {
            state = EffectState.GiveDamage;
            Boom();
            PrintSound();
            isRunning = false;
        }
        public void Boom()
        {
            GameObject gameObject = Instantiate<GameObject>(StaticDataManager.VanilaGameObject["BlackSilence4Boom"]);
            gameObject.transform.SetParent(BattleObjectManager.instance.GetAliveList().Find(x=>x.passiveDetail.HasPassive<PassiveAbility_1700051>()).view.gameObject.transform);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localRotation = Quaternion.Euler(0.0f, 180f, 0.0f);
            gameObject.AddComponent<AutoDestruct>().time = 4f;
            gameObject.SetActive(true);
        }
    }
}
