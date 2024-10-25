using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : MonoBehaviour
{
    [SerializeField] private BuildObjectData data;
    public BuildObjectData Data => data;
    [SerializeField] private GameObject model;
    public GameObject Model => model;
    [SerializeField] private Collider modelCollider;
    public Collider ModelCollider => modelCollider;
    [SerializeField] private List<BuildObject> attachedObjects = new List<BuildObject>();
    public List<BuildObject> AttachedObjects => attachedObjects;
    [SerializeField] private BuildObject parentObject;
    public BuildObject ParentObject => parentObject;

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
