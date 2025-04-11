using UnityEngine;

public class GraphicSetterService : MonoBehaviour
{
    public void SetFpsTarget(int value) => Application.targetFrameRate = value;
}
