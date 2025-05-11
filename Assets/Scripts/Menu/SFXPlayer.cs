using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class SFXPlayer : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip clickClip;
    public AudioClip lockClip;
    public AudioClip chestClip;
    public AudioClip doorClip;
    public AudioClip slideClip;
    public AudioClip pickClip;
    public AudioClip stepsClip;

    private Coroutine loopingCoroutine;
    public void PlayClick()
    {
        sfxSource.PlayOneShot(clickClip);
    }
    public void PlayLock()
    {
        sfxSource.PlayOneShot(lockClip);
    }

    public void PlayChest()
    {
        PlayFromTime(chestClip, 0.7f);
        //sfxSource.PlayOneShot(chestClip);
    }
    public void PlayDoor()
    {
        PlayClipSegment(doorClip, 0.1f,2.0f ,0.15f);
    }
    public void PlaySlide()
    {
        sfxSource.PlayOneShot(slideClip);
    }
    public void PlayPick()
    {
        sfxSource.PlayOneShot(pickClip);
    }
    public void PlayFromTime(AudioClip clip, float startTime)
    {
        sfxSource.clip = clip;
        sfxSource.time = startTime;
        sfxSource.Play();
    }

    // Método para reproducir desde un punto hasta otro con volumen personalizado
    public void PlayClipSegment(AudioClip clip, float startTime, float endTime, float volume = 1f)
    {
        if (clip == null || startTime >= clip.length || endTime <= startTime)
        {
            Debug.LogWarning("Parámetros inválidos para reproducir el clip.");
            return;
        }

        StartCoroutine(PlayClipSegmentCoroutine(clip, startTime, endTime, volume));
    }
    private IEnumerator PlayClipSegmentCoroutine(AudioClip clip, float startTime, float endTime, float volume)
    {
        float originalVolume = sfxSource.volume;

        sfxSource.clip = clip;
        sfxSource.time = startTime;
        sfxSource.volume = volume;
        sfxSource.Play();

        yield return new WaitForSeconds(endTime - startTime);

        sfxSource.Stop();
        sfxSource.volume = originalVolume; // Restaurar volumen original
    }
    
    public void PlayLooping(AudioClip clip, float delayBetweenLoops, float volume)
    {
        StopLooping(); // detener si ya se está reproduciendo algo
        loopingCoroutine = StartCoroutine(LoopClipWithDelay(clip, delayBetweenLoops, volume));
    }

    public void StopLooping()
    {
        if (loopingCoroutine != null)
        {
            StopCoroutine(loopingCoroutine);
            loopingCoroutine = null;
        }
        sfxSource.Stop();
        sfxSource.volume = 1f;
    }

    private IEnumerator LoopClipWithDelay(AudioClip clip, float delay, float volume)
    {
        while (true)
        {
            sfxSource.PlayOneShot(clip, volume);
            yield return new WaitForSeconds(clip.length + delay);
        }
    }

}
