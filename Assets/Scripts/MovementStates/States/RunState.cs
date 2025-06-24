using UnityEngine;

public class RunState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        movement.animator.SetBool("Running", true);        

    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.playerInput.actions["Sprint"].WasReleasedThisFrame()) ExitState(movement, movement.Walk);
        else if (movement.moveDirection.magnitude < 0.1f) ExitState(movement, movement.Idle);

        if (movement.vtInput < 0) movement.currentMoveSpeed = movement.runBackSpeed;
        else movement.currentMoveSpeed = movement.runSpeed;     
    }
    
        void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.animator.SetBool("Running", false);
        movement.SwitchState(state);
    }
}

