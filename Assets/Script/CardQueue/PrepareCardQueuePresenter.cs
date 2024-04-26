using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script {
        
    [Serializable]
    public class PrepareCardQueueView : CardQueueView{
        public Transform ItemContentRoot;
    }

    public abstract class PrepareCardQueuePresenter <C,M,V> : CardQueuePresenter<C,M,V> where C: StaffCardPresenter {
        public CardQueueItemPresenter ItemPrefab;
    }
}