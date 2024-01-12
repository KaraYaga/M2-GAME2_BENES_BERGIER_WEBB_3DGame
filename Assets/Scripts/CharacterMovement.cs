using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f, turnSpeed = 360;
    private Rigidbody rb;
    private Vector3 input;
    private Vector3 relative;
    private Quaternion rotate;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GatherInput();
        Look();
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

        //rotate = Quaternion.LookRotation(input.ToIso(), Vector3.up);

        relative = (transform.position + input) - transform.position;
        rotate = Quaternion.LookRotation(relative, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, turnSpeed * Time.deltaTime);

    }

    private void Move()
    {
        rb.MovePosition(transform.position + (transform.forward * input.normalized.magnitude) * speed * Time.deltaTime);
        //Vector3 movement = new Vector3(transform.forward.x * input.y * speed * Time.deltaTime,
        //    0, transform.forward.z * input.y * speed * Time.deltaTime);
        //rb.MovePosition(transform.position + movement);
    }
}
