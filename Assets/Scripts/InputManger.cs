using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(CameraController))]
[RequireComponent(typeof(UIController))]
public class InputManger : MonoBehaviour
{
    [SerializeField] private int UILayer = 5;
    [SerializeField] private float mouseDeltaSpeed = 0.1f;
    [SerializeField] private float rightClickTime = 0.2f;
    [SerializeField] private float rotateObjectSpeed = 1.0f;
    [SerializeField] private ModifierElement qModifierElement;
    [SerializeField] private ModifierElement wModifierElement;
    [SerializeField] private ModifierElement eModifierElement;
    [SerializeField] private ModifierElement rModifierElement;
    [SerializeField] private ModifierElement tModifierElement;
    [SerializeField] private RotateObjectController rotateObjectController;
    private GameManager gameManager;
    private CameraController camController;
    private CameraLookAtController camLookAtController;
    private UIController uiController;

    private bool rightMousePressed = false;
    private float lastRightMousePressedTime = 0.0f;
    private bool middleMousePressed = false;
    private Vector2 mouseDelta = Vector2.zero;

    private bool QPressed = false;
    private bool WPressed = false;
    private bool EPressed = false;
    private bool RPressed = false;
    private bool TPressed = false;

    private bool ShiftPressed = false;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        camController = GetComponent<CameraController>();
        camLookAtController = GetComponent<CameraLookAtController>();
        uiController = GetComponent<UIController>();
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

        if (rightMousePressed)
        {
            lastRightMousePressedTime = Time.time;
        }
        else
        {
            if (Time.time - lastRightMousePressedTime < rightClickTime)
            {
                gameManager.SwitchPrefabVariant();
            }
        }
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
        else if (ShiftPressed)
        {
            rotateObjectController.AddRotation(scrollDirection * rotateObjectSpeed);
        }
        else
        {
            camController.ZoomCamera(-scrollDirection);
        }
    }

    public void OnShift(InputAction.CallbackContext context)
    {
        ShiftPressed = context.ReadValueAsButton();
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

            // Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = false;
        }
        else
        {
            // Cursor.lockState = CursorLockMode.None;
            // Cursor.visible = true;
        }

        if (middleMousePressed)
        {
            camLookAtController.AddHeight(-mouseDelta.y * mouseDeltaSpeed);
        }
    }

    public void OnQ(InputAction.CallbackContext context)
    {
        QPressed = context.ReadValueAsButton();

        qModifierElement.SetActive(QPressed);
    }

    public void OnW(InputAction.CallbackContext context)
    {
        WPressed = context.ReadValueAsButton();

        wModifierElement.SetActive(WPressed);
    }

    public void OnE(InputAction.CallbackContext context)
    {
        EPressed = context.ReadValueAsButton();

        eModifierElement.SetActive(EPressed);
    }

    public void OnR(InputAction.CallbackContext context)
    {
        RPressed = context.ReadValueAsButton();

        rModifierElement.SetActive(RPressed);
    }

    public void OnT(InputAction.CallbackContext context)
    {
        TPressed = context.ReadValueAsButton();

        tModifierElement.SetActive(TPressed);
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            uiController.SetRestartGameActive(true);
        }
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
