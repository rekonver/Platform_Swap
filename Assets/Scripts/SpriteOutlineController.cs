using UnityEngine;

public class SpriteOutlineController : MonoBehaviour
{
    [Header("Target Sprite Renderer")]
    public SpriteRenderer targetSpriteRenderer;

    [Header("Outline Settings")]
    public Color activeOutlineColor = Color.white;
    public Color inactiveOutlineColor = Color.gray;
    public float outlineSize = 0.05f;

    [SerializeField]
    private bool isActive = true;

    private MaterialPropertyBlock propertyBlock;

    private void Awake()
    {
        if (targetSpriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer не призначений! Спроба знайти на цьому об'єкті...");
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        propertyBlock = new MaterialPropertyBlock();
        UpdateOutline();
    }

    private void OnValidate()
    {
        if (targetSpriteRenderer != null)
        {
            // Щоб уникнути помилки
            if (propertyBlock == null)
                propertyBlock = new MaterialPropertyBlock();

            UpdateOutline();
        }
    }

    public void SetActive(bool value)
    {
        isActive = value;
        UpdateOutline();
    }

    private void UpdateOutline()
    {
        if (targetSpriteRenderer == null || propertyBlock == null)
            return;

        targetSpriteRenderer.GetPropertyBlock(propertyBlock);

        Color outlineColor = isActive ? activeOutlineColor : inactiveOutlineColor;
        propertyBlock.SetColor("_OutlineColor", outlineColor);
        propertyBlock.SetFloat("_OutlineSize", outlineSize);

        targetSpriteRenderer.SetPropertyBlock(propertyBlock);
    }
}
