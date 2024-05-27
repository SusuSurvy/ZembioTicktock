using UnityEngine;
namespace cowsins {
public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStates currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    private PlayerMovement player;

    private PlayerStats stats; 
    private float _currentTime = 0;
    private Transform _head;
    private float initialY;
    
    private float jumpHeight = 1f; // 跳跃高度
    private float gravity = -16.81f; // 重力加速度
    private float jumpVelocity; // 起跳初速度
    private float verticalVelocity;
    public override void EnterState() {
        Debug.LogError("jump");
        player = _ctx.GetComponent<PlayerMovement>();
        stats = _ctx.GetComponent<PlayerStats>();
        _head = player.Head;
        player.events.OnJump.Invoke();
        player.Jump();
        _currentTime = 0;
        player.CanJump = false;
        initialY = 1.64f;
        jumpVelocity = Mathf.Sqrt(2 * jumpHeight * -gravity);
        verticalVelocity = jumpVelocity;   
    }

    public override void UpdateState() {
        CheckSwitchState(); 
        HandleMovement();
        CheckUnCrouch(); 
    }

    public override void FixedUpdateState() { }

    public override void ExitState() {}

    public override void CheckSwitchState() {
        verticalVelocity += gravity * Time.deltaTime;
        _head.localPosition += new Vector3(0, verticalVelocity * Time.deltaTime, 0);

        if (_head.localPosition.y <= initialY)
        {
            _head.localPosition = new Vector3(_head.localPosition.x, initialY, _head.localPosition.z);
            verticalVelocity = 0;
            SwitchState(_factory.Default());
        }
        _currentTime+= Time.deltaTime;
        // if (_currentTime > 1f)
        // {
        //   
        // }
        // if (player.ReadyToJump && InputManager.jumping && (player.CanJump && player.grounded || player.wallRunning || player.jumpCount > 0 && player.maxJumps > 1 && player.CanJump))
        //     SwitchState(_factory.Jump());

        if (stats.health <= 0) SwitchState(_factory.Die());

        // if (player.grounded || player.wallRunning )
        //    SwitchState(_factory.Default());
        // if (player.canDash && InputManager.dashing && (player.infiniteDashes || player.currentDashes > 0 && !player.infiniteDashes)) SwitchState(_factory.Dash());
        //
        // if (InputManager.crouchingDown && !player.wallRunning && player.allowCrouch && player.allowCrouchWhileJumping)
        // {
        //     SwitchState(_factory.Crouch());
        // }
    }

    public override void InitializeSubState() { }

    void HandleMovement()
    {
        player.Movement(stats.controllable);
        player.Look();
    }

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