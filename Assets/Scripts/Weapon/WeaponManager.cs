using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Fire Rate")]
    [SerializeField] float fireRate;
    float fireRateTimer;
    [SerializeField] bool semiAutomatic = true;

    [Header("Bullet Properties")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnLocation;
    [SerializeField] int bulletsPerShot;
    [SerializeField] float bulletVelocity;
    AimStateManager aim;

    [SerializeField] AudioClip fireSound;
    AudioSource audioSource;
    WeaponAmmo ammo;

    ActionStateManager actions;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        aim = GetComponentInParent<AimStateManager>();
        ammo = GetComponent<WeaponAmmo>();
        actions = GetComponentInParent<ActionStateManager>();
        fireRateTimer = fireRate;

    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldFire()) Fire();
    }

    bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;

        if (fireRateTimer < fireRate) return false;
        if (ammo.currentAmmo == 0) return false;
        if (actions.currentState == actions.Reload) return false;
        if (semiAutomatic && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (!semiAutomatic && Input.GetKey(KeyCode.Mouse0)) return true;
        return false;

    }

    void Fire()
    {
        fireRateTimer = 0f;
        bulletSpawnLocation.LookAt(aim.aimPosition);
        audioSource.PlayOneShot(fireSound);
        ammo.currentAmmo--;
        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject currentBullet = Instantiate(bulletPrefab, bulletSpawnLocation.position, bulletSpawnLocation.rotation);
            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            if (rb != null) rb.AddForce(bulletSpawnLocation.forward * bulletVelocity, ForceMode.Impulse);

        }
    }
}
