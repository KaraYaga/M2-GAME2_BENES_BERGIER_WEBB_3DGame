using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Windows;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float life = 5f;
    //[SerializeField] GameObject mainCharacter;
    [SerializeField] GameObject Enemy;
    private Animator animator;

    [Header("Movement")]
    [SerializeField] private Transform target;
    [SerializeField] float speed = 2f, radius = 1f, angle = 0f;

    [Header("Knockback")]
    [SerializeField] float knockback;
    [SerializeField] float timeOfKnockback;
    private bool isKnockback;


    private void Start()
    {
        animator = Enemy.GetComponent<Animator>();
    }

    void Update()
    {

        float x = target.position.x + Mathf.Cos(angle) * radius;
        float y = target.position.y;
        float z = target.position.z + Mathf.Sin(angle) * radius;

        transform.position = new Vector3(x, y * transform.right.y, z);
        angle += speed * Time.deltaTime;

        //Rotation
        transform.LookAt(target.position);
        transform.Rotate(new Vector3(0, 90, 0), Space.World);

        //Die
        if (life <= 0)
        {
            Die();
        }
    }

    public void SetLife(float damage, GameObject attacker)
    {
        life -= damage;
        Debug.Log(life);

        if (!isKnockback)
        {
            StartCoroutine("Knockback", attacker);
        }
    }

    IEnumerator Knockback(GameObject attacker)
    {
        Vector3 knockbackVector = attacker.transform.position - transform.position;
        knockbackVector = knockbackVector.normalized * knockback;
        target.GetComponent<Rigidbody>().AddForce(-knockbackVector, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddForce(-knockbackVector, ForceMode.Impulse);


        yield return new WaitForSeconds(timeOfKnockback);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        target.GetComponent<Rigidbody>().velocity = Vector3.zero;

        yield return new WaitForSeconds(timeOfKnockback);
        isKnockback = false;
    }

    private void Die()
    {
        //die
    }


}
