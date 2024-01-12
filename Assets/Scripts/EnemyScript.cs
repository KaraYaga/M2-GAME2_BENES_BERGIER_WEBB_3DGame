using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] float speed = 2f, radius = 1f, angle = 0f;

    void Update()
    {
        float x = target.position.x + Mathf.Cos(angle) * radius;
        float y = target.position.y;
        float z = target.position.z + Mathf.Sin(angle) * radius;

        transform.position = new Vector3(x, y, z);
        angle += speed * Time.deltaTime;

        //transform.LookAt(target.position + new Vector3(target.position.x + 45, 0,0));
        transform.LookAt(target.position);
    }
}
