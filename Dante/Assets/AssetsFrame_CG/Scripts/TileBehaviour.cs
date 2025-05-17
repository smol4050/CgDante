using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public Material normalMat;
    public Material iluminadoMat;
    [HideInInspector] public int index;
    private MeshRenderer mesh;

    void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    public void Highlight()
    {
        mesh.material = iluminadoMat;
    }
    public void Unhighlight()
    {
        mesh.material = normalMat;
    }
}
