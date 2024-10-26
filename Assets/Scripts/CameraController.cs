using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform cameraLookAt;
    [SerializeField] private float yawSpeed = 2.0f;
    [SerializeField] private float pitchSpeed = 2.0f;
    [SerializeField] private float pitchMin = -90.0f;
    [SerializeField] private float pitchMax = 90.0f;
    [SerializeField] private float zoomSpeed = 2.0f;
    [SerializeField] private float zoomMin = 1.0f;
    [SerializeField] private float zoomMax = 10.0f;
    [SerializeField] private float rotationLerpSpeed = 2.0f;
    [SerializeField] private float zoomLerpSpeed = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float distance = 0.0f;
    private float targetDistance = 0.0f;

    private void Start()
    {
        distance = Vector3.Distance(mainCamera.transform.position, cameraLookAt.position);
        targetDistance = distance;
        Vector3 angles = mainCamera.transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    public void RotateCamera(float x, float y)
    {
        yaw += x * yawSpeed;
        pitch -= y * pitchSpeed;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
    }

    public void ZoomCamera(float z)
    {
        targetDistance += z * zoomSpeed;
        targetDistance = Mathf.Clamp(targetDistance, zoomMin, zoomMax);
    }

    private void LateUpdate()
    {
        distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * zoomLerpSpeed);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0.0f);
        rotation = Quaternion.Lerp(mainCamera.transform.rotation, rotation, Time.deltaTime * rotationLerpSpeed);

        Vector3 position = cameraLookAt.position - (rotation * Vector3.forward * distance);

        mainCamera.transform.rotation = rotation;
        mainCamera.transform.position = position;
    }
}
