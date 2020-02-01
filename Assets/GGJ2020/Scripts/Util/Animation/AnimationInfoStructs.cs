using System.Collections.Generic;

[System.Serializable]
public struct LayerInfo
{
    public int layerIndex;
    public string layerName;
    public List<string> states;
}

[System.Serializable]
public struct EventInfo
{
    public string clipName;
    public string eventName;
}

[System.Serializable]
public struct StateInfo
{
    public int layerIndex;
    public string stateName;
    public string motionName;
}

