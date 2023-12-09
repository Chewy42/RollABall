using UnityEngine;
using System.Collections;


public class Enemy : MonoBehaviour
{
    [Header("References")]
    protected XP xp;
    protected Transform playerPosition;
    protected PlayerController playerController;

    protected float speed;

    protected float health;

    protected float damage;

    private Transform _floor;
    private Transform _parent;

    public GameObject XP_Prefab;
    public Transform XP_Parent;

    protected virtual void Awake()
    {
        xp = XP_Prefab.GetComponent<XP>();
        _floor = GameObject.FindGameObjectWithTag("Floor").transform;
        XP_Parent = GameObject.FindGameObjectWithTag("XPParent").transform;
        InitializeComponents();
    }

    protected virtual void Start()
    {
        ParentToEnemyContainer();
    }

    private void InitializeComponents()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = playerPosition.GetComponent<PlayerController>();
    }

    private void ParentToEnemyContainer()
    {
        _parent = GameObject.FindGameObjectWithTag("Enemies").transform;
        transform.SetParent(_parent, worldPositionStays: false);
    }

    private void FixedUpdate()
    {
        if (!IsAlive()) return;

        PerformMovement();
        CheckForFallOffMap();
    }

    protected virtual void PerformMovement()
    {
        if (playerPosition == null || playerController.IsCurrentlyLevelingUp()) return;

        Vector3 direction = (playerPosition.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private bool canHit = true;

    private IEnumerator HitCooldown()
    {
        canHit = false;
        yield return new WaitForSeconds(1f);
        canHit = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!canHit) return;

        Vector3 pointOfContact = other.contacts[0].point;
        if (other.gameObject.CompareTag("Projectile"))
        {
            health -= other.gameObject.GetComponent<Projectile>().GetDamage();
            if (!IsAlive()) Die(pointOfContact);
            StartCoroutine(HitCooldown());
        }
    }

    protected void SetXP(int amount)
    {
        xp.XP_Amount = amount;
    }

    
    public bool IsAlive()
    {
        return health >= 0;
    }

    
    private void CheckForFallOffMap()
    {
        if (transform.position.y < -10)
        {
            DieFromFall();
        }
    }

    
    protected void Die(Vector3 pointOfContact)
    {
        
        if (pointOfContact.x < -10 || pointOfContact.x > 10 || pointOfContact.z < -10 || pointOfContact.z > 10)
        {
            pointOfContact.x = Mathf.Clamp(pointOfContact.x, -10, 10);
            pointOfContact.z = Mathf.Clamp(pointOfContact.z, -10, 10);
        }
        GameObject xpOrb = Instantiate(XP_Prefab, pointOfContact, Quaternion.identity);
        xpOrb.transform.parent = XP_Parent;
        playerController.OnEnemyKilled();
        Destroy(gameObject);
    }

    protected void DieFromFall()
    {
        playerController.OnEnemyKilled();
        Destroy(gameObject);
    }

    public float GetDamage()
    {
        return damage;
    }
}
