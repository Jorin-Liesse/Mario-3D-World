using UnityEngine;

public class PlayerClimbingTree : MonoBehaviour
{
    private bool _isClimbing = false;
    [SerializeField] private Transform _target = null;
    [SerializeField] private float _speed = 5f;

    private void Update()
    {
        if (_isClimbing)
        {
            float step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _target.position, step);

            if (Vector3.Distance(transform.position, _target.position) < 0.1f)
            {
                _isClimbing = false;
                EnablePlayerController(gameObject);
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tree"))
        {
            _isClimbing = true;
            _target = other.transform.GetChild(0).transform;
            DisablePlayerController(gameObject);
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
