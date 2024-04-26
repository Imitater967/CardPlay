using System;
using Script.Player;
using Script.Prop;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script {
    public class ActiveCardOutline: MonoBehaviour, IDragHandler ,IEndDragHandler {
        public bool TargetIsPlayerCard;
        private GameObject m_LastObject;
        public void OnDrag(PointerEventData eventData) {
            String tag = TargetIsPlayerCard ? "PlayerCard" : "EnemyCard";
            if (eventData.pointerCurrentRaycast.gameObject != m_LastObject) {
                if (m_LastObject != null && m_LastObject.CompareTag(tag)) {
                    m_LastObject.GetComponent<CardOutline>().PointerExit();
                }

                m_LastObject = eventData.pointerCurrentRaycast.gameObject;
                if (m_LastObject != null && m_LastObject.CompareTag(tag)) {
                    m_LastObject.GetComponent<CardOutline>().PointerEnter();
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (m_LastObject != null && (m_LastObject.CompareTag("PlayerCard")||m_LastObject.CompareTag("EnemyCard"))) {
                if (m_LastObject.GetComponent<CardOutline>().Enabled) {
                    m_LastObject.GetComponent<StaffCardPropConsumer>().Consume(GetComponent<PropPresenter>().Model);
                }
                m_LastObject.GetComponent<CardOutline>().PointerExit();
            }
        }
    }
}