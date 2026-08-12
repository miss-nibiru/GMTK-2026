using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class InteractableGlow : MonoBehaviour
{
    [Header("Glow")]
    [ColorUsage(true, true)]
    [SerializeField] private Color glowColor = new Color(1f, 0.75f, 0.2f, 1f);

    [SerializeField] private float minimumIntensity = 0.02f;
    [SerializeField] private float maximumIntensity = 0.25f;
    [SerializeField] private float pulseSpeed = 1.5f;

    private readonly List<Material> _materials = new();
    private readonly List<Color> _originalEmissionColors = new();

    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);

        foreach (Renderer currentRenderer in renderers)
        {
            foreach (Material material in currentRenderer.materials)
            {
                if (material == null ||
                    !material.HasProperty(EmissionColor))
                    continue;

                material.EnableKeyword("_EMISSION");

                _materials.Add(material);
                _originalEmissionColors.Add(
                    material.GetColor(EmissionColor));
            }
        }
    }

    private void Update()
    {
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;

        float intensity = Mathf.Lerp(
            minimumIntensity,
            maximumIntensity,
            pulse);

        Color emission = glowColor * intensity;

        foreach (Material material in _materials)
        {
            if (material != null)
                material.SetColor(EmissionColor, emission);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _materials.Count; i++)
        {
            if (_materials[i] != null)
            {
                _materials[i].SetColor(
                    EmissionColor,
                    _originalEmissionColors[i]);
            }
        }
    }
}