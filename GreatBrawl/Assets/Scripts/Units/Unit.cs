using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace GreatBrawl
{
    public enum ETeam
    {
        Allied,
        Hostile
    }

    public abstract class Unit : MonoBehaviour
    {
        public int price = 1;
    
        [HideInInspector] public ETeam team;

        [SerializeField] protected GameObject deathAnimation;
        [SerializeField] protected TriggerCollider followTrigger;
        [SerializeField] protected TriggerCollider attackTrigger;
        [SerializeField] protected float speed;
        [SerializeField] protected int maxHealth;
        [SerializeField] protected int attackPower;
        [SerializeField] protected float attackCooldown;
        [SerializeField] protected float timeBeforeHit;

        protected const string runBlend = "Run Blend";
        protected const string attack = "Attack";
        protected const string die = "Die";
        protected NavMeshAgent navMeshAgent;
        protected Animator animator;
        protected Collider collider;
        protected Unit destination;
        protected Unit targetUnit;
        protected bool hasTargetPlace;
        protected Vector3 targetPlace;
        protected Unit followTarget;
        protected Unit attackTarget;
        protected int currentHealth;
        protected float attackTimer = 0;

        protected virtual void Awake()
        {
            followTrigger.onTriggerEnter.AddListener(FollowTriggered);
            followTrigger.onTriggerExit.AddListener(FollowUnTriggered);
            followTrigger.gameObject.SetActive(false);
            attackTrigger.onTriggerEnter.AddListener(AttackTriggered);
            attackTrigger.onTriggerExit.AddListener(AttackUnTriggered);
            followTrigger.gameObject.SetActive(false);

            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.enabled = false;

            animator = GetComponent<Animator>();
            
            collider = GetComponent<Collider>();
            collider.enabled = false;

            currentHealth = maxHealth;
        }

        private void Start()
        {
            var highlight = ResourceManager.CreatePrefabInstance<EElements, Transform>(team == ETeam.Allied 
                ? EElements.AlliedHighlight : EElements.HostileHighlight, transform.position);
            highlight.transform.parent = transform;
        }

        public virtual void Place(ETeam unitTeam, Unit target)
        {
            team = unitTeam;
            if (team == ETeam.Hostile)
                Destroy(GetComponent<Controllable>());
            followTrigger.gameObject.SetActive(true);
            attackTrigger.gameObject.SetActive(true);
            navMeshAgent.enabled = true;
            navMeshAgent.speed = speed;
            collider.enabled = true;
            destination = target;
            targetUnit = target;
        }

        protected virtual void Update()
        {
            if (!navMeshAgent.enabled)
                return;
            
            animator.SetFloat(runBlend, navMeshAgent.velocity.magnitude);
            if (attackTarget)
            {
                if (attackTimer <= 0)
                {
                    StartCoroutine(Attack(attackTarget));
                    attackTimer = attackCooldown;
                }
            }
            else if (followTarget)
            {
                transform.LookAt(followTarget.transform);
                navMeshAgent.destination = followTarget.transform.position;
            }
            else if (targetUnit)
            {
                navMeshAgent.destination = targetUnit.transform.position;
            }
            else if (hasTargetPlace)
            {
                navMeshAgent.destination = targetPlace;
                if (Vector3.Distance(transform.position, targetPlace) <= 0.1f)
                    hasTargetPlace = false;
            }
            else if (destination)
            {
                navMeshAgent.destination = destination.transform.position;
            }
            else
            {
                navMeshAgent.destination = transform.position;
            }

            if (attackTimer > 0)
                attackTimer -= Time.deltaTime;
        }

        public void SetDestination(Unit unit)
        {
            targetUnit = unit;
            hasTargetPlace = false;
        }

        public void SetDestination(Vector3 position)
        {
            targetUnit = null;
            hasTargetPlace = true;
            targetPlace = position;
        }

        protected virtual IEnumerator Attack(Unit target)
        {
            transform.LookAt(target.transform);
            animator.SetTrigger(attack);
            yield return new WaitForSeconds(timeBeforeHit);
            if(target != null)
                target.GetDamage(attackPower);
        }

        public virtual void GetDamage(int amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
                Die();
        }

        public virtual void Die()
        {
            Destroy(gameObject);
            if(deathAnimation)
                Instantiate(deathAnimation, transform.position, transform.rotation);
        }

        protected virtual void FollowTriggered(Collider col)
        {
            if (followTarget)
                return;
            
            var target = col.GetComponent<Unit>();
            if (target)
            {
                if (target.team != team)
                {
                    followTarget = target;
                }
            }
        }

        protected virtual void FollowUnTriggered(Collider col)
        {
            var target = col.GetComponent<Unit>();
            if (target)
            {
                if (followTarget == target)
                {
                    followTarget = null;

                    var targets = new List<Unit>();
                    foreach (var collider in followTrigger.GetContacts())
                    {
                        if (collider == null)
                            continue;
                        var unit = collider.GetComponent<Unit>();
                        if (unit)
                        {
                            if(unit.team != team)
                                targets.Add(unit);
                        }
                    }

                    if (targets.Count > 0)
                    {
                        targets.Sort((a, b) => Vector3.Distance(a.transform.position, 
                            transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));
                        followTarget = targets[0];
                    }
                    else
                        navMeshAgent.destination = transform.position;
                }
            }
        }

        private void AttackTriggered(Collider col)
        {
            if (attackTarget)
                return;
            
            var target = col.GetComponent<Unit>();
            if (target)
            {
                if (target.team != team)
                {
                    attackTarget = target;
                    navMeshAgent.destination = transform.position;
                }
            }
        }

        private void AttackUnTriggered(Collider col)
        {
            var target = col.GetComponent<Unit>();
            if (target)
            {
                if (attackTarget == target)
                {
                    attackTarget = null;
                    
                    var targets = new List<Unit>();
                    foreach (var collider in attackTrigger.GetContacts())
                    {
                        if (collider == null)
                            continue;
                        var unit = collider.GetComponent<Unit>();
                        if (unit)
                        {
                            if(unit.team != team)
                                targets.Add(unit);
                        }
                    }

                    if (targets.Count > 0)
                    {
                        targets.Sort((a, b) => Vector3.Distance(a.transform.position, 
                            transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position)));
                        attackTarget = targets[0];
                    }
                }
            }
        }
        
        protected virtual void OnDestroy()
        {
            followTrigger.onTriggerEnter.RemoveAllListeners();
            followTrigger.onTriggerExit.RemoveAllListeners();
            attackTrigger.onTriggerEnter.RemoveAllListeners();
            attackTrigger.onTriggerExit.RemoveAllListeners();
        }
    }
}

