using UnityEngine;

public class SwapState : ActionBaseState
{
    public override void EnterState(ActionStateManager actions)
    {
        actions.animator.SetTrigger("SwapWeapon");
        actions.lHandIK.weight = 0f;
        actions.rHandAnim.weight = 0f;
    }

    public override void UpdateState(ActionStateManager actions)
    {

    }
}

