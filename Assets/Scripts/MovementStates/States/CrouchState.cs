using UnityEngine;

public class CrouchState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.SetBool("Crouching", true);
    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.playerInput.actions["Sprint"].IsPressed()) ExitState(movement, movement.Run);

        if (movement.playerInput.actions["Crouch"].WasPressedThisFrame())
        {
            if (movement.moveDirection.magnitude < 0.1f) ExitState(movement, movement.Idle);
            else ExitState(movement, movement.Walk);
        }

        if (movement.vtInput < 0) movement.currentMoveSpeed = movement.crouchBackSpeed;
        else movement.currentMoveSpeed = movement.crouchSpeed;

    }
    
    void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.animator.SetBool("Crouching", false);
        movement.SwitchState(state);
    }
}
