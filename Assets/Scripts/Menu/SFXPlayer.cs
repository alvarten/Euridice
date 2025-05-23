using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SFXPlayer : MonoBehaviour
{
    [Header("Fuente base de sonido")]
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip clickClip;
    public AudioClip lockClip;
    public AudioClip chestClip;
    public AudioClip doorClip;
    public AudioClip slideClip;
    public AudioClip pickClip;
    public AudioClip stepsClip;

    private Coroutine loopingCoroutine;

    private List<AudioSource> activeSources = new List<AudioSource>();

    // --- M�todos de reproducci�n p�blica ---
    public void PlayClick() => PlayOneShot(clickClip);
    public void PlayLock() => PlayOneShot(lockClip);
    public void PlayChest() => PlayFromTime(chestClip, 0.7f);
    public void PlayDoor() => PlayClipSegment(doorClip, 0.1f, 1.9f, 0.15f);
    public void PlaySlide() => PlayOneShot(slideClip);
    public void PlayPick() => PlayOneShot(pickClip);

    // --- Reproduce m�ltiples sonidos simult�neamente ---
    public void PlayOneShot(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        AudioSource source = CreateTempSource(volume);
        source.PlayOneShot(clip);
        StartCoroutine(DestroyAfter(source, clip.length));
    }

    public void PlayFromTime(AudioClip clip, float startTime, float volume = 1f)
    {
        if (clip == null) return;

        AudioSource source = CreateTempSource(volume);
        source.clip = clip;
        source.time = startTime;
        source.Play();
        StartCoroutine(DestroyAfter(source, clip.length - startTime));
    }

    public void PlayClipSegment(AudioClip clip, float startTime, float endTime, float volume = 1f)
    {
        if (clip == null || startTime >= clip.length || endTime <= startTime) return;

        AudioSource source = CreateTempSource(volume);
        source.clip = clip;
        source.time = startTime;
        source.Play();
        StartCoroutine(StopAt(source, endTime - startTime));
    }

    // --- Looping manual ---
    public void PlayLooping(AudioClip clip, float delayBetweenLoops, float volume)
    {
        StopLooping();
        loopingCoroutine = StartCoroutine(LoopClipWithDelay(clip, delayBetweenLoops, volume));
    }

    public void StopLooping()
    {
        if (loopingCoroutine != null)
        {
            StopCoroutine(loopingCoroutine);
            loopingCoroutine = null;
        }
    }

    // --- Internos ---
    private AudioSource CreateTempSource(float volume)
    {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.spatialBlend = sfxSource != null ? sfxSource.spatialBlend : 0f;
        newSource.volume = volume;
        newSource.playOnAwake = false;
        activeSources.Add(newSource);
        return newSource;
    }

    private IEnumerator DestroyAfter(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (source != null)
        {
            activeSources.Remove(source);
            Destroy(source);
        }
    }

    private IEnumerator StopAt(AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (source != null)
        {
            source.Stop();
            activeSources.Remove(source);
            Destroy(source);
        }
    }

    private IEnumerator LoopClipWithDelay(AudioClip clip, float delay, float volume)
    {
        while (true)
        {
            PlayOneShot(clip, volume);
            yield return new WaitForSeconds(clip.length + delay);
        }
    }
}
