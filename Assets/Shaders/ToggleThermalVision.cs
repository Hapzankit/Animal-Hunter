using UnityEngine;

public class ToggleThermalVision : MonoBehaviour
{
    public Material standardMaterial;
    public Material thermalVisionMaterial;
    public Renderer animalRenderer;

    public bool isThermalVisionActive = false;

    public void ToggleThermalVisionMode()
    {
        isThermalVisionActive = !isThermalVisionActive;
        animalRenderer.material = isThermalVisionActive ? thermalVisionMaterial : standardMaterial;
    }
}
