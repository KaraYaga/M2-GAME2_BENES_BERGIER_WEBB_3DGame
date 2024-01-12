using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//SPAWNING DUCKS
public class DuckSpawner : MonoBehaviour
{
    [Header("Spawner Stats")]
    public GameObject[] duckPrefabs;
    [SerializeField]  public float minSpawnInterval = 0.5f;
    [SerializeField]  public float maxSpawnInterval = 2.0f;
    [SerializeField] public float spawnHeight = 20.0f;
    [SerializeField] private float gravityModifier = 1f;


    public float spawnEnd = -15.0f; // Set the limit where ducks will disappear
    private float timer = 0.0f;
    private Vector3 normalGravity;

    private void OnEnable()
    {
        normalGravity = Physics.gravity;
        Physics.gravity = new Vector3(0, -gravityModifier, 0);
    }

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

        // Calculate a Z position based on the range and a function that increases with distance
        float distanceFactor = Random.Range(0.0f, 1.0f); // Adjust the range as needed
        float zPosition = Mathf.Lerp(-20.0f, 30.0f, distanceFactor);

        Vector3 spawnPosition = new Vector3(
            Random.Range(-15.0f, 15.0f), // Adjust the X range as needed
            spawnHeight,
            zPosition
        );

        GameObject newDuck = Instantiate(randomDuckPrefab, spawnPosition, Quaternion.Euler(Random.Range(0, 359), Random.Range(0, 359), Random.Range(0, 359)));

        DuckDisappear duckDisappearScript = newDuck.AddComponent<DuckDisappear>();
        duckDisappearScript.spawnLimit = spawnEnd;
    }

    void CheckDucksForDisappearance()
    {
        // Check if any ducks are below the spawn limit and destroy them
        DuckDisappear[] ducks = FindObjectsOfType<DuckDisappear>();
        foreach (DuckDisappear duck in ducks)
        {
            duck.CheckAndDestroyIfBelowLimit(spawnEnd);
        }
    }
    private void OnDisable()
    {
        Physics.gravity = normalGravity;
    }
}

//DISAPPEARING DUCKS
public class DuckDisappear : MonoBehaviour
{
    public float spawnLimit = -15.0f; // Set the limit where ducks will disappear

    public void CheckAndDestroyIfBelowLimit(float limit)
    {
        // Check if the duck is below the specified limit and destroy it if true
        if (transform.position.y <= limit)
        {
            Destroy(gameObject);
        }
    }

    
}