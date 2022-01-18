using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Attributes;
using RPG.Movement;
using UnityEngine;
using GameDevTV.Utils;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTimer = 5f;
        [SerializeField] float agroCoolDownTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float pathRadius = 1.4f;
        [SerializeField] float dwellTime = 2.5f;
        [Range(0,1)]
        [SerializeField] float dwellSpeedMod = 0.2f;
        [SerializeField] float shoutDistance = 5f;

        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;

        LazyValue<Vector3> guardPos;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceAggravated = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Awake() 
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            guardPos = new LazyValue<Vector3>(GetInitialPos);
        }

        private Vector3 GetInitialPos()
        {
            return transform.position;
        }

        private void Start() 
        {
            guardPos.ForceInit();
        }

        private void Update()
        {
            if (health.getIsDead()) return;

            if (IsAggravated() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTimer)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        public void Aggravate()
        {
            timeSinceAggravated = 0;
        }

        private void UpdateTimers()
        {
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceAggravated += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPos.value;

            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    timeSinceArrivedAtWaypoint = 0f;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > dwellTime)
            {
                mover.StartMoveAction(nextPosition, dwellSpeedMod);
            }
        }

        private bool AtWayPoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < pathRadius;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWayPoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0f;
            fighter.Attack(player);

            AggravateNearbyEnemies();
        }

        private void AggravateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;

                ai.Aggravate();
            }
        }

        private bool IsAggravated()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggravated < agroCoolDownTime;
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}