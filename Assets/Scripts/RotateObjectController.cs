using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectController : MonoBehaviour
{
    [SerializeField] private Transform objectToRotate;
    [SerializeField] private float rotationSpeed = 1.0f;

    private float targetRotation = 0.0f;

    private void Update()
    {
        if (objectToRotate == null)
            return;

        float newRotation = Mathf.LerpAngle(objectToRotate.localEulerAngles.y, targetRotation, rotationSpeed * Time.deltaTime);
        objectToRotate.localEulerAngles = new Vector3(objectToRotate.localEulerAngles.x, newRotation, objectToRotate.localEulerAngles.z);
    }

    public void AddRotation(float amount)
    {
        targetRotation += amount;
    }
}
