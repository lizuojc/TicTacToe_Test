using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigLoader : Singleton<ConfigLoader>
{
    public Config gameConfig;

    private void Start()
    {
        if (gameConfig == null)
        {
            Debug.LogError("gameConfig√ª”–…Ë÷√£°");
        }
    }
}
