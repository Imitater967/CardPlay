using System;
using UnityEngine;

namespace Script {
    
    [Serializable]
    public class CardQueueView {
        public Transform CardContentRoot;
    }
    public class CardQueuePresenter <C,M,V> : Presenter<M,V> where C: StaffCardPresenter{
        
        public C StaffCardPrefab;
    }
}