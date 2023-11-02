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

    public static List<string> playerList;

    // Start is called before the first frame update
    void Start()
    {
        playerList = null;
    }

    public void UpdateLeaderboard(string playerNumber)
    {
        playerList.Add(playerNumber);
    }

    public void GameComplete()
    {
        int i = playerList.Count;
        foreach (string player in playerList)
        {
            GameObject entry = Instantiate(playerEntry);
            entry.transform.parent = leaderboard.gameObject.transform;
            TMP_Text entryText = entry.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            entryText.text = i + ")" + "Player " + player;
            i--;
        }
    }
}
