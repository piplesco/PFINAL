using UnityEngine.AI;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackDamage = 25f;
    public float attackInterval = 3f;
    public AudioClip zombieSound; 
    public AudioClip walkSound;  

    public float minDistanceForMaxVolume = 2f;
    public float maxDistanceForMinVolume = 10f;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private Codigo_salud playerHealth;
    private AudioSource audioSource; 
    private AudioSource walkAudioSource; 
    private float attackTimer;
    private Vector3 wanderPoint;
    private bool isWandering;

    public float walkSpeed = 2.5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSource = audioSources[0]; 
        walkAudioSource = audioSources.Length > 1 ? audioSources[1] : gameObject.AddComponent<AudioSource>();

        player = GameObject.FindWithTag("Player").transform;
        playerHealth = player.GetComponent<Codigo_salud>();

        agent.speed = walkSpeed;

        
        walkAudioSource.clip = walkSound;
        walkAudioSource.loop = true; 
        walkAudioSource.playOnAwake = false; 
        walkAudioSource.volume = 0f; 

        InvokeRepeating("PlayZombieSound", 0, 7f);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        attackTimer -= Time.deltaTime;

        AdjustVolumeByDistance(distanceToPlayer);
        AdjustWalkingSoundVolume(distanceToPlayer);

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

        HandleWalkingSound();
    }

    void AdjustVolumeByDistance(float distance)
    {
        float volume = Mathf.InverseLerp(maxDistanceForMinVolume, minDistanceForMaxVolume, distance);
        audioSource.volume = Mathf.Clamp01(volume);
    }

    void AdjustWalkingSoundVolume(float distance)
    {
        float volume = Mathf.InverseLerp(maxDistanceForMinVolume, minDistanceForMaxVolume, distance);
        walkAudioSource.volume = Mathf.Clamp01(volume);
    }

    void ChasePlayer()
    {
        isWandering = false;
        agent.SetDestination(player.position);
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    void AttackPlayer()
    {
        isWandering = false;
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

    void HandleWalkingSound()
    {
        if (animator.GetBool("isWalking") && !walkAudioSource.isPlaying)
        {
            walkAudioSource.Play();
        }
        else if (!animator.GetBool("isWalking") && walkAudioSource.isPlaying)
        {
            walkAudioSource.Stop();
        }
    }

    void GenerateWanderPoint()
    {
        float wanderRadius = 10f;
        Vector3 randomDirection = new Vector3(
            Random.Range(-wanderRadius, wanderRadius),
            0,
            Random.Range(-wanderRadius, wanderRadius)
        );

        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            wanderPoint = hit.position;
            isWandering = true;
        }
        else
        {
            isWandering = false;
        }
    }

    void PlayZombieSound()
    {
        if (audioSource != null && zombieSound != null)
        {
            audioSource.PlayOneShot(zombieSound);
        }
    }
}
