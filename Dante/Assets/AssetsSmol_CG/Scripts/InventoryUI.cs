using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public TextMeshProUGUI inventoryText;

    private bool isOpen = false;

    void Start()
    {
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        if (isOpen)
        {
            RefreshInventoryUI();
        }
    }

    void RefreshInventoryUI()
    {
        inventoryText.text = "Inventario:\n";
        foreach (var item in InventoryManager.Instance.items)
        {
            inventoryText.text += "- " + item + "\n";
        }
    }
}
