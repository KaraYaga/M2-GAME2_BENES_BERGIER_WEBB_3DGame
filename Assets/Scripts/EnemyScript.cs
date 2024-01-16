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
    [SerializeField] float timeOfKnockback, knockback;




    [Header("Attack")]
    [SerializeField] private GameObject mainCharacter;
    [SerializeField] private float speedToAttack, knockbackOfAttack;
    public bool isAttacking;
    public RigidbodyConstraints originalConstraints;


    private void Start()
    {
        animator = Enemy.GetComponent<Animator>();
        originalConstraints = GetComponent<Rigidbody>().constraints;
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
        }
        else if(!isBeingKnockback)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            transform.LookAt(mainCharacter.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, mainCharacter.transform.position, speedToAttack * Time.deltaTime);
            target.position += target.transform.forward * speedToAttack * Time.deltaTime;
        }
        else if(isBeingKnockback)
        {
            //check from where the projectile come from and go backward with transform.position
            Vector3 knockbackVector = mainCharacter.transform.position - transform.position;
            knockbackVector = knockbackVector.normalized * knockback;

            Vector3 targetKnockbackVector = mainCharacter.transform.position - target.transform.position;
            targetKnockbackVector = targetKnockbackVector.normalized * knockback;

            //target.GetComponent<Rigidbody>().AddForce(-targetKnockbackVector, ForceMode.Impulse);
            //GetComponent<Rigidbody>().AddForce(-knockbackVector, ForceMode.Impulse);
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

    public void SetLife(float damage, float knockback)
    {
        life -= damage;
        Debug.Log(life);
        isBeingKnockback = true;
        StartCoroutine("Knockback", knockback);
    }

    private IEnumerator Knockback(float knockback)
    {
        yield return new WaitForSeconds(timeOfKnockback);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        target.GetComponent<Rigidbody>().velocity = Vector3.zero;

        yield return new WaitForSeconds(timeOfKnockback);
        isBeingKnockback = false;
    }

    private void Die()
    {
        //die
    }


}
