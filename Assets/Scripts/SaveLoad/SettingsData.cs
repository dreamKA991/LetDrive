using System;

namespace Global.SaveLoad
{
    [Serializable]
    public class SettingsData
    {
        public bool isMusicEnabled;
        public float MusicVolume;
        public int TargetFPS;
    }
}