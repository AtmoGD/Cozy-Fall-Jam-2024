using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class MusicManager : MonoBehaviour
{
    public AudioMixerSnapshot DaySnapshot;
    public AudioMixerSnapshot NightSnapshot;    
    public AudioMixerSnapshot DayRainSnapshot;
    public AudioMixerSnapshot NightRainSnapshot;
    public AudioMixerSnapshot PauseSnapshot;
    public float crossfadeDuration = 5.0f;
    [SerializeField] private string currEnvironment = "Pause"; //possible states: "Day", "DayRain", "Night", "NightRain", "Pause"

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (DaySnapshot && NightSnapshot && DayRainSnapshot && NightRainSnapshot)
        {
            // set volume of inital environment; if unset, use day
            switch(currEnvironment){
                case "DayRain":
                    DayRainSnapshot.TransitionTo(0.2f);
                    break;
                case "Night":
                    NightSnapshot.TransitionTo(0.2f);
                    break;
                case "NightRain":
                    NightRainSnapshot.TransitionTo(0.2f);
                    break;
                case "Pause":
                    PauseSnapshot.TransitionTo(0.2f);
                    break;
                default: 
                    DaySnapshot.TransitionTo(0.2f);
                    currEnvironment = "Day";
                    break;
            }
        } else {
            Debug.LogWarning("No Audio Snapshots found on music manager.");
        }
    }

    public void StartCrossfade(string newEnvironment){
        if (currEnvironment == newEnvironment){ 
            return;
        } 
        
        switch(newEnvironment){
            case "DayRain":
                DayRainSnapshot.TransitionTo(crossfadeDuration);
                break;
            case "Night":
                NightSnapshot.TransitionTo(crossfadeDuration);
                break;
            case "NightRain":
                NightRainSnapshot.TransitionTo(crossfadeDuration);
                break;
            default: 
                DaySnapshot.TransitionTo(crossfadeDuration);
                break;
        }

        currEnvironment = newEnvironment;
    }

}
