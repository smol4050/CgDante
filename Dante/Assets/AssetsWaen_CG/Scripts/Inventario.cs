using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    private bool inventarioAbierto;

    public GameObject inventario;
    private int allSlots;
    private int slotsDisponibles;

    private GameObject[] slot;

    public GameObject slotHolder;



    void Start()
    {
        allSlots = slotHolder.transform.childCount;

        slot = new GameObject[allSlots];

        for (int i = 0; i < allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;

            if (slot[i].GetComponent<Slot>().item == null)
            {
                slot[i].GetComponent<Slot>().empty = true;
            }
            
        }

    }


    void Update()
    {

        if ((Input.GetKeyDown(KeyCode.I)))
        {
            inventarioAbierto = !inventarioAbierto;

        }

        if (inventarioAbierto)
        {
            inventario.SetActive(true);


        }
        else
        {
            inventario.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag==("Item"))
        {
            GameObject itemPickedUp = other.gameObject;

            Item item = itemPickedUp.GetComponent<Item>();

            AddItem(itemPickedUp, item.ID, item.tipo, item.descripcion, item.icon);
        }
    }

    void AddItem(GameObject itemObject, int itemID, string itemType, string itemDescription, Sprite itemIcon)
    {
        for (int i = 0; i < allSlots; i++)
        {
            if (slot[i].GetComponent<Slot>().empty)
            {
                itemObject.GetComponent<Item>().pickedUp = true;
                slot[i].GetComponent<Slot>().item = itemObject;
                slot[i].GetComponent<Slot>().ID = itemID;
                slot[i].GetComponent<Slot>().tipo = itemType;
                slot[i].GetComponent<Slot>().descripcion= itemDescription;
                slot[i].GetComponent<Slot>().icon = itemIcon;
                
                //itemObject.transform.parent = slot[i].transform;
                itemObject.SetActive(false);


                slot[i].GetComponent<Slot>().UpdateSlot();

                slot[i].GetComponent<Slot>().empty = false;


            }
            return;


        }

    }



}

