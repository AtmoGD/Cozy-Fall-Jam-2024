using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDistanceController : MonoBehaviour
{
    [SerializeField] private Light lightSource;
    [SerializeField] private Transform target;
    [SerializeField] private float dayIntensity = 1.0f;
    [SerializeField] private float nightIntensity = 0.0f;
    [SerializeField] private float distanceMultiplier = 1.0f;
    private float currentIntensity = 0.0f;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        currentIntensity = gameManager.IsDay ? dayIntensity : nightIntensity;

        if (lightSource && target)
        {
            float distance = Vector3.Distance(lightSource.transform.position, target.position);
            lightSource.intensity = currentIntensity + distance * distanceMultiplier;
        }
    }
}
