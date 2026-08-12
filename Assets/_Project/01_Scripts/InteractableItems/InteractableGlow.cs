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

    [Header("Activation")]
    [SerializeField] private float glowDistance = 4f;

    private readonly List<Material> _materials = new();
    private readonly List<Color> _originalEmissionColors = new();

    private Transform _player;
    private EvidenceDiscoverable _evidenceDiscoverable;
    private bool _isGlowing;
    private float _glowTimer;

    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private void Awake()
    {
        _evidenceDiscoverable = GetComponent<EvidenceDiscoverable>();
        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);

        foreach (Renderer currentRenderer in renderers)
        {
            foreach (Material material in currentRenderer.materials)
            {
                if (material == null || !material.HasProperty(EmissionColor))
                    continue;
                material.EnableKeyword("_EMISSION");

                _materials.Add(material);
                _originalEmissionColors.Add(
                    material.GetColor(EmissionColor));
            }
        }
    }

    private void Start()
    {
        FindPlayer();
        if (HasAlreadyBeenDiscovered())
            StopGlowingForever();
    }

    private void Update()
    {
        if (HasAlreadyBeenDiscovered())
        {
            StopGlowingForever();
            return;
        }

        if (_player == null) FindPlayer();
        if (_player == null) return;
        float distanceSquared = (transform.position - _player.position).sqrMagnitude;
        bool playerIsClose = distanceSquared <= glowDistance * glowDistance;

        if (!playerIsClose)
        {
            _glowTimer = 0f;
            RestoreOriginalEmission();
            return;
        }

        _glowTimer += Time.deltaTime;
        float pulse = (Mathf.Sin((_glowTimer * pulseSpeed) - (Mathf.PI * 0.5f)) + 1f) * 0.5f;

        float intensity = Mathf.Lerp(
            minimumIntensity,
            maximumIntensity,
            pulse);

        Color emission = glowColor * intensity;
        foreach (Material material in _materials)
        {
            if (material != null) material.SetColor(EmissionColor, emission);
        }

        _isGlowing = true;
    }

    private bool HasAlreadyBeenDiscovered()
    {
        if (_evidenceDiscoverable == null) return false;
        EvidenceData evidence = _evidenceDiscoverable.EvidenceData;

        if (evidence == null ||
            evidence.ThoughtOnly ||
            string.IsNullOrWhiteSpace(evidence.EvidenceId))
            return false;

        return EvidenceTracker
            .GetOrCreate()
            .HasDiscovered(evidence.EvidenceId);
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) _player = playerObject.transform;
    }

    private void StopGlowingForever()
    {
        RestoreOriginalEmission();
        enabled = false;
    }

    private void RestoreOriginalEmission()
    {
        if (!_isGlowing) return;

        for (int i = 0; i < _materials.Count; i++)
        {
            if (_materials[i] != null)
            {
                _materials[i].SetColor(
                    EmissionColor,
                    _originalEmissionColors[i]);
            }
        }

        _isGlowing = false;
    }

    private void OnDisable()
    {
        RestoreOriginalEmission();
    }
}