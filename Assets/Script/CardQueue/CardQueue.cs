using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script {
    public abstract class CardQueue : ScriptableObject {
       
        public ReactiveCollection<StaffCard> FieldCards;
        [SerializeField]
        private List<StaffCard> DefaultCards;

        //注意,这里是浅拷贝
        public virtual void Initialize() {
            FieldCards.Clear();
            for (var i = 0; i < DefaultCards.Count; i++) {
                DefaultCards[i].Inited = false;
                FieldCards.Add(DefaultCards[i]);
                DefaultCards[i].Initialize();
            }
        }
    }
}