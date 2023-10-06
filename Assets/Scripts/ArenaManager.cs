using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public float arenaShrinkRate, minArenaSize;
    public GameObject innerCircle, outerCircle;

    // Update is called once per frame
    void Update()
    {
        if (innerCircle.transform.localScale.y > minArenaSize)
        {
            innerCircle.transform.localScale = new Vector2(innerCircle.transform.localScale.x + Time.deltaTime * arenaShrinkRate, innerCircle.transform.localScale.y + Time.deltaTime * arenaShrinkRate);
            outerCircle.transform.localScale = new Vector2(outerCircle.transform.localScale.x + Time.deltaTime * arenaShrinkRate, outerCircle.transform.localScale.y + Time.deltaTime * arenaShrinkRate);
        }
    }
}
