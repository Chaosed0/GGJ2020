using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class EnemyData
{
    public string version = "v0";
    public List<SingleEnemyData> enemyDatas = new List<SingleEnemyData>();
}

public class SingleEnemyData
{
    public string name = "Enemy1";
    public int health = 0;
    public int damage = 0;
    public int speed = 0;
}

public class EnemyDataParser
{
    public static EnemyData Parse(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        var o = File.ReadAllText(path);
        var enemyData = JsonConvert.DeserializeObject<EnemyData>(o);

        return enemyData;
    }
}
