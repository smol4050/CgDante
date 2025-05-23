using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
public class TileScript : MonoBehaviour
{
    public int x, y;
    public Material defaultMaterial;
    public Material highlightMaterial;
    private Renderer rend;
    private BoxCollider boxCollider;
    public void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        rend = GetComponent<Renderer>();
        rend.sharedMaterial = defaultMaterial;
    }
    public void Highlight()
    { 
        boxCollider.isTrigger = false;
        rend.sharedMaterial = highlightMaterial;
    }
    public void HideHighlight() {
        rend.sharedMaterial = defaultMaterial;
    }

    public void ResetTile() {
        boxCollider.isTrigger = true;
        rend.sharedMaterial= defaultMaterial;
    }
}
