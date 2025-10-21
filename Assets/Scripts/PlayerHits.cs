using UnityEngine;

public class PlayerHits : MonoBehaviour
{
    [SerializeField] private UIController _uiController = null;
    [SerializeField] private CharacterController _charCtrl = null;

    [SerializeField] private Material _playerBodyNormal = null;
    [SerializeField] private float _damageImunityTime = 5f;
    [HideInInspector] public bool isHit = false;

    [HideInInspector] public int Lifes = 2;
    [HideInInspector] public bool IsCat = false;

    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private PlaySound _hitSound = null;

    private float _timeSinceLastHit = 0f;
    private bool _isImune = false;

    void Update()
    {
        if (_isImune)
        {
            _timeSinceLastHit += Time.deltaTime;
            if (_timeSinceLastHit >= _damageImunityTime)
            {
                _isImune = false;
                _timeSinceLastHit = 0f;
            }
        }

        if (!isHit) return;
        isHit = false;

        if (_isImune) return;
        _hitSound.PlaySoundEffect();
        Lifes--;

        _isImune = true;

        if (Lifes <= 0)
        {
            gameObject.SetActive(false);
            _uiController.CurrentScreen = UIController.Screen.GameOver;
        }
        else 
        {
            SkinnedMeshRenderer meshRenderer = transform.Find("MarioModel").GetComponent<SkinnedMeshRenderer>();

            if (meshRenderer) {
                if (IsCat) {
                    meshRenderer.material = _playerBodyNormal;
                    Lifes++;
                    IsCat = false;
                }

                else {
                    transform.localScale = new Vector3(0.75f, 0.5f, 0.75f);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!other.gameObject.CompareTag("Enemy")) return;

        if (Physics.Raycast(transform.position + _charCtrl.center + Vector3.up * _charCtrl.height / 2f, Vector3.down, out RaycastHit hit, 10f, _groundLayer))
        {
            if (hit.collider == other)
            {
                other.gameObject.GetComponent<EnemyController>().Death = true;
                other.gameObject.GetComponent<Animator>().SetTrigger("TR_Death");
                other.gameObject.GetComponent<CharacterController>().enabled = false;
            }
        }

        else isHit = true;
    }
}
