using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip clickClip;

    public void PlayClick()
    {
        sfxSource.PlayOneShot(clickClip);
    }
}
