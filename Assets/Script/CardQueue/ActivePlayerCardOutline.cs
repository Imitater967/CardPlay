using Script.Player;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script {
    public class ActivePlayerCardOutline : MonoBehaviour, IDragHandler , IEndDragHandler{
        private GameObject m_LastObject;
        public void OnDrag(PointerEventData eventData) {
            if (eventData.pointerCurrentRaycast.gameObject != m_LastObject) {
                if (m_LastObject != null && m_LastObject.CompareTag("PlayerCard")) {
                    m_LastObject.GetComponent<CardOutline>().PointerExit();
                }

                m_LastObject = eventData.pointerCurrentRaycast.gameObject;
                if (m_LastObject != null && m_LastObject.CompareTag("PlayerCard")) {
                    m_LastObject.GetComponent<CardOutline>().PointerEnter();
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (m_LastObject != null && m_LastObject.CompareTag("PlayerCard")) {
                m_LastObject.GetComponent<CardOutline>().PointerExit();
            }
        }
    }
}