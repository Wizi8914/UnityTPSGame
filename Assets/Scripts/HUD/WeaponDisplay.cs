using System;
using TMPro;
using UnityEngine;

public class WeaponDispla : MonoBehaviour
{
    public TMP_Text currentWeaponText;
    public TMP_Text currentAmmoText;
    public TMP_Text ammoLeftText;

    public GameObject player;

    private ActionStateManager actions;

    void Start()
    {
        actions = player.GetComponent<ActionStateManager>();
        updateHUD();
    }

    void Update()
    {
        if (actions.currentWeapon != null)
        {
            updateHUD();
        }
    }

    void updateHUD()
    {
        currentWeaponText.text = actions.currentWeapon.weaponName;
        currentAmmoText.text = actions.currentWeapon.ammo.currentAmmo.ToString();
        ammoLeftText.text = actions.currentWeapon.ammo.extraAmmo.ToString();
    }


}