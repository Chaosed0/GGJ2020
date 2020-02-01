using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    private const string keyBindingPath = "Data/keys.data";

    private KeyCode upKey = KeyCode.None;
    private KeyCode leftKey = KeyCode.None;
    private KeyCode rightKey = KeyCode.None;
    private KeyCode downKey = KeyCode.None;

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
        Dictionary<string, string> keyBindings = KeyValueFormatParser.Parse(MetaLoadUtil.GetPath(keyBindingPath));

        if (keyBindings == null)
        {
            Debug.LogError("Key bindings file not found!");
            return;
        }

        upKey = MapKey(keyBindings, "up");
        downKey = MapKey(keyBindings, "down");
        leftKey = MapKey(keyBindings, "left");
        rightKey = MapKey(keyBindings, "right");
    }

    private KeyCode MapKey(Dictionary<string, string> map, string key)
    {
        string strValue;
        if (!map.TryGetValue(key, out strValue))
        {
            Debug.LogError($"Key '{key}' not found in keys.data!");
            return KeyCode.None;
        }

        int intValue;
        if (!int.TryParse(strValue, out intValue))
        {
            Debug.LogError($"Value for key '{key}' is not an integer!");
            return KeyCode.None;
        }

        if (!System.Enum.IsDefined(typeof(KeyCode), intValue))
        {
            Debug.LogError($"Value {intValue} for key '{key}' is not a valid KeyCode!");
            return KeyCode.None;
        }

        return (KeyCode)intValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.upKey != KeyCode.None && Input.GetKeyDown(this.upKey))
        {
            player.TryMove(Direction.Up);
            PingEnemeyToPerformAction();
        }
        if (this.downKey != KeyCode.None && Input.GetKeyDown(this.downKey))
        {
            player.TryMove(Direction.Down);
            PingEnemeyToPerformAction();
        }
        if (this.leftKey != KeyCode.None && Input.GetKeyDown(this.leftKey))
        {
            player.TryMove(Direction.Left);
            PingEnemeyToPerformAction();
        }
        if (this.rightKey != KeyCode.None && Input.GetKeyDown(this.rightKey))
        {
            player.TryMove(Direction.Right);
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
