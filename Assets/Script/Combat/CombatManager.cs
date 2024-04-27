using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Enemy;
using Script.Player;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Script.Combat {
    public class CombatManager : MonoBehaviour {
        public CombatEnemyCardQueuePresenter Enemies;
        public CombatPlayerCardQueuePresenter Alies;

        public ReactiveProperty<bool> RoundEnded;
        public UnityEvent OnRoundEnd;
        
        public List<StaffCardPresenter> First;
        public List<StaffCardPresenter> Second;
        public bool IsPlayerFirst;
        public int CurrentRound = 0;
        public static CombatManager Instance=>m_Instance;
        private static CombatManager m_Instance;

        private void Awake() {
            m_Instance = this;
        }

        private void Start() {
            StartGame();
        }

        public async void StartGame() {
            InitActOrder();
            InitEvents();


            while (!RoundEnded.Value) {
                await StartRound();
                await UniTask.Delay(3000);
                Debug.Log($"Round Start {CurrentRound++}");
            }

            return;
            void InitEvents() {
                foreach (var staffCardPresenter in First) {
                    staffCardPresenter.Model.OnDeath.AddListener(CheckSacrifice);
                    staffCardPresenter.Model.OnDeath.AddListener(RefreshRoundState);
                    staffCardPresenter.Model.OnRoundStart.AddListener(RoundStart);
                    staffCardPresenter.Model.OnShieldReduce.AddListener(ShieldReduce);
                }

                foreach (var staffCardPresenter in Second) {
                    staffCardPresenter.Model.OnDeath.AddListener(CheckSacrifice);
                    staffCardPresenter.Model.OnRoundStart.AddListener(RoundStart);
                    staffCardPresenter.Model.OnDeath.AddListener(RefreshRoundState);
                    staffCardPresenter.Model.OnShieldReduce.AddListener(ShieldReduce);
                }
            }

            void InitActOrder() {
                if (Random.Range(0, 1) <= 0.5f) {
                    First = Enemies.Presenters;
                    Second = Alies.Presenters;
                    IsPlayerFirst = false;
                }
                else {
                    First = Alies.Presenters;
                    IsPlayerFirst = true;
                    Second = Enemies.Presenters;
                }
            }
        }

        private void ShieldReduce(StaffCard arg0, int arg1) {
            if (arg0.Skill is ShieldSkill skill) {
                if (arg0.IsEnemy) {
                    var staffCard = Enemies.Presenters.Where(x=>!x.Model.Dead.Value).ToArray();
                    int recover = arg1 / Math.Min(1,staffCard.Length);
                    foreach (var staffCardPresenter in staffCard) {
                        staffCardPresenter.Model.AddHealth(recover);
                    }
                }
            }
        }


        private void CheckSacrifice(StaffCard pter,StaffCard self) {
            if (self.Skill is SacrificeSkill) {
                if (self.IsEnemy) {
                    for (int i = 0; i < 2; i++) {
                        Debug.Log("执行牺牲针对玩家");
                        foreach (var staffCardPresenter in this.Alies.Presenters) {
                            //死亡时候对所有敌人造成攻击
                            staffCardPresenter.Model.ReceiveDamage(self,self.Attack.Value);
                            if (staffCardPresenter.Model.TryMarkDeath(self)) {
                                //如果有死亡,那么就额外触发
                                i -= 1;
                                Debug.Log("有人死亡,额外牺牲");
                            }
                        }
                        
                    }
                }
                else {
                    for (int i = 0; i < 2; i++) {
                        Debug.Log("执行牺牲针对敌人");
                        foreach (var staffCardPresenter in this.Enemies.Presenters) {
                            //死亡时候对所有敌人造成攻击
                            staffCardPresenter.Model.ReceiveDamage(self,self.Attack.Value);
                            if (staffCardPresenter.Model.TryMarkDeath(self)) {
                                //如果有死亡,那么就额外触发
                                i -= 1;
                                Debug.Log("有人死亡,额外牺牲");
                            }
                        }
                        
                    }
                    
                }
            }
        }

        private void RefreshRoundState(StaffCard presenter,StaffCard self) {
            if (Alies.Presenters.Any(x => !x.Model.Dead.Value) ||
                Enemies.Presenters.Any(x => !x.Model.Dead.Value)) {
                return;
            }

            this.RoundEnd();
        }

        private void RoundEnd() {
            RoundEnded.Value = true;
            OnRoundEnd?.Invoke();
        }


        public async UniTask StartRound() {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            Debug.Log("先手场景初始技能");
            foreach (var staffCardPresenter in First) {
                staffCardPresenter.Model.OnRoundStart?.Invoke(staffCardPresenter.Model,CurrentRound);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            Debug.Log("后手场景初始技能");
            foreach (var staffCardPresenter in Second) {
                staffCardPresenter.Model.OnRoundStart?.Invoke(staffCardPresenter.Model,CurrentRound);
            }
            var enumerator = GetNextAct();
            while (enumerator.MoveNext()) {
               await Attack(enumerator.Current);
            }
        }

        private void RoundStart(StaffCard arg0, int arg1) {
            if (arg0.Dead.Value) {
                return;
            }
            if (arg0.Skill is IntrospectionSkill skill) {
                var currentHealth = arg0.CurrentHealth.Value;
                var healthToReduce = Mathf.RoundToInt(arg0.MaxHealth.Value * skill.SelfDamagePercent);
                int shieldToAdd = 0;
                if (healthToReduce >= currentHealth) {
                    shieldToAdd = currentHealth - 1;
                }
                else {
                    shieldToAdd = healthToReduce;
                }

                arg0.CurrentHealth.Value -= shieldToAdd;
                arg0.CurrentShield.Value += shieldToAdd;

            }

            if (arg0.Skill is ShieldSkill shieldSkill) {
                if (CurrentRound % shieldSkill.RoundToActive == 0) {
                    arg0.MarkImmunityOneTime();
                }
            }
        }
        
        public async UniTask Attack(StaffCardPresenter dataCompound) { 
            var card = dataCompound;
            Vector3 originPos = card.transform.position;
            var enemy = GetEnemy(card.Model.IsEnemy);
            if (enemy == null) {
                await UniTask.Delay(1000);
                return;
            }
            Debug.Log($"卡牌{card.name}攻击{enemy.name}");
            card.GetComponent<Canvas>().sortingOrder += 10;
            await card.transform.DOMove(enemy.GetComponent<AttackAnchor>().AnchorTransform.position,1)
                .SetEase(Ease.InFlash).ToUniTask();

            ProcessDamageWithSkill(card.Model, enemy.Model);
            ProcessDamage(enemy.Model,card.Model);
            var selfShake = card.transform.DOShakePosition(1, 10).ToUniTask();
            var enemyShake = enemy.transform.DOShakePosition(1, 10).ToUniTask();
            await UniTask.WhenAll(selfShake, enemyShake);
            await card.transform.DOMove(originPos, 1).SetEase(Ease.InFlash).ToUniTask();
            card.GetComponent<Canvas>().sortingOrder -= 10;
        }

        private void ProcessDamageWithSkill(StaffCard cardModel, StaffCard enemyModel) {
            var damageRemain = enemyModel.Attack.Value;
            cardModel.ReceiveDamage(enemyModel,damageRemain);
            
            if (cardModel.Skill is HealthStealSkill healthSteal) {
                cardModel.CurrentHealth.Value +=
                    Mathf.RoundToInt(cardModel.Attack.Value * healthSteal.HealthStealPercent);
                cardModel.CurrentHealth.Value = Math.Min(cardModel.MaxHealth.Value, cardModel.CurrentHealth.Value);
                cardModel.Attack.Value += Mathf.RoundToInt(healthSteal.AtkSteal);
                enemyModel.Attack.Value -= Mathf.RoundToInt(healthSteal.AtkSteal);
                enemyModel.Attack.Value = Mathf.Max(0, enemyModel.Attack.Value);
            }

          
        }

        private void ProcessDamage(StaffCard cardModel, StaffCard enemyModel) {
            //计算伤害
            var damageRemain = enemyModel.Attack.Value;
            cardModel.ReceiveDamage(enemyModel,damageRemain);
          
        }

        public StaffCardPresenter GetEnemy(bool isEnemy) {
            //如果是玩家,那么就返回敌人
            if (!isEnemy) {
                var set = this.Enemies.Presenters.Where(x => x.Model.CurrentHealth.Value > 0).ToArray();
                if (set.Length == 0) {
                    return null;
                }
                return set[Random.Range(0, set.Length)];
            }
            else {
                var set = this.Alies.Presenters.Where(x => x.Model.CurrentHealth.Value > 0).ToArray();
                if (set.Length == 0) {
                    return null;
                }
                return set[Random.Range(0, set.Length)];
            }
        }
        
        public IEnumerator<StaffCardPresenter> GetNextAct() {
            for (int i = 0; i < First.Count; i++) {
                var first = First[i];
                if (!first.Model.Dead.Value) {
                    yield return (first);
                }

                var second = Second[i];
                if (!second.Model.Dead.Value) {
                    yield return (second);
                }
            }
        }
    }
}