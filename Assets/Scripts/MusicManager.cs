using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MusicManager : MonoBehaviour
{
    private AudioSource musicDay;
    private AudioSource musicDayRain;
    private AudioSource musicNight;
    private AudioSource musicNightRain;
    public float crossfadeDuration = 5.0f;
    private string currEnvironment = "Day"; //possible states: "Day", "DayRain", "Night", "NightRain"

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform musicDayTransform = transform.Find("musicDay");
        Transform musicDayRainTransform = transform.Find("musicDayRain");
        Transform musicNightTransform = transform.Find("musicNight");
        Transform musicNightRainTransform = transform.Find("musicNightRain");
        if (musicDayTransform && musicDayRainTransform && musicNightTransform && musicNightRainTransform ){
            musicDay = musicDayTransform.GetComponent<AudioSource>();
            musicDayRain = musicNightRainTransform.GetComponent<AudioSource>();
            musicNight = musicNightTransform.GetComponent<AudioSource>();
            musicNightRain = musicNightRainTransform.GetComponent<AudioSource>();
            
            if (musicDay && musicDayRain && musicNight && musicNightRain)
            {
                // set volume of inital environment; if unset, use day
                switch(currEnvironment){
                    case "DayRain":
                        musicDay.volume = 0f;
                        musicDayRain.volume = 1f;
                        musicNight.volume = 0f;
                        musicNightRain.volume = 0f;
                        break;
                    case "Night":
                        musicDay.volume = 0f;
                        musicDayRain.volume = 0f;
                        musicNight.volume = 1f;
                        musicNightRain.volume = 0f;
                        break;
                    case "NightRain":
                        musicDay.volume = 0f;
                        musicDayRain.volume = 0f;
                        musicNight.volume = 1f;
                        musicNightRain.volume = 0f;
                        break;
                    default: 
                        musicDay.volume = 1f;
                        musicDayRain.volume = 0f;
                        musicNight.volume = 0f;
                        musicNightRain.volume = 0f;
                        currEnvironment = "Day";
                        break;
                }
            } else {
                Debug.LogWarning("No Audio Sources found on children of music manager.");
            }
        } else {
            Debug.LogWarning("No children found on music manager.");
        }
    }

    public void StartCrossfade(string newEnvironment){
        if (currEnvironment == newEnvironment){ 
            return;
        } 
        StopAllCoroutines();
        StartCoroutine(CrossfadeCoroutine(newEnvironment));
        currEnvironment = newEnvironment;
    }

    private IEnumerator CrossfadeCoroutine(string newEnvironment){
        float timeStep = 0.05f;
        float timePassed = 0;
        AudioSource prevMusic;
        AudioSource targetMusic; 

        switch(currEnvironment){
            case "NightRain":
                prevMusic = musicNightRain;
                break;
            case "DayRain":
                prevMusic = musicDayRain;
                break;
            case "Night":
                prevMusic = musicNight;
                break;
            default: 
                prevMusic = musicDay;
                break;
        }
        switch(newEnvironment){
            case "NightRain":
                targetMusic = musicNightRain;
                break;
            case "DayRain":
                targetMusic = musicDayRain;
                break;
            case "Night":
                targetMusic = musicNight;
                break;
            default: 
                targetMusic = musicDay;
                break;
        }
        while (timePassed < crossfadeDuration) {
            float t = timePassed/ crossfadeDuration;

            prevMusic.volume = Mathf.Lerp(1f, 0f, t);
            targetMusic.volume = Mathf.Lerp(0f, 1f, t);

            timePassed += timeStep;
            yield return new WaitForSecondsRealtime(timeStep);
        }

    }
}
