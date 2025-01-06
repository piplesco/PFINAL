using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackDamage = 25f;
    public float attackInterval = 3f;
    public AudioClip zombieSound;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private Codigo_salud playerHealth;
    private AudioSource audioSource;
    private float attackTimer;
    private Vector3 wanderPoint; // Punto aleatorio al que el zombie se moverá al deambular
    private bool isWandering;

    public float walkSpeed = 2.5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player").transform;
        playerHealth = player.GetComponent<Codigo_salud>();

        agent.speed = walkSpeed;

        // Generar el primer punto para deambular
        GenerateWanderPoint();

        InvokeRepeating("PlayZombieSound", 0, 7f);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        attackTimer -= Time.deltaTime;

        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            ChasePlayer();
        }
        else if (distanceToPlayer <= attackRange)
        {
            if (attackTimer <= 0f)
            {
                AttackPlayer();
                attackTimer = attackInterval;
            }
        }
        else
        {
            Wander();
        }
    }

    void ChasePlayer()
    {
        isWandering = false; // Detener el comportamiento de deambular
        agent.SetDestination(player.position);
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    void AttackPlayer()
    {
        isWandering = false; // Detener el comportamiento de deambular
        agent.SetDestination(transform.position);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

        playerHealth.RecibirDaño(attackDamage);
    }

    void Wander()
    {
        if (!isWandering || Vector3.Distance(transform.position, wanderPoint) < 1f)
        {
            GenerateWanderPoint();
        }

        agent.SetDestination(wanderPoint);
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    void GenerateWanderPoint()
    {
        // Generar un punto aleatorio dentro de un rango para que el zombie deambule
        float wanderRadius = 10f;
        Vector3 randomPoint = new Vector3(
            transform.position.x + Random.Range(-wanderRadius, wanderRadius),
            transform.position.y,
            transform.position.z + Random.Range(-wanderRadius, wanderRadius)
        );

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, wanderRadius, NavMesh.AllAreas))
        {
            wanderPoint = hit.position;
            isWandering = true;
        }
    }

    void PlayZombieSound()
    {
        if (zombieSound != null)
        {
            audioSource.PlayOneShot(zombieSound);
        }
    }
}
