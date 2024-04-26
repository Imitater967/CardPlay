using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Script.Helper;
using Script.Player;
using Script.Prop;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Player {

    [Serializable]
    public class PlayerPrepareCardQueueView : PrepareCardQueueView {
        public Transform SubstituteContentRoot;
    }

    public class PreparePlayerPrepareCardQueuePresenter : PrepareCardQueuePresenter<PlayerStaffCardPresenter,PlayerCardQueue,PlayerPrepareCardQueueView> {

        [Serializable]
        public struct CardCompound {
            public StaffCard Model;
            public PlayerStaffCardPresenter CardPresenter;
            public CardQueueItemPresenter ItemPresenter;
        }
        public Transform DragParent;
        [SerializeField]
        private List<CardCompound> m_FieldCardCompounds;
        [SerializeField]
        private List<CardCompound> m_SubstituteCardCompounds;
        private void Awake() {
            //初始化我方对症顺序
            InitializeBattleField();
            //初始化我方替补人员
            InitializeSubstitute();

            Model.SubstituteCards.ObserveReplace().Subscribe((x) => { SyncSubstituteModelReplace();}).AddTo(this);
            Model.FieldCards.ObserveReplace().Subscribe((x) => { SyncFieldModelReplace();}).AddTo(this);
            
            Model.SubstituteCards.ObserveAdd().Subscribe((x) => { SyncSubstituteModelReplace();}).AddTo(this);
            Model.FieldCards.ObserveAdd().Subscribe((x) => { SyncFieldModelReplace();}).AddTo(this);

            return;

        }

        private void Start() {
            EventManager.Instance.PlayerSwitchCardPosition.AsObservable().Subscribe(ProcessModelCardSwitch).AddTo(this);
            EventManager.Instance.PlayerSwitchCardToEnd.AsObservable().Subscribe(ProcessModelCardEnd).AddTo(this);
            PrepareBattleManager.Instance.OnPropSelected.AsObservable().Subscribe(IfPositiveEnable).AddTo(this);
            PrepareBattleManager.Instance.OnPropDeslected.AsObservable().Subscribe(IfPositiveDisable).AddTo(this);
            PrepareBattleManager.Instance.OnPropSelected.AsObservable().Subscribe(IfNegativeEnable).AddTo(this);
            PrepareBattleManager.Instance.OnPropDeslected.AsObservable().Subscribe(IfNegativeDisable).AddTo(this);

            PrepareBattleManager.Instance.OnPropDeslected.AsObservable().Subscribe((x) => {
                foreach (var cardInfoGroup in this.m_FieldCardCompounds) {
                    cardInfoGroup.CardPresenter.GetComponent<ItemDrag>().Enable();
                }

                foreach (var cardInfoGroup in this.m_SubstituteCardCompounds) {
                    cardInfoGroup.CardPresenter.GetComponent<ItemDrag>().Enable();
                }
            }).AddTo(this);
            
            PrepareBattleManager.Instance.OnPropSelected.AsObservable().Subscribe((x) => {
                foreach (var cardInfoGroup in this.m_FieldCardCompounds) {
                    cardInfoGroup.CardPresenter.GetComponent<ItemDrag>().Disable();
                }

                foreach (var cardInfoGroup in this.m_SubstituteCardCompounds) {
                    cardInfoGroup.CardPresenter.GetComponent<ItemDrag>().Disable();
                }
            }).AddTo(this);
        }
        
        private void IfNegativeDisable(PropPresenter prop) {
            if (prop.Model.IsPositive) {
                return;
            }

            foreach (var cardInfoGroup in this.m_FieldCardCompounds) {
                cardInfoGroup.CardPresenter.GetComponent<CardColorMask>().PointerExit();
            }
            foreach (var cardInfoGroup in this.m_SubstituteCardCompounds) {
                cardInfoGroup.CardPresenter.GetComponent<CardColorMask>().PointerExit();
            }
        }
        private void IfNegativeEnable(PropPresenter prop) {
            if (prop.Model.IsPositive) {
                return;
            }
            foreach (var cardInfoGroup in this.m_SubstituteCardCompounds) {
                cardInfoGroup.CardPresenter.GetComponent<CardColorMask>().PointerEnter();
            }
            foreach (var cardInfoGroup in this.m_FieldCardCompounds) {
                cardInfoGroup.CardPresenter.GetComponent<CardColorMask>().PointerEnter();
            }
        }
        
        private void IfPositiveDisable(PropPresenter prop) {
            if (!prop.Model.IsPositive) {
                return;
            }

            foreach (var cardInfoGroup in this.m_FieldCardCompounds) {
                cardInfoGroup.CardPresenter.GetComponent<CardOutline>().PointerExit();
            }
            foreach (var cardInfoGroup in this.m_SubstituteCardCompounds) {
                cardInfoGroup.CardPresenter.GetComponent<CardOutline>().PointerExit();
            }
        }
        private void IfPositiveEnable(PropPresenter prop) {
            if (!prop.Model.IsPositive) {
                return;
            }

            foreach (var cardInfoGroup in this.m_FieldCardCompounds) {
                cardInfoGroup.CardPresenter.GetComponent<CardOutline>().PointerEnter();
            }
            foreach (var cardInfoGroup in this.m_SubstituteCardCompounds) {
                cardInfoGroup.CardPresenter.GetComponent<CardOutline>().PointerEnter();
            }
        }
        
        void InitializeSubstitute() {
            foreach (var modelCard in this.Model.SubstituteCards) {
                var staff = Instantiate(StaffCardPrefab, View.SubstituteContentRoot);
                m_SubstituteCardCompounds.Add(new CardCompound(){Model = modelCard,CardPresenter = staff});
                staff.Initialize(modelCard);
                staff.GetComponent<ItemDrag>().Initialize(DragParent);
                staff.IsSubstitute = true;

            }
        }

        void InitializeBattleField() {
            foreach (var modelCard in this.Model.FieldCards) {
                var item = Instantiate(ItemPrefab, View.ItemContentRoot);
                var staff = Instantiate(StaffCardPrefab, View.CardContentRoot);
                m_FieldCardCompounds.Add(new CardCompound() {
                    Model = modelCard,CardPresenter = staff,
                    ItemPresenter = item,
                });
                item.Initialize(modelCard);
                staff.GetComponent<ItemDrag>().Initialize(DragParent);
                staff.Initialize(modelCard);
                staff.IsSubstitute = false;
            }
        }
        //M->P->V
        private void SyncSubstituteModelReplace() {
            foreach (var substituteCardCompound in m_SubstituteCardCompounds) {
                Destroy(substituteCardCompound.CardPresenter.gameObject);
            }
            m_SubstituteCardCompounds.Clear();
            InitializeSubstitute();
        }
        private void SyncFieldModelReplace() {
            foreach (var substituteCardCompound in m_FieldCardCompounds) {
                Destroy(substituteCardCompound.CardPresenter.gameObject);
                Destroy(substituteCardCompound.ItemPresenter.gameObject);
            }
            m_FieldCardCompounds.Clear();
            InitializeBattleField();
        }

        private void ProcessModelCardEnd(PlayerStaffCardPresenter itemDrag) {
            if (itemDrag.IsSubstitute) {
                int dragIndex = Model.SubstituteCards.IndexOf(itemDrag.Model);
                Model.SubstituteCards.RemoveAt(dragIndex);
                Model.SubstituteCards.Add(itemDrag.Model);
            }

            if (!itemDrag.IsSubstitute) {
                int dragIndex = Model.FieldCards.IndexOf(itemDrag.Model);
                Model.FieldCards.RemoveAt(dragIndex);
                Model.FieldCards.Add(itemDrag.Model);
            }
        }
        //V->Event(P)->M
        private void ProcessModelCardSwitch(Tuple<PlayerStaffCardPresenter, PlayerStaffCardPresenter> x) {
            var itemDrag = x.Item1;
            var itemDrop = x.Item2;
            if (itemDrag.IsSubstitute && itemDrop.IsSubstitute) {
                int dragIndex = Model.SubstituteCards.IndexOf(itemDrag.Model);
                int dropIndex = Model.SubstituteCards.IndexOf(itemDrop.Model);
                Model.SubstituteCards.SwapElement(dragIndex,dropIndex);
                return;
            }

            if (!itemDrag.IsSubstitute && !itemDrop.IsSubstitute) {
                int dragIndex = Model.FieldCards.IndexOf(itemDrag.Model);
                int dropIndex = Model.FieldCards.IndexOf(itemDrop.Model);
                Model.FieldCards.SwapElement(dragIndex,dropIndex);
                return;
            }

            if (itemDrag.IsSubstitute && !itemDrop.IsSubstitute) {
                int dropIndex = Model.FieldCards.IndexOf(itemDrop.Model);
                int dragIndex = Model.SubstituteCards.IndexOf(itemDrag.Model);
                Model.FieldCards.Remove(itemDrop.Model);
                Model.SubstituteCards.Remove(itemDrag.Model);
                Model.FieldCards.Insert(dropIndex,itemDrag.Model);
                Model.SubstituteCards.Insert(dragIndex,itemDrop.Model);
            }
            if (!itemDrag.IsSubstitute && itemDrop.IsSubstitute) {
                int dropIndex = Model.SubstituteCards.IndexOf(itemDrop.Model);
                int dragIndex = Model.FieldCards.IndexOf(itemDrag.Model);
                Model.FieldCards.Remove(itemDrag.Model);
                Model.SubstituteCards.Remove(itemDrop.Model);
                Model.FieldCards.Insert(dragIndex,itemDrop.Model);
                Model.SubstituteCards.Insert(dropIndex,itemDrag.Model);
            }
        }
        
    }
}
