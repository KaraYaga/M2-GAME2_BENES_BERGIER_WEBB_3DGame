using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Duck_Collection : MonoBehaviour
{
    [Header("Duck Collection")]
    [SerializeField] private int maxDucks = 15;
    [SerializeField] private int currentDucks;
    [SerializeField] private float collectionRange = 1;
    [SerializeField] private TextMeshProUGUI duckCountText;

    [Header("Duck Throw")]
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private LayerMask throwLayer; //Be set in Unity Editor
    [SerializeField] private GameObject[] duckPrefabs;

    private List<GameObject> collectedDucks = new List<GameObject>();

// Start with NO DUCKS
    void Start()
    {
        currentDucks = 0;

        if (duckCountText == null)
        {
            Debug.LogError("TextMeshProUGUI component not assigned!");
        }
        else
        {
            UpdateDuckCountText();
        }
    }

//Collect Ducks
    public void CollectDuck(GameObject duckObject)
    {
        if (currentDucks < maxDucks)
        {
            currentDucks++;
            Debug.Log("Duck collected!");

            duckObject.SetActive(false);
            collectedDucks.Add(duckObject);

        }
        else
        {
            Debug.Log("You have collected all your ducks!");
        }
    }

// Throw them ducks
    public void ThrowDuck()
    {
        if (currentDucks > 0)
        {
            currentDucks--;
            UpdateDuckCountText();

            if (collectedDucks.Count > 0)
            {
                int randomIndex = Random.Range(0, collectedDucks.Count);
                GameObject duckPrefab = collectedDucks[randomIndex];

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, throwLayer))
                {
                    // Use hit.point as the position to spawn the duck
                    Vector3 spawnPosition = hit.point;

                    // Instantiate the duck at the spawn position
                    GameObject duckInstance = Instantiate(duckPrefab, transform.position, Quaternion.identity);
                    duckInstance.SetActive(true);
                    Rigidbody duckRigidbody = duckInstance.GetComponent<Rigidbody>();

                    // Apply force in the direction of the hit point
                    Vector3 throwDirection = (hit.point - transform.position).normalized;
                    duckRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);
                }
                else
                {
                    Debug.Log("Failed to throw duck. Ensure the throwLayer is set correctly.");
                }
            }
            else
            {
                Debug.Log("No ducks to throw!");
            }
        }
    }

    // Update Duck Count
    private void UpdateDuckCountText()
    {
        if (duckCountText != null)
        {
            duckCountText.text = "Ducks: " + currentDucks.ToString();
        }
    }
}