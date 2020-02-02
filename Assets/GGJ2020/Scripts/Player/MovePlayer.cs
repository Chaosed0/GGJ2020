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

    private void Start()
    {
        LoadKeyBindings();
    }

    private void LoadKeyBindings()
    {
        upKey = MapKey(KeyValueFormatParser.keyValuePairs, "Up");
        downKey = MapKey(KeyValueFormatParser.keyValuePairs, "Down");
        leftKey = MapKey(KeyValueFormatParser.keyValuePairs, "Left");
        rightKey = MapKey(KeyValueFormatParser.keyValuePairs, "Right");
    }

    public bool AreKeysCorrectlyBound()
    {
        if (upKey.ToLower() == "w" || upKey.ToLower() == "up")
        {
            if (downKey.ToLower() == "s" || downKey.ToLower() == "down")
            {
                if (leftKey.ToLower() == "a" || leftKey.ToLower() == "left")
                {
                    if (rightKey.ToLower() == "d" || rightKey.ToLower() == "right")
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private string MapKey(Dictionary<string, string> map, string key)
    {
        string strValue;
        if (!map.TryGetValue(key, out strValue) ||
            string.IsNullOrEmpty(strValue))
        {
            Debug.LogError($"Key '{key}' is unmapped!");
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

    private static WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    private void PingEnemeyToPerformAction()
    {
        StartCoroutine(BlahCoroutine());
    }

    private IEnumerator BlahCoroutine()
    {
        yield return waitForFixedUpdate;
        GameObject enemyobject = GameObject.FindGameObjectWithTag("Enemy");
        if (enemyobject != null)
        {
            MoveEnemy enemyscript = enemyobject.GetComponent<MoveEnemy>();
            enemyscript.PerformAction();
        }
    }
}
