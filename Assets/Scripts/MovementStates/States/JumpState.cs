using UnityEngine;

public class JumpState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement)
    {
        if (movement.previousState == movement.Idle) movement.animator.SetTrigger("IdleJump");
        else if (movement.previousState == movement.Walk || movement.previousState == movement.Run) movement.animator.SetTrigger("RunJump");
        
    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.jumped && movement.IsGrounded())
        {
            movement.jumped = false;
            if (movement.hzInput == 0 && movement.vtInput == 0) movement.SwitchState(movement.Idle);  
            else if (movement.playerInput.actions["Sprint"].IsPressed()) movement.SwitchState(movement.Run); 
            else movement.SwitchState(movement.Walk);
        }
    }
}
