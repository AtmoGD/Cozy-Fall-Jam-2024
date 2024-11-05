using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum MushroomState
{
    Idle,
    Growing,
    Shrinking
}

[Serializable]
public class MushroomStateData
{
    public MushroomState state;
    public float timeMin;
    public float timeMax;
    [HideInInspector] public float time;
}

public class MushroomController : MonoBehaviour
{
    public string id = "Mushroom";
    public Vector3 shrinkScale = Vector3.zero;
    public Vector3 growScale = Vector3.one;
    public List<MushroomStateData> stateData = new List<MushroomStateData>();

    private int stateIndex = 0;
    private float stateStartTime = 0f;

    void Start()
    {
        transform.localScale = shrinkScale;
        stateStartTime = Time.time;
    }

    void Update()
    {
        float progress = (Time.time - stateStartTime) / stateData[stateIndex].time;

        switch (stateData[stateIndex].state)
        {
            case MushroomState.Idle:
                break;
            case MushroomState.Growing:
                transform.localScale = Vector3.Lerp(shrinkScale, growScale, progress);
                break;
            case MushroomState.Shrinking:
                transform.localScale = Vector3.Lerp(growScale, shrinkScale, progress);
                break;
        }

        if (progress >= 1f)
        {
            stateIndex++;
            if (stateIndex >= stateData.Count)
            {
                stateIndex = 0;
            }
            stateData[stateIndex].time = UnityEngine.Random.Range(stateData[stateIndex].timeMin, stateData[stateIndex].timeMax);
            stateStartTime = Time.time;
        }
    }
}
