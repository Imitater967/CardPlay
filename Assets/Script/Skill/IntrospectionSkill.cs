using UnityEngine;

namespace Script {
    [CreateAssetMenu(menuName = "MyCard/Skill/自省")]
    public class IntrospectionSkill : Skill {
        [field: SerializeField,Tooltip("自我造成最大生命值伤害百分比")]
        public float SelfDamagePercent { get; private set; }
        [field: SerializeField,Tooltip("护盾存在的时候获得的减伤")]
        public float DamageReducePercent { get; private set; }

    }
}