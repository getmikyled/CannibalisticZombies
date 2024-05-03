 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace CannibalisticZombies
{
    public class ZombieAI : MonoBehaviour
    {

        [SerializeField] private NavMeshAgent agent;

        [SerializeField] Transform player;

        [SerializeField] LayerMask ground, thePlayer;

        [SerializeField] Vector3 walkPoint;

        [SerializeField] float HP;

        [SerializeField] private float attackCooldown;

        private bool walkPointSet;
        private float walkPointRange;
        private bool alreadyAttacked;

        [SerializeField] private float sightRange, attackRange;
        private bool playerInSightRange, playerInAttackRange;



        // Update is called once per frame
        void Update()
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, thePlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, thePlayer);

            if (!playerInSightRange && !playerInAttackRange) Patrolling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();

        }


        private void Awake()
        {
            player = GameObject.Find("PlayerObj").transform;
            agent = GetComponent<NavMeshAgent>();

        }

        private void Patrolling()
        {
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet) agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            //Reached WalkPoint
            if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
        }

        private void SearchWalkPoint()
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            if (Physics.Raycast(walkPoint, -transform.up, 2f, ground)) walkPointSet = true;

        }
        private void ChasePlayer()
        {
            agent.SetDestination(player.position);
        }

        private void AttackPlayer()
        {
            agent.SetDestination(transform.position);
            transform.LookAt(player);

            if (!alreadyAttacked)
            {
                ///Zombie will perform attack, code to be written
                ///

                alreadyAttacked = true;
           
                Invoke(nameof(ResetAttack), attackCooldown);
            }
        }
        private void ResetAttack()
        {
            alreadyAttacked = false;
        }

        private void takeDamage(int damage)
        {
            HP -= damage;

            if (HP <= 0) Invoke(nameof(DestroyEnemy), 0.25f);
        }

        private void DestroyEnemy()
        {
            Destroy(gameObject);
        }
        // Start is called before the first frame update
        void Start()
        {

        }


    }
}

