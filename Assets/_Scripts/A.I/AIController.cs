using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent agent;

    [SerializeField]
    private ZombieStates zombieStates = ZombieStates.patrol;

    public Transform player;

    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange;
    public bool playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        switch (zombieStates)
        {
            case ZombieStates.patrol:
                Patrol();
                if (playerInSightRange && !playerInAttackRange) zombieStates = ZombieStates.chase;
                if (playerInSightRange && playerInAttackRange) zombieStates = ZombieStates.attack;

                break;
            case ZombieStates.chase:
                ChasePlayer();
                if (!playerInSightRange && !playerInAttackRange) zombieStates = ZombieStates.patrol;
                if (playerInSightRange && playerInAttackRange) zombieStates = ZombieStates.attack;
                break;

            case ZombieStates.attack:
                AttackPlayer();
                if (!playerInSightRange && !playerInAttackRange) zombieStates = ZombieStates.patrol;
                if (playerInSightRange && !playerInAttackRange) zombieStates = ZombieStates.chase;
                break;

            default:
                break;
        }


        
        
        // if (!playerInSightRange && !playerInAttackRange) Patrol();
        // if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        // if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
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
            FindObjectOfType<AudioManager>().Play("ZombieEating");
            Debug.Log("Zombie ate you");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    public enum ZombieStates
    {
        patrol,
        chase,
        attack
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
