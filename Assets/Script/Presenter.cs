using UnityEngine;

namespace Script {
    public abstract class Presenter<M,V> : MonoBehaviour {
        [field: SerializeField]
        public M Model { get; protected set; }
        [field: SerializeField]
        public V View { get; protected set; }

        public virtual void Initialize(M model) {
            Model = model;
        }
    }
}