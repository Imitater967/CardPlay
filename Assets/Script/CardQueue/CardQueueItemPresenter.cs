using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace Script {
    [Serializable]
    public class CardQueueItemView {
        public TMP_Text Atk;
        public TMP_Text Health;
        public TMP_Text Skill;
    }

    public class CardQueueItemPresenter : Presenter<StaffCard,CardQueueItemView> {
        public override void Initialize(StaffCard model) {
            base.Initialize(model);
            View.Atk.text = $"攻击: {model.Attack.Value}";
            View.Health.text = $"生命值: {model.CurrentHealth.Value}/{model.MaxHealth.Value}";
            View.Skill.text = $"技能: {model.Skill.name}";
            model.Attack.Subscribe(x => {
                View.Atk.text = $"攻击: {x}";
            }).AddTo(this);
            model.CurrentHealth.Subscribe(x => {
                View.Health.text = $"生命值: {x}/{model.MaxHealth.Value}";
            }).AddTo(this);
        }
    }
}