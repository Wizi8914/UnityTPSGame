using System;
using UnityEngine;

public class DefaultState : ActionBaseState
{

    public float scrollDireciton;

    public override void EnterState(ActionStateManager actions)
    {
    }

    public override void UpdateState(ActionStateManager actions)
    {
        actions.rHandAnim.weight = Mathf.Lerp(actions.rHandAnim.weight, 0.3f, Time.deltaTime * 10f);
        //actions.lHandIK.weight = Mathf.Lerp(actions.lHandIK.weight, 1f, Time.deltaTime * 10f);
        if (actions.lHandIK.weight == 0) actions.lHandIK.weight = 1;


        if (actions.playerInput.actions["Reload"].triggered && CanReload(actions))
        {
            actions.SwitchState(actions.Reload);
        }
        else if (Input.mouseScrollDelta.y != 0)
        {
            scrollDireciton = Input.mouseScrollDelta.y;
            actions.SwitchState(actions.Swap);
        }
    }
    
    bool CanReload(ActionStateManager action)
    {
        if (action.ammo.currentAmmo == action.ammo.clipSize) return false;
        else if (action.ammo.extraAmmo == 0) return false;
        else return true;
    }
}