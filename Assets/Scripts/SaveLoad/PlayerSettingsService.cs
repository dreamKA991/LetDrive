using Global.SaveLoad;
using Zenject;

public class PlayerSettingsService
{
    private IStorageService _storageService;
    private AudioSetterService _audioSetterService;
    private GraphicSetterService _graphicSetterService;

    [Inject]
    private void Construct(IStorageService storageService, AudioSetterService audioSetterService, GraphicSetterService graphicSetterService)
    {
        _storageService = storageService;
        _audioSetterService = audioSetterService;
        _graphicSetterService = graphicSetterService;
    }
    
    public void SaveSettings(bool isMusicEnabled, float musicVolume, int targetFps)
    {
        SettingsData newSettingsData = new SettingsData();
        newSettingsData.isMusicEnabled = isMusicEnabled;
        newSettingsData.MusicVolume = musicVolume;
        newSettingsData.TargetFPS = targetFps;
        _storageService.Save(ProjectConstantKeys.SETTINGSDATA, newSettingsData);
    }

    public void LoadAndApplySettings()
    {
        SettingsData settingsData = LoadSettings();
        _graphicSetterService.SetFpsTarget(settingsData.TargetFPS);
        _audioSetterService.SetMusicVolume(settingsData.MusicVolume);
        _audioSetterService.SetMusicToggle(settingsData.isMusicEnabled);
    }

    public SettingsData LoadSettings()
    {
        SettingsData settingsData = _storageService.Load<SettingsData>(ProjectConstantKeys.SETTINGSDATA);
        if (settingsData == null)
        {
            SettingsData defaultSettings = new SettingsData
            {
                isMusicEnabled = true,
                MusicVolume = 1f,
                TargetFPS = 0 // 0 - 60 fps, 1 - 30 fps
            };
            _storageService.Save(ProjectConstantKeys.SETTINGSDATA, defaultSettings);
            return defaultSettings;
        } else return settingsData;
    }
}
