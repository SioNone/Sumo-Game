using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static int numPlayers;

    [SerializeField]
    private AudioClip clickSound;

    public void TwoPlayers()
    {
        numPlayers = 2;
        StartCoroutine(NextScene());
    }

    public void ThreePlayers()
    {
        numPlayers = 3;
        StartCoroutine(NextScene());
    }

    public void FourPlayers()
    {
        numPlayers = 4;
        StartCoroutine(NextScene());
    }

    IEnumerator NextScene()
    {
        SFXManager.instance.PlaySFX(clickSound, transform, 0.75f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Game");
    }
}
