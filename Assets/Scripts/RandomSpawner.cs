using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] itemPrefab;
    [SerializeField] float secondSpawn = 1f;
    [SerializeField] float minTras;
    [SerializeField] float maxTras;

    public float radius = 1f;

    void Start()
    {
        StartCoroutine(SpawnObjectAtRandom());
    }

    IEnumerator SpawnObjectAtRandom()
    {
        while (true)
        {
            var wanted = Random.Range(minTras, maxTras);
            Vector3 randomPos = Random.insideUnitCircle * radius;
            GameObject gameObject = Instantiate(itemPrefab[Random.Range(0, itemPrefab.Length)], randomPos, Quaternion.identity);
            yield return new WaitForSeconds(secondSpawn);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(this.transform.position, radius);
    }
}
