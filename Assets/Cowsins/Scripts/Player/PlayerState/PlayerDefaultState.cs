using UnityEngine;
namespace cowsins {
public class PlayerDefaultState : PlayerBaseState
{
    public PlayerDefaultState(PlayerStates currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    private PlayerMovement player;

    private PlayerStats stats; 
    public override void EnterState()
    {
        player = _ctx.GetComponent<PlayerMovement>();
        stats = _ctx.GetComponent<PlayerStats>();
        player.CanJump = true;
    }

    public override void UpdateState() {
        HandleMovement();
        if (!stats.controllable) return; 
        CheckSwitchState();
        HandleInteractions();
        CheckUnCrouch(); 
    }

    public override void FixedUpdateState()  {  }

    public override void ExitState() { }

    public override void CheckSwitchState() {

        if(stats.health <= 0 ) SwitchState(_factory.Die());

        if (InputManager.jumping && (player.CanJump))
                SwitchState(_factory.Jump());

        if(player.canDash && InputManager.dashing && (player.infiniteDashes ||player.currentDashes > 0 && !player.infiniteDashes)) SwitchState(_factory.Dash());

        if (InputManager.crouchingDown && !player.wallRunning && player.allowCrouch)
        {
            if(player.grounded)
            SwitchState(_factory.Crouch());
            else
            {
                if(player.allowCrouchWhileJumping) SwitchState(_factory.Crouch());
            }
           
        }
    }

    public override void InitializeSubState() { }

    void HandleMovement()
    {
        if (InputManager.x != 0 || InputManager.y != 0)
        {
            player.events.OnMove.Invoke();
            player.Movement(stats.controllable);
            if (!stats.controllable) return;
        }
        player.Look();
        player.FootSteps();
        player.HandleVelocities();
        player.HandleCoyoteJump(); 
    }

    void HandleInteractions()  {}

    private bool canUnCrouch = false;

    private void CheckUnCrouch()
    {

        RaycastHit hitt;
        if (!InputManager.crouching) // Prevent from uncrouching when there磗 a roof and we can get hit with it
        {
            if (Physics.Raycast(_ctx.transform.position, _ctx.transform.up, out hitt, 5.5f, player.weapon.hitLayer))
            {
                canUnCrouch = false;
            }
            else
                canUnCrouch = true;
        }
        if (canUnCrouch)
        {
            player.events.OnStopCrouch.Invoke(); // Invoke your own method on the moment you are standing up NOT WHILE YOU ARE NOT CROUCHING
            player.StopCrouch();
        }
    }
}
}