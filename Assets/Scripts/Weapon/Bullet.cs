using System.Threading;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [HideInInspector] public WeaponManager weapon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<EnemyHealth>())
        {
            EnemyHealth enemy = collision.gameObject.GetComponentInParent<EnemyHealth>();
            enemy.TakeDamage(weapon.damage); // Example damage value
        }
        Destroy(this.gameObject);
    }
}
