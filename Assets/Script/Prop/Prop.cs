using System;
using UnityEngine;

namespace Script.Prop {
    public enum PropType {
        CommercialInsurance,
        SleepBag,
        BackgroundInvestigate, 
    }

    [CreateAssetMenu(menuName = "TheCard/Prop")]
    public class Prop : ScriptableObject {
        public Sprite Icon;
        public PropType PropType;
        public String Name;
        public Buffer Buffer;
        public bool IsPositive = true;
    }
}