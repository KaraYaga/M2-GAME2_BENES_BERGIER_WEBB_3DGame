using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyScript
{
    [Header("FieldOfView")]
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    public float radiusFromPlayer;
    public bool canSeePlayer;

    [Range(0, 360)]
    public float angleFromPlayer;

    private void Update()
    {
        if (canSeePlayer)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        if (life <= 0)
        {
            StartCoroutine(DestroyWithParticles());
        }
    }

    private void Start()
    {
        StartCoroutine(FOVRoutine());
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
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}
