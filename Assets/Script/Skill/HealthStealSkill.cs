using UnityEngine;

namespace Script {
    [CreateAssetMenu(menuName = "MyCard/Skill/生命偷取")]
    public class HealthStealSkill : Skill {
        [field: SerializeField,Tooltip("偷取目标攻击力数")]
        public float AtkSteal { get; private set; }
        [field: SerializeField,Tooltip("恢复本次造成伤害的生命值的百分比")]
        public float HealthStealPercent { get; private set; }
    }
}