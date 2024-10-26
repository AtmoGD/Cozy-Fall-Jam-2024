using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private BuildObjectData data;
    public void OnClick()
    {
        FindObjectOfType<GameManager>().SelectObject(data);
    }
}
