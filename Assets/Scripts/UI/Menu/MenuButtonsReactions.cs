using Global.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(GraphicSetterService), typeof(AudioSetterService), typeof(MenuAnimations))]
public class MenuButtonsReactions : MonoBehaviour
{
    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private TMP_Dropdown _fpsDropdown;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _backButton;
    private GraphicSetterService _graphicSetterService;
    private AudioSetterService _audioSetterService;
    private MenuAnimations _menuAnimations;
    private IStorageService _storageService;
    private PlayerSettingsService _playerSettingsService;
    
    [Inject]
    private void Construct(IStorageService storageService, PlayerSettingsService playerSettingsService)
    {
        _storageService = storageService;
        _playerSettingsService = playerSettingsService;
    }

    private void Start() => Init();

    private void Init()
    {
        _graphicSetterService = GetComponent<GraphicSetterService>();
        _audioSetterService = GetComponent<AudioSetterService>();
        _menuAnimations = GetComponent<MenuAnimations>();
        SubscribeUIElements();
        FirstLoadSettings();
    }

    private void SubscribeUIElements()
    {
        _musicToggle.onValueChanged.AddListener(OnMusicCheckBoxChanged);
        _musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        _fpsDropdown.onValueChanged.AddListener(OnFpsTargetDropdownChanged);
        _playButton.onClick.AddListener(PlayButtonPressed);
        _settingsButton.onClick.AddListener(SettingsButtonPressed);
        _backButton.onClick.AddListener(BackButtonPressed);
    }
    
    public void PlayButtonPressed()
    {
        SceneManager.LoadSceneAsync("Garage");
    }

    public void SettingsButtonPressed() => _menuAnimations.ShowSettingsCanvas();
    
    public void BackButtonPressed() => _menuAnimations.ShowMainCanvas();
    
    public void SaveSettings()
    {
        _playerSettingsService.SaveSettings(_musicToggle.isOn, _musicSlider.value, _fpsDropdown.value);
    }
    
    public void FirstLoadSettings()
    {
        SettingsData settingsData = _playerSettingsService.LoadSettings();

        _musicToggle.isOn = settingsData.isMusicEnabled;
        OnMusicCheckBoxChanged(settingsData.isMusicEnabled);
        
        _musicSlider.value = settingsData.MusicVolume;
        OnMusicSliderChanged(settingsData.MusicVolume);
        
        _fpsDropdown.value = settingsData.TargetFPS; 
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
        _graphicSetterService.SetFpsTarget(value);
        SaveSettings();
    }
}
