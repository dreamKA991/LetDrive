using Global.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(GraphicSetterService), typeof(AudioSetterService))]
public class MenuButtonsReactions : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TMP_Dropdown fpsDropdown;
    private GraphicSetterService _graphicSetterService;
    private AudioSetterService _audioSetterService;
    private IStorageService _storageService;
    [Inject]
    private void Construct(IStorageService storageService)
    {
        _storageService = storageService;
    }

    private void Start() => Init();

    private void Init()
    {
        _graphicSetterService = GetComponent<GraphicSetterService>();
        _audioSetterService = GetComponent<AudioSetterService>();
        SubscribeUIElements();
        LoadSettings();
    }

    private void SubscribeUIElements()
    {
        musicToggle.onValueChanged.AddListener(OnMusicCheckBoxChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        fpsDropdown.onValueChanged.AddListener(OnFpsTargetDropdownChanged);
    }
    
    public void PlayButtonPressed()
    {
        SceneManager.LoadSceneAsync("Garage");
    }

    public void SettingsButtonPressed()
    {
        
    }

    public void SaveSettings()
    {
        SettingsData newSettingsData = new SettingsData();
        newSettingsData.isMusicEnabled = musicToggle.isOn;
        newSettingsData.MusicVolume = musicSlider.value;
        newSettingsData.TargetFPS = fpsDropdown.value;
        _storageService.Save(ProjectConstantKeys.SETTINGSDATA, newSettingsData);
    }

    public void LoadSettings()
    {
        SettingsData settingsData = _storageService.Load<SettingsData>(ProjectConstantKeys.SETTINGSDATA);
        if (settingsData == null)
        {
            SaveSettings();
            settingsData = _storageService.Load<SettingsData>(ProjectConstantKeys.SETTINGSDATA);
        }

        musicToggle.isOn = settingsData.isMusicEnabled;
        OnMusicCheckBoxChanged(settingsData.isMusicEnabled);
        
        musicSlider.value = settingsData.MusicVolume;
        OnMusicSliderChanged(settingsData.MusicVolume);
        
        fpsDropdown.value = settingsData.TargetFPS; 
        OnFpsTargetDropdownChanged(settingsData.TargetFPS);
    }
    
    private void OnMusicCheckBoxChanged(bool value)
    {
        Debug.Log("OnMusicCheckBoxChanged: " + value);
        _audioSetterService.SetMusicToggle(value);
        SaveSettings();
    }
    
    private void OnMusicSliderChanged(float value)
    {
        Debug.Log("OnMusicSliderChanged: " + value);
        _audioSetterService.SetMusicVolume(value);
        SaveSettings();
    }

    private void OnFpsTargetDropdownChanged(int value)
    {
        Debug.Log("OnFpsTargetDropdownChanged: " + value);
        int fpsValue = int.Parse(fpsDropdown.options[fpsDropdown.value].text);
        _graphicSetterService.SetFpsTarget(fpsValue);
        SaveSettings();
    }
}
