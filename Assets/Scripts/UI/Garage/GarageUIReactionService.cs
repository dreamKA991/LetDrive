using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GarageUIReactionService : MonoBehaviour
{
    // CAR INFO 
    [SerializeField] private Slider _speedSlider;
    [SerializeField] private TMP_Text _speedText;
    
    [SerializeField] private Slider _handlingSlider;
    [SerializeField] private TMP_Text _handlingText;
    
    [SerializeField] private Slider _brakingSlider;
    [SerializeField] private TMP_Text _brakingText;
    
    // UI
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _backMenuButton;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _selectNextCarArrowButton;
    [SerializeField] private Button _selectPreviuosCarArrowButton;

    private void Start()
    {
        SubscribeButtons();
    }

    private void SubscribeButtons()
    {
        _playButton.onClick.AddListener(PlayButtonPressed);
        _backMenuButton.onClick.AddListener(BackButtonPressed);
        _selectNextCarArrowButton.onClick.AddListener(NextCarButtonPressed);
        _selectPreviuosCarArrowButton.onClick.AddListener(PreviousCarButtonPressed);
        _buyButton.onClick.AddListener(BuyButtonPressed);
    }

    public void NextCarButtonPressed()
    {
        
    }
    
    public void PreviousCarButtonPressed() {}

    public void BuyButtonPressed() {}
    
    public void PlayButtonPressed()
    {
        SceneManager.LoadSceneAsync("GamePlay");
    }
    
    public void BackButtonPressed()
    {
        SceneManager.LoadSceneAsync("Menu");
    }
    
    public void SetSpeedUI(float value)
    {
        _speedSlider.value = value;
        _speedText.text = value.ToString();
    }

    public void SetHandlingUI(float value)
    {
        _handlingSlider.value = value;
        _handlingText.text = value.ToString();
    }

    public void SetBrakingUI(float value)
    {
        _brakingSlider.value = value;
        _brakingText.text = value.ToString();
    }
}
