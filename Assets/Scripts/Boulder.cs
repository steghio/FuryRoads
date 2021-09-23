using UnityEngine;

public class Boulder : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] AudioClip _audioClip;

    private void Awake()
    {
        _audioSource = GetComponentInParent<AudioSource>(); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            _audioSource.PlayOneShot(_audioClip);
        }
    }
}
