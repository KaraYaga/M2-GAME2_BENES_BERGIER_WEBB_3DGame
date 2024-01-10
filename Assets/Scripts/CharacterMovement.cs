using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f, turnSpeed = 360;
    [SerializeField] GameObject playerAvatar;
    //[SerializeField] Animator animator;
    private Rigidbody rb;
    private Animator animator;
    private Vector3 input;
    private Vector3 relative;
    private Quaternion rotate;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = playerAvatar.GetComponent<Animator>();
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Animation();
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

        relative = (transform.position + input) - transform.position;
        rotate = Quaternion.LookRotation(relative, Vector3.up);

        rotate *= Quaternion.AngleAxis(45, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, turnSpeed * Time.deltaTime);

    }

    private void Move()
    {
        rb.MovePosition(transform.position + (transform.forward * input.normalized.magnitude) * speed * Time.deltaTime);
    }

    private void Animation()
    {
        if (input.sqrMagnitude != 0)
        {
            if (input != Vector3.zero)
            {
                animator.SetBool("Move", true);
            }
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }
}
