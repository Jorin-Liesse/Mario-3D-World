using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target= null;
    [SerializeField] private float _followSpeed = 5f;
    [SerializeField] private Vector3 _offset = Vector3.zero;

    [SerializeField] private bool _xLockAxis = false;
    [SerializeField] private bool _yLockAxis = false;
    [SerializeField] private bool _zLockAxis = false;

    void Start()
    {
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = _target.position + _offset;
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, _followSpeed * Time.deltaTime);

        if (_xLockAxis) transform.position = new Vector3(_target.position.x + _offset.x, transform.position.y, transform.position.z);

        if (_yLockAxis) transform.position = new Vector3(transform.position.x, _target.position.y + _offset.y, transform.position.z);

        if (_zLockAxis) transform.position = new Vector3(transform.position.x, transform.position.y, _target.position.z + _offset.z);
    }
}
