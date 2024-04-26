using System;
using Script.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Script {
    public class EventManager : MonoBehaviour {
        public static EventManager Instance;
        //From,To
        public UnityEvent<PlayerStaffCardPresenter, PlayerStaffCardPresenter> PlayerSwitchCardPosition;
        public UnityEvent<PlayerStaffCardPresenter> PlayerSwitchCardToEnd;
        private void Awake() {
            Instance = this;
        }
    }
}