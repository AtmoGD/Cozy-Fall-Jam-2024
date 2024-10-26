using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAtController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float targetHeight = 1.0f;
    [SerializeField] private float minHeight = 1.0f;
    [SerializeField] private float maxHeight = 10.0f;

    private void Update()
    {
        if (target == null)
            return;

        float newHeight = Mathf.Lerp(target.position.y, targetHeight, speed * Time.deltaTime);
        target.position = new Vector3(target.position.x, newHeight, target.position.z);
    }

    public void AddHeight(float amount)
    {
        targetHeight += amount;
        targetHeight = Mathf.Clamp(targetHeight, minHeight, maxHeight);
    }
}
