using UnityEngine;

public class GraphicSetterService : MonoBehaviour
{
    private const int DEFAULT_FPS = 60;
    public void SetFpsTarget(int value)
    {
        int newFpsValue = DEFAULT_FPS;
        switch (value)
        {
            case 0:
                newFpsValue = 60;
                break;
            case 1:
                newFpsValue = 30;
                break;
        }
        
        Application.targetFrameRate = newFpsValue;
    }
}
