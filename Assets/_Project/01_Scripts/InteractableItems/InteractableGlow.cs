using UnityEngine;


[RequireComponent(typeof(Renderer))]
public class InteractableGlow : MonoBehaviour
{
    
    [SerializeField] private float minimumWidth = 0.015f;
    [SerializeField] private float maximumWidth = 0.03f;
    [SerializeField] private float pulseSpeed = 1.5f;

    private Renderer _outlineRenderer;
    private MaterialPropertyBlock _propertyBlock;

    private static readonly int OutlineWidth =
        Shader.PropertyToID("_OutlineWidth");

    private void Awake()
    {
        _outlineRenderer = GetComponent<Renderer>();
        _propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;

        float width = Mathf.Lerp(
            minimumWidth,
            maximumWidth,
            pulse);

        _outlineRenderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetFloat(OutlineWidth, width);
        _outlineRenderer.SetPropertyBlock(_propertyBlock);
    }
    
}
