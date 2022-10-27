using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] waypoints;
    [SerializeField] Animator droneAnimationController;
    // only a collider for the right hand of the drone is required as 3 of the 4 attacks are done with the right hand
    // while the other attack has both the hands together
    [SerializeField] DamagePlayerCollider rightHandDamagePlayerCollider;
    [SerializeField] GameObject droneRagdoll;

    public float lostLineOfSightTimer = 5f;
    [SerializeField] float droneStopTimer = 5f;
    [SerializeField] float reachTargetRange = 0.1f;
    [SerializeField] float sightDistance = 20f;
    [SerializeField] float scanSpeed = 200f;
    [SerializeField] float droneWalkSpeed = 1f;
    [SerializeField] float droneChaseSpeed = 3f;
    [SerializeField] float attackDistance = 1f;
    [SerializeField] float health;
    [SerializeField] ParticleSystem bloodFX;
    [SerializeField] [Range(0, 90)] float maxScanAngle = 90f;

    float currentSightAngle = 0f;
    int currentWaypoint = 0;
    public bool spottedPlayer = false;
    bool attackingPlayer = false;
    public bool droneBeingAttacked = false;
    public bool droneAlive = true;

    const float constantFactor = 80;
    const float dampingFactor = 1.5f;

    Vector3 raycastDirection;
    
    public NavMeshAgent drone;

    private void Start()
    {
        drone = GetComponent<NavMeshAgent>();
        GoToNextWaypoint();
    }

    private void Update()
    {
        if (droneAlive)
        {
            //Debug.Log("Spotted player = " + spottedPlayer);
            if (!spottedPlayer)
            {
                if (ScanForPlayer())
                {
                    // chasing state
                    spottedPlayer = true;
                    drone.speed = droneChaseSpeed;
                    drone.SetDestination(player.transform.position);
                    StartCoroutine(Chase());
                }
                else if (!ScanForPlayer())
                {
                    // patrol state
                    if (drone.remainingDistance < reachTargetRange)
                    {
                        GoToNextWaypoint();
                    }
                }
            }
            else if (spottedPlayer)
            {
                // 1 drone.remainingDistance < attackDistance && !attackingPlayer i.e. in attack distance but not attacking
                // 2 drone.remainingDistance < attackDistance && attackingPlayer i.e. in attack distance and attacking
                // 3 drone.remainingDistance > attackDistance && !attackingPlayer i.e. not in attack distance and not attacking
                // 4 drone.remainingDistance > attackDistance && attackingPlayer i.e. not in attack distance but attacking
                if (drone.remainingDistance < attackDistance)
                {
                    if (!attackingPlayer) // 1
                    {
                        attackingPlayer = true;

                        drone.isStopped = true;

                        StartCoroutine(Attack());
                    }
                    else // 2
                    {
                        // the navmesh agent gets ridiculously confused when the player manages to stand in attack distance but out of sight of the ray cast for 5 seconds
                        // this is some code that i found from https://www.codegrepper.com/code-examples/csharp/navmeshagent+look+at+target 
                        // it just makes the drone face the player if they manage to be cheeky and be in attack distance but out of sight
                        Vector3 lookPos = player.transform.position - transform.position;
                        lookPos.y = 0;
                        Quaternion rotation = Quaternion.LookRotation(lookPos);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.5f);


                    }
                }
                else
                {
                    if (!attackingPlayer) // 3
                    {
                        drone.isStopped = false;
                    }
                    else // 4
                    {
                        // if i can think of a check for this scenario that won't break anything, i'll put it here
                    }
                }
            }
        }
    }

    void GoToNextWaypoint()
    {
        if (currentWaypoint >= waypoints.Length)
        {
            currentWaypoint = 0;
        }

        drone.SetDestination(waypoints[currentWaypoint].transform.position);
        currentWaypoint++;
    }

    bool ScanForPlayer()
    {
        currentSightAngle += scanSpeed * Time.deltaTime;
        currentSightAngle %= maxScanAngle;

        float angle = (currentSightAngle * 2) -maxScanAngle;
        raycastDirection = transform.TransformDirection(Quaternion.Euler(0, angle, 0) * Vector3.forward) * sightDistance;
        Vector3 droneNeckHeight = new Vector3(transform.position.x, transform.position.y + 1.8f, transform.position.z);

        RaycastHit hit;
        if (Physics.Raycast(droneNeckHeight, raycastDirection, out hit, sightDistance) && hit.collider.tag == "Player")
        {
            Debug.DrawRay(droneNeckHeight, raycastDirection, Color.red);
            return true;
        }
        else
        {
            Debug.DrawRay(droneNeckHeight, raycastDirection, Color.green);
            return false;
        }
    }

    IEnumerator Chase()
    {
        float timeElapsed = 0f;

        while (timeElapsed < lostLineOfSightTimer)
        {
            if (ScanForPlayer())
            {
                timeElapsed = 0f;
            }

            drone.SetDestination(player.transform.position);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        drone.isStopped = true;
        drone.speed = 0f;
        timeElapsed = 0f;

        while (timeElapsed < droneStopTimer)
        {
            if (ScanForPlayer())
            {
                spottedPlayer = true;
                drone.isStopped = false;
                drone.speed = droneChaseSpeed;
                drone.SetDestination(player.transform.position);
                //StartCoroutine(Chase());  // recursion, could cause issues
                yield break;
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        drone.isStopped = false;
        drone.speed = droneWalkSpeed;
        spottedPlayer = false;
        GoToNextWaypoint();
    }

    IEnumerator Attack()
    {
        rightHandDamagePlayerCollider.currentlyAttacking = true; // enable hit detection between the box collider on the right hand of the drone
                                                                 // and the player collider

        drone.velocity = Vector3.zero;

        int randNum = Random.Range(1, 5);

        switch (randNum)
        {
            // these hardcoded values are the lengths of the animation clips for attacking
            // currently i cannot find a better way to get these values
            // i did try to retrieve the length of the currently playing animation clip using Animator.GetCurrentAnimatorStateInfo
            // but unfortunately it kept retrieving the wrong clip length which caused some strange behaviour
            case 1:
                droneAnimationController.SetTrigger("attack_01");
                yield return new WaitForSeconds(2.867f);
                break;

            case 2:
                droneAnimationController.SetTrigger("attack_02");
                yield return new WaitForSeconds(1.967f);
                break;

            case 3:
                droneAnimationController.SetTrigger("attack_03");
                yield return new WaitForSeconds(2.667f);
                break;

            case 4:
                droneAnimationController.SetTrigger("attack_04");
                yield return new WaitForSeconds(1.017f);
                break;
        }

        attackingPlayer = false;
        rightHandDamagePlayerCollider.currentlyAttacking = false; // disabling hit detection between the box collider on the right hand of the drone
                                                                  // as i only want hits to register on the player when the drone is in an attack state
    }

    public void TakeDamage(float damageAmount, RaycastHit hit)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            health = 0;
            Die(damageAmount, hit);
        }
        else
        {
            if (damageAmount <= 20)
            {
                droneBeingAttacked = true;
                float droneVelocity = Mathf.Pow((constantFactor / hit.distance), (1 / dampingFactor));
                drone.velocity = -hit.normal * droneVelocity;
                StartCoroutine(WaitForStagger("stagger_01"));
                droneAnimationController.SetTrigger("stagger_01");
            }
            else
            {
                droneBeingAttacked = true;
                float droneVelocity = Mathf.Pow((constantFactor / hit.distance), (1 / dampingFactor));
                drone.velocity = -hit.normal * droneVelocity;
                StartCoroutine(WaitForStagger("stagger_02"));
                droneAnimationController.SetTrigger("stagger_02");
            }
        }
    }

    public void SpawnHitFX(RaycastHit initialHit, RaycastHit hit)
    {
        // blood fx particle system
        ParticleSystem bloodPS = Instantiate(bloodFX, hit.point, Quaternion.Euler(-hit.normal), hit.transform);
        Destroy(bloodPS);
    }

    void Die(float damageTaken, RaycastHit hit)
    {
        GameObject ragdoll = Instantiate(droneRagdoll, transform.position, transform.rotation);

        Rigidbody[] ragdollBodies = ragdoll.GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rb in ragdollBodies)
        {
            rb.AddExplosionForce(damageTaken * 2, hit.point, 5f, 0f, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }

    IEnumerator WaitForStagger(string stagger)
    {
        switch (stagger)
        {
            case "stagger_01":
                yield return new WaitForSeconds(1.583f); // time will be length of stagger01 animation
                break;

            case "stagger_02":
                yield return new WaitForSeconds(2.167f); //time will be length of stagger02 animation
                break;
        }

        droneBeingAttacked = false;
    }

    public bool GetWalking()
    {
        if (drone.speed == droneWalkSpeed && drone.velocity.magnitude <= droneWalkSpeed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetChasing()
    {
        if (drone.speed == droneChaseSpeed && drone.velocity.magnitude > droneWalkSpeed && !droneBeingAttacked)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetLookingAround()
    {
        if (drone.speed == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}