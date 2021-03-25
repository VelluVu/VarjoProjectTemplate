using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class Swaps the material into the chosen material, 
/// when functions are called.
/// </summary>
public class MaterialSwapper : MonoBehaviour
{
    public Material myMaterial;
    public Material highLightMaterial;

    Renderer rend;

    private void Awake() {
        rend = GetComponent<Renderer>();
        myMaterial = rend.material;    
    }

    /// <summary>
    /// Sets the original material.
    /// </summary>
    public void SetNormalMaterial()
    {
        rend.material = myMaterial;
    }

    /// <summary>
    /// Sets the other material
    /// </summary>
    public void SetHighLightMaterial()
    {     
        rend.material = highLightMaterial;
    }

}
