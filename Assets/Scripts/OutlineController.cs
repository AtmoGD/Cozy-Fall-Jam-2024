using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private int outlineIndex = 0;

    public void SetOutlineThickness(float thickness)
    {
        meshRenderer.materials[outlineIndex].SetFloat("_Outline_Thickness", thickness);
    }
}
