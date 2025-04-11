using UnityEngine;
using UnityEngine.Audio;

public class AudioSetterService : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    private float _musicVolume;

    public void SetMusicToggle(bool value)
    {
        float newVolume = value ? _musicVolume : -80f;
        SetVolume(newVolume, "Music");
    }

    public void SetMusicVolume(float value)
    {
        float newVolume = Mathf.Lerp(-80f, 0f, value);
        SetVolume(newVolume, "Music");
        _musicVolume = newVolume;
    }

    
    private void SetVolume(float value, string name) => 
        _audioMixerGroup.audioMixer.SetFloat(name, value);
}
