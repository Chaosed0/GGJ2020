using UnityEngine;

namespace Obelus
{
    public class RotateAround: MonoBehaviour
    {
        public Transform target;
        public float speed;

        private void Update()
        {
            Vector3 targetPosition = target?.position ?? Vector3.zero;
            Vector3 relative = transform.position - targetPosition;
            float angle = Mathf.Atan2(relative.x, relative.z);
            float newAngle = angle + speed * Time.deltaTime;
            transform.position = new Vector3(Mathf.Sin(newAngle) * relative.magnitude, transform.position.y, Mathf.Sin(newAngle) * relative.magnitude);
        }
    }
}
