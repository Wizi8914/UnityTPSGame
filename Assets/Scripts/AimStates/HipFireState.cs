using UnityEngine;

public class HipFireState : AimBaseState
{
    public override void EnterState(AimStateManager aim)
    {
        aim.animator.SetBool("Aiming", false);
        aim.currentFov = aim.hipFov;
    }
    public override void UpdateState(AimStateManager aim)
    {
        if (aim.playerInput.actions["Aim"].IsPressed()) aim.SwitchState(aim.Aim);
    }
}
