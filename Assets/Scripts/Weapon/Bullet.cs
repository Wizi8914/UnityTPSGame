using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [HideInInspector] public WeaponManager weapon;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float holeSizeMultiplier;

    [SerializeField] private GameObject bulletImpactPrefab;
    [SerializeField] private AudioClip ennemyHitSound;
    [SerializeField] private AudioClip ennemyHeadShotSound;
    [HideInInspector] public AudioSource audioSource;


    ParticleSystem bulletParticle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletParticle = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        Destroy(this.gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<EnemyHealth>())
        {
            EnemyHealth enemy = collision.gameObject.GetComponentInParent<EnemyHealth>();

            // Head Shot
            if (enemy.headCollider.bounds.Contains(collision.contacts[0].point))
            {
                if (!enemy.isDead) playHitSound(true);
                enemy.TakeDamage(weapon.damage * 2);

            }
            else
            {
                if (!enemy.isDead) playHitSound(false);
                enemy.TakeDamage(weapon.damage);
            }

            if (enemy.health <= 0f && enemy.isDead == false)
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(direction * weapon.ennemyKickBackForce, ForceMode.Impulse);
                enemy.isDead = true;
            }
            
            SpawnImpactEffect(collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal), false);

            return;
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
        impact.transform.Rotate(0, impact.transform.rotation.x + 180, Random.Range(0, 360));

        DecalProjector decal = impact.GetComponent<DecalProjector>();
        decal.size = new Vector3(decal.size.x * holeSizeMultiplier, decal.size.y * holeSizeMultiplier, decal.size.z * holeSizeMultiplier);

        if (!isASurfaceImpact) decal.enabled = false;
        impact.transform.position -= impact.transform.forward / 100;
    }
    
    private void playHitSound(bool isHeadShot = false)
    {
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<TrailRenderer>().enabled = false;
        audioSource.PlayOneShot(isHeadShot ? ennemyHeadShotSound : ennemyHitSound);
        Destroy(this.gameObject, isHeadShot ? ennemyHeadShotSound.length : ennemyHitSound.length);
    }

}

