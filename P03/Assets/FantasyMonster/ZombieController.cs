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

    public float walkSpeed = 2.5f;  // Velocidad ajustada para caminar un poco más rápido

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player").transform;
        playerHealth = player.GetComponent<Codigo_salud>();

        // Configurar la velocidad de caminar un poco más rápido
        agent.speed = walkSpeed;

        // Establecer un destino predeterminado para que el zombie se mueva al inicio
        SetInitialDestination();

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

    void SetInitialDestination()
    {
        // Establece un destino predeterminado (puede ser un punto aleatorio o el jugador)
        Vector3 randomPoint = new Vector3(Random.Range(-10, 10), transform.position.y, Random.Range(-10, 10));
        agent.SetDestination(randomPoint);
        animator.SetBool("isWalking", true);  // Asegúrate de que la animación se reproduzca
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

        playerHealth.RecibirDaño(attackDamage);
    }

    void Wander()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
    }

    void PlayZombieSound()
    {
        if (zombieSound != null)
        {
            audioSource.PlayOneShot(zombieSound);
        }
    }
}
