using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;
using UnityEngine.EventSystems;

public class PropDeselectHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData) {
        PrepareBattleManager.Instance.CurrentSelectedProp = null;
    }
}
