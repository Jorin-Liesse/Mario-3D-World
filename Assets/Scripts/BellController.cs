using UnityEngine;

public class BellController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _range = 0.5f;
    private Vector3 _startPosition;


    void Start()
    {
        _startPosition = transform.position;
    }
    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * _speed) * _range;
        Vector3 newPosition = _startPosition + Vector3.up * offsetY;
        transform.position = newPosition;
    }
}
