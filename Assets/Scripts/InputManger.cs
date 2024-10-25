using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(CameraController))]
public class InputManger : MonoBehaviour
{
    private GameManager gameManager;
    private CameraController camController;

    private bool leftMousePressed = false;
    private bool rightMousePressed = false;
    private Vector2 mousePosition = Vector2.zero;
    private Vector2 mouseDelta = Vector2.zero;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        camController = GetComponent<CameraController>();
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        leftMousePressed = context.ReadValueAsButton();
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        rightMousePressed = context.ReadValueAsButton();
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        if (rightMousePressed)
        {
            camController.ZoomCamera(-context.ReadValue<float>());
        }
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void OnMouseDelta(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();

        if (rightMousePressed)
        {
            camController.RotateCamera(mouseDelta.x, mouseDelta.y);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
