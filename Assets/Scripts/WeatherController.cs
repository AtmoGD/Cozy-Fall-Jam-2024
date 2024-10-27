using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class WeatherController : MonoBehaviour
{
    [SerializeField] private ParticleSystem rain;
    [SerializeField] private float emissionRate = 100.0f;

    private GameManager gameManager;
    private ParticleSystem.EmissionModule rainEmission;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    private void Start()
    {
        rainEmission = rain.emission;
        rainEmission.enabled = gameManager.IsRain;
    }

    private void Update()
    {
        rainEmission.enabled = gameManager.IsRain;
    }
}
