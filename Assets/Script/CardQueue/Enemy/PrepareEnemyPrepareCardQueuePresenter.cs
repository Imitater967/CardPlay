using System;
using System.Collections.Generic;
using Script.Player;
using Script.Prop;
using UniRx;

namespace Script.Enemy {
    [Serializable]
    public class EnemyPrepareCardQueueView : PrepareCardQueueView{
    }


    public class PrepareEnemyPrepareCardQueuePresenter : PrepareCardQueuePresenter<StaffCardPresenter,CardQueue,EnemyPrepareCardQueueView> {

        private struct CardInfoGroup {
            public CardQueueItemPresenter ItemInfo;
            public StaffCardPresenter StaffInfo;
        }
        
        private Dictionary<StaffCard, CardInfoGroup> m_Cache = new Dictionary<StaffCard, CardInfoGroup>();
        private void Awake() {
            foreach (var modelCard in this.Model.FieldCards) {
                var item = Instantiate(ItemPrefab, View.ItemContentRoot);
                var staff = Instantiate(StaffCardPrefab, View.CardContentRoot);
                m_Cache[modelCard] = new CardInfoGroup() {
                    ItemInfo = item,
                    StaffInfo = staff,
                };
                item.Initialize(modelCard);
                staff.Initialize(modelCard);
            }
        }

        private void Start() {
            PrepareBattleManager.Instance.OnPropSelected.AsObservable().Subscribe(IfNegativeEnable).AddTo(this);
            PrepareBattleManager.Instance.OnPropDeslected.AsObservable().Subscribe(IfNegativeDisable).AddTo(this);
            PrepareBattleManager.Instance.OnPropSelected.AsObservable().Subscribe(IfPositiveEnable).AddTo(this);
            PrepareBattleManager.Instance.OnPropDeslected.AsObservable().Subscribe(IfPositiveDisable).AddTo(this);

        }
        private void IfPositiveDisable(PropPresenter prop) {
            if (!prop.Model.IsPositive) {
                return;
            }

            foreach (var cardInfoGroup in this.m_Cache.Values) {
                cardInfoGroup.StaffInfo.GetComponent<CardColorMask>().PointerExit();
            }
        }
        private void IfPositiveEnable(PropPresenter prop) {
            if (!prop.Model.IsPositive) {
                return;
            }

            foreach (var cardInfoGroup in this.m_Cache.Values) {
                cardInfoGroup.StaffInfo.GetComponent<CardColorMask>().PointerEnter();
            }
        }
        private void IfNegativeDisable(PropPresenter prop) {
            if (prop.Model.IsPositive) {
                return;
            }

            foreach (var cardInfoGroup in this.m_Cache.Values) {
                cardInfoGroup.StaffInfo.GetComponent<CardOutline>().PointerExit();
            }
        }
        private void IfNegativeEnable(PropPresenter prop) {
            if (prop.Model.IsPositive) {
                return;
            }

            foreach (var cardInfoGroup in this.m_Cache.Values) {
                cardInfoGroup.StaffInfo.GetComponent<CardOutline>().PointerEnter();
            }
        }
    }
}