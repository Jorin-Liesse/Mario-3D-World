using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private CharacterController _charCtrl = null;
    [SerializeField] private Transform _playerTransform = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _maxSpeedWalking = (50f * 1000) / (60 * 60); // [m/s], 50 km/h
    [SerializeField] private float _maxSpeedRunning = (100f * 1000) / (60 * 60); // [m/s], 100 km/h
    [SerializeField] private float _dragOnGround = 10f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _gravityMulti = 2f;
    [SerializeField] private bool _detectCollisions = true;
    [SerializeField] private bool _enableOverlapRecovery = true;
    [SerializeField] private bool _callMoveFunction = true;
    [SerializeField] private float _wanderingRange = 10f;
    [SerializeField] private float _triggerRange = 5f;

    [HideInInspector] public bool Death = false;

    private float _maxSpeed;
    private Vector3 _velocity;
    private Vector3 _inputVector;
    private Vector3 _startPosition;
    private Vector3 _targetPosition;

    void Start()
    {
        _charCtrl.detectCollisions = _detectCollisions;
        _charCtrl.enableOverlapRecovery = _enableOverlapRecovery;

        _startPosition = transform.position;
        RandomTargetPosition();
    }

    void Update()
    {

        if (Death &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 &&
            !animator.IsInTransition(0) &&
            animator.GetCurrentAnimatorStateInfo(0).IsName("GoombaDead")) Destroy(gameObject);

        if (Death) return;
        if (Vector3.Distance(transform.position, _playerTransform.position) < _triggerRange)
        {
            MoveTowardsPlayer();
            _maxSpeed = _maxSpeedRunning;
        }
        else
        {
            MoveTowardsTarget();
            _maxSpeed = _maxSpeedWalking;
        }
    }

    private void FixedUpdate()
    {
        if (Death) return;
        if (_callMoveFunction)
        {
            ApplyGravity();
            ApplyMovement();
            ApplyGroundDrag();
            ApplySpeedLimitation();
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
            _velocity *= 1 - Time.deltaTime * _dragOnGround;
        }
    }

    private void ApplySpeedLimitation()
    {
        float tempY = _velocity.y;
        _velocity.y = 0;
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

    private void ApplyRotation()
    {
        float step = _rotationSpeed * Time.deltaTime;
        
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, _inputVector, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void RandomTargetPosition()
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);
        _targetPosition = _startPosition + new Vector3(Mathf.Cos(randomAngle), 0f, Mathf.Sin(randomAngle)) * _wanderingRange;
    }

    private void MoveTowardsPlayer()
    {
        _inputVector = (_playerTransform.position - transform.position).normalized;
        _inputVector.y = 0;

        if (Vector3.Distance(transform.position, _playerTransform.position) < 0.1f)
        {
            RandomTargetPosition();
        }
    }

    private void MoveTowardsTarget()
    {
        _inputVector = (_targetPosition - transform.position).normalized;
        _inputVector.y = 0;

        if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
        {
            RandomTargetPosition();
        }
    }   
}
