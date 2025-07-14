using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
    [Header("Fire Rate")]
    [SerializeField] float fireRate;
    float fireRateTimer;
    [SerializeField] bool semiAutomatic = true;

    [Header("Burst Fire")]
    [SerializeField] bool isBurstFire = false;
    [SerializeField] float burstInterval = 0.08f;
    bool isBursting = false;

    [Header("Bullet Properties")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnLocation;
    [SerializeField] int bulletsPerShot;
    [SerializeField] float bulletVelocity;
    public float damage = 10;
    AimStateManager aim;

    [SerializeField] AudioClip fireSound;
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public WeaponAmmo ammo;
    WeaponBloom bloom;
    WeaponRecoil recoil;

    Light muzzleFlashLight;
    ParticleSystem muzzleFlashPArticle;
    float lightIntensity;
    [SerializeField] float lightReturnSpeed = 20f;

    public float ennemyKickBackForce = 100f;

    public Transform leftHandTarget, leftHandHint;
    WeaponClassManager weaponClass;

    ActionStateManager actions;

    void Start()
    {
        aim = GetComponentInParent<AimStateManager>();
        bloom = GetComponent<WeaponBloom>();
        actions = GetComponentInParent<ActionStateManager>();
        muzzleFlashLight = bulletSpawnLocation.GetComponentInChildren<Light>();
        lightIntensity = muzzleFlashLight.intensity;
        muzzleFlashLight.intensity = 0f;
        muzzleFlashPArticle = bulletSpawnLocation.GetComponentInChildren<ParticleSystem>();
        fireRateTimer = fireRate;
    }

    void OnEnable()
    {
        if (weaponClass == null)
        {
            weaponClass = GetComponentInParent<WeaponClassManager>();
            audioSource = GetComponent<AudioSource>();
            ammo = GetComponent<WeaponAmmo>();
            recoil = GetComponent<WeaponRecoil>();
            recoil.recoilFollowPos = weaponClass.recoilFollowPos;
        }
        weaponClass.SetCurrentWeapon(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (isBurstFire)
        {
            if (!isBursting && ShouldFire())
            {
                StartCoroutine(BurstFire());
            }
        }
        else
        {
            if (ShouldFire()) Fire();
        }
        muzzleFlashLight.intensity = Mathf.Lerp(muzzleFlashLight.intensity, 0f, lightReturnSpeed * Time.deltaTime);
    }

    bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;

        if (fireRateTimer < fireRate) return false;
        if (ammo.currentAmmo == 0) return false;
        if (actions.currentState == actions.Reload) return false;
        if (actions.currentState == actions.Swap) return false;
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

            bullet.direction = bulletSpawnLocation.transform.forward;

            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();

            if (rb != null) rb.AddForce(bulletSpawnLocation.forward * bulletVelocity, ForceMode.Impulse);
        }
    }

    IEnumerator BurstFire()
    {
        isBursting = true;
        for (int i = 0; i < bulletsPerShot; i++)
        {
            if (ammo.currentAmmo == 0 || actions.currentState == actions.Reload || actions.currentState == actions.Swap)
                break;

            Fire();
            yield return new WaitForSeconds(burstInterval);
        }
        isBursting = false;
    }


    void TriggerMuzzleFlash()
    {
        muzzleFlashPArticle.Play();
        muzzleFlashLight.intensity = lightIntensity;
    }
}
