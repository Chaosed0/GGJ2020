using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public enum Direction
{
    Left,
    Right,
    Down,
    Up
}

public class Player : MonoBehaviour
{
    public UnityAction<Vector3, Vector3> OnPositionChanged = null;
    public UnityAction<Vector3, Vector3> OnPositionRejected = null;

    public string maskLabel; // this should really just be named layer but whatever

    private Vector2 DirectionToVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.Down:
                return Vector2.down;
            case Direction.Up:
                return Vector2.up;
            case Direction.Right:
                return Vector2.right;
            case Direction.Left:
                return Vector2.left;
        }

        return Vector2.zero;
    }

    public bool TryMove(Direction direction)
    {
        var vector = DirectionToVector(direction);
        var hit = Physics2D.Raycast(this.transform.position, vector, 1f, ~LayerMask.GetMask(maskLabel));

        var oldPosition = this.transform.position;
        var newPosition = this.transform.position + Util.three(vector);

        if (hit.collider == null)
        {
            this.transform.position = newPosition;
            OnPositionChanged?.Invoke(oldPosition, newPosition);
            return true;
        }
        else
        {
            if (maskLabel == "Player" && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Status playerStatus = hit.collider.gameObject.GetComponent<Status>();
                playerStatus.takeDamage(1); // TODO: Read this from somewhere!!
            }
            if (maskLabel == "Enemey" && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Status enemyStatus = hit.collider.gameObject.GetComponent<Status>();
                enemyStatus.takeDamage(1); // TODO: Read this from somewhere!
            }
            OnPositionRejected?.Invoke(oldPosition, newPosition);
            return false;
        }
    }
}
