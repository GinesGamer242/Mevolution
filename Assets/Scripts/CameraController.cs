using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static UnityEngine.InputSystem.InputAction;

public class CameraController : MonoBehaviour
{
    Vector3 originalMousePosition;
    Vector3 newMousePosition;
    Vector3 originalCameraPosition;
    bool isDraggingCamera;

    [Header ("References")]
    [SerializeField]
    Camera usedCamera;

    [Header("Attributes")]
    [SerializeField]
    float cameraMoveSpeed;

    private void Start()
    {
        isDraggingCamera = false;

        if (usedCamera == null)
            usedCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        if (isDraggingCamera)
        {
            newMousePosition = Input.mousePosition;
            Vector3 cameraOffset = new Vector3((newMousePosition.x - originalMousePosition.x), (newMousePosition.y - originalMousePosition.y), 0f) * cameraMoveSpeed;

            usedCamera.transform.position = (originalCameraPosition - cameraOffset);
        }
    }

    public void OnDraggingCamera(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            originalMousePosition = Input.mousePosition;
            originalCameraPosition = usedCamera.transform.position;
            isDraggingCamera = true;
        }
        else
        {
            originalMousePosition = Vector3.zero;
            newMousePosition = Vector3.zero;
            isDraggingCamera = false;
        }
    }
}
