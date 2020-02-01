using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    [SerializeField]
    private Vector3 axis = Vector3.up;

    [SerializeField]
    private float speed = 10.0f;

    private void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(speed * Time.deltaTime, axis);
    }
}
