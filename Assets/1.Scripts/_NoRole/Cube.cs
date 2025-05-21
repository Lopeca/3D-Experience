using System;
using UnityEngine;

public class Cube:MonoBehaviour, IInteractable
{
    private static readonly int OutlineThickness = Shader.PropertyToID("_OutlineThickness");
    private Material outlineMaterial;

    
    private void Awake()
    {
        outlineMaterial = GetComponent<MeshRenderer>().materials[1];
    }

    public void OnTargeted()
    {
        outlineMaterial.SetFloat(OutlineThickness, 1.05f);
    }

    public void OnTargetLost()
    {
        outlineMaterial.SetFloat(OutlineThickness, 1f);
    }

    public void Interact()
    {
        Debug.Log("Interact");
    }
}
