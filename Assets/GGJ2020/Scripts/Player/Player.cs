﻿using UnityEngine;
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
    [SerializeField]
    private Status status = null;

    public UnityAction<Vector3, Vector3> OnPositionChanged = null;
    public UnityAction<Vector3, Vector3, Collider2D> OnPositionRejected = null;

    public string maskLabel; // this should really just be named layer but whatever

    private void OnValidate()
    {
        this.Autofill(ref status, true);
    }

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

            Status otherStatus = hit.collider.GetComponentInParent<Status>();
            if (otherStatus != null)
            {
                Debug.Log($"{otherStatus}", otherStatus);
                otherStatus.takeDamageFrom(this.status);
            }

            OnPositionRejected?.Invoke(oldPosition, newPosition, hit.collider);

            return false;
        }
    }
}
