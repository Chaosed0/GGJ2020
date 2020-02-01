using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Obelus;

[RequireComponent(typeof(Graphic))]
public class CanvasRendererMaterialModifier : MonoBehaviour, IMaterialModifier
{
    [Sirenix.OdinInspector.ShowInInspector]
    [Sirenix.OdinInspector.ReadOnly]
    private HashSet<IMaterialModifier> materialModifiers = new HashSet<IMaterialModifier>();

    private Material duplicatedMaterial = null;
    private Graphic graphic = null;

    Material IMaterialModifier.GetModifiedMaterial(Material baseMaterial)
    {
        this.LogVerbose($"Replacer modifying material {baseMaterial}");

        if (duplicatedMaterial == null)
        {
            duplicatedMaterial = new Material(baseMaterial);
        }
        else
        {
            duplicatedMaterial.CopyPropertiesFromMaterial(baseMaterial);
        }

        foreach (var modifier in materialModifiers)
        {
            duplicatedMaterial = modifier.GetModifiedMaterial(duplicatedMaterial);
        }

        return duplicatedMaterial;
    }

    public void RegisterModifier(IMaterialModifier modifier)
    {
        materialModifiers.Add(modifier);
    }

    public void SetMaterialDirty()
    {
        if (graphic == null)
        {
            graphic = GetComponent<Graphic>();
        }

        graphic.SetMaterialDirty();
    }

    public void UnregisterModifier(IMaterialModifier modifier)
    {
        materialModifiers.Remove(modifier);
    }

    private void OnDestroy()
    {
        if (duplicatedMaterial != null)
        {
            if (Application.isPlaying)
            {
                Destroy(duplicatedMaterial);
            }
            else
            {
                DestroyImmediate(duplicatedMaterial);
            }
        }
    }
}
