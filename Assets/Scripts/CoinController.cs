using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f;

    void Start() 
    {
        Vector3 randomRotation = new (0, Random.Range(0, 180), 0);
        transform.Rotate(randomRotation);
    }
    void Update()
    {
        Vector3 rotate = new (0, _rotationSpeed * Time.deltaTime, 0);
        transform.Rotate(rotate);
    }
}
