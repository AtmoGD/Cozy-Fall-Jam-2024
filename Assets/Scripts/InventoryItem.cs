using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InventoryItem : MonoBehaviour
{
    [SerializeField] private BuildObjectData buildObjectData;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        int count = gameManager.Inventory.Find(x => x.data == buildObjectData).count;

        if (count > 0 || gameManager.InfiniteInventory)
        {
            text.text = count.ToString();
            button.interactable = true;
        }
        else
        {
            text.text = "";
            button.interactable = false;
        }
    }

    public void OnClick()
    {
        gameManager.SelectObject(buildObjectData);
        gameManager.PlaySelectSound();
    }
}
