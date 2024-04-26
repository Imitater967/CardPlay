using System;
using UnityEngine;

namespace Script {
    public class Skill : ScriptableObject {
        [field: SerializeField]
        public Sprite Icon;

        public String Description;
    }
}