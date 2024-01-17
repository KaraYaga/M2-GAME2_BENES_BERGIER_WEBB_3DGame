using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterMovement : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] GameObject playerAvatar;
    [SerializeField] private float enemyKnockback = 15f, health = 15;
    private Rigidbody rb;
    private Animator animator;
    private bool throwDuck;

    [Header("Movement")]
    [SerializeField] private float dashTime;
    [SerializeField] private float speed = 5f, turnSpeed = 360f, dashSpeed = 150f;
    private Vector3 input;
    private Vector3 relative;
    private Vector3 startVelocity;
    private Quaternion rotate;
    private bool isDash;

    [Header("Knockback")]
    [SerializeField] float knockback;
    [SerializeField] float timeOfKnockback;
    private bool isBeingKnockback;
    private Vector3 knockbackLocation;
    private Vector3 knockBackAngle;
    private GameObject gameObjectForward;

    [Header("Invincibility")]
    [SerializeField] GameObject meshRenderer;
    [SerializeField] float timeOfInvincibility;
    private bool isInvincible;
    private Color oldColor;
    //private Color newColor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = playerAvatar.GetComponent<Animator>();
        oldColor = meshRenderer.GetComponent<Renderer>().material.color;
        //newColor = new Color(oldColor.r, oldColor.g, oldColor.b, newAlpha);
        startVelocity = rb.velocity;
    }

    void Update()
    {
        Animation();
        GatherInput();
        Look();

        if (health <= 0)
        {
            Die();
        }

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.E)) && !isDash) //Dash
        {
            isDash = true;
        }

        if (Input.GetKeyDown(KeyCode.F)) //F Button to collect ducks
        {
            Duck_Collection.instance.CollectDuckInRange();
        }

        if (Input.GetMouseButtonDown(1))
        {
            throwDuck = true;
        }
        if (isBeingKnockback)
        {
            knockBackAngle = gameObjectForward.transform.forward;
            transform.position += new Vector3(knockBackAngle.x, gameObject.transform.forward.y, knockBackAngle.z) * knockback * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (throwDuck)
        {
            Duck_Collection.instance.ThrowDuck();
            throwDuck = false;
        }
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

    public void SetLife(float damage, GameObject gameObjectDirection)
    {
        if (!isInvincible)
        {
            health -= damage;
            Debug.Log(health);
            isInvincible = true;
            StartCoroutine("Invincibility", timeOfInvincibility);
        }

        isBeingKnockback = true;
        gameObjectForward = gameObjectDirection;
        StartCoroutine("Knockback", knockback);
    }

    private IEnumerator Knockback(float knockback)
    {
        yield return new WaitForSeconds(timeOfKnockback);
        isBeingKnockback = false;
    }

    private IEnumerator Invincibility(float invincible)
    {
        meshRenderer.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(invincible);
        meshRenderer.GetComponent<Renderer>().material.color = oldColor;
        yield return new WaitForSeconds(invincible);
        meshRenderer.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(invincible);
        meshRenderer.GetComponent<Renderer>().material.color = oldColor;
        yield return new WaitForSeconds(invincible);
        meshRenderer.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(invincible);
        meshRenderer.GetComponent<Renderer>().material.color = oldColor;
        yield return new WaitForSeconds(invincible);
        meshRenderer.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(invincible);
        meshRenderer.GetComponent<Renderer>().material.color = oldColor;
        yield return new WaitForSeconds(invincible);
        yield return new WaitForSeconds(invincible);

        isInvincible = false;
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

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var currentAnimLayer = animator.GetCurrentAnimatorClipInfo(0);
        var curentAnimName = currentAnimLayer[0].clip.name;

        if (other.gameObject.tag == "Enemy")
        {
            if (curentAnimName == "attack")
            {
                other.gameObject.GetComponent<EnemyScript>().SetLife(1f, enemyKnockback, gameObject);
            }            
        }
    }

    private void Die()
    {
        //
    }
}
