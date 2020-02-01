using UnityEngine;
using System.Collections.Generic;

public enum AutofillMode
{
    LookOnlyInSelf,
    LookInChildren,
    LookInParents
}

public static class AutofillComponentExtension
{
    public static void Autofill<T>(this MonoBehaviour mb, ref T field, bool nullable, bool lookInParents)
    {
        mb.Autofill(ref field, nullable, (lookInParents ? AutofillMode.LookInParents : AutofillMode.LookOnlyInSelf));
    }

    public static void Autofill<T>(this MonoBehaviour mb, ref T field, bool nullable = false, AutofillMode mode = AutofillMode.LookOnlyInSelf)
    {
        if (field == null)
        {
            if (mode == AutofillMode.LookOnlyInSelf)
            {
                field = mb.GetComponent<T>();
            }
            else if (mode == AutofillMode.LookInParents)
            {
                field = mb.GetComponentInParent<T>();
            }
            else if (mode == AutofillMode.LookInChildren)
            {
                field = mb.GetComponentInChildren<T>();
            }

#if UNITY_EDITOR
            if (field != null && !Application.isPlaying)
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(mb.gameObject.scene);
                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(mb);
            }
#endif
        }

        if (field == null && !nullable)
        {
            Debug.LogError($"Component of type {typeof(T)} is not assigned to and no component was found on the object", mb);
        }
    }

    public static void Autofill<T>(this MonoBehaviour mb, ref List<T> field, bool nullable = false, AutofillMode mode = AutofillMode.LookOnlyInSelf)
    {
        if (field == null || field.Count == 0)
        {
            if (mode == AutofillMode.LookOnlyInSelf)
            {
                mb.GetComponents<T>(field);
            }
            else if (mode == AutofillMode.LookInParents)
            {
                mb.GetComponentsInParent<T>(true, field);
            }
            else if (mode == AutofillMode.LookInChildren)
            {
                mb.GetComponentsInChildren<T>(true, field);
            }

#if UNITY_EDITOR
            if (field != null && field.Count > 0 && !Application.isPlaying)
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(mb.gameObject.scene);
                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(mb);
            }
#endif
        }

        if (field == null && !nullable)
        {
            Debug.LogError($"Component of type {typeof(T)} is not assigned to and no component was found on the object", mb);
        }
    }
}
