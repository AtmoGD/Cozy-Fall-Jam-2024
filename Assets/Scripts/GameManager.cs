using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraController))]
[RequireComponent(typeof(InputManger))]
public class GameManager : MonoBehaviour
{
    private CameraController camController;
    private InputManger inputManager;

    [SerializeField] private GameObject buildObject;
    private GameObject selectedObject;

    private void Awake()
    {
        camController = GetComponent<CameraController>();
        inputManager = GetComponent<InputManger>();
    }
}
