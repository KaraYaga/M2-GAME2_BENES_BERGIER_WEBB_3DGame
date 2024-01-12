using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f, turnSpeed = 360f, dashSpeed = 150f, dashTime;
    [SerializeField] GameObject playerAvatar;
    private Rigidbody rb;
    private Animator animator;
    private Vector3 input;
    private Vector3 relative;
    private Vector3 startVelocity;
    private Quaternion rotate;

    private bool isDash;

    private Duck_Collection duckCollection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = playerAvatar.GetComponent<Animator>();
        duckCollection = GetComponent<Duck_Collection>();
        startVelocity = rb.velocity;
    }

    void Update()
    {
        Animation();
        GatherInput();
        Look();

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.E)) && !isDash) //Dash
        {
            isDash = true;
        }

        if (Input.GetKeyDown(KeyCode.F)) //F Button to collect ducks
        {
            Duck_Collection.instance.CollectDuckInRange();
        }

        //if (Input.GetMouseButtonDown(1)) // Left mouse click to throw duck
        //{
        //    Duck_Collection.instance.SeeShootDuck();
        //}
        if (Input.GetMouseButtonUp(1))
        {
            Duck_Collection.instance.ThrowDuck();
        }
    }

    private void FixedUpdate()
    {
        Move();

        if(isDash)
        {
            StartCoroutine(Dash());
        }
        else if(!isDash)
        {
            rb.velocity = startVelocity;
        }
    }

    private void GatherInput()
    {
        input = new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal"), 0, UnityEngine.Input.GetAxisRaw("Vertical"));
    }

    IEnumerator Dash()
    {
        animator.SetBool("Move", false);
        animator.SetBool("Dashing", true);
        rb.MovePosition(transform.position + (transform.forward * input.normalized.magnitude) * speed * dashSpeed * Time.deltaTime);
        yield return new WaitForSeconds(dashTime);
        isDash = false;
        animator.SetBool("Dashing", false);
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
            if (input != Vector3.zero && !isDash)
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
