using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponClassManager : MonoBehaviour
{
    [SerializeField] TwoBoneIKConstraint leftHandIK;
    public Transform recoilFollowPos;

    ActionStateManager actions;

    public WeaponManager[] weapon;
    int currentWeaponIndex;

    private void Awake()
    {
        currentWeaponIndex = 0;
        for (int i = 0; i < weapon.Length; i++)
        {
            if (i == 0) weapon[i].gameObject.SetActive(true);
            else weapon[i].gameObject.SetActive(false);
        }
    }

    public void SetCurrentWeapon(WeaponManager weapon)
    {
        if (actions == null) actions = GetComponent<ActionStateManager>();
        leftHandIK.data.target = weapon.leftHandTarget;
        leftHandIK.data.hint = weapon.leftHandHint;
        actions.SetWeapon(weapon);
    }

    public void ChangeWeapon(float direction)
    {
        weapon[currentWeaponIndex].gameObject.SetActive(false);
        if (direction < 0)
        {
            if (currentWeaponIndex == 0) currentWeaponIndex = weapon.Length - 1;
            else currentWeaponIndex--;
        }
        else
        {
            if (currentWeaponIndex == weapon.Length - 1) currentWeaponIndex = 0;
            else currentWeaponIndex++;
        }
        weapon[currentWeaponIndex].gameObject.SetActive(true);
    }

    public void WeaponPutAway()
    {
        ChangeWeapon(actions.Default.scrollDireciton);
    }

    public void WeaponPulledOut()
    {
        actions.SwitchState(actions.Default);
    }
}
