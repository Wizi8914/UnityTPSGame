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
        audioSource.PlayOneShot(fireSound);
        recoil.TriggerRecoil();
        TriggerMuzzleFlash();
        ammo.currentAmmo--;
        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject currentBullet = Instantiate(bulletPrefab, bulletSpawnLocation.position, bulletSpawnLocation.rotation);
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
