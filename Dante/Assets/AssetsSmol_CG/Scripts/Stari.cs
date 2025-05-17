using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stari : MonoBehaviour
{
    public GameObject stepPrefab;

    public int stepCount = 10;

    public float stepHeight = 1.0f;
    public float stepWidth = 1.0f;

    public List<GameObject> steps;

    public Transform StairOrigin;

    void Start()
    {
        CreateStair();
    }


    void CreateStair()
    {
        for (int i = 0; i < stepCount; i++)
        {
            GameObject newObject = Instantiate(stepPrefab, StairOrigin);
            newObject.transform.localPosition = Vector3.right* i;
            newObject.transform.localScale = new Vector3(1,stepHeight * i+ stepHeight, stepWidth);

            steps.Add(newObject);
        }
    }

}
