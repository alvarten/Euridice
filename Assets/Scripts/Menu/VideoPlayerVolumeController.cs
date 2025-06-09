using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoPlayerVolumeController : MonoBehaviour
{
    [Header("Mixer de audio")]
    public AudioMixerGroup musicMixerGroup;

    [Header("Índice de pista de audio (por defecto 0)")]
    public ushort audioTrackIndex = 0;

    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // Obtener volumen en decibelios desde el AudioMixer
        if (musicMixerGroup.audioMixer.GetFloat("MusicVolume", out float volumeDB))
        {
            // Convertir de decibelios a lineal (0–1)
            float linearVolume = Mathf.Pow(10f, volumeDB / 20f);

            // Aplicar volumen directamente al VideoPlayer
            videoPlayer.SetDirectAudioVolume(audioTrackIndex, linearVolume);
        }
        else
        {
            Debug.LogWarning("No se pudo leer 'MusicVolume' del AudioMixer.");
        }
    }
}
