using System;
using Script.Player;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script {

    [Serializable]
    public class StaffView {
        [Tooltip("生命值条")]
        public Slider HealthBar;
        [Tooltip("生命值条")]
        public Slider ShieldBar;
        [Tooltip("生命值条的数值显示")]
        public TMP_Text HealthText;
        [Tooltip("护盾条的数值显示")]
        public TMP_Text ShieldText;
        [Tooltip("头像")]
        public Image Avatar;
        [Tooltip("攻击力")]
        public TMP_Text Atk;
        [Tooltip("人物姓名")]
        public TMP_Text Name;

        [FormerlySerializedAs("SkillIcon")] [Tooltip("技能")]
        public SkillPreview SkillPreview;
    }

    public class StaffCardPresenter: Presenter<StaffCard,StaffView> , IPointerClickHandler{
        public override void Initialize(StaffCard model) {
            base.Initialize(model);
            View.Name.text = model.Prototype.Name;
            View.SkillPreview.Initialize(model.Skill);
            View.Avatar.sprite = model.Prototype.CardImage;
            View.HealthBar.maxValue = View.ShieldBar.maxValue = model.MaxHealth.Value;
            View.HealthBar.value = model.CurrentHealth.Value;
            View.ShieldBar.value = model.CurrentShield.Value;
            View.HealthText.text = $"{model.CurrentHealth.Value}/{model.MaxHealth.Value}";
            View.ShieldText.text = $"{model.CurrentShield.Value}";
            View.Atk.text = model.Attack.Value.ToString();

            model.Attack.Subscribe(x => {
                View.Atk.text = x.ToString();
            }).AddTo(this);
            model.CurrentShield.Subscribe(x => {
                if (x<=0&&View.ShieldBar.gameObject.activeSelf) {
                    View.ShieldBar.gameObject.SetActive(false);
                }
                if(x>0&&!View.ShieldBar.gameObject.activeSelf){
                    View.ShieldBar.gameObject.SetActive(true);
                }
                View.ShieldText.text = $"{x}";
                View.ShieldBar.value = x;
                
            }).AddTo(this);
            model.CurrentHealth.Subscribe(x => {
                View.HealthBar.value = x;
                View.HealthText.text = $"{x}/{model.MaxHealth.Value}";
            }).AddTo(this);
            model.Dead.Subscribe(x => {
                if (x) {
                    GetComponent<CardColorMask>().PointerEnter();
                }
            }).AddTo(this);

        }

        public void OnPointerClick(PointerEventData eventData) {
            if (PrepareBattleManager.Instance == null) {
                return;
            }
            var prop = PrepareBattleManager.Instance.CurrentSelectedProp;
            if (!GetComponent<CardOutline>().Enabled) {
                return;
            }
            if (prop != null) {
                GetComponent<StaffCardPropConsumer>().Consume(prop.Model);
                Destroy(PrepareBattleManager.Instance.CurrentSelectedProp.gameObject);
            }

            PrepareBattleManager.Instance.CurrentSelectedProp = null;
        }
    }
}