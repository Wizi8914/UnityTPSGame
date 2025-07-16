using UnityEngine;

public class AimState : AimBaseState
{
    public override void EnterState(AimStateManager aim)
    {
        aim.animator.SetBool("Aiming", true);
        aim.currentFov = aim.adsFov;
    }
    public override void UpdateState(AimStateManager aim)
    {
        if (aim.playerInput.actions["Aim"].WasReleasedThisFrame()) aim.SwitchState(aim.Hip);
    }
}
