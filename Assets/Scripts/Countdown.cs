using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField]
    private float countdown;

    [SerializeField]
    private TMP_Text countdownText;

    public static bool gameStarted = false;

    void Update()
    {
        if (countdown <= 1)
        {
            countdownText.text = "SUMO!";
            StartCoroutine(DestroyCountdown());
            gameStarted = true;
        }
        else
        {
            countdownText.text = countdown.ToString("0");
            countdown -= Time.deltaTime;
        }
    }

    IEnumerator DestroyCountdown()
    {
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
    }
}
