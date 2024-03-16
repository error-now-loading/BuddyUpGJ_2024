using UnityEngine;

public class CyclicMovement : MonoBehaviour
{
    public float size = 10f;
    public float ratio = 0.5f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        float cameraX = mainCamera.transform.position.x;
        float movement = (cameraX % size) * ratio;
        if (movement > size / 2)
        {
            movement -= size;
        }
        else if (movement < -size / 2)
        {
            movement += size;
        }
        Vector3 newPosition = transform.position;
        newPosition.x = movement;
        transform.position = newPosition;
    }
}
