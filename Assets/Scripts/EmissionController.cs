using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionController : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Light lightSource;
    [SerializeField] private Color emissionColor = Color.white;
    [SerializeField] private float emissionIntensity = 1.0f;
    [SerializeField] private float lightIntensity = 1.0f;
    [SerializeField] private bool invert = false;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (meshRenderer)
            meshRenderer.material.SetColor("_EmissionColor", emissionColor * (emissionIntensity * (gameManager.IsDay ? (invert ? 0.0f : 1.0f) : (invert ? 1.0f : 0.0f))));

        if (lightSource)
            lightSource.intensity = lightIntensity * (gameManager.IsDay ? (invert ? 0.0f : 1.0f) : (invert ? 1.0f : 0.0f));
    }
}
