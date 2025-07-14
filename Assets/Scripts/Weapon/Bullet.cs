using System.Threading;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [HideInInspector] public WeaponManager weapon;
    [HideInInspector] public Vector3 direction;

    [SerializeField] private GameObject bulletImpactPrefab;

    ParticleSystem bulletParticle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletParticle = GetComponent<ParticleSystem>();
        Destroy(this.gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<EnemyHealth>())
        {
            EnemyHealth enemy = collision.gameObject.GetComponentInParent<EnemyHealth>();
            enemy.TakeDamage(weapon.damage);

            if (enemy.health <= 0f && enemy.isDead == false)
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(direction * weapon.ennemyKickBackForce, ForceMode.Impulse);
                enemy.isDead = true;
            }
            SpawnImpactEffect(collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal), false);
        }
        else
        {
            SpawnImpactEffect(collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal), true);
        }

        bulletParticle.Play();
        Destroy(this.gameObject);
    }

    private void SpawnImpactEffect(Vector3 position, Quaternion rotation, bool isASurfaceImpact = false)
    {
        GameObject impact = Instantiate(bulletImpactPrefab, position, rotation);
        impact.transform.Rotate(0, 0, Random.Range(0, 360));
        if (!isASurfaceImpact) impact.GetComponent<SpriteRenderer>().sprite = null; // Ensure the impact effect is rendered above other objects
        impact.transform.position += impact.transform.forward / 1000; // Offset to avoid z-fighting
    }

}

