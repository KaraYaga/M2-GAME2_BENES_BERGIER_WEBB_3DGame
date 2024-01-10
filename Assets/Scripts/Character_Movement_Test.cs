using Unity.VisualScripting;
using UnityEngine;

public class Character_Movement_Test : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float collectionRange = 2f;

    private Duck_Collection duckCollection;

    void Start()
    {
        duckCollection = GetComponent<Duck_Collection>();
    }

    void Update()
    {
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.F))
        {
            CollectDuckInRange();
        }

        if (Input.GetMouseButtonDown(0))
        {
            ThrowDuck();
            Debug.Log("Duck thrown!");
        }
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    void CollectDuckInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, collectionRange);
        Debug.Log("Duck detection started!");

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Duck"))
            {
                Debug.Log("Duck detected!");
                duckCollection.CollectDuck(collider.gameObject);
                return;
            }
        }

        Debug.Log("No duck found in range.");
    }

    void ThrowDuck()
    {
        duckCollection.ThrowDuck();
    }

    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
}
