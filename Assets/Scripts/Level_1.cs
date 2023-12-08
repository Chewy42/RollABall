using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_1 : BaseLevel
{
    void Awake()
    {
        _level = 1;
        _enemy_spawnrate = 2f;
        _max_bosses = 1;
    }
}
