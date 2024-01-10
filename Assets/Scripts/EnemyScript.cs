using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] float speed = 2f, radius = 1f, angle = 0f;

    void Update()
    {
        //rb.MovePosition(transform.position + (transform.forward) * speed * Time.deltaTime);
        float x = target.position.x + Mathf.Cos(angle) * radius;
        float y = target.position.y;
        float z = target.position.z + Mathf.Sin(angle) * radius;

        transform.position = new Vector3(x, y, z);
        angle += speed * Time.deltaTime;
    }
}
