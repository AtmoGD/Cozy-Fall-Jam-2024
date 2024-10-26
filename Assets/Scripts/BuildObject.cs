using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : MonoBehaviour
{

    [SerializeField] public bool isBase = false;
    public bool IsBase => isBase;
    [SerializeField] private BuildObjectData data;
    public BuildObjectData Data => data;
    [SerializeField] private GameObject model;
    public GameObject Model => model;
    [SerializeField] private Collider modelCollider;
    public Collider ModelCollider => modelCollider;
    [SerializeField] private OutlineController outlineController;
    public OutlineController OutlineController => outlineController;
    [SerializeField] private MeshRenderer meshRenderer;
    public MeshRenderer MeshRenderer => meshRenderer;


    [Header("Debugging (Do not modify)")]
    [SerializeField] private BuildObject parentObject;
    public BuildObject ParentObject => parentObject;
    [SerializeField] private List<BuildObject> childObjects = new List<BuildObject>();
    public List<BuildObject> ChildObjects => childObjects;

    public void SetParentObject(BuildObject parentObject)
    {
        this.parentObject = parentObject;
    }
}
