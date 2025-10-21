using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _charCtrl = null;

    [SerializeField] private float _acceleration = 10f;

    [SerializeField] private float _maxWalkingSpeed = (30.0f * 1000) / (60 * 60); // [m/s], 30 km/h
    [SerializeField] private float _maxRunningSpeed = (50.0f * 1000) / (60 * 60); // [m/s], 50 km/h

    [SerializeField] private float _dragOnGround = 10f;

    [SerializeField] private float _jumpHeight = 2;

    [SerializeField] private float _rotationSpeed = 5.0f;

    [SerializeField] private float _gravityMulti = 2f;

    [SerializeField] private bool _detectCollisions = true;
    [SerializeField] private bool _enableOverlapRecovery = true;

    [SerializeField] private bool _callMoveFunction = true;

    [SerializeField] private Vector3 _endPosition;
    [SerializeField] private Vector3 _endRotation;

    [SerializeField] private PlayerAnimationController _animationController;
    [SerializeField] private UIController _uiController = null;

    [SerializeField] private PlaySound _jumpSound = null;

    private Vector3 _velocity;
    private Vector3 _inputVector;
    private Vector3 _jumpForce;
    private float _maxSpeed;
    private bool _jump;
    private bool _isJumping = false;
    private bool _isDoubleJumping = false;

    void Start()
    {
        _charCtrl.detectCollisions = _detectCollisions;
        _charCtrl.enableOverlapRecovery = _enableOverlapRecovery;

        _jumpForce = -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * _gravityMulti * _jumpHeight);
    }

    void Update()
    {
        if (_uiController.CurrentScreen == UIController.Screen.End) {
            _animationController.PlayerState = PlayerAnimationController.PlayerStates.Dansing;
            transform.SetPositionAndRotation(
                Vector3.MoveTowards(transform.position, _endPosition, 0.1f),
                Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_endRotation), 1f));
        }

        if (_uiController.CurrentScreen != UIController.Screen.InGame) return;
        _inputVector.x = Input.GetAxis("Horizontal");
        _inputVector.z = Input.GetAxis("Vertical");

        CheckMoveStates();
        CheckJumpStates();
    }

    private void FixedUpdate()
    {
        if (_uiController.CurrentScreen != UIController.Screen.InGame) return;
        if (_callMoveFunction)
        {
            ApplyGravity();
            ApplyMovement();
            ApplyGroundDrag();
            ApplySpeedLimitation();
            ApplyJump();
            ApplyRotation();
            _charCtrl.Move(_velocity * Time.deltaTime);
        }
    }

    private void ApplyMovement()
    {
        _velocity += _inputVector * _acceleration;
    }

    private void ApplyGroundDrag()
    {
        if (_charCtrl.isGrounded)
        {
            _velocity *= (1 - Time.deltaTime * _dragOnGround);
        }
    }

    private void ApplySpeedLimitation()
    {
        float tempY = _velocity.y;

        _velocity.y = 0;

        if (_animationController.PlayerState == PlayerAnimationController.PlayerStates.Waling) _maxSpeed = _maxWalkingSpeed;
        if (_animationController.PlayerState == PlayerAnimationController.PlayerStates.Running) _maxSpeed = _maxRunningSpeed;

        _velocity = Vector3.ClampMagnitude(_velocity, _maxSpeed);

        _velocity.y = tempY;
    }

    private void ApplyGravity()
    {
        if (_charCtrl.isGrounded)
        {
            _velocity.y = Physics.gravity.y * _gravityMulti * _charCtrl.skinWidth;
        }
        else
        {
            _velocity.y += Physics.gravity.y * _gravityMulti * Time.deltaTime;
        }
    }

    private void ApplyJump()
    {
        if (_jump && _charCtrl.isGrounded)
        {
            _isJumping = true;
            _velocity += _jumpForce;
        }

        if (_isDoubleJumping)
        {
            _velocity.y = 0;
            _velocity += _jumpForce;
            _isDoubleJumping = false;
            _isJumping = false;
        }
        _jump = false;
    }

    private void ApplyRotation()
    {
        float step = _rotationSpeed * Time.deltaTime;
        
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, _inputVector, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void CheckMoveStates() {
        if (!_charCtrl.isGrounded) return;
        
        if (_inputVector.magnitude < 0.01)
        {
            _animationController.PlayerState = PlayerAnimationController.PlayerStates.Standing;
        }
        else if (_inputVector.magnitude >= 0.01)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                _animationController.PlayerState = PlayerAnimationController.PlayerStates.Running;
            }
            else
            {
                if (_charCtrl.isGrounded) 
                _animationController.PlayerState = PlayerAnimationController.PlayerStates.Waling;
            }
        }
    }

    private void CheckJumpStates()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _jump = true;
            _jumpSound.PlaySoundEffect();

            _animationController.PlayerState = PlayerAnimationController.PlayerStates.Jumping;

            if (_isJumping)
            {
                _isDoubleJumping = true;
                _animationController.PlayerState = PlayerAnimationController.PlayerStates.DoubleJumping;
            }
        }

        if (_charCtrl.isGrounded)
        {
            _isJumping = false;
        }
    }
}
