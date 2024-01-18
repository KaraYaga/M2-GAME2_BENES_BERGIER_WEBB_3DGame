using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemInstantiator : MonoBehaviour
{
    [SerializeField] public float delay = 10f; // Delay
    public GameObject particleSystemPrefab;

    void Start()
    {
        Invoke("InstantiateParticleSystem", delay);
    }

    void InstantiateParticleSystem()
    {
        GameObject particleSystemInstance = Instantiate(particleSystemPrefab, transform.position, transform.rotation);
        particleSystemInstance.transform.parent = transform;
    }
}