using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class DayNightController : MonoBehaviour
{
    [SerializeField] private Light mainLight;
    [SerializeField] private float dayIntensity = 1.0f;
    [SerializeField] private Vector3 dayRotation = new Vector3(50.0f, 0.0f, 0.0f);
    [SerializeField] private float nightIntensity = 0.0f;
    [SerializeField] private Vector3 nightRotation = new Vector3(50.0f, 0.0f, 0.0f);
    [SerializeField] private float lightSpeed = 1.0f;
    [SerializeField] private float rotationSpeed = 1.0f;
    [SerializeField] private GameObject dayVolume;
    [SerializeField] private GameObject nightVolume;
    [SerializeField] private float dayY = 0.0f;
    [SerializeField] private float nightY = -15.0f;
    [SerializeField] private float volumeSpeed = 1.0f;
    [SerializeField] private MeshRenderer background;
    [SerializeField] private Color dayColor = Color.white;
    [SerializeField] private Color nightColor = Color.black;
    [SerializeField] private float colorSpeed = 1.0f;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    private void Update()
    {
        float targetIntensity = gameManager.IsDay ? dayIntensity : nightIntensity;
        mainLight.intensity = Mathf.Lerp(mainLight.intensity, targetIntensity, lightSpeed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.Euler(gameManager.IsDay ? dayRotation : nightRotation);
        mainLight.transform.localRotation = Quaternion.Lerp(mainLight.transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);

        dayVolume.transform.localPosition = new Vector3(dayVolume.transform.localPosition.x, Mathf.Lerp(dayVolume.transform.localPosition.y, gameManager.IsDay ? dayY : nightY, volumeSpeed * Time.deltaTime), dayVolume.transform.localPosition.z);
        nightVolume.transform.localPosition = new Vector3(nightVolume.transform.localPosition.x, Mathf.Lerp(nightVolume.transform.localPosition.y, gameManager.IsDay ? nightY : dayY, volumeSpeed * Time.deltaTime), nightVolume.transform.localPosition.z);

        background.material.color = Color.Lerp(background.material.color, gameManager.IsDay ? dayColor : nightColor, colorSpeed * Time.deltaTime);
    }
}
