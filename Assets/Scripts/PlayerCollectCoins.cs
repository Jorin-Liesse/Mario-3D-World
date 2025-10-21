using UnityEngine;
using UnityEngine.UI;

public class PlayerCollectCoins : MonoBehaviour
{
    [SerializeField] private Text _scoreText = null;

    [SerializeField] private PlaySound _coinSound = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            _scoreText.text = (int.Parse(_scoreText.text) + 1).ToString();
            Destroy(other.gameObject);

            _coinSound.PlaySoundEffect();
        }
    }
}
