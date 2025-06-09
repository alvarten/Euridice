using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio; // Asegúrate de incluir esto

public class SFXPlayer : MonoBehaviour
{
    [Header("Fuente base de sonido")]
    public AudioSource sfxSource;

    [Header("Grupo de mezcla")]
    public AudioMixerGroup sfxMixerGroup;

    [Header("Clips")]
    public AudioClip clickClip;
    public AudioClip lockClip;
    public AudioClip chestClip;
    public AudioClip doorClip;
    public AudioClip slideClip;
    public AudioClip pickClip;
    public AudioClip stepsClip;
    public AudioClip bubleClip;
    public AudioClip pageClip;
    public AudioClip errorClip;
    public AudioClip waterClip;

    [Header("Teclas piano")]
    public AudioClip doPianoClip;
    public AudioClip rePianoClip;
    public AudioClip miPianoClip;
    public AudioClip faPianoClip;
    public AudioClip solPianoClip;
    public AudioClip laPianoClip;
    public AudioClip siPianoClip;

    [Header("Creepy")]
    public AudioClip walkIntroClip;
    public AudioClip chokeClip;
    public AudioClip laugh1Clip;
    public AudioClip laugh2Clip;
    public AudioClip laugh3Clip;
    public AudioClip laugh4Clip;
    public AudioClip errorMen1Clip;
    public AudioClip errorMen2Clip;
    public AudioClip errorMen3Clip;

    private Coroutine loopingCoroutine;
    private List<AudioSource> activeSources = new List<AudioSource>();

    [Header("Referencia al Controlador de Eventos")]
    public ControladorEventos controladorEventos; 


    // --- Métodos de reproducción pública ---
    public void PlayClick() => PlayOneShot(clickClip);
    public void PlayLock() => PlayOneShot(lockClip);
    public void PlayChest() => PlayFromTime(chestClip, 0.7f);
    public void PlayDoor() => PlayClipSegment(doorClip, 0.1f, 1.9f, 0.15f);
    public void PlaySlide() => PlayOneShot(slideClip);
    public void PlayPick() => PlayOneShot(pickClip, 0.5f);
    public void PlayBuble() => PlayOneShot(bubleClip, 0.5f);
    public void PlayPage() => PlayOneShot(pageClip, 1.3f);
    public void PlayWater() => PlayClipSegment(waterClip, 0.1f, 3f);
    public void PlayIntroGuardian() => PlayOneShot(walkIntroClip);
    public void PlayChoke() => PlayWithDelayBefore(chokeClip, 4.55f, 2f, 4.1f, 2.8f);
    public void PlayError()
    {
        if (controladorEventos != null && controladorEventos.tiempoTranscurrido >= 260f)
        {
            // 1/3 de probabilidad de reproducir el clip normal, 2/3 de reproducir un errorMen
            float probabilidad = Random.value;

            if (probabilidad <= 1f / 3f)
            {
                PlayOneShot(errorClip, 0.5f);
            }
            else
            {
                PlayErrorMen();  
            }
        }
        else
        {
            // Si el tiempo es menor a 260, siempre reproduce el clip normal
            PlayOneShot(errorClip, 0.5f);
        }
    }

    // Reproduce un error aleatorio de "errorMen"
    public void PlayErrorMen()
    {
        AudioClip[] errorClips = new AudioClip[] { errorMen1Clip, errorMen2Clip, errorMen3Clip };
        AudioClip randomError = GetRandomClip(errorClips);
        PlayOneShot(randomError, 1f);
    }

    // Reproduce una risa aleatoria
    public void PlayLaugh()
    {
        AudioClip[] laughClips = new AudioClip[] { laugh1Clip, laugh2Clip, laugh3Clip, laugh4Clip };
        AudioClip randomLaugh = GetRandomClip(laughClips);
        PlayOneShot(randomLaugh, 1f);
    }
    public void PlayPianoNote(string noteCode)
    {
        AudioClip noteClip = null;

        switch (noteCode.ToUpper())
        {
            case "DO":
                noteClip = doPianoClip;
                break;
            case "RE":
                noteClip = rePianoClip;
                break;
            case "MI":
                noteClip = miPianoClip;
                break;
            case "FA":
                noteClip = faPianoClip;
                break;
            case "SOL":
                noteClip = solPianoClip;
                break;
            case "LA":
                noteClip = laPianoClip;
                break;
            case "SI":
                noteClip = siPianoClip;
                break;
            default:
                Debug.LogWarning($"Nota de piano desconocida: {noteCode}");
                return;
        }
        PlayOneShot(noteClip, 0.2f);
    }


    // --- Reproduce múltiples sonidos simultáneamente ---
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

        // Aquí se asigna el grupo de mezcla
        if (sfxMixerGroup != null)
            newSource.outputAudioMixerGroup = sfxMixerGroup;

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

    // Método de utilidad para obtener un clip aleatorio de un array
    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return null;

        int index = Random.Range(0, clips.Length);
        return clips[index];
    }

    public void PlayWithDelayBefore(AudioClip clip, float delay, float volume, float startTime, float cutAfterSeconds)
    {
        if (clip == null || startTime < 0 || startTime >= clip.length || cutAfterSeconds <= 0 || (startTime + cutAfterSeconds) > clip.length)
        {
            Debug.LogWarning("Parámetros inválidos para PlayWithDelayBefore.");
            return;
        }

        StartCoroutine(PlayWithDelayCoroutine(clip, delay, volume, startTime, cutAfterSeconds));
    }
    private IEnumerator PlayWithDelayCoroutine(AudioClip clip, float delay, float volume, float startTime, float cutAfterSeconds)
    {
        yield return new WaitForSeconds(delay);

        AudioSource source = CreateTempSource(volume);
        source.clip = clip;
        source.time = startTime;
        source.Play();

        yield return new WaitForSeconds(cutAfterSeconds);
        if (source != null)
        {
            source.Stop();
            activeSources.Remove(source);
            Destroy(source);
        }
    }
}
