using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//SPAWNING DUCKS
public class DuckSpawner : MonoBehaviour
{
    public GameObject[] duckPrefabs;
    public float minSpawnInterval = 0.5f;
    public float maxSpawnInterval = 2.0f;
    public float spawnHeight = 20.0f;
    public float spawnLimit = -10.0f; // Set the limit where ducks will disappear

    private float timer = 0.0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= Random.Range(minSpawnInterval, maxSpawnInterval))
        {
            SpawnDuck();
            timer = 0.0f;
        }

        // Check for ducks below the spawn limit and destroy them
        CheckDucksForDisappearance();
    }

    void SpawnDuck()
    {
        // Choose a random duck prefab from the list
        GameObject randomDuckPrefab = duckPrefabs[Random.Range(0, duckPrefabs.Length)];

        Vector3 spawnPosition = new Vector3(
            Random.Range(-10.0f, 10.0f), // Adjust the X range as needed
            spawnHeight,
            Random.Range(-10.0f, 10.0f) // Adjust the Z range as needed
        );

        GameObject newDuck = Instantiate(randomDuckPrefab, spawnPosition, Quaternion.identity);

        // Attach a script to the duck to handle disappearing
        DuckDisappear duckDisappearScript = newDuck.AddComponent<DuckDisappear>();
        duckDisappearScript.spawnLimit = spawnLimit;
    }

    void CheckDucksForDisappearance()
    {
        // Check if any ducks are below the spawn limit and destroy them
        DuckDisappear[] ducks = FindObjectsOfType<DuckDisappear>();
        foreach (DuckDisappear duck in ducks)
        {
            duck.CheckAndDestroyIfBelowLimit(spawnLimit);
        }
    }
}

//DISAPPEARING DUCKS
public class DuckDisappear : MonoBehaviour
{
    public float spawnLimit = -10.0f; // Set the limit where ducks will disappear

    void Update()
    {
        // Empty for now; logic is moved to DuckSpawner
    }

    public void CheckAndDestroyIfBelowLimit(float limit)
    {
        // Check if the duck is below the specified limit and destroy it if true
        if (transform.position.y <= limit)
        {
            Destroy(gameObject);
        }
    }
}