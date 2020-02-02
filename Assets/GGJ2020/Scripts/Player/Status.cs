using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class Status : MonoBehaviour
{
    public string id; // matches both an object and a layer name.
    public string enemyDataType; // Should match the type of the enemy in the data.data json

    public int health = 5;
    public int attack = 1;

    CinemachineImpulseSource impulseSource = null;

    public UnityAction OnDead = null;

    private void Start()
    {
        this.Autofill(ref impulseSource, true);
        LoadKey($"{enemyDataType}_Health", ref this.health);
        LoadKey($"{enemyDataType}_Damage", ref this.attack);

        this.health = Mathf.Max(this.health, 1);
    }

    private void LoadKey(string key, ref int value)
    {
        string strValue = null;
        int intValue = 0;
        if (!KeyValueFormatParser.keyValuePairs.TryGetValue(key, out strValue))
        {
            Debug.LogError($"{key} not defined in Data.data!");
        }
        else if (!int.TryParse(strValue, out intValue))
        {
            Debug.LogError($"{key} is not an integer!");
        }
        else
        {
            value = intValue;
        }
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
            impulseSource.GenerateImpulse(Random.onUnitSphere * Mathf.Clamp(damage / 10f, 1f, 50f));
        }
    }
}
