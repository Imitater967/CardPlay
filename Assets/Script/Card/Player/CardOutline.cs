using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.Player {
    public class CardOutline : MonoBehaviour {
        [SerializeField]
        protected Image outline;
        public bool Enabled;

        private void Awake() {
            outline.gameObject.SetActive(false);
        }

        public void PointerEnter() {
            outline.gameObject.SetActive(true);
            Enabled = true;
        }

        public void PointerExit() {
            outline.gameObject.SetActive(false);
            Enabled = false;
        }
    }
}