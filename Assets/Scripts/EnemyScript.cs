using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float life = 5f;
    //[SerializeField] GameObject Enemy;
    [SerializeField] protected Animator animator;
    [SerializeField] ParticleSystem particleGhostDeath;
    //private Animator animator;

    [Header("Movement")]
    [SerializeField] private Transform target;
    [SerializeField] float speedToTurn = 2f, radiusFromPoint = 1f, angleFromPoint = 0f;


    [Header("Knockback")]
    [SerializeField] bool isBeingKnockback;
    [SerializeField] float knockback, timeOfKnockback;
    private Vector3 knockbackLocation;
    private Vector3 knockBackAngle;
    private GameObject gameObjectForward;

    [Header("Attack")]
    [SerializeField] private float speedToAttack; //knockbackOfAttack;
    protected bool isAttacking;
    public GameObject mainCharacter;


    private void Start()
    {
        animator = GetComponent<Animator>();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void Update()
    {
        if (life <= 0)
        {
            //Die();
        }
        if (gameObject == null)
        {
            Instantiate(particleGhostDeath);
        }
    }


    private void FixedUpdate()
    {
        target.transform.rotation = transform.rotation;

        if (!isAttacking && !isBeingKnockback)
        {
            animator.SetBool("Attacking", false);

            float x = target.position.x + Mathf.Cos(angleFromPoint) * radiusFromPoint;
            float y = target.position.y;
            float z = target.position.z + Mathf.Sin(angleFromPoint) * radiusFromPoint;

            transform.position = new Vector3(x, y, z);
            angleFromPoint += speedToTurn * Time.deltaTime;

            //Rotation
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            transform.Rotate(new Vector3(0, 90, 0), Space.World);

            knockbackLocation = mainCharacter.transform.position;
        }
        else if(!isBeingKnockback)
        {
            animator.SetBool("Attacking", true);
            
            transform.LookAt(mainCharacter.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, mainCharacter.transform.position, speedToAttack * Time.deltaTime);
            target.position += target.transform.forward * speedToAttack * Time.deltaTime;

            knockbackLocation = mainCharacter.transform.position;
        }
        else if (isBeingKnockback)
        {
            knockBackAngle = gameObjectForward.transform.forward;

            transform.position += new Vector3(knockBackAngle.x, gameObject.transform.forward.y, knockBackAngle.z) * knockback * Time.deltaTime;
            target.transform.position += new Vector3(knockBackAngle.x, gameObject.transform.forward.y, knockBackAngle.z) * knockback * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            isAttacking = false;
            collision.gameObject.GetComponent<CharacterMovement>().SetLife(1, gameObject);
            Debug.Log("attack");
        }
    }

    public void SetLife(float damage, float knockback, GameObject gameObjectDirection)
    {
        life -= damage;
        Debug.Log(life);
        isBeingKnockback = true;
        gameObjectForward = gameObjectDirection;
        StartCoroutine("Knockback", knockback);
    }

    private IEnumerator Knockback(float knockback)
    {
        yield return new WaitForSeconds(timeOfKnockback);
        isBeingKnockback = false;
    }

    private void Die()
    {
        Destroy(gameObject);
    }


}
