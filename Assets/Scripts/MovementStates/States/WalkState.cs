using UnityEngine;
using UnityEngine.InputSystem;

public class WalkState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.SetBool("Walking", true);
    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.playerInput.actions["Sprint"].IsPressed()) ExitState(movement, movement.Run);
        else if (movement.playerInput.actions["Crouch"].WasPressedThisFrame()) ExitState(movement, movement.Crouch);
        else if (movement.moveDirection.magnitude < 0.1f) ExitState(movement, movement.Idle);

        if (movement.vtInput < 0) movement.currentMoveSpeed = movement.walkBackSpeed;
        else movement.currentMoveSpeed = movement.walkSpeed;

        if (movement.playerInput.actions["Jump"].WasPressedThisFrame())
        {
            movement.previousState = this;
            ExitState(movement, movement.Jump);
        }
    }

    void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.animator.SetBool("Walking", false);
        movement.SwitchState(state);
    }
}

