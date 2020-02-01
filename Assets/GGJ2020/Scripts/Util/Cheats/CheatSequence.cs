using UnityEngine;
using System.Collections.Generic;

public class CheatSequence
{
    List<KeyCode> keySequence;
    int count = 0;

    public CheatSequence(List<KeyCode> sequence)
    {
        this.keySequence = sequence;
    }

    public CheatSequence(string s)
    {
        var keyCodeList = new List<KeyCode> { KeyCode.BackQuote };
        foreach (char c in s.ToLower())
        {
            keyCodeList.Add(KeyCode.A + (c - 'a'));
        }
        keyCodeList.Add(KeyCode.Return);
        this.keySequence = keyCodeList;
    }

    public bool CheckCheat()
    {
        if (Input.GetKeyDown(keySequence[count]))
        {
            count++;
            if (count >= keySequence.Count)
            {
                count = 0;
                return true;
            }
        }
        else if (Input.anyKeyDown)
        {
            count = 0;
        }

        return false;
    }
}
