using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static int numPlayers;

    private string[] controllerList;

    [SerializeField]
    private GameObject controllerWarning;

    [SerializeField]
    private TMP_Text warningText;

    [SerializeField]
    private AudioClip clickSound;

    void Start()
    {
        controllerList = Input.GetJoystickNames();
    }

    public void TwoPlayers()
    {
        SFXManager.instance.PlaySFX(clickSound, transform, 0.75f);

        if (controllerList.Length == 2)
        {
            numPlayers = 2;
            StartCoroutine(NextScene());
        }
        else
        {
            PlayWarning(2);
        }
    }

    public void ThreePlayers()
    {
        SFXManager.instance.PlaySFX(clickSound, transform, 0.75f);

        if (controllerList.Length == 3)
        {
            numPlayers = 3;
            StartCoroutine(NextScene());
        }
        else
        {
            PlayWarning(3);
        }
    }

    public void FourPlayers()
    {
        SFXManager.instance.PlaySFX(clickSound, transform, 0.75f);

        if (controllerList.Length == 4)
        {
            numPlayers = 4;
            StartCoroutine(NextScene());
        }
        else
        {
            PlayWarning(4);
        }
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Game");
    }

    public void PlayWarning(int numSelected)
    {
        controllerWarning.SetActive(true);
        warningText.text = "Connect " + numSelected.ToString() + " controllers!";
    }
}
