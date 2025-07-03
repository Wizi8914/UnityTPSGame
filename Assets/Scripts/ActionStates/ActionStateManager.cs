using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class ActionStateManager : MonoBehaviour
{
    [HideInInspector] public ActionBaseState currentState;

    public DefaultState Default = new DefaultState();
    public ReloadState Reload = new ReloadState();

    [HideInInspector] public PlayerInput playerInput;

    public GameObject currentWeapon;
    [HideInInspector] public WeaponAmmo ammo;
    AudioSource audioSource;

    [HideInInspector] public Animator animator;
    public MultiAimConstraint rHandAnim;
    public TwoBoneIKConstraint lHandIK;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SwitchState(Default);
        playerInput = GetComponent<PlayerInput>();
        ammo = currentWeapon.GetComponent<WeaponAmmo>();
        audioSource = currentWeapon.GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(ActionBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    public void WeaponReloaded()
    {
        ammo.Reload();
        rHandAnim.weight = 0.3f;
        lHandIK.weight = 1f;
        SwitchState(Default);
    }

    public void MagazineIn() => audioSource.PlayOneShot(ammo.magazineOutSound);
    
    public void MagazineOut() => audioSource.PlayOneShot(ammo.magazineInSound);
    
    public void ReleaseSlide() => audioSource.PlayOneShot(ammo.releaseSlideSource);
    
}
