using BaseMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace ContractReward
{
    public class PassiveAbility_1700031 : PassiveAbilityBase
    {
        public UnitKit original=new UnitKit();
        public BattleUnitModel Angelica=null;
        public UnitKit solo = new UnitKit();
        public UnitKit coop =new UnitKit();
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            owner.personalEgoDetail.AddCard(Tools.MakeLorId(17000331));
            solo.Deck = owner.allyCardDetail;
            solo.Passive = owner.passiveDetail;
            solo.EGO = owner.personalEgoDetail;
            coop.EGO = new BattlePersonalEgoCardDetail(owner);
            coop.EGO.Init();
            coop.Deck=new BattleAllyCardDetail(owner);
            List<DiceCardXmlInfo> list = new List<DiceCardXmlInfo>();
            foreach (LorId i in Singleton<DeckXmlList>.Instance.GetData(Tools.MakeLorId(17000203)).cardIdList)
                list.Add(ItemXmlDataList.instance.GetCardItem(i));
            coop.Deck.Init(list);
            coop.Deck.DrawCards(owner.Book.GetStartDraw() + 1);
            coop.Passive = new BattleUnitPassiveDetail(owner);
            coop.Passive._passiveList = new List<PassiveAbilityBase>(owner.passiveDetail.PassiveList);
            coop.Passive._passiveList.RemoveAll(x => x is PassiveAbility_170006 || x is PassiveAbility_260004 || x is PassiveAbility_250002);
            coop.Passive._passiveList.Add(new PassiveAbility_1701030(owner));
            coop.Passive._passiveList.Add(new PassiveAbility_1700032(owner));
            coop.Passive._passiveList.Add(new PassiveAbility_1700033(owner));
        }

        public void Dance(BattleUnitModel unit)
        {
            if (unit.faction != owner.faction)
                return;
            BookModel angelica = new BookModel(Singleton<BookXmlList>.Instance.GetData(Tools.MakeLorId(17000103)));
            Angelica = unit;
            original.Deck = unit.allyCardDetail;
            original.Passive = unit.passiveDetail;
            original.EGO = unit.personalEgoDetail;
            Contingecy_Contract.CCInitializer.UnitBookId.Add(Angelica, Angelica.Book.GetBookClassInfoId());
            Angelica.Book.SetHp(angelica.HP);
            Angelica.Book.SetBp(angelica.Break);
            Angelica.Book.SetSpeedDiceMax(angelica.SpeedMax);
            Angelica.Book.SetSpeedDiceMin(angelica.SpeedMin);
            Angelica.Book.SetResistHP(BehaviourDetail.Slash, angelica.GetResistHP(BehaviourDetail.Slash));
            Angelica.Book.SetResistHP(BehaviourDetail.Penetrate, angelica.GetResistHP(BehaviourDetail.Penetrate));
            Angelica.Book.SetResistHP(BehaviourDetail.Hit, angelica.GetResistHP(BehaviourDetail.Hit));
            Angelica.Book.SetResistBP(BehaviourDetail.Slash, angelica.GetResistBP(BehaviourDetail.Slash));
            Angelica.Book.SetResistBP(BehaviourDetail.Penetrate, angelica.GetResistBP(BehaviourDetail.Penetrate));
            Angelica.Book.SetResistBP(BehaviourDetail.Hit, angelica.GetResistBP(BehaviourDetail.Hit));
            Angelica.Book._maxPlayPoint=angelica.GetMaxPlayPoint();
            Angelica.Book.ClassInfo._id = angelica.GetBookClassInfoId().id;
            Angelica.Book.ClassInfo.workshopID = angelica.GetBookClassInfoId().packageId;
            Angelica.view.ChangeSkin(angelica.GetCharacterName());
            Angelica.view.charAppearance.ChangeMotion(ActionDetail.Standing);
            Angelica.cardSlotDetail.LosePlayPoint(Angelica.cardSlotDetail.GetMaxPlayPoint());
            Angelica.cardSlotDetail.RecoverPlayPoint(Angelica.cardSlotDetail.GetMaxPlayPoint());
            Angelica.allyCardDetail = new BattleAllyCardDetail(Angelica);
            List<DiceCardXmlInfo> list = new List<DiceCardXmlInfo>();
            foreach (LorId i in Singleton<DeckXmlList>.Instance.GetData(Tools.MakeLorId(17000103)).cardIdList)
                list.Add(ItemXmlDataList.instance.GetCardItem(i));
            Angelica.allyCardDetail.Init(list);
            Angelica.allyCardDetail.DrawCards(4);
            Angelica.personalEgoDetail = new BattlePersonalEgoCardDetail(Angelica);
            Angelica.personalEgoDetail.Init();
            Angelica.passiveDetail = new BattleUnitPassiveDetail(owner);
            foreach (GiftModel equipped in Angelica.UnitData.unitData.giftInventory.GetEquippedList())
                Angelica.passiveDetail._passiveList.AddRange(equipped.CreateScripts());
            foreach (PassiveAbilityBase passive in Angelica.passiveDetail._passiveList)
                passive.Init(Angelica);
            PassiveAbilityBase speed = new PassiveAbility_10008();
            speed.owner = unit;
            speed.name = Singleton<PassiveDescXmlList>.Instance.GetName(10008);
            speed.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(10008);
            speed.rare = Rarity.Unique;
            Angelica.passiveDetail._passiveList.Add(speed);
            Angelica.passiveDetail._passiveList.Add(new PassiveAbility_1700000(Angelica));
            Angelica.passiveDetail._passiveList.Add(new PassiveAbility_1701030(Angelica));
            Angelica.passiveDetail._passiveList.Add(new PassiveAbility_1701031(Angelica));
            Angelica.passiveDetail._passiveList.Add(new PassiveAbility_1701032(Angelica,owner, original));
            owner.allyCardDetail = coop.Deck;
            owner.personalEgoDetail = coop.EGO;
            owner.passiveDetail = coop.Passive;
            owner.bufListDetail.AddBuf(new BattleUnitBuf_SoulLink(Angelica) { stack = 4 });
            Angelica.bufListDetail.AddBuf(new BattleUnitBuf_SoulLink(owner) { stack = 4 });
            owner.OnRoundStartOnlyUI();
            owner.RollSpeedDice();
            Angelica.OnRoundStartOnlyUI();
            Angelica.RollSpeedDice();
        }
    }
    public class UnitKit
    {
        public BattleAllyCardDetail Deck;
        public BattleUnitPassiveDetail Passive;
        public BattlePersonalEgoCardDetail EGO;
    }
}
