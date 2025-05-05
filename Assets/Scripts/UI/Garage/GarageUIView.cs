using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GarageUIView : MonoBehaviour
{
    // PLAYER DATA
    [SerializeField] private TMP_Text _moneyText;

    // CAR INFO 
    [SerializeField] private Slider _speedSlider;
    [SerializeField] private TMP_Text _speedText;
    [SerializeField] private TMP_Text _priceCarText;
    [SerializeField] private Button _speedUpgradeButton;

    [SerializeField] private Slider _handlingSlider;
    [SerializeField] private TMP_Text _handlingText;
    [SerializeField] private Button _handlingUpgradeButton;

    [SerializeField] private Slider _brakingSlider;
    [SerializeField] private TMP_Text _brakingText;
    [SerializeField] private Button _brakingUpgradeButton;

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
        _speedUpgradeButton.onClick.AddListener(SpeedUpgradeButtonPressed);
        _handlingUpgradeButton.onClick.AddListener(HandlingUpgradeButtonPressed);
        _brakingUpgradeButton.onClick.AddListener(BrakingUpgradeButtonPressed);
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

    public void SpeedUpgradeButtonPressed()
    {
        _showCarCommand.TryBuySpeedUpgrade();
    }

    public void HandlingUpgradeButtonPressed()
    {
        _showCarCommand.TryBuyHandlingUpgrade();
    }

    public void BrakingUpgradeButtonPressed()
    {
        _showCarCommand.TryBuyBrakeUpgrade();
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

    public void SetUpgradeButtonsActive(bool isActive)
    {
        SetSpeedUpgradeButtonActive(isActive);
        SetHandlingUpgradeButtonActive(isActive);
        SetBrakingUpgradeButtonActive(isActive);
    }
 
    public void SetSpeedUpgradeButtonActive(bool isActive)
    {
        _speedUpgradeButton.gameObject.SetActive(isActive);
    }

    public void SetHandlingUpgradeButtonActive(bool isActive)
    {
        _handlingUpgradeButton.gameObject.SetActive(isActive);
    }

    public void SetBrakingUpgradeButtonActive(bool isActive)
    {
        _brakingUpgradeButton.gameObject.SetActive(isActive);
    }

}
