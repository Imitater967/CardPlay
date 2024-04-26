using UnityEngine;

namespace Script {
    [CreateAssetMenu(menuName = "MyCard/Skill/护盾")]
    public class ShieldSkill : Skill {
        [field: SerializeField,Tooltip("每过多少回合激活一次")]
        public int RoundToActive { get; private set; }
        [field: SerializeField,Tooltip("总生命值百分之多少的护盾")]
        public float ShieldAmountPercent { get; private set; }
        

    }
}