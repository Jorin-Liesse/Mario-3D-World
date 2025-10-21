using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private AudioClip soundEffect;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySoundEffect()
    {
        audioSource.PlayOneShot(soundEffect);
    }
}
