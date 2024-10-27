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
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        meshRenderer.material.SetColor("_EmissionColor", emissionColor * (emissionIntensity * (gameManager.IsDay ? 0.0f : 1.0f)));
        lightSource.intensity = lightIntensity * (gameManager.IsDay ? 0.0f : 1.0f);
    }
}
