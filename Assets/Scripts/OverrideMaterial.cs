using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverrideMaterial : MonoBehaviour
{
    public List<MeshRenderer> meshes = new List<MeshRenderer>();
    
    public void SetNewMaterial(Material newMaterial) 
    { 
        for (short i=0; i<meshes.Count; i++)
        {
            if (meshes[i] == null) continue;

            meshes[i].material = newMaterial;
        }
    }
}
