using UnityEngine;
using UnityEngine.UI;

namespace Script.Player {
    public class CardColorMask : MonoBehaviour {
        [SerializeField]
        protected Image m_ColorMask;        
        public bool Enabled;

        private void Awake() {
            m_ColorMask.gameObject.SetActive(false);
        }

        public void PointerEnter() {
            m_ColorMask.gameObject.SetActive(true);
            Enabled = true;
        }

        public void PointerExit() {
            m_ColorMask.gameObject.SetActive(false);
            Enabled = false;
        }
    }
}