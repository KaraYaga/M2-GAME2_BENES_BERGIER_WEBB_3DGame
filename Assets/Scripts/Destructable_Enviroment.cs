using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable_Enviroment : MonoBehaviour
{
    [SerializeField] float life = 1f;
    [SerializeField] GameObject destructionParticlesPrefab;

    public void SetLife(float damage)
    {
        life -= damage;
        Debug.Log(life);

    }

    //Check if life 0
    public void Update()
    {
        if (life <= 0f)
        {
            StartCoroutine(DestroyWithParticles());
        }
    }

    //Instantiate Particles and Destroy Object
    private IEnumerator DestroyWithParticles()
    {
        Instantiate(destructionParticlesPrefab, transform.position, transform.rotation);

        yield return null;

        Destroy(gameObject);
    }
}
