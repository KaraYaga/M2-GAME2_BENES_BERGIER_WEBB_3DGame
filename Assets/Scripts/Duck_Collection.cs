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
    [SerializeField] private LayerMask throwLayer;
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

// Check Duck Count
    void Update()
    {
        //DUCK COLLECTION AND THROWING CALL IN CHARACTER MOVEMENT
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    CollectDuckInRange();
        //    UpdateDuckCountText();
        //}

        //if (Input.GetMouseButtonDown(0)) // Left mouse click to throw duck
        //{
        //    ThrowDuck();
        //}
    }

// Collecting Ducks
    private void CollectDuckInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, collectionRange);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Duck"))
            {
                CollectDuck(collider.gameObject);
                return;
            }
        }
    }

    private void CollectDuck(GameObject duckObject)
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
    private void ThrowDuck()
    {
        if (currentDucks > 0)
        {
            currentDucks--;
            UpdateDuckCountText();

            if (collectedDucks.Count > 0)
            {
                // Randomly select a duck from the collected ducks
                int randomIndex = Random.Range(0, collectedDucks.Count);
                GameObject duckPrefab = collectedDucks[randomIndex];

                // Raycast to determine where to throw the duck
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, throwLayer))
                {
                    // Calculate the direction to throw the duck
                    Vector3 throwDirection = hit.point - transform.position;
                    throwDirection.y = 0f; // Ensure the throw is along the horizontal plane

                    // Spawn the duck and apply force
                    GameObject duckInstance = Instantiate(duckPrefab, transform.position, Quaternion.identity);
                    Rigidbody duckRigidbody = duckInstance.GetComponent<Rigidbody>();
                    duckRigidbody.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);
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