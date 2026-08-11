using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class InteractableGlow : MonoBehaviour
{
    
    [ColorUsage(true, true)]
    [SerializeField] private Color glowColor = new Color(1f, 0.65f, 0.15f);
    [SerializeField] private float minimumIntensity;
    [SerializeField] private float maximumIntensity;
    [SerializeField] private float pulseSpeed;
    [SerializeField] private Renderer[] targetRenderers;

    private readonly List<Material> _materials = new();
    private readonly List<Color> _originalEmissionColors = new();
    private IInteractable _interactable;

    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        _interactable = GetComponent<IInteractable>();

        if (targetRenderers == null || targetRenderers.Length == 0)
            targetRenderers = GetComponentsInChildren<Renderer>(true);

        CacheMaterials();
    }

    private void Update()
    {
        bool shouldGlow = _interactable != null && _interactable.CanInteract();

        if (!shouldGlow)
        {
            RestoreOriginalEmission();
            return;
        }

        float pulse = Mathf.PingPong(Time.time * pulseSpeed, maximumIntensity - minimumIntensity);
        float intensity = minimumIntensity + pulse;

        Color currentGlow = glowColor * Mathf.LinearToGammaSpace(intensity);

        foreach (Material material in _materials)
        {
            if (material != null && material.HasProperty(EmissionColor))
                material.SetColor(EmissionColor, currentGlow);
        }
    }

    private void CacheMaterials()
    {
        _materials.Clear();
        _originalEmissionColors.Clear();

        foreach (Renderer targetRenderer in targetRenderers)
        {
            if (targetRenderer == null)
                continue;

            foreach (Material material in targetRenderer.materials)
            {
                if (!material.HasProperty(EmissionColor))
                    continue;

                material.EnableKeyword("_EMISSION");

                _materials.Add(material);
                _originalEmissionColors.Add(
                    material.GetColor(EmissionColor));
            }
        }
    }

    private void RestoreOriginalEmission()
    {
        for (int i = 0; i < _materials.Count; i++)
        {
            if (_materials[i] != null)
                _materials[i].SetColor(
                    EmissionColor,
                    _originalEmissionColors[i]);
        }
    }

    private void OnDisable()
    {
        RestoreOriginalEmission();
    }
}