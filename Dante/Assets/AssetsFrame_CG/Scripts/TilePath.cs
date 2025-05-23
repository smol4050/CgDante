using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TilePath
{
    public string name;

    [Tooltip("Arrastra aquí el GameObject que CONTIENE al TileScript")]
    public List<GameObject> tileParents = new List<GameObject>();
}
