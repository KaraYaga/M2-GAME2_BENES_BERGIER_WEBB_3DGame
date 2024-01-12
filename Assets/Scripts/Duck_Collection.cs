using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Duck_Collection : MonoBehaviour
{
    public static Duck_Collection instance;
    [Header("Duck Collection")]
    [SerializeField] private int maxDucks = 15;
    [SerializeField] private int currentDucks;
    [SerializeField] private float collectionRange = 1;
    [SerializeField] private TextMeshProUGUI duckCountText;

    [Header("Duck Throw")]
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private LayerMask throwLayer; //Be set in Unity Editor
    [SerializeField] private GameObject[] duckPrefabs;
    [SerializeField] private GameObject mainCharacter;
    private Rigidbody rb;

    private List<GameObject> collectedDucks = new List<GameObject>();

// Start with NO DUCKS
    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();

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
    
// Detecting when ducks are in range
    public void CollectDuckInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, collectionRange);
        Debug.Log("Duck detection started!");

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Duck"))
            {
                Debug.Log("Duck detected!");
                CollectDuck(collider.gameObject);
                return;
            }
        }

        Debug.Log("No duck found in range.");
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

    //public void SeeShootDuck()
    //{
    //    if (currentDucks > 0)
    //    {
    //        currentDucks--;
    //        UpdateDuckCountText();

    //        if (collectedDucks.Count > 0)
    //        {
    //            int randomIndex = Random.Range(0, collectedDucks.Count);
    //            GameObject duckPrefab = collectedDucks[randomIndex];

    //            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //            //RaycastHit hit;

    //            //if (Physics.Raycast(ray, out hit, Mathf.Infinity, throwLayer))
    //            //{
    //            //    // Use hit.point as the position to spawn the duck
    //            //    Vector3 spawnPosition = hit.point;

    //            //    // Instantiate the duck at the spawn position
    //            //    GameObject duckInstance = Instantiate(duckPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
    //            //    duckInstance.SetActive(true);
    //            //    Rigidbody duckRigidbody = duckInstance.GetComponent<Rigidbody>();

    //            //    // Apply force in the direction of the hit point
    //            //    Vector3 throwDirection = (hit.point - transform.position).normalized;
    //            //    duckRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);
    //            //}
    //            //else
    //            //{
    //            //    Debug.Log("Failed to throw duck. Ensure the throwLayer is set correctly.");
    //            //}
    //        }
    //        else
    //        {
    //            Debug.Log("No ducks to throw!");
    //        }
    //    }
    //}

    // Throw them ducks
    public void ThrowDuck()
    {
        if (currentDucks > 0)
        {
            UpdateDuckCountText();

            int randomIndex = Random.Range(0, collectedDucks.Count);
            GameObject duckPrefab = collectedDucks[randomIndex];

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, throwLayer))
            {
                // Create the duck at the spawn position
                duckPrefab.SetActive(true);
                duckPrefab.transform.position = mainCharacter.transform.position + new Vector3(0, 0.5f, 0);
                duckPrefab.transform.rotation = Quaternion.LookRotation(hit.point);

                // Apply force in the direction of the hit point
                Vector3 throwDirection = (transform.position - hit.point).normalized;
                Vector2 newVelocity = throwDirection * throwForce;
                rb.velocity = newVelocity;
                currentDucks--;

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

// Update Duck Count
    private void UpdateDuckCountText()
    {
        if (duckCountText != null)
        {
            duckCountText.text = "Ducks: " + currentDucks.ToString();
        }
    }
}