using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Attributes")]
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _health;
    [SerializeField] private float _speed;
    [SerializeField] private int _projectileShots = 1;
    [SerializeField] private float _projectileCooldown = 2f;
    [SerializeField] private float _jumpCooldown = 3f;
    [SerializeField] private int _playerLevel = 1;
    [SerializeField] private float _playerXP = 0;
    [SerializeField] private float _playerXPToNextLevel = 5f;
    [SerializeField] private int _kills = 0;


    [Header("References")]
    public Transform projectileParent;
    public GameObject enemiesParent;
    public GameView gameView;
    public GameObject projectile;

    [Header("Other")]
    private Rigidbody rb;
    private GameController gameController;
    private Vector3 pos;
    private bool _isCurrentlyLevelingUp = false;

    private void Start()
    {
        gameController = GetComponentInParent<GameController>();

        _health = 50f;
        rb = GetComponent<Rigidbody>();

        pos = transform.position;

        StartCoroutine(AutoShoot());
    }

    private void FixedUpdate()
    {
        MovePlayer();
        CheckForFallOffMap();
    }

    private void MovePlayer()
    {
        if(!_isCurrentlyLevelingUp)
        {
            pos = transform.position;

            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            rb.AddForce(movement * _speed);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Jump());
            }
        }else{
            rb.velocity = Vector3.zero;
        }
    }

    private IEnumerator Jump()
    {
        if (_jumpCooldown <= 0 && transform.position.y == 1)
        {
            rb.AddForce(Vector3.up * 250f);
            _jumpCooldown = 3f;
        }
        else if (transform.position.y != 1)
        {
            Debug.Log("Player is not on the ground!");
        }
        else
        {
            Debug.Log("Jump is on cooldown!");
        }

        while (_jumpCooldown > 0)
        {
            _jumpCooldown -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator AutoShoot()
    {
        while (true)
        {
            if(gameController.IsGamePlaying())
            {
                ShootClosestEnemy();
            }else{
                print("Cant shoot!");
            }
            yield return new WaitForSeconds(_projectileCooldown);
        }
    }

    private Enemy GetClosestEnemy()
    {
        if (enemiesParent.transform.childCount == 0) return null;

        Enemy closestEnemy = null;
        float minDistance = float.MaxValue;

        List<Enemy> enemies = new List<Enemy>();

        foreach (Transform child in enemiesParent.transform)
        {
            enemies.Add(child.GetComponent<Enemy>());
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i]) return null;

            float distance = Vector3.Distance(pos, enemies[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemies[i];
            }
        }

        return closestEnemy;
    }

    private GameObject CreateBullet(Vector3 direction)
    {
        GameObject bullet = Instantiate(projectile, pos, Quaternion.identity);
        bullet.transform.parent = projectileParent;
        bullet.GetComponent<Projectile>().SetDamage(1f);
        bullet.GetComponent<Rigidbody>().velocity = direction * 50f;
        return bullet;
    }

    private void ShootClosestEnemy()
    {
        StartCoroutine(ShootBullets());
    }

    IEnumerator ShootBullets()
    {
        for (int i = 0; i < _projectileShots; i++)
        {
            Enemy closestEnemy = GetClosestEnemy();

            if (closestEnemy == null) yield break;

            Vector3 direction = closestEnemy.transform.position - transform.position;
            direction.Normalize();

            CreateBullet(direction);

            if (_projectileShots > 1)
            {
                yield return new WaitForSeconds(0.3f);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void OnEnemyKilled()
    {
        _kills++;
        gameView.SetKillsText(_kills);
    }

    private bool enemiesExist(List<Enemy> enemies)
    {
        return enemies.Count > 0;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _health -= other.gameObject.GetComponent<Enemy>().GetDamage();
            StartCoroutine(FlashBlueMaterial());
            if (_health <= 0)
            {
                Lose();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("XP"))
        {
            _playerXP += other.gameObject.GetComponent<XP>().XP_Amount;
            Destroy(other.gameObject);
            if (_playerXP >= _playerXPToNextLevel)
            {
                LevelUp();
            }
        }
    }

    private IEnumerator FlashBlueMaterial()
    {
        GetComponent<Renderer>().material.color = Color.blue;
        yield return new WaitForSeconds(0.3f);
        GetComponent<Renderer>().material.color = Color.white;
    }

    private void LevelUp()
    {
        _isCurrentlyLevelingUp = true;
        _playerLevel++;
        _playerXP = 0;
        _playerXPToNextLevel *= 1.5f;
        _health = _maxHealth;
        gameView.SetPlayerLevelText(_playerLevel);
        gameController.InitializeLevelUp();
    }

    

    private void Lose()
    {
        transform.position = new Vector3(0f, 0f, 0f);
        gameController.Lose();
    }

    private void CheckForFallOffMap()
    {
        if (transform.position.y < -15f)
        {
            Lose();
        }
    }

    public bool IsAlive()
    {
        return _health > 0;
    }

    public bool IsCurrentlyLevelingUp()
    {
        return _isCurrentlyLevelingUp;
    }
    
    public void SetIsCurrentlyLevelingUp(bool value)
    {
        _isCurrentlyLevelingUp = value;
    }

    public float GetHealth()
    {
        return _health;
    }

    public int GetKills()
    {
        return _kills;
    }

    public int GetPlayerLevel()
    {
        return _playerLevel;
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    public void SetMaxHealth(float amount)
    {
        _maxHealth = amount;
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public void SetSpeed(float amount)
    {
        _speed = amount;
    }

    public int GetProjectileShots()
    {
        return _projectileShots;
    }

    public void SetProjectileShots(int amount)
    {
        _projectileShots = amount;
    }

}