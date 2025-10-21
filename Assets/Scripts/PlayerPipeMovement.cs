using System;
using UnityEngine;

public class PlayerPipeMovement : MonoBehaviour
{
    [SerializeField] private PlayerAnimationController _animationController;
    [SerializeField] private float _speed = 5f; 
    [SerializeField] private float _rotationSpeed = 5.0f;
    private int _currentPointIndex = 0;
    private Transform[] _points;
    private bool _isMoving = false;
    private GameObject _pipe = null;
    private string _entryPoint;
    private bool _prefIsMoving = false;

    void Update()
    {
        if(_pipe == null) return;

        if (_isMoving)
        {
            MoveToNextPoint(_points);
            DisablePlayerController(gameObject);
            _animationController.PlayerState = PlayerAnimationController.PlayerStates.Crawling;
        }

        if (_isMoving == false && _prefIsMoving == true) {
            EnablePlayerController(gameObject);
            _animationController.PlayerState = PlayerAnimationController.PlayerStates.Standing;
        }

        _prefIsMoving = _isMoving;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isMoving == true) return;
        if (other.CompareTag("StartPipe") || other.CompareTag("EndPipe"))
        {
            GameObject parentObject = other.gameObject.transform.parent.gameObject;
            GameObject childObject = parentObject.transform.Find("Path").gameObject;
            Path pathObject = childObject.GetComponent<Path>();

            _pipe = parentObject;
            _points = pathObject.waypoints;
            _isMoving = true;

            _entryPoint = other.tag;
        }
    }

    void MoveToNextPoint(Transform[] points)
    {
        if (points.Length == 0) return;

        Vector3 targetPoint;
        if (_entryPoint == "StartPipe") targetPoint = points[_currentPointIndex].position;
        else targetPoint = points[Math.Abs(_currentPointIndex - _points.Length + 1)].position;
        
        Vector3 moveDirection = (targetPoint - transform.position).normalized;
        transform.position += _speed * Time.deltaTime * moveDirection;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            _currentPointIndex = (_currentPointIndex + 1) % points.Length;
            if (_currentPointIndex == 0)
            {
                _isMoving = false;
            }
        }
    }

    void DisablePlayerController(GameObject obj)
    {
        PlayerController playerController = obj.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    void EnablePlayerController(GameObject obj)
    {
        PlayerController playerController = obj.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}
