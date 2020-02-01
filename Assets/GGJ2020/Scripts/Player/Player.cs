using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

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

    ContactFilter2D contactFilter;
    List<RaycastHit2D> raycastHits = new List<RaycastHit2D>();
    public bool TryMove(Direction direction)
    {
        contactFilter.NoFilter();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(~LayerMask.GetMask(maskLabel));

        var vector = DirectionToVector(direction);
        var hitCount = Physics2D.Raycast(this.transform.position, vector, contactFilter, raycastHits, 1f);

        var oldPosition = this.transform.position;
        var newPosition = this.transform.position + Util.three(vector);

        if (hitCount <= 0)
        {
            this.transform.position = newPosition;
            OnPositionChanged?.Invoke(oldPosition, newPosition);
            return true;
        }
        else
        {
            var hit = raycastHits[0];

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
