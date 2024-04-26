using System;
using UnityEngine;

namespace Script {
    public enum BufferType {
        DamageReceiveIncrease
    }

    [CreateAssetMenu(menuName = "TheCard/Buff")]
    public class Buffer : ScriptableObject{
        public bool Positive;
        public BufferType BufferType;
        public String BufferName;
    }
}