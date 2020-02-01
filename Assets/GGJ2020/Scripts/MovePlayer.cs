using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // TODO: Load these from a file!!
    public string upKey = "w";
    public string leftKey = "a";
    public string rightKey = "d";
    public string downKey = "s";

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(this.upKey))
        {
            Debug.Log(this.upKey);
            this.transform.position += Vector3.up;
        }
        if(Input.GetKeyDown(this.downKey))
        {
            Debug.Log(this.downKey);
            this.transform.position += Vector3.down;
        }
        if(Input.GetKeyDown(this.leftKey))
        {
            Debug.Log(this.leftKey);
            this.transform.position += Vector3.left;
        }
        if(Input.GetKeyDown(this.rightKey))
        {
            Debug.Log(this.rightKey);
            this.transform.position += Vector3.right;
        }
    }
}
