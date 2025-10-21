using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private Mesh _usedMesh = null;
    [SerializeField] private Material _playerBodyYellow = null;
    [SerializeField] private Material _usedMaterial = null;
    [SerializeField] private GameObject _powerUp = null;
    [SerializeField] private Vector3 _powerOffset = new Vector3(0, 0.5f, 0);
    [SerializeField] private PlayerHits _playerHits = null;
    [SerializeField] private PlaySound _powerUpSound = null;
    [SerializeField] private PlaySound _bellSound = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            MeshFilter meshFilter = other.gameObject.GetComponent<MeshFilter>();
            meshFilter.mesh = _usedMesh;

            MeshRenderer meshRenderer = other.gameObject.GetComponent<MeshRenderer>();
            meshRenderer.materials = new Material[0];
            meshRenderer.material = _usedMaterial;

            Instantiate(_powerUp, other.transform.position + _powerOffset, Quaternion.identity);

            other.gameObject.tag = "UsedPowerUp";    
            _powerUpSound.PlaySoundEffect();  
        }

        if (other.CompareTag("Bell"))
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            SkinnedMeshRenderer meshRenderer = transform.Find("MarioModel").GetComponent<SkinnedMeshRenderer>();
            meshRenderer.material = _playerBodyYellow;
            _playerHits.Lifes = 2;
            _playerHits.IsCat = true;
            Destroy(other.gameObject);
            _bellSound.PlaySoundEffect();
        }
    }
}
