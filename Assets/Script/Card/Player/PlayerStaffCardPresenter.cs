using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Script.Player {
    public class PlayerStaffCardPresenter : StaffCardPresenter ,IEndDragHandler{
        [FormerlySerializedAs("IsSubtitute")] public bool IsSubstitute;
        public void OnEndDrag(PointerEventData eventData) {
            var currentObject = eventData.pointerCurrentRaycast.gameObject;
            if (currentObject!=null && currentObject.TryGetComponent(out PlayerStaffCardPresenter presenter)) {
                EventManager.Instance.PlayerSwitchCardPosition.Invoke(presenter,this);
                Debug.Log($"Switch Position Between {presenter.Name} And {this.Name}");
            }
            else {
                EventManager.Instance.PlayerSwitchCardToEnd.Invoke(this);
                Debug.Log($"Switch {Name} To The End");
            }
        }

        public string Name {
            get => Model.Prototype.Name;
        }
    }
}