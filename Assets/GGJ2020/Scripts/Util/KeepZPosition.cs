using UnityEngine;

public class KeepZPosition : MonoBehaviour
{
    private float z = 0f;

    private void Awake()
    {
        this.z = this.transform.position.z;
    }

    private void LateUpdate()
    {
        var position = this.transform.position;
        position.z = this.z;
        this.transform.position = position;
    }
}
