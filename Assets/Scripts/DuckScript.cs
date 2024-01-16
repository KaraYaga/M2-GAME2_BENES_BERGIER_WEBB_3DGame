using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckScript : MonoBehaviour
{
    [SerializeField] float enemyKnockback = 10f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //collision.gameObject.GetComponent<EnemyScript>().GetComponent<Rigidbody>().constraints = collision.gameObject.GetComponent<EnemyScript>().originalConstraints;
            collision.gameObject.GetComponent<EnemyScript>().SetLife(2f, enemyKnockback);
            Debug.Log("damage");
        }
    }
}
