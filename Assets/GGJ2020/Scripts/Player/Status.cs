using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class Status : MonoBehaviour
{
    private const string EnemyDataPath = "Data/data.data";
    private static EnemyData fullEnemyData = null;

    public string id; // matches both an object and a layer name.
    public string enemyDataType; // Should match the type of the enemy in the data.data json

    private int health = 5;
    private int attack = 1;

    CinemachineImpulseSource impulseSource = null;

    public UnityAction OnDead = null;

    [RuntimeInitializeOnLoadMethod]
    private static void ParseEnemyData()
    {
        var path = MetaLoadUtil.GetPath(EnemyDataPath);
        fullEnemyData = EnemyDataParser.Parse(path);
        if (fullEnemyData == null)
        {
            Debug.LogError($"Enemy data does not exist at {path}!");
        }
    }

    private void Start()
    {
        this.Autofill(ref impulseSource, true);
        LoadEntityData();
    }

    private void LoadEntityData()
    {
        var selfEnemyData = fullEnemyData.enemyDatas.Find((x) => x.name == enemyDataType);
        if (selfEnemyData == null)
        {
            Debug.LogError($"No entity named '{enemyDataType}' found in data.data, falling back to default stats");
            return;
        }

        this.health = selfEnemyData.health;
        this.attack = selfEnemyData.damage;
    }

    public bool isDead()
    {
        return this.health <= 0;
    }

    public void takeDamageFrom(Status attacker)
    {
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
            OnDead?.Invoke();
            GameObject.Destroy(this.gameObject);
        }

        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse(Random.onUnitSphere * Mathf.Min(damage, 999f));
        }
    }
}
