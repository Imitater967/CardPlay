using System;
using System.Collections.Generic;
using Script.Prop;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Player {
    [Serializable]
    public class PlayerPropView {
        public List<Transform> ItemSlot;
    }

    public class PlayerPropListPresenter: Presenter<PlayerCardQueue,PlayerPropView> {
        public PropPresenter PropPrefab;
        public Transform DragParent;
        private Dictionary<Prop.Prop, PropPresenter> m_Cache = new Dictionary<Prop.Prop, PropPresenter>();
        private void Awake() {
            for (var i = 0; i < Model.Props.Count && i < View.ItemSlot.Count; i++) {
                var model = Model.Props[i];
                var parent = View.ItemSlot[i];
                var prop = Instantiate(PropPrefab, parent);
                prop.Initialize(model);
                // prop.GetComponent<ItemDrag>().Initialize(DragParent);
                m_Cache[model] = prop;
            }
        }
    }
}