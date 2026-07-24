using UnityEngine;

public class HouseLightsController : MonoBehaviour
{
    [SerializeField] private Light[] houseLights;

    public void TurnOffLights()
    {
        foreach (Light houseLight in houseLights)
            if (houseLight != null) houseLight.enabled = false;
    }
    
}
