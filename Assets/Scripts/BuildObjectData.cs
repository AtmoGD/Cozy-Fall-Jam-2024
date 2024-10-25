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
    public bool isBase = false;
    public ObjectType objectType;
    public List<ObjectType> canBePlacedOn;
    public GameObject prefab;
    public GameObject previewPrefab;

    public bool CanBePlacedOn(BuildObjectData obj)
    {
        if (!canBePlacedOn.Contains(obj.objectType)) return false;

        return true;
    }
}
