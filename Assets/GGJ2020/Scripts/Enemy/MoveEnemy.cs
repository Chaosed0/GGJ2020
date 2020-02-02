using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    private void OnValidate()
    {
        this.Autofill(ref player, true);
    }

    void Start()
    {
        // ...
    }

    // To be called whenever the player has
    // performed their action, and the enemies
    // get to take their turn to perform an action.
    public void PerformAction()
    {
        Status status = this.gameObject.GetComponent<Status>();
        if (status.isDead())
        {
            return;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if(playerObject == null)
        {
            return;
        }

        if (playerObject.transform.position.x < 0)
        {
            return;
        }

        if(playerObject.transform.position.x < this.transform.position.x)
        {
            player.TryMove(Direction.Left);
        }
        else if(playerObject.transform.position.x > this.transform.position.x)
        {
            player.TryMove(Direction.Right);
        }
        else if(playerObject.transform.position.y < this.transform.position.y)
        {
            player.TryMove(Direction.Down);
        }
        else if(playerObject.transform.position.y > this.transform.position.y)
        {
            player.TryMove(Direction.Up);
        }

        // TODO: Check if player is in their room
        // TODO: Deal damage to the player on collision
        // TODO: Game over if the player has not enough health?
        // TODO: Let player kill enemy if they run into it
    }
}
