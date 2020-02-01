using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb = null;

    public Vector3 rotation = Vector3.zero;

    [SerializeField]
    private bool useUnscaledTime = false;

    private void OnValidate()
    {
        this.Autofill(ref rb, true, AutofillMode.LookInParents);
    }

    private void Update()
    {
        if (rb == null)
        {
            float deltaTime = (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
            this.transform.rotation *= Quaternion.Euler(rotation * deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            float deltaTime = (useUnscaledTime ? Time.fixedUnscaledDeltaTime : Time.fixedUnscaledDeltaTime);
            rb.MoveRotation(rb.rotation + rotation.z * deltaTime);
        }
    }
}
