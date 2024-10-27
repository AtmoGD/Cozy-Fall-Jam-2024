using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifierElement : MonoBehaviour
{
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = Color.gray;
    [SerializeField] private Image image;

    private void Awake()
    {
        SetActive(false);
    }

    public void SetActive(bool active)
    {
        image.color = active ? activeColor : inactiveColor;
    }
}
