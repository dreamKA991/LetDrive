using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GarageUIReactionService : MonoBehaviour
{
    // PLAYER DATA
    [SerializeField] private TMP_Text _moneyText;
    
    // CAR INFO 
    [SerializeField] private Slider _speedSlider;
    [SerializeField] private TMP_Text _speedText;
    [SerializeField] private TMP_Text _priceCarText;
    
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

    private ShowCarCommand _showCarCommand;
    
    [Inject]
    private void Construct(ShowCarCommand showCarCommand)
    {
        _showCarCommand = showCarCommand;
    }
    
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

    public void NextCarButtonPressed() => _showCarCommand.ShowNextCar();
    
    public void PreviousCarButtonPressed() => _showCarCommand.ShowPreviousCar();

    public void BuyButtonPressed() => _showCarCommand.TryBuyCar();

    public void SetActiveBuyButtonAndPriceText(bool active)
    {
        _priceCarText.gameObject.SetActive(active);
        _buyButton.gameObject.SetActive(active);
    }

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

    public void SetMoneyText(int value)
    {
        _moneyText.text = value.ToString();
    }

    public void SetCarPriceText(float value)
    {
        _priceCarText.text = "COST: " + value.ToString();
    }
}
