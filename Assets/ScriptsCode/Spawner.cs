using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Hazard[] prefabs;

    void Start()
    {

        InvokeRepeating(nameof(Spawn), 1f, 2f);
    }

    void Spawn()
    {
        if (prefabs.Length == 0) return;

        float randomX = Random.Range(-8f, 8f);
        Vector3 spawnPos = new Vector3(randomX, transform.position.y, 0);

        Hazard randomPrefab = prefabs[Random.Range(0, prefabs.Length)];
        Instantiate(randomPrefab, spawnPos, Quaternion.identity);
    }
}