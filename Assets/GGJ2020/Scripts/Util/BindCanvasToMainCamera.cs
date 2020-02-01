using UnityEngine;

[RequireComponent(typeof(Canvas))]
[ExecuteInEditMode]
public class BindCanvasToMainCamera : MonoBehaviour
{
    private Canvas canvas = null;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        if (canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }
    }
}
