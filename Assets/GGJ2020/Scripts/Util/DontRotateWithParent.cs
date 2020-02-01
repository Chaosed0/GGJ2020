using UnityEngine;

public class DontRotateWithParent : MonoBehaviour
{
    private Quaternion orientation = Quaternion.identity;

    private void Awake()
    {
        this.orientation = this.transform.rotation;
    }

    private void LateUpdate()
    {
        this.transform.rotation = orientation;
    }
}
