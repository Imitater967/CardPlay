using System;
using Script.Player;
using Script.Prop;
using UnityEngine;

namespace Script {
    public class StaffCardPropConsumer : MonoBehaviour {
        private StaffCard m_StaffCard;

        private void Start() {
            m_StaffCard = GetComponent<StaffCardPresenter>().Model;
        }

        public void Consume(Prop.Prop prop) {
            switch (prop.PropType) {
                case PropType.SleepBag:
                    m_StaffCard.CurrentHealth.Value = m_StaffCard.MaxHealth.Value;
                    break;
                case PropType.BackgroundInvestigate:
                    m_StaffCard.AddBuffer(prop.Buffer);
                    break;
                case PropType.CommercialInsurance:
                    foreach (var modelFieldCard in GetComponentInParent<PreparePlayerPrepareCardQueuePresenter>().Model.FieldCards) {
                        modelFieldCard.CurrentShield.Value = Mathf.RoundToInt(modelFieldCard.MaxHealth.Value * 0.15f);
                    }
                    
                    foreach (var modelFieldCard in GetComponentInParent<PreparePlayerPrepareCardQueuePresenter>().Model.SubstituteCards) {
                        modelFieldCard.CurrentShield.Value = Mathf.RoundToInt(modelFieldCard.MaxHealth.Value * 0.15f);
                    }
                    break;
            }
        }
    }
}