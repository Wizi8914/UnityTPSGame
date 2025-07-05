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
    public float damage = 10;
    AimStateManager aim;

    [SerializeField] AudioClip fireSound;
    AudioSource audioSource;
    WeaponAmmo ammo;
    WeaponBloom bloom;
    WeaponRecoil recoil;

    Light muzzleFlashLight;
    ParticleSystem muzzleFlashPArticle;
    float lightIntensity;
    [SerializeField] float lightReturnSpeed = 20f;

    ActionStateManager actions;

    void Start()
    {
        recoil = GetComponent<WeaponRecoil>();
        audioSource = GetComponent<AudioSource>();
        aim = GetComponentInParent<AimStateManager>();
        ammo = GetComponent<WeaponAmmo>();
        bloom = GetComponent<WeaponBloom>();
        actions = GetComponentInParent<ActionStateManager>();
        muzzleFlashLight = bulletSpawnLocation.GetComponentInChildren<Light>();
        lightIntensity = muzzleFlashLight.intensity;
        muzzleFlashLight.intensity = 0f;
        muzzleFlashPArticle = bulletSpawnLocation.GetComponentInChildren<ParticleSystem>();
        fireRateTimer = fireRate;

    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldFire()) Fire();
        muzzleFlashLight.intensity = Mathf.Lerp(muzzleFlashLight.intensity, 0f, lightReturnSpeed * Time.deltaTime);
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
        bulletSpawnLocation.localEulerAngles = bloom.bloomAngle(bulletSpawnLocation);

        // Play fire sound
        audioSource.pitch = Random.Range(0.85f, 1.1f);
        audioSource.volume = Random.Range(0.8f, 1f);
        //audioSource.time = Random.Range(0f, fireSound.length);
        audioSource.PlayOneShot(fireSound);

        recoil.TriggerRecoil();
        TriggerMuzzleFlash();
        ammo.currentAmmo--;
        for (int i = 0; i < bulletsPerShot; i++)
        {
            
            GameObject currentBullet = Instantiate(bulletPrefab, bulletSpawnLocation.position, bulletSpawnLocation.rotation);
            Bullet bullet = currentBullet.GetComponent<Bullet>();
            bullet.weapon = this; // Assign the weapon to the bullet

            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            
            if (rb != null) rb.AddForce(bulletSpawnLocation.forward * bulletVelocity, ForceMode.Impulse);
        }
    }

    void TriggerMuzzleFlash()
    {
        muzzleFlashPArticle.Play();
        muzzleFlashLight.intensity = lightIntensity;
    }
}
