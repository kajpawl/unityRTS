﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player[] players;

    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    // returns a random enemy player
    public Player GetRandomEnemyPlayer(Player me)
    {
        Player ranPlayer = players[Random.Range(0, players.Length)];

        while (ranPlayer == me)
        {
            ranPlayer = players[Random.Range(0, players.Length)];
        }

        return ranPlayer;
    }
}
