using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    public GameObject postFX;
    public GameObject pauseMenu;
    public GameObject pauseMainMenu;
    public GameObject settingsMenu;
    public GameObject gameGUI;
    public RawImage crosshair;
    public TextMeshProUGUI textMasterVolume;
    public Slider sliderMasterVolume;
    public TextMeshProUGUI textMusicVolume;
    public Slider sliderMusicVolume;
    public TextMeshProUGUI textSFXVolume;
    public Slider sliderSFXVolume;
    public TextMeshProUGUI textAmbienceVolume;
    public Slider sliderAmbienceVolume;
    public TextMeshProUGUI textSensitivity;
    public Slider sliderSensitivity;
    public TextMeshProUGUI textBrightness;
    public Slider sliderBrightness;

    RectTransform crosshairRect;
    public List<Texture> crosshairTextures;
    public static event Action OnPause;
    public static event Action OnUnpause;

    public static event Action<float> OnMasterSliderChanged;
    public static event Action<float> OnMusicSliderChanged;
    public static event Action<float> OnSFXSliderChanged;
    public static event Action<float> OnAmbienceSliderChanged;
    public static event Action<float> OnSensitivitySliderChanged;
    public static event Action<float> OnBrightnessSliderChanged;
    public static event Action OnButtonPressed;

    bool isPaused = false;
    public Animator transition;
    public float transitionTime = 1f;

    private void Awake()
    {
        if (crosshair != null)
        {
            crosshairRect = crosshair.GetComponent<RectTransform>();
            crosshairRect.sizeDelta = new Vector2(crosshairRect.sizeDelta.x, 128);
        }
    }

    private void OnEnable()
    {
        InteractWithObject.OnHoverInteractable += ChangeCrosshairToInteract;
        InteractWithObject.OnOutsideInteractable += ChangeCrosshairToDefault;
    }

    private void OnDisable()
    {
        InteractWithObject.OnHoverInteractable -= ChangeCrosshairToInteract;
        InteractWithObject.OnOutsideInteractable -= ChangeCrosshairToDefault;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                OnPause?.Invoke();
                HandlePause();
            }
            else
            {
                OnUnpause?.Invoke();
                HandleUnpause();
            }
        }
    }

    public void LoadScene(int index)
    {
        StartCoroutine(LoadLevel(index));
    }

    IEnumerator LoadLevel(int index)
    {
        if (transition != null)
            transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(index);
    }

    void ChangeCrosshairToInteract()
    {
        crosshairRect.sizeDelta = new Vector2(crosshairRect.sizeDelta.x, 256);
        crosshairRect.localScale = new Vector2(0.25f, 0.25f);
        crosshair.GetComponent<RawImage>().texture = crosshairTextures[(int)CrosshairType.INTERACT];
    }

    void ChangeCrosshairToDefault()
    {
        crosshairRect.sizeDelta = new Vector2(crosshairRect.sizeDelta.x, 128);
        crosshairRect.localScale = new Vector2(0.50f, 0.50f);
        crosshair.GetComponent<RawImage>().texture = crosshairTextures[(int)CrosshairType.DEFAULT];
    }

    public void OpenSettings()
    {
        OnButtonPressed?.Invoke();
        pauseMainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        isPaused = !isPaused;
        OnButtonPressed?.Invoke();
        OnUnpause?.Invoke();
        HandleUnpause();
    }

    public void CloseSettingsGoBack()
    {
        OnButtonPressed?.Invoke();
        pauseMainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    void HandlePause()
    {
        OnButtonPressed?.Invoke();
        pauseMenu.SetActive(true);
        pauseMainMenu.SetActive(true);
        if (gameGUI != null)
            gameGUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    void HandleUnpause()
    {
        OnButtonPressed?.Invoke();
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        if (gameGUI != null)
            gameGUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MasterVolumeChanged()
    {
        OnMasterSliderChanged?.Invoke(sliderMasterVolume.value);
        textMasterVolume.text = Mathf.Round(Mathf.Clamp(100 * sliderMasterVolume.value, 0f, 100f)).ToString();
    }

    public void MusicVolumeChanged()
    {
        OnMusicSliderChanged?.Invoke(sliderMusicVolume.value);
        textMusicVolume.text = Mathf.Round(Mathf.Clamp(100 * sliderMusicVolume.value, 0f, 100f)).ToString();
    }

    public void SFXVolumeChanged()
    {
        OnSFXSliderChanged?.Invoke(sliderSFXVolume.value);
        textSFXVolume.text = Mathf.Round(Mathf.Clamp(100 * sliderSFXVolume.value, 0f, 100f)).ToString();
    }

    public void AmbienceVolumeChanged()
    {
        OnAmbienceSliderChanged?.Invoke(sliderAmbienceVolume.value);
        textAmbienceVolume.text = Mathf.Round(Mathf.Clamp(100 * sliderAmbienceVolume.value, 0f, 100f)).ToString();
    }

    public void SensitivityChanged()
    {
        OnSensitivitySliderChanged?.Invoke(sliderSensitivity.value);
        textSensitivity.text = Math.Round(sliderSensitivity.value, 2).ToString();
    }

    public void BrightnessChanged()
    {
        OnBrightnessSliderChanged?.Invoke(sliderBrightness.value);
        textBrightness.text = sliderBrightness.value.ToString();
        float brightnessValue = Mathf.Round(((sliderBrightness.value / 100f) * 3.5f) * 2f) / 2f;
        PostProcessVolume volume = postFX.GetComponent<PostProcessVolume>();
        if (volume != null && volume.profile.TryGetSettings(out AutoExposure autoExposure))
            autoExposure.keyValue.value = brightnessValue;
    }
}

public enum CrosshairType
{
    INTERACT,
    DEFAULT,
}
