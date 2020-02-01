using System.Collections.Generic;
using UnityEngine;

public class ReferenceHolder : MonoBehaviour
{
    [SerializeField]
    private List<ScriptableObject> refs = new List<ScriptableObject>();
}
