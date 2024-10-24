using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Base,
    Joint,
    Accessory
}

public class BuildObject : MonoBehaviour
{
    [SerializeField] private ObjectType objectType;
    [SerializeField] private bool isBase = false;
    private BuildObject parentObject;


    public ObjectType GetObjectType()
    {
        return objectType;
    }

    public BuildObject GetParentObject()
    {
        return parentObject;
    }

    public void SetParentObject(BuildObject parent)
    {
        parentObject = parent;
    }
}
