using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonsReactions : MonoBehaviour
{
    public void PlayButtonPressed()
    {
        SceneManager.LoadSceneAsync("Garage");
    }

    public void SettingsButtonPressed()
    {
        
    }
}
