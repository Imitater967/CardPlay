using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.Combat {
    [Serializable]
    public class CombatUIView {
        [FormerlySerializedAs("Pause")] public Button PauseBnt;
        [FormerlySerializedAs("Scale")] public Button ScaleBnt;
        [FormerlySerializedAs("Skip")] public Button SkipBnt;
        [FormerlySerializedAs("Surrender")] public Button SurrenderBnt;
        
        public void PauseAction() {
            PauseBnt.GetComponentInChildren<TMP_Text>().text = "恢复";
        }

        public void UnpauseAction() {
            PauseBnt.GetComponentInChildren<TMP_Text>().text = "暂停";
        }
        
        public void ScaleAction() {
            ScaleBnt.GetComponentInChildren<TMP_Text>().text = "X1";
        }

        public void UnScaleAction() {
            ScaleBnt.GetComponentInChildren<TMP_Text>().text = "X2";
        }
    }

    public class CombatUIPresenter : MonoBehaviour {
        public CombatUIView UIView;
        public bool Pause;
        public bool Scale;
        private void Awake() {
            UIView.PauseBnt.onClick.AddListener(TogglePause);
            UIView.ScaleBnt.onClick.AddListener(ToggleScale);
        }

        private void ToggleScale() {
            if (Scale) {
                UIView.UnScaleAction();
                Time.timeScale = 1;
                Scale = false;
            }
            else {
                UIView.ScaleAction();
                Time.timeScale = 5;
                Scale = true;
            }
        }

        private void TogglePause() {
            if (Pause) {
                UIView.UnpauseAction();
                Time.timeScale = 1;
                Pause = false;
            }
            else {
                UIView.PauseAction();
                Time.timeScale = 0;
                Pause = true;
            }
        }
    }
}