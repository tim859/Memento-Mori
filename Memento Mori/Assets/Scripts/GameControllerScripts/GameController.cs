using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // padlock combination password variables
    public int lockPassword1;
    public int lockPassword2;
    public int lockPassword3;
    public TMP_Text bloodNumber1;
    public TMP_Text bloodNumber2;
    public TMP_Text bloodNumber3;
    public bool padlock1Unlocked = false;
    public bool padlock2Unlocked = false;
    public bool padlock3Unlocked = false;

    // getting references to all the doors in the level so they can be controlled from here
    [SerializeField] GameObject sportsHallDoubleDoor1;
    [SerializeField] GameObject sportsHallDoubleDoor2;
    [SerializeField] GameObject sportsHallDoubleDoor3;
    [SerializeField] GameObject sportsHallDoubleDoor4;
    [SerializeField] GameObject secondRoomDoor;
    [SerializeField] GameObject thirdRoomDoor;
    [SerializeField] GameObject thirdRoomDoubleDoor;
    [SerializeField] GameObject fourthRoomDoubleDoor;
    [SerializeField] GameObject exitDoorA;
    [SerializeField] GameObject exitDoorB;
    Vector3 exitDoorAOpenTransform;
    Vector3 exitDoorBOpenTransform;

    [SerializeField] GameObject player;
    [SerializeField] GameObject dronePrefab;
    [SerializeField] GameObject[] spawnPoints;

    bool exitOpen;

    private void Start()
    {
        lockPassword1 = Random.Range(1000, 9999);
        lockPassword2 = Random.Range(1000, 9999);
        lockPassword3 = Random.Range(1000, 9999);

        bloodNumber1.text = lockPassword1.ToString();
        bloodNumber2.text = lockPassword2.ToString();
        bloodNumber3.text = lockPassword3.ToString();

        sportsHallDoubleDoor2.GetComponent<DoubleDoorController>().doorLocked = true;
        sportsHallDoubleDoor3.GetComponent<DoubleDoorController>().doorLocked = true;
        sportsHallDoubleDoor4.GetComponent<DoubleDoorController>().doorLocked = true;

        exitDoorAOpenTransform = exitDoorA.transform.localEulerAngles + new Vector3(0, 0, 90);
        exitDoorBOpenTransform = exitDoorB.transform.localEulerAngles + new Vector3(0, 0, -90);
    }

    IEnumerator OpenExit()
    {
        float timeElapsed = 0f;

        while (timeElapsed < 1)
        {
            exitDoorA.transform.localEulerAngles = Vector3.Lerp(exitDoorA.transform.localEulerAngles, exitDoorAOpenTransform, timeElapsed / 1);
            exitDoorB.transform.localEulerAngles = Vector3.Lerp(exitDoorB.transform.localEulerAngles, exitDoorBOpenTransform, timeElapsed / 1);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        exitDoorA.transform.localEulerAngles = exitDoorAOpenTransform;
        exitDoorB.transform.localEulerAngles = exitDoorBOpenTransform;
    }

    public void CheckpointOne()
    {
        // spawn zombies that run towards the player at spawn points 0 through 5
        //foreach (GameObject spawnPoint in spawnPoints)
        //{
        //    GameObject drone = Instantiate(spawnPoint, spawnPoint.transform.position, Quaternion.identity);

        //    drone.GetComponent<DroneController>().spottedPlayer = true;
        //    drone.GetComponent<DroneController>().lostLineOfSightTimer = 30f;
        //    drone.GetComponent<DroneController>().drone.SetDestination(player.transform.position);
        //}

        //for (int i = 0; i < 6; i++)
        //{
        //    Debug.Log(i);
        //    GameObject drone = Instantiate(dronePrefab, spawnPoints[i].transform.position, Quaternion.identity);

        //    Debug.Log("here");
        //    drone.GetComponent<DroneController>().spottedPlayer = true;
        //    Debug.Log("here 2");
        //    drone.GetComponent<DroneController>().lostLineOfSightTimer = 30f;
        //    drone.GetComponent<DroneController>().drone.SetDestination(player.transform.position);
        //}
    }

    public void CheckpointTwo()
    {
        sportsHallDoubleDoor2.GetComponent<DoubleDoorController>().doorLocked = false;
        sportsHallDoubleDoor2.GetComponentInChildren<Light>().color = Color.green;

        sportsHallDoubleDoor3.GetComponent<DoubleDoorController>().doorLocked = false;
        sportsHallDoubleDoor3.GetComponentInChildren<Light>().color = Color.green;
    }

    public void CheckpointThree()
    {
        sportsHallDoubleDoor4.GetComponent<DoubleDoorController>().doorLocked = false;
        sportsHallDoubleDoor4.GetComponentInChildren<Light>().color = Color.green;
    }

    public void CheckpointFour()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    private void Update()
    {
        if (!exitOpen)
        {
            if (padlock1Unlocked && padlock2Unlocked && padlock3Unlocked)
            {
                // open exit
                exitOpen = true;
                StartCoroutine(OpenExit());
            }
        }
    }
}
