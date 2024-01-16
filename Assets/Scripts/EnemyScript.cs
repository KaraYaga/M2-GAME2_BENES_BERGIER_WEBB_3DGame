using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Windows;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float life = 5f;
    [SerializeField] GameObject Enemy;
    private Animator animator;

    [Header("Movement")]
    [SerializeField] private Transform target;
    [SerializeField] float speed = 2f, radiusFromPoint = 1f, angleFromPoint = 0f;

    [Header("FieldOfView")]
    public GameObject mainCharacter;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public float radiusFromPlayer;
    public bool canSeePlayer;
    [Range(0, 360)]
    public float angleFromPlayer;

    [Header("Knockback")]
    [SerializeField] float timeOfKnockback;


    private void Start()
    {
        animator = Enemy.GetComponent<Animator>();
        StartCoroutine(FOVRoutine());
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
        if (!canSeePlayer)
        {
            float x = target.position.x + Mathf.Cos(angleFromPoint) * radiusFromPoint;
            float y = target.position.y;
            float z = target.position.z + Mathf.Sin(angleFromPoint) * radiusFromPoint;

            transform.position = new Vector3(x, y * transform.right.y, z);
            angleFromPoint += speed * Time.deltaTime;

            //Rotation
            transform.LookAt(target.position);
            transform.Rotate(new Vector3(0, 90, 0), Space.World);
        }
        else
        {
            //dash on character
        }
    }

    public void SetLife(float damage, float knockback)
    {
        life -= damage;
        Debug.Log(life);

        StartCoroutine("Knockback", knockback);
    }

    private IEnumerator Knockback(float knockback)
    {
        Vector3 knockbackVector = mainCharacter.transform.position - transform.position;
        knockbackVector = knockbackVector.normalized * knockback;
        target.GetComponent<Rigidbody>().AddForce(-knockbackVector, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddForce(-knockbackVector, ForceMode.Impulse);


        yield return new WaitForSeconds(timeOfKnockback);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        target.GetComponent<Rigidbody>().velocity = Vector3.zero;

        yield return new WaitForSeconds(timeOfKnockback);
    }

    private IEnumerator FOVRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radiusFromPlayer, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angleFromPlayer / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if(canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    private void Die()
    {
        //die
    }


}
