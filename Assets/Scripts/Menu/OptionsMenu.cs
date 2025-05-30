using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using System.Collections.Generic;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject mainMenuPanel;
    public GameObject controlsPanel;
    public GameObject mutedIcon; //el icono cuando se mutee el volumen master
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixer audioMixer;

    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    private List<Vector2Int> allowedResolutions;

    void Start()
    {
        //IsVolumeMuted();

        // Volumen de los distintos canales
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;

        SetMasterVolume(master);
        SetMusicVolume(music);
        SetSFXVolume(sfx);

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        // Ajustable de pantalla completa
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;
        SetFullscreen(isFullscreen);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        // Resoluciones 16:9 específicas
        allowedResolutions = new List<Vector2Int>
        {
            new Vector2Int(3840, 2160), // 4K
            new Vector2Int(2560, 1440), // 2K
            new Vector2Int(1920, 1080), // Full HD
            new Vector2Int(1280, 720)   // HD
        };

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < allowedResolutions.Count; i++)
        {
            Vector2Int res = allowedResolutions[i];
            string option = res.x + " x " + res.y;
            options.Add(option);

            if (Screen.currentResolution.width == res.x && Screen.currentResolution.height == res.y)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        savedIndex = Mathf.Clamp(savedIndex, 0, allowedResolutions.Count - 1);

        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();
        SetResolution(savedIndex);

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void OpenControls()
    {
        controlsPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void CloseControls()
    {
        controlsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void SetResolution(int index)
    {
        if (index < 0 || index >= allowedResolutions.Count) return;

        Vector2Int res = allowedResolutions[index];
        Screen.SetResolution(res.x, res.y, Screen.fullScreenMode);
        PlayerPrefs.SetInt("ResolutionIndex", index);
    }
    
    /*
    public void IsVolumeMuted()
    {
        if (masterSlider.value == 0) //lo he intentado con el valor del slider pero no me lo cogia, no se como ponerlo con la musica
        {
            mutedIcon.SetActive(true);
        }
        else
        {
            mutedIcon.SetActive(false);
        }

    }*/
}
