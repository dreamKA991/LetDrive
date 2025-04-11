using Unity.VisualScripting;
using UnityEngine;

public class CharacterAILogic : MonoBehaviour
{
    private ICharacterCar _characterCarController;
    private SphereCollider _sphereCollider;
    private PrometeoCarController _botCarController;

    private void Start()
    {
        _characterCarController = GetComponent<ICharacterCar>();
        CreateTrigger();
    }

    private void CreateTrigger()
    {
        _sphereCollider = this.AddComponent<SphereCollider>();
        _sphereCollider.radius = 1f;
        _sphereCollider.isTrigger = true;
        _sphereCollider.center = new Vector3(0f, 1.2f, 20f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PrometeoCarController>(out PrometeoCarController _botCarController))
        {
            _characterCarController.ChangeLine();
        }
    }
}
