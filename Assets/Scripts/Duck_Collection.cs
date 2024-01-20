using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class Duck_Collection : MonoBehaviour
{
    public static Duck_Collection instance;

    [Header("Duck Collection")]
    [SerializeField] private int maxDucks = 15;
    [SerializeField] private static int currentDucks;
    [SerializeField] private float collectionRange = 1;
    [SerializeField] private TextMeshProUGUI duckCountText;
    private List<GameObject> collectedDucks = new List<GameObject>();

    [Header("Duck Throw")]
    [SerializeField] private float throwForce;
    [SerializeField] private float upForce, enemyRange;
    [SerializeField] private LayerMask throwLayer; //Be set in Unity Editor
    [SerializeField] private GameObject mainCharacter;

    [Header("Other")]
    [SerializeField] private GameObject FKey;
    [SerializeField] private float fKeyXOffset, fKeyYOffset;
    [SerializeField] private List<GameObject> alreadyCollectedDucks = new List<GameObject>();


    // Start with NO DUCKS
    void Start()
    {
        instance = this;

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "Sibelle 2")
        {
            currentDucks = 5;

            for(int i = 0; i < currentDucks; i++)
            {
                collectedDucks.Add(alreadyCollectedDucks[i]);
            }
            
        }
        else
        {
            currentDucks = 0;
        }

        FKey.SetActive(false);

        if (duckCountText == null)
        {
            Debug.LogError("TextMeshProUGUI component not assigned!");
        }
        else
        {
            UpdateDuckCountText();
        }
    }

    private void Update()
    {
        CheckDuckRange();
    }

    // Detecting when ducks are in range
    public void CheckDuckRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, collectionRange);
        Debug.Log("Duck detection started!");

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Duck"))
            {
                FKey.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position) + new Vector3(fKeyXOffset, fKeyYOffset, 0);
                FKey.SetActive(true);
                return;
            }
            else
            {
                FKey.SetActive(false);
            }
        }

        Debug.Log("No duck found in range.");
    }

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

            UpdateDuckCountText();

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, throwLayer))
            {
                // Create the duck at the spawn position
                int randomIndex = Random.Range(0, collectedDucks.Count);
                GameObject duckPrefab = collectedDucks[randomIndex];

                duckPrefab.transform.position = mainCharacter.transform.position + transform.up;
                duckPrefab.transform.LookAt(hit.transform.position);
                duckPrefab.GetComponent<Rigidbody>().velocity = Vector3.zero;

                float distance = Vector3.Distance(duckPrefab.transform.position, hit.transform.position);
                duckPrefab.SetActive(true);

                Vector3 throwDirection = (duckPrefab.transform.forward * throwForce * distance) + new Vector3(0, upForce, 0);
                duckPrefab.GetComponent<Rigidbody>().AddForce(throwDirection, ForceMode.VelocityChange);
                currentDucks--;

                collectedDucks.Remove(duckPrefab);

                UpdateDuckCountText();
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
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "Sibelle 2")
        {
            if (duckCountText != null)
            {
                duckCountText.text = currentDucks.ToString() + " / 15";
            }
        }
        else
        {
            if (duckCountText != null)
            {
                duckCountText.text = currentDucks.ToString() + " / 5";
            }
        }


    }

    public float GetDuckCount()
    {
        return currentDucks;
    }
}