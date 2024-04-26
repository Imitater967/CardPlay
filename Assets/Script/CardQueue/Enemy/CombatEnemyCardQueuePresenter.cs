using System;
using System.Collections.Generic;

namespace Script.Enemy {

    [Serializable]
    public class CombatEnemyPrepareCardQueueView : CardQueueView {
        
    }
    public class CombatEnemyCardQueuePresenter: CardQueuePresenter<StaffCardPresenter,EnemyCardQueue,CombatEnemyPrepareCardQueueView> {
        private struct CardInfoGroup {
            public StaffCardPresenter StaffInfo;
        }
        
        private Dictionary<StaffCard, CardInfoGroup> m_Cache = new Dictionary<StaffCard, CardInfoGroup>();
        public List<StaffCardPresenter> Presenters = new List<StaffCardPresenter>();
        private void Awake() {
            foreach (var modelCard in this.Model.FieldCards) {
                modelCard.IsEnemy = true;
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