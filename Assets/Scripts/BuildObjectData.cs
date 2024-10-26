using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Base,
    Joint,
    Accessory
}

[CreateAssetMenu(fileName = "BuildObjectData", menuName = "BuildObjectData", order = 1)]
public class BuildObjectData : ScriptableObject
{
    public ObjectType objectType;
    public List<ObjectType> canBePlacedOn;
    public List<GameObject> prefabs;
    public GameObject previewPrefab;
    public bool CanRotateX = false;
    public int RotationXSteps = 0;
    public float RotationXMin = 0;
    public float RotationXMax = 360;
    public bool CanRotateY = false;
    public int RotationYSteps = 0;
    public float RotationYMin = 0;
    public float RotationYMax = 360;
    public bool CanRotateZ = false;
    public int RotationZSteps = 0;
    public float RotationZMin = 0;
    public float RotationZMax = 360;
    public bool CanAddDistance = false;
    public float distanceStep = 0;
    public float MinDistance = 0;
    public float MaxDistance = 0;
    public float startDistance = 0;
    public bool CanScale = false;
    public float scaleStep = 0;
    public float MinScale = 0;
    public float MaxScale = 0;

    public bool CanBePlacedOn(BuildObjectData obj)
    {
        if (!canBePlacedOn.Contains(obj.objectType)) return false;

        return true;
    }

    public GameObject GetPrefab()
    {
        return prefabs[Random.Range(0, prefabs.Count)];
    }
}
