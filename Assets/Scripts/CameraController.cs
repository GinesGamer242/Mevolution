using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static UnityEngine.InputSystem.InputAction;
using System.Collections.Generic;
using System.Linq;

public class CameraController : MonoBehaviour
{
    [Serializable]
    public struct ZoomRange
    {
        public float minZoomValue;
        public float maxZoomValue;
    }

    Vector3 originalMousePosition;
    Vector3 newMousePosition;
    Vector3 originalCameraPosition;
    float zoomValue;
    bool isDraggingCamera;

    [Header ("References")]
    [SerializeField]
    Camera usedCamera;

    [Header("Attributes")]
    [SerializeField]
    float cameraMoveSpeed;
    [SerializeField]
    ZoomRange zoomRange;


    private void Start()
    {
        isDraggingCamera = false;

        if (usedCamera == null)
            usedCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        // MOVE CAMERA ACCORDINGLY
        if (isDraggingCamera)
        {
            newMousePosition = Input.mousePosition;
            Vector3 cameraOffset = new Vector3((newMousePosition.x - originalMousePosition.x), (newMousePosition.y - originalMousePosition.y), 0f) * cameraMoveSpeed;

            usedCamera.transform.position = (originalCameraPosition - cameraOffset);
        }

        // ZOOM ACCORDINGLY
        float newZoomValue = (usedCamera.orthographicSize += zoomValue);
        newZoomValue = Mathf.Clamp(newZoomValue, zoomRange.minZoomValue, zoomRange.maxZoomValue);

        usedCamera.orthographicSize = newZoomValue;
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

    public void OnZoomCamera(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<float>();

        zoomValue = -scrollValue;
    }
}
