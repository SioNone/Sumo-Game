using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void ReturnToStart()
    {
        SceneManager.LoadScene("PlayerSelect");
    }
}
