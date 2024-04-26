using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Player {
    [Serializable]
    public class CombatPlayerPrepareCardQueueView : CardQueueView {
    }

    public class CombatPlayerCardQueuePresenter : CardQueuePresenter<StaffCardPresenter,PlayerCardQueue,CombatPlayerPrepareCardQueueView> {
        private struct CardInfoGroup {
            public StaffCardPresenter StaffInfo;
        }
        
        public List<StaffCardPresenter> Presenters = new List<StaffCardPresenter>();
        private Dictionary<StaffCard, CardInfoGroup> m_Cache = new Dictionary<StaffCard, CardInfoGroup>();
        private void Awake() {
            foreach (var modelCard in this.Model.FieldCards) {
                modelCard.IsEnemy = false;
                var staff = Instantiate(StaffCardPrefab, View.CardContentRoot);
                m_Cache[modelCard] = new CardInfoGroup() {
                    StaffInfo = staff,
                };
                Presenters.Add(staff);
                staff.Initialize(modelCard);
            }
        }
    }
}