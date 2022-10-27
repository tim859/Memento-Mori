using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerCollider : MonoBehaviour
{
    public bool currentlyAttacking = false;
    [SerializeField] float damageDealt = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (currentlyAttacking)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().TakeDamage(damageDealt);
            }
        }
    }

}
