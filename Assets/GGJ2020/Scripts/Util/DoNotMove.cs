using UnityEngine;

public class DoNotMove : MonoBehaviour
{
    private Vector3 position;

    private void Awake()
    {
        this.position = transform.position;
    }

    private void LateUpdate()
    {
        this.transform.position = this.position;
    }
}
