using System;
using System.Collections.Generic;
using Script.Helper;
using Script.Player;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script {
    
    [CreateAssetMenu(menuName = "TheCard/SelfCardQueue")]
    public class PlayerCardQueue : CardQueue {
        public static PlayerCardQueue Instance {
            get { return m_Instance; }
        }

        private static PlayerCardQueue m_Instance;
        [SerializeField]
        private List<StaffCard> DefaultSubstitute;
        [SerializeField]
        private List<Prop.Prop> DefaultProps;
        
        public ReactiveCollection<Prop.Prop> Props;
        public ReactiveCollection<StaffCard> SubstituteCards;
        public override void Initialize() {
            m_Instance = this;
            base.Initialize();
            SubstituteCards.Clear();
            Props.Clear();
            
            for (var i = 0; i < DefaultSubstitute.Count; i++) {
                SubstituteCards.Add(DefaultSubstitute[i]);
            }
            
            for (var i = 0; i < DefaultProps.Count; i++) {
                Props.Add(DefaultProps[i]);
            }
        }
    }
}