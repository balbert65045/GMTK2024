using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public const float MAX_VELOCITY = 20f;

    //Scriptable object which holds all the player's movement parameters. If you don't want to use it
    //just paste in all the parameters, though you will need to manuly change all references in this script

    //HOW TO: to add the scriptable object, right-click in the project window -> create -> Player Data
    //Next, drag it into the slot in playerMovement on your player

    public PlayerData Data;

    #region Variables
    //Components
    public Rigidbody2D RB { get; private set; }

    //Variables control the various actions the player can perform at any time.
    //These are fields which can are public allowing for other sctipts to read them
    //but can only be privately written to.
    public bool IsFacingRight { get; private set; }
    //public bool IsJumping { get; private set; }
    public bool IsJumping;
    public bool IsWallJumping;

    public bool IsWallHanging;


    //Timers (also all fields, could be private and a method returning a bool could be used)
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }

    //Jump
    public bool _isJumpCut;
    private bool _isJumpFalling;

    //Wall Jump
    private float _wallJumpStartTime;
    private int _lastWallJumpDir;

    private Vector2 _moveInput;

    //Set all of these up in the inspector
    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [Space(5)]
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
    [SerializeField] GameObject visual;

    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    #endregion


    //[SerializeField] float CapedVerticalJumpVelocity = 20f;

    public Action OnJump;
    List<Collider2D> allSurfacesTouching;



    //public Collider2D 

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        IsFacingRight = true;
    }

    private void Start()
    {
        Unfreeze();
        SetGravityScale(Data.gravityScale);
    }

    bool Frozen = false;
    public void Freeze()
    {
        Frozen = true;
        RB.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Unfreeze()
    {
        Frozen = false;
        RB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void UseNoGravity()
    {
        SetGravityScale(0);
    }

    public void UseGravity()
    {
        SetGravityScale(Data.gravityScale);
    }

    public void DisableJump()
    {
        allowJump = false;
        _isJumpCut = false;
    }

    public void EnableJump()
    {
        allowJump = true;
    }

    bool allowJump = true;

    public bool allowMoveInputs = true;
    public void DisableMoveInputs()
    {
        allowMoveInputs = false;
        _isJumpCut = false;
    }


    public void EnableMoveInputs()
    {
        allowMoveInputs = true;
    }

    public void SetJumpCut(bool jumpCut)
    {
        _isJumpCut = jumpCut;
        _isJumpFalling = true;
    }

    [SerializeField] bool AllowDoubleJump;
    bool isDoubleJumping = false;
    public EventHandler<bool> OnDoubleJumpChange;

    public Action OnJumpAction;
    public Action OnDoubleJump;

    bool jumpPressed;
    public bool touchingCeiling = false;
    public bool touchingGround = false;
    public bool touchingWall = false;

    public Vector2 velocityBeforeTouchingASurface;

    void ChangeDoubleJump(bool value)
    {
        if (value != isDoubleJumping)
        {
            if (OnDoubleJumpChange != null) { OnDoubleJumpChange(this, value); }
            isDoubleJumping = value;
        }
    }

    public void Jump()
    {
        if (CanJump())
        {
            if (OnJumpAction != null)
            {
                OnJumpAction();
            }

            IsJumping = true;
            ChangeDoubleJump(false);
            _isJumpCut = false;
            _isJumpFalling = false;

            DoJump();
        }
        else if (CanWallJump())
        {
            if (OnJumpAction != null)
            {
                OnJumpAction();
            }
            IsWallHanging = false;
            IsWallJumping = true;
            IsJumping = false;
            _isJumpCut = false;
            _isJumpFalling = false;
            _wallJumpStartTime = Time.time;
            _lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;

            WallJump(_lastWallJumpDir);
        }
    }

    public void JumpRelease()
    {
        OnJumpUpInput();
    }

    public void Move(Vector2 dir)
    {

        if (Frozen) { return; }
        _moveInput.x = dir.x;
        _moveInput.y = dir.y;
        if (_moveInput.x != 0)
            CheckDirectionToFace(dir.x > 0);
    }

    public void ReleaseFromSwing()
    {
        swingRelease = true;
    }
    bool swingRelease = false;
    [SerializeField] float SwingReleaseGravityScale = .5f;

    private void Update()
    {
        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;

        #endregion


        #region COLLISION CHECKS
        List<Collider2D> surfacesTouching = new List<Collider2D>();
        List<Collider2D> NonCeilingTouching = new List<Collider2D>();
        Collider2D[] AllGroundColliders = Physics2D.OverlapBoxAll(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer);

        Collider2D[] rightColliders = Physics2D.OverlapBoxAll(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer);
        foreach (Collider2D collider in rightColliders)
        {
            surfacesTouching.Add(collider);
            NonCeilingTouching.Add(collider);

        }
        if (rightColliders.Length > 0)
        {
            ChangeDoubleJump(false);

            //isDoubleJumping = false;
        }

        Collider2D[] leftColliders = Physics2D.OverlapBoxAll(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer);
        foreach (Collider2D collider in leftColliders)
        {
            surfacesTouching.Add(collider);
            NonCeilingTouching.Add(collider);

        }
        if (leftColliders.Length > 0)
        {
            ChangeDoubleJump(false);
        }

        allSurfacesTouching = surfacesTouching;


        //Ground Check
        if (AllGroundColliders.Length > 0 && !IsJumping) //checks if set box overlaps with ground
        {
            LastOnGroundTime = Data.coyoteTime; //if so sets the lastGrounded to coyoteTime
        }

        //Right Wall Check
        if (rightColliders.Length > 0)
        {
            LastOnWallRightTime = 0.05f;
        }

        //Right Wall Check
        if (leftColliders.Length > 0)
        {
            LastOnWallLeftTime = 0.05f;

        }

        //Two checks needed for both left and right walls since whenever the play turns the wall checkPoints swap sides
        LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
        if (LastOnWallTime < 0) { disableWallHang = false; }

        #endregion
        if (AllGroundColliders.Length > 0 && RB.velocity.y <= .01f)
        {
            touchingGround = true;
            IsJumping = false;
            IsWallJumping = false;
            swingRelease = false;
        }
        else
        {
            touchingGround = false;
        }

        //Fix this
        if (allSurfacesTouching.Count == 0)
        {
            velocityBeforeTouchingASurface = RB.velocity;
        }

        touchingWall = rightColliders.Length > 0 || leftColliders.Length > 0;
        if (IsJumping && RB.velocity.y < 0)
        {
            IsJumping = false;
        }

        if (!IsWallJumping)
        {
        }

        if (IsWallJumping && Time.time - _wallJumpStartTime > Data.wallJumpTime)
        {
            IsWallJumping = false;
        }


        if (LastOnGroundTime > 0 && !IsJumping)
        {
            _isJumpCut = false;

            if (!IsJumping)
                _isJumpFalling = false;
        }

        if (CanHang() && ((LastOnWallLeftTime > 0) || (LastOnWallRightTime > 0)))
        {
            IsWallHanging = true;
            if (LastOnWallLeftTime > 0)
            {
                visual.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (LastOnWallRightTime > 0)
            {
                visual.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
        }
        else
        {
            IsWallHanging = false;
            visual.transform.rotation = Quaternion.Euler(0, 0, 0);
        }


        if (!allowMoveInputs) {
            SetGravityScale(Data.gravityScale);
            return; }
        #region GRAVITY
        // if (RB.velocity.y < 0 && _moveInput.y <= 0)
        if (IsWallHanging)
        {
            SetGravityScale(Data.gravityScale * 0f);
            //RB.velocity = Vector2.zero;
        }
        else if (_moveInput.y < -.3f)
        {
            //Much higher gravity if holding down
            SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
        }

        //NEED to do something about this and using propulsion cannon
        else if (_isJumpCut && RB.velocity.y > 0)
        {
            //Higher gravity if jump button released
            SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
        }
        else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        {
            SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
        }
        else if (swingRelease)
        {
            SetGravityScale(Data.gravityScale * SwingReleaseGravityScale);
        }
        else if (RB.velocity.y < 0)
        {
            //Higher gravity if falling
            SetGravityScale(Data.gravityScale * Data.fallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
        }
        else
        {
            //Default gravity if standing on a platform or moving upwards
            SetGravityScale(Data.gravityScale);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        if (!allowMoveInputs)
        {
            return;
        }
        if (IsWallJumping)
        {
            Run(Data.wallJumpRunLerp);
        }
        else
        {
            Run(1);
        }
    }

    #region INPUT CALLBACKS

    public void OnJumpUpInput()
    {
        if ((CanJumpCut() || CanWallJumpCut()))
        {
            _isJumpCut = true;
        }
    }
    #endregion


    public void DisableGravity()
    {
        RB.gravityScale = 0;
        gravityEnabled = false;
    }

    public void EnableGravity()
    {
        RB.gravityScale = 1;
        RB.mass = 1;
        gravityEnabled = true;
    }
    bool gravityEnabled = true;
    #region GENERAL METHODS
    public void SetGravityScale(float scale)
    {
        if (gravityEnabled)
        {
            RB.gravityScale = scale;
        }
    }
    #endregion



    float currentSlippyness = 0f;
    public void SetNewSliperiness(float slipperiness)
    {
        currentSlippyness = slipperiness;
    }
    //MOVEMENT METHODS
    #region RUN METHODS
    private void Run(float lerpAmount)
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = _moveInput.x * Data.runMaxSpeed;
        float accelRate;

        if (IsWallHanging)
        {
            float vertMultiplier = .75f;
            float verticalDeccel = Data.runDeccelAmount / vertMultiplier;
            if (LastOnWallLeftTime > 0)
            {
                //We can reduce are control using Lerp() this smooths changes to are direction and speed
                targetSpeed = Mathf.Lerp(RB.velocity.y, targetSpeed, lerpAmount) * vertMultiplier;
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : (verticalDeccel - (verticalDeccel * currentSlippyness));
                //
                //Calculate difference between current velocity and desired velocity
                float speedDifVert = targetSpeed - RB.velocity.y;
                //Calculate force along x-axis to apply to thr player

                float movementVert = speedDifVert * accelRate * vertMultiplier;
                //Convert this to a vector and apply to rigidbody

                RB.AddForce(movementVert * Vector2.up, ForceMode2D.Force);

                //RB.AddForce(targetSpeed * Vector2.up * 20, ForceMode2D.Force);
                if (touchingGround && _moveInput.x < 0)
                {
                    disableWallHang = true;
                }
            }
            else
            {

                targetSpeed = Mathf.Lerp(RB.velocity.y, targetSpeed, lerpAmount) * vertMultiplier;
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : (verticalDeccel - (verticalDeccel * currentSlippyness));
                //
                //Calculate difference between current velocity and desired velocity
                //
                float speedDifVert = targetSpeed + RB.velocity.y;
                //Calculate force along x-axis to apply to thr player

                float movementVert = speedDifVert * accelRate;
                //Convert this to a vector and apply to rigidbody

                RB.AddForce(movementVert * Vector2.down, ForceMode2D.Force);
                if (touchingGround && _moveInput.x > 0)
                {
                    disableWallHang = true;
                }
            }
            return;
        }

        //We can reduce are control using Lerp() this smooths changes to are direction and speed
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : (Data.runDeccelAmount - (Data.runDeccelAmount * currentSlippyness));
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
        #endregion

        #region Add Bonus Jump Apex Acceleration
        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }
        #endregion

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - RB.velocity.x;
        //Calculate force along x-axis to apply to thr player

        float movement = speedDif * accelRate;
        
        //
        //Convert this to a vector and apply to rigidbody

        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);


    }


    private void Turn()
    {
        Vector3 scale = visual.transform.localScale;
        scale.x *= -1;
        visual.transform.localScale = scale;

        IsFacingRight = !IsFacingRight;
    }
    #endregion

    public void DoJump()
    {
        LastOnGroundTime = 0;
        float force = Data.jumpForce;
        if (RB.velocity.y < 0)
            force -= RB.velocity.y;

        if (OnJump != null) { OnJump(); }
        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        RB.velocity = new Vector2(RB.velocity.x, Mathf.Clamp(RB.velocity.y, 0, MAX_VELOCITY));
    }

    private void WallJump(int dir)
    {
        LastOnGroundTime = 0;
        LastOnWallRightTime = 0;
        LastOnWallLeftTime = 0;

        Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
        force.x *= -dir; //apply force in opposite direction of wall

        if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
            force.x -= RB.velocity.x;
        if (RB.velocity.y < 0)
        {
            RB.velocity = new Vector2(RB.velocity.x, 0);
        }

        if (RB.velocity.y > 0)
        {
            float upwardVelocity = Mathf.Clamp(RB.velocity.y, 0, Data.wallJumpForce.y);
            //13.5 is dependent on data jump force
            float multiplier = upwardVelocity / 13.5f;
            force.y = force.y * (1 - multiplier);
        }

        if (OnJump != null) { OnJump(); }

        RB.AddForce(force, ForceMode2D.Impulse);
        RB.velocity = new Vector2(Mathf.Clamp(RB.velocity.x, -MAX_VELOCITY, MAX_VELOCITY), Mathf.Clamp(RB.velocity.y, 0, MAX_VELOCITY));
    }


    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }

    private bool CanJump()
    {
        if (!allowJump) { return false; }
        return LastOnGroundTime > 0 && !IsJumping;
    }

    private bool CanWallJump()
    {
        if (!allowJump) { return false; }
        return LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping ||
             (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
    }

    private bool CanWallJumpCut()
    {
        return IsWallJumping && RB.velocity.y > 0;
    }


    private bool CanJumpCut()
    {
        return (IsJumping || isDoubleJumping) && RB.velocity.y > 0;
    }

    bool disableWallHang = false;
    public bool CanHang()
    {
        //!IsWallJumping
        if (IsWallJumping) { return false; }
        if (LastOnWallTime > 0 && !disableWallHang)
            return true;
        else
            return false;
    }


    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
        Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
    }
    #endregion
}
