using Unity.VisualScripting;
using UnityEngine;

public class Character_Movement_Test : MonoBehaviour
{
    [SerializeField] private float speed = 5f, turnSpeed = 360;
    private Rigidbody rb;
    private Vector3 input;
    private Vector3 relative;
    private Quaternion rotate;
    
    private Duck_Collection duckCollection;

    void Start()
    {
        duckCollection = GetComponent<Duck_Collection>(); 
        rb = GetComponent<Rigidbody>();
    }

// Update is called once per frame
    void Update()
    {
        GatherInput();
        Look();

        if (Input.GetKeyDown(KeyCode.F)) //F Button to collect ducks
        {
            Duck_Collection.instance.CollectDuckInRange();
        }

        if (Input.GetMouseButtonDown(0)) // Left mouse click to throw duck
        {
            Duck_Collection.instance.ThrowDuck();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GatherInput()
    {
        input = new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal"), 0, UnityEngine.Input.GetAxisRaw("Vertical"));
    }

    private void Look()
    {
        if (input == Vector3.zero)
        {
            return;
        }

        relative = (transform.position + input) - transform.position;
        rotate = Quaternion.LookRotation(relative, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, turnSpeed * Time.deltaTime);

    }

    private void Move()
    {
        rb.MovePosition(transform.position + (transform.forward * input.normalized.magnitude) * speed * Time.deltaTime);
    }
}
