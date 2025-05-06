using UnityEngine;
using UnityEngine.Events;

public class SignalBus : MonoBehaviour
{
    public static UnityEvent onCharacterLose = new UnityEvent();
}
