using UnityEngine;

public class IdleState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {

    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.moveDirection.magnitude > 0.1f)
        {
            if (movement.playerInput.actions["Sprint"].IsPressed()) movement.SwitchState(movement.Run);
            else movement.SwitchState(movement.Walk);
        }


        if (movement.playerInput.actions["Crouch"].WasPressedThisFrame())
        {
            movement.SwitchState(movement.Crouch);
        }
        
        if (movement.playerInput.actions["Jump"].WasPressedThisFrame())
        {
            movement.previousState = this;
            movement.SwitchState(movement.Jump);
        }
    }
}
