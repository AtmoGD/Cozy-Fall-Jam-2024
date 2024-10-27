using UnityEngine;

public class EnviromentSettingsController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject dayTimeDayImage;
    [SerializeField] private GameObject dayTimeNightImage;
    [SerializeField] private GameObject weatherSunnyImage;
    [SerializeField] private GameObject weatherRainyImage;

    private void Start()
    {
        dayTimeDayImage.SetActive(gameManager.IsDay);
        dayTimeNightImage.SetActive(!gameManager.IsDay);
        weatherSunnyImage.SetActive(!gameManager.IsRain);
        weatherRainyImage.SetActive(gameManager.IsRain);
    }

    public void ToggleDayTime()
    {
        gameManager.SetIsDay(!gameManager.IsDay);

        dayTimeDayImage.SetActive(gameManager.IsDay);
        dayTimeNightImage.SetActive(!gameManager.IsDay);
    }

    public void ToggleWeather()
    {
        gameManager.SetIsRain(!gameManager.IsRain);

        weatherSunnyImage.SetActive(!gameManager.IsRain);
        weatherRainyImage.SetActive(gameManager.IsRain);
    }
}
