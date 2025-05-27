using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GameObject item;
    public int ID;
    public string tipo;
    public string descripcion;

    public bool empty;
    public Sprite icon;

    private Image itemImage;

    void Start()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemImage.enabled = false; // Ocultamos inicialmente
    }

    public void UpdateSlot()
    {
        if (icon != null)
        {
            itemImage.sprite = icon;
            itemImage.enabled = true;
        }
    }

}
