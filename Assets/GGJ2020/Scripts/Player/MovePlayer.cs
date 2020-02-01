using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private const string keyBindingPath = "Data/keys.data";

    private string upKey = null;
    private string leftKey = null;
    private string rightKey = null;
    private string downKey = null;

    void Start()
    {
        LoadKeyBindings();
    }

    private void LoadKeyBindings()
    {
        Dictionary<string, string> keyBindings = KeyValueFormatParser.Parse(MetaLoadUtil.GetPath(keyBindingPath));

        if (keyBindings == null)
        {
            Debug.LogError("Key bindings file not found!");
            return;
        }

        MapKey(keyBindings, "up", out upKey);
        MapKey(keyBindings, "down", out downKey);
        MapKey(keyBindings, "left", out leftKey);
        MapKey(keyBindings, "right", out rightKey);
    }

    private void MapKey(Dictionary<string, string> map, string key, out string value)
    {
        if (!map.TryGetValue(key, out value))
        {
            Debug.LogError($"Key '{key}' not found in keys.data!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.upKey != null && Input.GetKeyDown(this.upKey))
        {
            this.transform.position += Vector3.up;
            PingEnemeyToPerformAction();
        }
        if (this.downKey != null && Input.GetKeyDown(this.downKey))
        {
            this.transform.position += Vector3.down;
            PingEnemeyToPerformAction();
        }
        if (this.leftKey != null && Input.GetKeyDown(this.leftKey))
        {
            this.transform.position += Vector3.left;
            PingEnemeyToPerformAction();
        }
        if (this.rightKey != null && Input.GetKeyDown(this.rightKey))
        {
            this.transform.position += Vector3.right;
            PingEnemeyToPerformAction();
        }
    }

    private void PingEnemeyToPerformAction()
    {
        GameObject enemyobject = GameObject.Find("Enemy");
        MoveEnemy enemyscript = enemyobject.GetComponent<MoveEnemy>();
        enemyscript.PerformAction();
    }
}
