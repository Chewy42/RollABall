// Import necessary libraries
using System.Collections; // Required for IEnumerator
using UnityEngine;

// Boss class derived from the Enemy base class
public class Enemy_Boss : Enemy
{
    [Header("Boss Specific")]
    [SerializeField]
    private float rushCooldown = 2f;
    [SerializeField]
    private float rushSpeedMultiplier = 5f; // Multiplier for the boss's speed during the rush

    private Transform _playerTransform; // Reference to the player's transform

    // Awake method to initialize boss specific settings
    protected override void Awake()
    {
        base.Awake();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Set boss game object to the "Ignore Raycast" layer
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    // Start method to set up the boss behavior
    protected override void Start()
    {
        base.Start();
        speed = 2f;
        health = 3f;
        StartCoroutine(RushPlayerRoutine());
    }

    // Override the TakeDamage method to incorporate boss specific damage logic
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage); // Use the base class method to handle damage
    }

    // Coroutine for the boss to rush towards the player at intervals
    private IEnumerator RushPlayerRoutine()
    {
        while (IsAlive())
        {
            float randomCooldown = Random.Range(rushCooldown * 0.5f, rushCooldown * 1.5f);
            yield return new WaitForSeconds(randomCooldown);

            float rushDuration = Random.Range(0.5f, 1.5f);
            float startTime = Time.time;
            float endTime = startTime + rushDuration;

            // Store the player's position at the start of the rush
            Vector3 startPosition = _playerTransform.position;

            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / rushDuration;
                float easedT = Mathf.SmoothStep(0f, 1f, t);
                float currentSpeed = Mathf.Lerp(2f, 2f * rushSpeedMultiplier, easedT);

                // Move towards the player's position at the start of the rush
                Vector3 direction = (startPosition - transform.position).normalized;
                transform.position += direction * currentSpeed * Time.deltaTime;

                yield return null;
            }

            speed = 2f;
        }
    }


    // Set isKinematic to true to prevent boss from taking knockback
    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);
    }
}
