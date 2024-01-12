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

        transform.position = new Vector3(x, y * transform.right.y, z);
        angle += speed * Time.deltaTime;
        //Rotation
        transform.LookAt(target.position);
        transform.Rotate(new Vector3 (0, 90, 0), Space.World);
    }
}
