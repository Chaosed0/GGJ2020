using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    void Start()
    {
        // ...
    }

    // To be called whenever the player has
    // performed their action, and the enemies
    // get to take their turn to perform an action.
    public void PerformAction()
    {
        this.transform.position += Vector3.up;
        // TODO: Check if player is in their room
        // TODO: Naively move towards the player
        // TODO: Deal damage to the player
        // TODO: Game over if the player has not enough health?
    }
}
