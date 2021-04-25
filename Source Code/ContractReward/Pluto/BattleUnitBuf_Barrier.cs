using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class BattleUnitBuf_Barrier: BattleUnitBuf
    {
        private int count;
        private GameObject _auraEffect;
        public BattleUnitModel Pluto;
        private bool _bActivated;
        protected override string keywordId => "Barrier";
        protected override string keywordIconId => "Pluto_Barrier";
        public override bool IsActionable() => false;
        public override bool IsTargetable() => false;
        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            this.stack = 0;
            count = 0;
            this._bActivated = false;
        }
        public override void OnRoundEndTheLast()
        {
            if (!this._bActivated)
            {
                this._bActivated = true;
                UnityEngine.Object original = Resources.Load("Prefabs/Battle/DiceAttackEffects/New/FX/Mon/Pluto/FX_Mon_Pluto_Lock");
                if (!(original != (UnityEngine.Object)null))
                    return;
                GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
                if (!((UnityEngine.Object)gameObject != (UnityEngine.Object)null))
                    return;
                if ((UnityEngine.Object)this._auraEffect != (UnityEngine.Object)null)
                    UnityEngine.Object.Destroy((UnityEngine.Object)this._auraEffect);
                this._auraEffect = gameObject;
                Pluto1st_BarrierAura component = gameObject.GetComponent<Pluto1st_BarrierAura>();
                if (!((UnityEngine.Object)component != (UnityEngine.Object)null))
                    return;
                component.Init(this._owner.view);
            }
            else
            {
                count += 1;
                if (count == 2)
                    this.Destroy();
            }
        }
        public override void OnDie()
        {
            base.OnDie();
            Pluto.allyCardDetail.AddNewCard(18900005);
            if (!((UnityEngine.Object)this._owner?.view != (UnityEngine.Object)null))
                return;
            this._owner.view.deadEvent += new BattleUnitView.DeadEvent(this.OnDeadEvent);
        }

        private void OnDeadEvent(BattleUnitView view)
        {
            Pluto.allyCardDetail.AddNewCard(18900005);
            if (!((UnityEngine.Object)this._auraEffect != (UnityEngine.Object)null))
                return;
            UnityEngine.Object.Destroy((UnityEngine.Object)this._auraEffect);
        }

        public override void Destroy()
        {
            base.Destroy();
            Pluto.allyCardDetail.AddNewCard(18900005);
            if (!((UnityEngine.Object)this._auraEffect != (UnityEngine.Object)null))
                return;
            UnityEngine.Object.Destroy((UnityEngine.Object)this._auraEffect);
        }
    }
}
