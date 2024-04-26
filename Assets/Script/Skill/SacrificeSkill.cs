using UnityEngine;

namespace Script {
    [CreateAssetMenu(menuName = "MyCard/Skill/牺牲")]
    public class SacrificeSkill : Skill {
        [field: SerializeField]
        public float DamagePercent { get; private set; }
    }
}