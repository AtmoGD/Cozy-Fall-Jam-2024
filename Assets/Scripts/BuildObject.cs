using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : MonoBehaviour
{
    [SerializeField] private BuildObjectData data;
    public BuildObjectData Data => data;
    [SerializeField] private GameObject model;
    [SerializeField] private Collider modelCollider;
    private List<BuildObject> attachedObjects = new List<BuildObject>();
    private BuildObject parentObject;


    public BuildObject GetParentObject()
    {
        return parentObject;
    }

    public void SetParentObject(BuildObject parent)
    {
        parentObject = parent;
    }

    public bool CanBePlacedOn(BuildObject obj)
    {
        if (!data.CanBePlacedOn(obj.data))
        {
            return false;
        }

        if (CollidesWithAttachedChildren())
        {
            return false;
        }

        return true;
    }

    private bool CollidesWithAttachedChildren()
    {
        foreach (BuildObject obj in attachedObjects)
        {
            if (obj.modelCollider.bounds.Intersects(modelCollider.bounds))
            {
                return true;
            }
        }

        return false;
    }
}
