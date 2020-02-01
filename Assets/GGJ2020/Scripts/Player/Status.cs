using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public int health = 5; // TODO: Read this from a file!

    public void takeDamage(int damage) {
        this.health -= damage;
        Debug.Log("Took " + damage);
        if(this.health <= 0) {
            Debug.Log("Died");
        }
    }
}
