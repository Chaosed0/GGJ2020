using UnityEngine;
using System.Collections;

public class FollowOther : MonoBehaviour
{
    [SerializeField]
    private Transform mockParent = null;

    private Vector3 localPosition = Vector3.zero;
    private Quaternion localRotation = Quaternion.identity;
    private Vector3 localScale = Vector3.one;

    private void Awake()
    {
        localPosition = this.transform.position - mockParent.transform.position;
        localRotation = this.transform.rotation * Quaternion.Inverse(mockParent.transform.rotation);
        localScale = Divide(this.transform.localScale, mockParent.transform.localScale);
    }

    private void LateUpdate()
    {
        this.transform.position = mockParent.transform.position + localPosition;
        this.transform.rotation =  mockParent.transform.rotation * localRotation;
        this.transform.localScale = mockParent.transform.localScale + localScale;
    }

    private Vector3 Divide(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }
}
