using UnityEngine;

public class PlayerUIProviderService : MonoBehaviour
{
    [SerializeField] private GameObject throttleButton;
    [SerializeField] private GameObject reverseButton;
    [SerializeField] private GameObject turnRightButton;
    [SerializeField] private GameObject turnLeftButton;

    public void ApplyGUI(PrometeoCarController prometeoCarController)
    {
        prometeoCarController.throttleButton = throttleButton;
        prometeoCarController.reverseButton = reverseButton;
        prometeoCarController.turnRightButton = turnRightButton;
        prometeoCarController.turnLeftButton = turnLeftButton;
    }
}
