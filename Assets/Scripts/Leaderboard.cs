using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private GameObject leaderboard;

    [SerializeField]
    private GameObject playerEntry;

    public List<string> playerList = new List<string>();

    public List<string> GameComplete()
    {
        return playerList;
    }
}
