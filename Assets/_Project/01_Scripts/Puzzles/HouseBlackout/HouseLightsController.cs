using UnityEngine;

public class HouseLightsController : MonoBehaviour
{
    public void TurnOffLights()
    {
        Light[] houseLights = GetComponentsInChildren<Light>(true);

        foreach (Light houseLight in houseLights)
            houseLight.enabled = false;
    }
}

