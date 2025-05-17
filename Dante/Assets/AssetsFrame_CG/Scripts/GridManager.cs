using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public TileBehaviour tilePrefab;        // Asigna aquí tu prefab Tile
    public Transform origin;                // Asigna aquí GridOrigin
    public int size = 5;                    // 5×5 para el primer nivel
    [HideInInspector] public List<TileBehaviour> tiles = new List<TileBehaviour>();

    void Start()
    {
        CreateGrid();
    }

    public void CreateGrid()
    {
        // Limpiar si ya hay tiles
        foreach (var t in tiles) Destroy(t.gameObject);
        tiles.Clear();

        float spacing = 1.1f;  // separa un poco las baldosas
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Vector3 pos = origin.position + new Vector3(x * spacing, 0, -y * spacing);
                var tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                tile.index = y * size + x;   // índice lineal
                tiles.Add(tile);
            }
        }
    }
}