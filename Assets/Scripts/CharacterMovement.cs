using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement instance;

    [Header("Health")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float health = 15, maxHealth = 15;

    [Header("Attributes")]
    [SerializeField] GameObject playerAvatar;
    [SerializeField] private float enemyKnockback = 15f;
    [SerializeField] ParticleSystem particleHit, particleCharacterDeath;
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

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "Sibelle 2")
        {
            health = ForNextLevelScript.Instance.life;
        }
        else if(sceneName == "Sibelle 1")
        {
            health = maxHealth;
        }

        rb = GetComponent<Rigidbody>();
        animator = playerAvatar.GetComponent<Animator>();
        oldColor = meshRenderer.GetComponent<Renderer>().material.color;
        startVelocity = rb.velocity;

        healthSlider.value = health;
    }

    void Update()
    {
        Animation();
        GatherInput();
        Look();

        if (health <= 0)
        {
            StartCoroutine(DestroyWithParticles());
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

        if(Duck_Collection.instance.GetDuckCount() >= 15)
        {
            SceneManager.LoadScene(4);
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

        if (isBeingKnockback)
        {
            knockBackAngle = gameObjectForward.transform.forward;
            transform.position += new Vector3(knockBackAngle.x, gameObject.transform.forward.y, knockBackAngle.z) * knockback * Time.deltaTime;
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
            healthSlider.value = health;
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

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
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
                Instantiate(particleHit, other.gameObject.transform.position, Quaternion.Euler(-90, 0, 0));

                other.gameObject.GetComponent<EnemyScript>().SetLife(1f, enemyKnockback, gameObject);
            }            
        }

        if(other.gameObject.tag == "Destruction")
        {
            other.gameObject.GetComponent<Destructable_Enviroment>().SetLife(1f);
        }

        if(other.gameObject.tag == "Boat")
        {
            if(Duck_Collection.instance.GetDuckCount() >= 5)
            {
                SceneManager.LoadScene(3);
            }
            else
            {
                Debug.Log("You don't have enough duck");
            }
            
        }
    }

    //Death
    public IEnumerator DestroyWithParticles()
    {
        Instantiate(particleCharacterDeath, transform.position, Quaternion.Euler(-90, 0, 0));

        yield return null;

        gameObject.SetActive(false);

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(5);

    }

    public float GetHealth()
    {
        return health;
    }
}
