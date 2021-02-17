using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
    public Material myMaterial;
    public Material highLightMaterial;

    Renderer rend;

    private void Awake() {
        rend = GetComponent<Renderer>();
        myMaterial = rend.material;    
    }

    public void SetNormalMaterial()
    {
        rend.material = myMaterial;
    }

    public void SetHighLightMaterial()
    {     
        rend.material = highLightMaterial;
    }

}
