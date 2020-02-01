using UnityEngine;

public class FixedFrameCounter : MonoBehaviour
{
    public static int fixedFrameCount = 0;

    private void OnEnable()
    {
        fixedFrameCount = 0;
    }

    private void FixedUpdate()
    {
        ++fixedFrameCount;
    }
}
