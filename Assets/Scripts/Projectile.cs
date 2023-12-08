using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private float _damage = 1f;
    void Start()
    {
        StartCoroutine(Countdown());
    }

    void Update()
    {

    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public float GetDamage()
    {
        return _damage;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }
}
