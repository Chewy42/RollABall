using System.Collections;
using UnityEngine;

public class Enemy_Boss : Enemy
{
    [Header("Boss Specific")]
    [SerializeField] private float rushCooldown = 2f;
    [SerializeField] private float rushSpeed = 10f;
    [SerializeField] private float rushSpeedMultiplier = 5f;
    [SerializeField] private float rushAccelerationTime = 0.1f;
    [SerializeField] private float rushDecelerationTime = 0.2f;
    public bool IsFinalBoss { get; set; } = false;

    private Transform _playerTransform;

    protected override void Awake()
    {
        base.Awake();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Start()
    {
        base.Start();
        speed = 2.25f;
        health = 15f;
        StartCoroutine(RushPlayerRoutine());
    }

    private IEnumerator RushPlayerRoutine()
    {
        while (IsAlive() && playerController.IsCurrentlyLevelingUp() == false)
        {
            float randomCooldown = Random.Range(rushCooldown * 0.5f, rushCooldown * 1.5f);
            yield return new WaitForSeconds(randomCooldown);

            float rushDuration = Random.Range(0.5f, 1.5f);
            float startTime = Time.time;
            float endTime = startTime + rushDuration;

            Vector3 startPosition = _playerTransform.position;
            Vector3 direction = (startPosition - transform.position).normalized;

            yield return StartCoroutine(SmoothMovement(rushSpeedMultiplier, rushAccelerationTime));

            while (Time.time < endTime)
            {
                transform.position += direction * rushSpeed * Time.deltaTime;

                yield return null;
            }

            yield return StartCoroutine(SmoothMovement(1f, rushDecelerationTime));
        }
    }

    private IEnumerator SmoothMovement(float targetSpeed, float transitionTime)
    {
        float startingSpeed = speed;
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            speed = Mathf.Lerp(startingSpeed, targetSpeed, elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        speed = targetSpeed;
    }
}
