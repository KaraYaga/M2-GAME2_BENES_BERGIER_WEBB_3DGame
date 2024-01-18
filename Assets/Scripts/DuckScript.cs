using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckScript : MonoBehaviour
{
    [SerializeField] float enemyKnockback = 10f;
    [SerializeField] ParticleSystem particleHit;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;

            Instantiate(particleHit, pos, rot);

            collision.gameObject.GetComponent<EnemyScript>().SetLife(2f, enemyKnockback, gameObject);
            Debug.Log("damage");
            
        }
    }
}
