using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<string> items = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Evita duplicados
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistencia
        }
    }

    public void AddItem(string itemName)
    {
        items.Add(itemName);
        Debug.Log($"Item agregado: {itemName}");
    }

    public void RemoveItem(string itemName)
    {
        if (items.Contains(itemName))
            items.Remove(itemName);
    }
}
