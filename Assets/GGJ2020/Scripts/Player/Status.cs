using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public string id; // matches both an object and a layer name.

    private int health = 5; // TODO: Read this from a file!
    private int attack = 1; // TODO: Read this from a file!
    // TODO: You can put a bunch of other stats here like defense or critical chance just to obfuscate the files.

    public void takeDamageFrom(Status attacker) {
        if (this.health <= 0)
        {
            return;
        }
        var damage = attacker.attack;
        this.health -= damage;
        Debug.Log(this.id + " took " + damage + " damage");
        if (this.health <= 0)
        {
            Debug.Log(this.id + " died");
            GameObject.Destroy(GameObject.Find(this.id));
        }
    }
}
