using UnityEngine;

public static class Transform2DExtension
{
    public static void SetPosition2D(this Transform transform, Vector2 vec)
    {
        transform.position = new Vector3(vec.x, vec.y);
    }
}
