using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform target; 
    [SerializeField] private float rotationSpeed = 0.2f; 
    [SerializeField] private float minXRotation = 5f; 
    [SerializeField] private float maxXRotation = 60f; 
    [SerializeField] private float distance = 5f; 

    private Vector2 lastTouchPosition;
    private float currentXRotation = 0f;
    private float currentYRotation = 0f;
    private bool isDragging = false;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        currentXRotation = angles.x;
        currentYRotation = angles.y;
        
        Quaternion rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);
        Vector3 direction = rotation * new Vector3(0, 0, -distance);
        transform.position = target.position + direction;
        transform.LookAt(target);
    }


    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                float deltaX = touch.position.x - lastTouchPosition.x;
                float deltaY = touch.position.y - lastTouchPosition.y;

                currentYRotation += deltaX * rotationSpeed;
                currentXRotation -= deltaY * rotationSpeed;
                currentXRotation = Mathf.Clamp(currentXRotation, minXRotation, maxXRotation);

                Quaternion rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);
                Vector3 direction = rotation * new Vector3(0, 0, -distance);
                transform.position = target.position + direction;
                transform.LookAt(target);

                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
    }
}