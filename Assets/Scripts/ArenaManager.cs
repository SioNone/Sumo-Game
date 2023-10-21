using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public float arenaShrinkRate, minArenaSize;
    public GameObject arenaCircle;

    // Update is called once per frame
    void Update()
    {
        if (arenaCircle.transform.localScale.y > minArenaSize)
        {
            arenaCircle.transform.localScale = new Vector2(arenaCircle.transform.localScale.x - Time.deltaTime * arenaShrinkRate, arenaCircle.transform.localScale.y - Time.deltaTime * arenaShrinkRate);
        }
    }
}
