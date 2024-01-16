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
    [SerializeField] GameObject Enemy;
    private Animator animator;

    [Header("Movement")]
    [SerializeField] private Transform target;
    [SerializeField] float speedToTurn = 2f, radiusFromPoint = 1f, angleFromPoint = 0f;


    [Header("Knockback")]
    [SerializeField] bool isBeingKnockback;
    [SerializeField] float knockback, timeOfKnockback;
    private Vector3 knockbackLocation;
    private float yAxisLocation;
    private GameObject gameObjectForward;

    [Header("Attack")]
    [SerializeField] private float speedToAttack; //knockbackOfAttack;
    protected bool isAttacking;
    public GameObject mainCharacter;
    //private RigidbodyConstraints originalConstraints;


    private void Start()
    {
        animator = Enemy.GetComponent<Animator>();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        yAxisLocation = transform.position.y;
        //target.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void Update()
    {
        //Die
        if (life <= 0)
        {
            Die();
        }
    }


    private void FixedUpdate()
    {
        target.transform.rotation = transform.rotation;

        if (!isAttacking && !isBeingKnockback)
        {
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
            transform.LookAt(mainCharacter.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, mainCharacter.transform.position, speedToAttack * Time.deltaTime);
            target.position += target.transform.forward * speedToAttack * Time.deltaTime;

            knockbackLocation = mainCharacter.transform.position;
        }
        else if (isBeingKnockback)
        {
            transform.position += (gameObjectForward.transform.forward) * knockback * Time.deltaTime;
            transform.Rotate(0, 90, 0);
            //check from where the projectile come from and go backward with transform.position

            //Debug.Log(mainCharacter.transform.position);
            //Debug.Log(-mainCharacter.transform.position);
            //transform.position = Vector3.MoveTowards(transform.position, -knockbackLocation, knockback * Time.deltaTime);


        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            isAttacking = false;
            Debug.Log("attack");
            //StartCoroutine("Knockback", knockback);
            //GetComponent<Rigidbody>().constraints = originalConstraints;    
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
        //GetComponent<Rigidbody>().velocity = Vector3.zero;
        //target.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //Vector3 knockbackVector = mainCharacter.transform.position - transform.position;
        //knockbackVector = knockbackVector.normalized * knockback;

        //Vector3 targetKnockbackVector = mainCharacter.transform.position - target.transform.position;
        //targetKnockbackVector = targetKnockbackVector.normalized * knockback;

        //GetComponent<Rigidbody>().AddForce(-knockbackVector, ForceMode.Impulse);
        //target.GetComponent<Rigidbody>().AddForce(-targetKnockbackVector, ForceMode.Impulse);

        //yield return new WaitForSeconds(timeOfKnockback);

        yield return new WaitForSeconds(timeOfKnockback);
        isBeingKnockback = false;
    }

    private void Die()
    {
        //die
    }


}
