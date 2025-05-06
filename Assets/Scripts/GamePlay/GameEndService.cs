using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class GameEndService : MonoBehaviour
{
    [SerializeField] private Button _endGameButton;
    [SerializeField] private GameObject _endGameCanvas;
    [SerializeField] private TMP_Text _rewardText;
    private PlayerDataService _playerDataService;
    private int _reward = 789;
    
    [Inject]
    private void Construct(PlayerDataService playerDataService)
    {
        _playerDataService = playerDataService;
    }
    
    private void Start()
    {
        _endGameCanvas.gameObject.SetActive(false);
        SignalBus.onCharacterLose.AddListener(GameEnd);
        _endGameButton.onClick.AddListener(OnButtonPressed);
    }

    private void GameEnd()
    {
        _endGameCanvas.gameObject.SetActive(true);
        _rewardText.text = _reward.ToString();
        _playerDataService.AddMoney(_reward);
        Time.timeScale = 0;
    }

    private void OnButtonPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("Garage");
    }
    
}
