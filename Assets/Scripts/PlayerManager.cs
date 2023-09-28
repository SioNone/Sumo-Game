using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static int numPlayers;

    public void TwoPlayers()
    {
        numPlayers = 2;
    }

    public void ThreePlayers()
    {
        numPlayers = 3;
    }

    public void FourPlayers()
    {
        numPlayers = 4;
    }

    public void NextScene()
    {
        SceneManager.LoadScene("Game");
    }
}
