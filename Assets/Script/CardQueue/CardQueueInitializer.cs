using System;
using UnityEngine;

namespace Script {
    [DefaultExecutionOrder(-100)]
    public class CardQueueInitializer : MonoBehaviour {
        public CardQueue[] Queues;

        private void Awake() {
            foreach (var cardQueue in Queues) {
                cardQueue.Initialize();
            }
        }
    }
}