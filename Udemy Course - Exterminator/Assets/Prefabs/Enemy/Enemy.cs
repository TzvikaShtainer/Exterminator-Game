using System;
using Prefabs.Enemy.Spawner;
using Prefabs.Framework;
using Prefabs.Framework.AI;
using Prefabs.Framework.AI.BehaviorTree;
using Prefabs.Framework.AI.Perception;
using Prefabs.Framework.Damage;
using Prefabs.Framework.Health;
using Prefabs.Framework.Reward;
using Unity.VisualScripting;
using UnityEngine;

namespace Prefabs.Enemy
{
    public abstract class Enemy : MonoBehaviour, IBehaviorTree, ITeamInterface, ISpawnInterface
    {
        [SerializeField] private HealthComponent healthComponent;
        [SerializeField] private Animator anim;
        [SerializeField] private PerceptionComponent perceptionComp;
        [SerializeField] private MovementComponent movementComponent;
        [SerializeField] private int teamID = 2;
        
        [SerializeField] Reward killerReward;

        private Vector3 prevPos;

        public Animator Animator
        {
            get {return anim;}
            private set { anim = value; }
        }

        [SerializeField] private BehaviorTree behaviorTree;
        

        private void OnDisable()
        {
            perceptionComp.onPerceptionTargetChanged -= TargetChanged;
            healthComponent.onDeath -= StartDeath;
            healthComponent.onTakeDamage -= TakenDamage;
        }

        private void Awake()
        {
            perceptionComp.onPerceptionTargetChanged += TargetChanged;
        }

        protected virtual void Start()
        {
            if (healthComponent != null)
            {
                healthComponent.onDeath += StartDeath;
                healthComponent.onTakeDamage += TakenDamage;
            }
            
            prevPos = transform.position;
        }

        private void TargetChanged(GameObject target, bool sensed)
        {
            if (sensed)
            {
                behaviorTree.Blackboard.SetOrAddData("Target", target);
            }
            else
            {
                behaviorTree.Blackboard.SetOrAddData("LastSeenLocation", target.transform.position); //get the last known location of target
                behaviorTree.Blackboard.RemoveBlackboardData("Target");
            }
        }

        private void StartDeath(GameObject killer)
        {
            TriggerDeathAnimation();
            
            IRewardListener[] rewardListeners = killer.GetComponents<IRewardListener>();
            foreach (IRewardListener rewardListener in rewardListeners)
            {
                rewardListener.Reward(killerReward);
            }
        }

        private void TriggerDeathAnimation()
        {
            if (anim != null)
            {
                anim.SetTrigger("Death");
            }
        }

        public void OnDeathAnimationFinished()
        {
            Debug.Log("des");
            Dead();
            Destroy(gameObject);
        }

        private void Update()
        {
            CalcSpeed();
        }

        private void CalcSpeed()
        {
            if(movementComponent == null) return;
            
            //calc the speed for the anim when we change our pos
            Vector3 posDelta = transform.position - prevPos;
            float speed = posDelta.magnitude / Time.deltaTime;

            Animator.SetFloat("Speed", speed);

            prevPos = transform.position;
        }

        private void TakenDamage(float health, float amt, float maxHealth, GameObject instigator)
        {
        
        }

        private void OnDrawGizmos()
        {
            if (behaviorTree && behaviorTree.Blackboard.GetBlackboardData("Target", out GameObject target))
            {
                Vector3 drawTargetPos = target.transform.position + Vector3.up;
                Gizmos.DrawWireSphere(drawTargetPos, 1f);
            
                Gizmos.DrawLine(transform.position + Vector3.up, drawTargetPos);
            }
        }

        public void RotateTowards(GameObject target, bool verticalAim = false)
        {
            Vector3 aimDir = target.transform.position - transform.position;
            aimDir.y = verticalAim ? aimDir.y : 0;
            aimDir = aimDir.normalized;
            
            movementComponent.RotateTowards(aimDir);
        }

        public virtual void AttackTarget(GameObject target)
        {
            //override in child
        }

        public int GetTeamID()
        {
            return teamID;
        }

        public void SpawnedBy(GameObject spawnerGameObject)
        {
            BehaviorTree spawnerBehaviorTree = spawnerGameObject.GetComponent<BehaviorTree>();
            if (spawnerBehaviorTree != null &&
                spawnerBehaviorTree.Blackboard.GetBlackboardData<GameObject>("Target", out GameObject spawnerTarget))
            {
                PerceptionStimuli targetStimuli = spawnerTarget.GetComponent<PerceptionStimuli>();

                if (targetStimuli && perceptionComp)
                {
                    perceptionComp.AssignPerceivedStimuli(targetStimuli);
                }
            }
        }

        protected virtual void Dead()
        {
            
        }
    }
}
