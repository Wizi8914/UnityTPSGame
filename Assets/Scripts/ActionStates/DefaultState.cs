using System;
using UnityEngine;

public class DefaultState : ActionBaseState
{
    public override void EnterState(ActionStateManager actions)
    {
    }

    public override void UpdateState(ActionStateManager actions)
    {
        actions.rHandAnim.weight = Mathf.Lerp(actions.rHandAnim.weight, 0.3f, Time.deltaTime * 10f);
        actions.lHandIK.weight = Mathf.Lerp(actions.lHandIK.weight, 1f, Time.deltaTime * 10f);

        if (actions.playerInput.actions["Reload"].triggered && CanReload(actions))
        {
            actions.SwitchState(actions.Reload);
        }
    }
    
    bool CanReload(ActionStateManager action)
    {
        if (action.ammo.currentAmmo == action.ammo.clipSize) return false;
        else if (action.ammo.extraAmmo == 0) return false;
        else return true;
    }
}