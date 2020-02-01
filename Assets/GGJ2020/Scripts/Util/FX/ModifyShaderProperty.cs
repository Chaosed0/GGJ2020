using UnityEngine;
using System.Collections.Generic;
using Obelus;

[ExecuteInEditMode]
public class ModifyShaderProperty : MonoBehaviour, UnityEngine.UI.IMaterialModifier
{
    private static MaterialPropertyBlock materialPropertyBlock = null;

    private enum Type
    {
        Color,
        HdrColor,
        Float,
        Texture,
    }

    [SerializeField]
    private string propertyName = "_Color";

    [SerializeField]
    private Type type = Type.Color;

    [SerializeField]
    [Sirenix.OdinInspector.ShowIf("type", Type.Color)]
    private Color color = Color.white;

    [SerializeField]
    [ColorUsage(false, true)]
    [Sirenix.OdinInspector.ShowIf("type", Type.HdrColor)]
    private Color hdrColor = Color.black;

    [SerializeField]
    [Sirenix.OdinInspector.ShowIf("type", Type.Float)]
    private float value = 1f;

    [SerializeField]
    [Sirenix.OdinInspector.ShowIf("type", Type.Texture)]
    private Texture texture = null;

    [SerializeField]
    private List<Renderer> renderers = new List<Renderer>();

    [SerializeField]
    private List<CanvasRendererMaterialModifier> canvasRendererMaterialModifiers = new List<CanvasRendererMaterialModifier>();

    [SerializeField]
    private bool findAllInChildren = false;

    private Vector4 oldColor = Vector4.zero;
    private float oldValue = -99999f;
    private Texture oldTexture = null;

    void OnEnable()
    {
        foreach (var modifier in canvasRendererMaterialModifiers)
        {
            modifier.RegisterModifier(this);
        }
    }

    void OnDisable()
    {
        foreach (var modifier in canvasRendererMaterialModifiers)
        {
            modifier.UnregisterModifier(this);
        }
    }

    void Awake()
    {
        if (Application.isPlaying && findAllInChildren)
        {
            GetComponentsInChildren<Renderer>(true, renderers);
            GetComponentsInChildren<CanvasRendererMaterialModifier>(true, canvasRendererMaterialModifiers);
        }
    }

    private void LateUpdate()
    {
        if (materialPropertyBlock == null)
        {
            materialPropertyBlock = new MaterialPropertyBlock();
        }

        if (!ShouldUpdate())
        {
            return;
        }

        if (renderers != null)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.GetPropertyBlock(materialPropertyBlock);
                switch (type)
                {
                    case Type.Color:
                        materialPropertyBlock.SetColor(propertyName, color);
                        oldColor = color;
                        break;
                    case Type.HdrColor:
                        materialPropertyBlock.SetColor(propertyName, hdrColor);
                        oldColor = hdrColor;
                        break;
                    case Type.Float:
                        materialPropertyBlock.SetFloat(propertyName, value);
                        oldValue = value;
                        break;
                    case Type.Texture:
                        materialPropertyBlock.SetTexture(propertyName, texture);
                        oldTexture = texture;
                        break;
                }
                renderer.SetPropertyBlock(materialPropertyBlock);
            }
        }

        foreach (var modifier in canvasRendererMaterialModifiers)
        {
            modifier.SetMaterialDirty();
        }
    }

    private Vector4 cachedColor = Vector4.zero;
    private bool ShouldUpdate()
    {
        bool shouldUpdate = false;
        switch (type)
        {
            case Type.Color:
                cachedColor.x = color.r;
                cachedColor.y = color.g;
                cachedColor.z = color.b;
                cachedColor.w = color.a;
                shouldUpdate = Vector4.Distance(cachedColor, oldColor) > Vector4.kEpsilon;
                break;
            case Type.HdrColor:
                cachedColor.x = hdrColor.r;
                cachedColor.y = hdrColor.g;
                cachedColor.z = hdrColor.b;
                cachedColor.w = hdrColor.a;
                shouldUpdate = Vector4.Distance(cachedColor, oldColor) > Vector4.kEpsilon;
                break;
            case Type.Float:
                shouldUpdate = Mathf.Abs(oldValue - value) > Mathf.Epsilon;
                break;
            case Type.Texture:
                shouldUpdate = oldTexture != texture;
                break;
        }

        oldColor = cachedColor;
        oldValue = value;

        return shouldUpdate;
    }

    Material UnityEngine.UI.IMaterialModifier.GetModifiedMaterial(Material material)
    {
        this.LogVerbose($"Modifying material {material}");

        if (material != null)
        {
            switch (type)
            {
                case Type.Color:
                    material.SetColor(propertyName, color);
                    break;
                case Type.HdrColor:
                    material.SetColor(propertyName, hdrColor);
                    break;
                case Type.Float:
                    material.SetFloat(propertyName, value);
                    break;
            }
        }

        return material;
    }

    public void SetColor(Color color)
    {
        Debug.Assert(type == Type.Color || type == Type.HdrColor);
        if (type == Type.Color)
        {
            this.oldColor = this.color;
            this.color = color;
        }
        else if (type == Type.HdrColor)
        {
            this.oldColor = this.hdrColor;
            this.hdrColor = color;
        }
    }

    public void SetFloat(float value)
    {
        Debug.Assert(type == Type.Float);
        this.oldValue = this.value;
        this.value = value;
    }

    public void OnDestroy()
    {
        foreach (var modifier in canvasRendererMaterialModifiers)
        {
            modifier.UnregisterModifier(this);
        }
    }
}
