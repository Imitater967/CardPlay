using System;
using Script.Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.Prop {
    [Serializable]
    public class PropView {
        public Image Icon;
        public TMP_Text Name;
    }

    public class PropPresenter: Presenter<Prop,PropView>, IPointerClickHandler {
        public CardOutline Outline;
        public override void Initialize(Prop model) {
            base.Initialize(model);
            View.Icon.sprite = model.Icon;
            View.Name.text = model.Name;
            // GetComponent<ActiveCardOutline>().TargetIsPlayerCard = this.Model.IsPositive;
        }

        public void OnPointerClick(PointerEventData eventData) {
            PrepareBattleManager.Instance.CurrentSelectedProp = this;
            Debug.Log(eventData.pointerClick.gameObject);
        }

        public void ActiveOutline() {
            Outline.PointerEnter();
        }

        public void DeactivateOutline() {
            Outline.PointerExit();
        }
    }
}