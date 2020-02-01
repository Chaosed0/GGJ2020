using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Status : MonoBehaviour
{
    public string id; // matches both an object and a layer name.

    public int health = 5; // TODO: Read this from a file!
    public int attack = 1; // TODO: Read this from a file!
    // TODO: You can put a bunch of other stats here like defense or critical chance just to obfuscate the files.

    public bool isDead()
    {
        return this.health <= 0;
    }

    public void takeDamageFrom(Status attacker) {
        if (this.health <= 0)
        {
            return;
        }
        var damage = attacker.attack;
        this.health -= damage;
        Debug.Log(this.id + " took " + damage + " damage");
        if (this.isDead())
        {
            Debug.Log(this.id + " died");
            GameObject.Destroy(GameObject.Find(this.id));

            if (this.id == "Player")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
