using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(CameraController))]
public class InputManger : MonoBehaviour
{
    [SerializeField] private int UILayer = 5;
    [SerializeField] private float mouseDeltaSpeed = 0.1f;
    private GameManager gameManager;
    private CameraController camController;
    private CameraLookAtController camLookAtController;

    private bool rightMousePressed = false;
    private bool middleMousePressed = false;
    private Vector2 mouseDelta = Vector2.zero;

    private bool QPressed = false;
    private bool WPressed = false;
    private bool EPressed = false;
    private bool RPressed = false;
    private bool TPressed = false;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        camController = GetComponent<CameraController>();
        camLookAtController = GetComponent<CameraLookAtController>();
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (IsPointerOverUIElement()) return;

        if (context.performed)
        {
            gameManager.ClickStart();
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (IsPointerOverUIElement()) return;

        rightMousePressed = context.ReadValueAsButton();
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        if (IsPointerOverUIElement()) return;

        middleMousePressed = context.ReadValueAsButton();
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        if (IsPointerOverUIElement()) return;

        float scrollValue = context.ReadValue<float>();
        int scrollDirection = scrollValue > 0 ? 1 : -1;

        if (IsAnyKeyDown())
        {
            if (QPressed)
            {
                gameManager.RotateObjectX(scrollDirection);
            }
            else if (WPressed)
            {
                gameManager.RotateObjectY(scrollDirection);
            }
            else if (EPressed)
            {
                gameManager.RotateObjectZ(scrollDirection);
            }
            else if (RPressed)
            {
                gameManager.AddModelDistance(scrollDirection);
            }
            else if (TPressed)
            {
                gameManager.ScaleObject(scrollDirection);
            }
        }
        else
        {
            camController.ZoomCamera(-scrollDirection);
        }
    }

    private bool IsAnyKeyDown()
    {
        return QPressed || WPressed || EPressed || RPressed || TPressed;
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

        if (middleMousePressed)
        {
            camLookAtController.AddHeight(-mouseDelta.y * mouseDeltaSpeed);
        }
    }

    public void OnQ(InputAction.CallbackContext context)
    {
        QPressed = context.ReadValueAsButton();
    }

    public void OnW(InputAction.CallbackContext context)
    {
        WPressed = context.ReadValueAsButton();
    }

    public void OnE(InputAction.CallbackContext context)
    {
        EPressed = context.ReadValueAsButton();
    }

    public void OnR(InputAction.CallbackContext context)
    {
        RPressed = context.ReadValueAsButton();
    }

    public void OnT(InputAction.CallbackContext context)
    {
        TPressed = context.ReadValueAsButton();
    }

    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
            {
                return true;
            }
        }
        return false;
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
