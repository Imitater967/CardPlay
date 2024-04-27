using System;
using System.Collections.Generic;
using System.Linq;
using Script.Combat;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Script {
    [Serializable]
    public class StaffCard {
        [field: SerializeField]
        public StaffPrototype Prototype { get; private set; }

        public UnityEvent<StaffCard, int> OnShieldReduce;
        public UnityEvent<StaffCard,StaffCard> OnDeath;
        public UnityEvent<StaffCard,int> OnRoundStart;
        public bool IsEnemy;
        public ReactiveProperty<int> MaxHealth;
        public ReactiveProperty<int> CurrentHealth;
        public ReactiveProperty<int> CurrentShield;
        public ReactiveProperty<int> Attack;
        public ReactiveCollection<Buffer> Buffers;
        public Skill Skill;
        public ReactiveProperty<bool> Dead;
        public bool ImmunityOneTime = false;
        public bool Inited = false;
        public void Initialize() {
            this.Attack.Value = Prototype.Attack;
            this.CurrentHealth.Value = this.MaxHealth.Value = Prototype.Health;
            this.CurrentHealth.Value = Mathf.RoundToInt(this.MaxHealth.Value * 0.8f);
            this.CurrentShield.Value = 0;
            Dead.Value = false;
            this.Buffers.Clear();
            Inited = true;
        }

        public void AddBuffer(Buffer buffer) {
            Buffers.Add(buffer);
        }
        
        public bool TryMarkDeath(StaffCard presenter) {
            if (!this.Dead.Value&&this.CurrentHealth.Value <= 0) {
                MarkDeath(presenter);
                return true;
            }
            return false;
        }

        protected void MarkDeath(StaffCard presenter) {
            Dead.Value = true;
            OnDeath?.Invoke(presenter,this);
        }

        public void ReceiveDamage(StaffCard attacker,int atk) {
            int damageRemain = atk;
            if (Skill is IntrospectionSkill introspectionSkill&& CurrentShield.Value > 0) {
                damageRemain = Mathf.RoundToInt(damageRemain * (1 - introspectionSkill.DamageReducePercent));
            }

            if (ImmunityOneTime) {
                if (Skill is ShieldSkill skill) {
                    int health = Mathf.RoundToInt(skill.ShieldAmountPercent * this.MaxHealth.Value);
                    var list = IsEnemy ? CombatManager.Instance.Enemies.Presenters : CombatManager.Instance.Alies.Presenters;
                    var staffCards = list.Where(x=>!x.Model.Dead.Value).Select(x=>x.Model).ToArray();
                    health = health / staffCards.Length;
                    foreach (var staffCard in staffCards) {
                        staffCard.AddHealth(health);
                    }
                }
                ImmunityOneTime = false;
                return;
            }
            
            //伤害 -= 护盾值 = 剩余伤害
            //20伤害-50护盾值 = -30点伤害
            damageRemain -= this.CurrentShield.Value;
            //伤害剩余量是0
            damageRemain = Math.Max(0, damageRemain);
            //护盾减少 
            int shieldDecrease = atk - damageRemain;
            CurrentShield.Value -= shieldDecrease;
            OnShieldReduce?.Invoke(this,shieldDecrease);
            CurrentHealth.Value -= damageRemain;
            TryMarkDeath(attacker);
        }

        public void MarkImmunityOneTime() {
            ImmunityOneTime = true;
        }

        public void AddHealth(int recover) {
            if (Dead.Value) {
                return;
            }
            int newHealth = CurrentHealth.Value + recover;
            CurrentHealth.Value = Mathf.Min(MaxHealth.Value, newHealth);
        }
    }
}