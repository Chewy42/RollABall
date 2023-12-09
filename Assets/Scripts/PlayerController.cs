using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _maxHealth = 50f;
    private float _health = 50f;
    private float _speed = 10f;
    private int _damage = 1;
    private int _projectileShots = 1;
    private float _projectileCooldown = 2f;
    private int _playerLevel = 1;
    private float _playerXP = 0;
    private float _playerXPToNextLevel = 5f;
    private int _kills = 0;
    private bool _canShoot = true;


    [Header("References")]
    public GameObject upgradeEffect;
    public Transform projectileParent;
    public GameObject enemiesParent;
    public GameView gameView;
    public GameObject projectile;
    public GameStateManager gameStateManager;
    public GameController gameController;
    public LevelManager levelManager;

    [Header("Other")]
    private Rigidbody rb;
    private Vector3 pos;
    private bool _isCurrentlyLevelingUp = false;
    private bool _isJumping = false;
    private float _jumpForce = 250f;
    private float _jumpCooldown = 0.5f;
    private float _groundedThreshold = 0.5f; 
    private float _lastJumpTime = 0f;

    private void Start()
    {
    
        upgradeEffect.SetActive(false);
        _health = 50f;
        rb = GetComponent<Rigidbody>();

        pos = transform.position;

        StartCoroutine(AutoShoot());
    }

    private void FixedUpdate()
    {
        MovePlayer();
        CheckForJumpInput();
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
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void CheckForJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (!_isJumping && IsGrounded())
        {
            _isJumping = true;
            rb.AddForce(Vector3.up * _jumpForce);
        }
    }

     private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, _groundedThreshold);
    }


    public IEnumerator AutoShoot()
    {
        while (_canShoot)
        {
            ShootClosestEnemy();
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
        bullet.GetComponent<Projectile>().SetDamage(_damage);
        bullet.GetComponent<Rigidbody>().velocity = direction * 69f;
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
        if (other.gameObject.CompareTag("Floor"))
        {
            _isJumping = false;
        }
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

    public IEnumerator FlashUpgradeEffect()
    {
        upgradeEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        upgradeEffect.SetActive(false);
    }

    private void LevelUp()
    {
        _isCurrentlyLevelingUp = true;
        _playerLevel++;
        _playerXP = 0;
        _playerXPToNextLevel *= 1.5f;
        _health = _maxHealth;
        gameView.SetPlayerLevelText(_playerLevel);
        _canShoot = false;
        gameView.ShowLevelUpView();
    }

    public void OnLevelUpFinished()
    {
        StartCoroutine(FlashUpgradeEffect());
    }

    private void Lose()
    {
        transform.position = new Vector3(0f, 0f, 0f);
        gameController.GameLost();
    }

    private void CheckForFallOffMap()
    {
        if (transform.position.y < -15f)
        {
            Lose();
        }
    }

    public void SetCanShoot(bool value)
    {
        _canShoot = value;
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

    public int GetDamage()
    {
        return _damage;
    }

    public void SetDamage(int amount)
    {
        _damage = amount;
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