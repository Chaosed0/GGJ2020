using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MovePlayer : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    private const string keyBindingPath = "Data/keys.data";

    private string upKey = null;
    private string leftKey = null;
    private string rightKey = null;
    private string downKey = null;

    private void OnValidate()
    {
        this.Autofill(ref player, true);
    }

    void Start()
    {
        LoadKeyBindings();
    }

    private void LoadKeyBindings()
    {
        var path = MetaLoadUtil.GetPath(keyBindingPath);
        Dictionary<string, string> keyBindings = KeyValueFormatParser.Parse(path);

        if (keyBindings == null)
        {
            var fullPath = Path.GetFullPath(path);
            Debug.LogError($"Key bindings file not found at {fullPath}!");
            return;
        }

        upKey = MapKey(keyBindings, "up");
        downKey = MapKey(keyBindings, "down");
        leftKey = MapKey(keyBindings, "left");
        rightKey = MapKey(keyBindings, "right");
    }

    private string MapKey(Dictionary<string, string> map, string key)
    {
        string strValue;
        if (!map.TryGetValue(key, out strValue) ||
            string.IsNullOrEmpty(strValue))
        {
            Debug.LogError($"Key '{key}' is unmapped! Check keys.data");
            return null;
        }

        return strValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.upKey != null && Input.GetKeyDown(this.upKey))
        {
            player.TryMove(Direction.Up);
            PingEnemeyToPerformAction();
        }
        if (this.downKey != null && Input.GetKeyDown(this.downKey))
        {
            player.TryMove(Direction.Down);
            PingEnemeyToPerformAction();
        }
        if (this.leftKey != null && Input.GetKeyDown(this.leftKey))
        {
            player.TryMove(Direction.Left);
            PingEnemeyToPerformAction();
        }
        if (this.rightKey != null && Input.GetKeyDown(this.rightKey))
        {
            player.TryMove(Direction.Right);
            PingEnemeyToPerformAction();
        }
    }

    private void PingEnemeyToPerformAction()
    {
        GameObject enemyobject = GameObject.Find("Enemy");
        if (enemyobject != null)
        {
            MoveEnemy enemyscript = enemyobject.GetComponent<MoveEnemy>();
            enemyscript.PerformAction();
        }
    }
}
