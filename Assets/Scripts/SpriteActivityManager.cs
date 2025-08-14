using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class SpriteActivityManager : MonoBehaviour
{
    [Header("SpriteRenderers")]
    public SpriteRenderer spriteRendererWithMaterial;
    public SpriteRenderer spriteRendererWithoutMaterial;

    [Header("Матеріал")]
    public Material sourceMaterial;

    [Header("Активність (тільки читання)")]
    public bool Active { get; private set; } = false;

    [Header("Кольори (Active / Inactive)")]
    public Color spriteColorActive = Color.white;
    public Color spriteColorInactive = Color.gray;

    public Color materialColorActive = Color.green;
    public Color materialColorInactive = Color.red;

    private static readonly string ReplaceColorProp = "_ReplaceColor";
    private Material runtimeMaterial;
    private static List<SpriteActivityManager> allInstances = new List<SpriteActivityManager>();

    void OnEnable()
    {
        if (!allInstances.Contains(this))
            allInstances.Add(this);

        SetupMaterial();

        if (gameObject.name.Contains("Player") || gameObject.CompareTag("Player"))
        {
            SetActive(true);
        }
        else
        {
            SetActive(false);
        }
    }

    void OnDisable()
    {
        allInstances.Remove(this);
    }

#if UNITY_EDITOR
    void Update()
    {
        if (!Application.isPlaying)
        {
            ApplyState();
        }
    }
#endif

    private void SetupMaterial()
    {
        if (spriteRendererWithMaterial == null || sourceMaterial == null)
            return;

        if (runtimeMaterial == null || spriteRendererWithMaterial.sharedMaterial != runtimeMaterial)
        {
            runtimeMaterial = new Material(sourceMaterial);
            spriteRendererWithMaterial.sharedMaterial = runtimeMaterial;
        }
    }

    public void SetActive(bool value)
    {
        if (value)
            DeactivateOthers();

        Active = value;
        ApplyState();
    }

    private void ApplyState()
    {
        if (spriteRendererWithMaterial != null)
            spriteRendererWithMaterial.color = Active ? spriteColorActive : spriteColorInactive;

        if (spriteRendererWithoutMaterial != null)
            spriteRendererWithoutMaterial.color = Active ? spriteColorActive : spriteColorInactive;

        if (runtimeMaterial != null)
            runtimeMaterial.SetColor(ReplaceColorProp, Active ? materialColorActive : materialColorInactive);
    }

    private void DeactivateOthers()
    {
        foreach (var manager in allInstances)
        {
            if (manager != null && manager != this && manager.Active)
            {
                manager.Active = false;
                manager.ApplyState();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(manager);
#endif
            }
        }
    }
}
