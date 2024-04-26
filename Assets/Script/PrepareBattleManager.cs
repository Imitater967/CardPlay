using System;
using Script.Prop;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Script {
    public class PrepareBattleManager : MonoBehaviour {
        public static PrepareBattleManager Instance => m_Instance;
        private static PrepareBattleManager m_Instance;
        public UnityEvent<PropPresenter> OnPropSelected;
        public UnityEvent<PropPresenter> OnPropDeslected;
        public PropPresenter CurrentSelectedProp {
            set {
                if (m_CurrentSelectedProp == value) {
                    return;
                }

                if (CurrentSelectedProp != null) {
                    CurrentSelectedProp.DeactivateOutline();
                    OnPropDeslected?.Invoke(CurrentSelectedProp);
                }

                m_CurrentSelectedProp = value;
                if (CurrentSelectedProp != null) {
                    CurrentSelectedProp.ActiveOutline();
                    OnPropSelected?.Invoke(CurrentSelectedProp);
                }
            }
            get { return m_CurrentSelectedProp; }
        }

        private PropPresenter m_CurrentSelectedProp;

        private void Awake() {
            m_Instance = this;
        }
        
        
    }
}