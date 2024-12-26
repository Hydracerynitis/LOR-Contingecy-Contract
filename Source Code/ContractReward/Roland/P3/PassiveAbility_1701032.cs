using BaseMod;
using HP=Contingecy_Contract.CCInitializer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1701032 : PassiveAbilityBase
    {
        BattleUnitModel Roland;
        UnitKit original;
        public override void Init(BattleUnitModel self)
        {
            base.Init(self);
            this.name = Singleton<PassiveDescXmlList>.Instance.GetName(Tools.MakeLorId(1701032));
            this.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(Tools.MakeLorId(1701032));
            this.rare = Rarity.Unique;
        }
        public void Setup(BattleUnitModel r, UnitKit o)
        {
            Roland = r;
            original = o;
        }
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            base.OnDieOtherUnit(unit);
            if (unit== Roland)
            {
                owner.allyCardDetail = original.Deck;
                owner.personalEgoDetail = original.EGO;
                owner.passiveDetail = original.Passive;
                owner.breakDetail.LoseBreakGauge(owner.breakDetail.GetDefaultBreakGauge() / 2);
                owner.Book.SetHp(owner.Book.ClassInfo.EquipEffect.Hp);
                owner.Book.SetBp(owner.Book.ClassInfo.EquipEffect.Break);
                owner.Book.SetSpeedDiceMax(owner.Book.ClassInfo.EquipEffect.Speed);
                owner.Book.SetSpeedDiceMin(owner.Book.ClassInfo.EquipEffect.SpeedMin);
                owner.Book._maxPlayPoint = owner.Book.ClassInfo.EquipEffect.MaxPlayPoint;
                owner.Book.SetResistHP(BehaviourDetail.Slash, owner.Book.ClassInfo.EquipEffect.SResist);
                owner.Book.SetResistHP(BehaviourDetail.Penetrate, owner.Book.ClassInfo.EquipEffect.PResist);
                owner.Book.SetResistHP(BehaviourDetail.Hit, owner.Book.ClassInfo.EquipEffect.HResist);
                owner.Book.SetResistBP(BehaviourDetail.Slash, owner.Book.ClassInfo.EquipEffect.SBResist);
                owner.Book.SetResistBP(BehaviourDetail.Penetrate, owner.Book.ClassInfo.EquipEffect.PBResist);
                owner.Book.SetResistBP(BehaviourDetail.Hit, owner.Book.ClassInfo.EquipEffect.HBResist);
                if (HP.UnitBookId.ContainsKey(owner))
                {
                    owner.Book.ClassInfo._id = HP.UnitBookId[owner].id;
                    owner.Book.ClassInfo.workshopID = HP.UnitBookId[owner].packageId;
                    HP.UnitBookId.Remove(owner);
                }
                owner.view.ChangeSkin(owner.Book.ClassInfo.GetCharacterSkin());
                owner.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
                owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 2);
                owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Endurance, 2);
                owner.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_SoulLink));
            }
        }
    }
}
