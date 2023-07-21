using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameDirector : MonoBehaviour
{
    public GameObject[] cars; // prefab


    public List<GameObject> currentGameObjects = new List<GameObject>();

    public Transform spawnBarrier, hitBarrier;


    public List<GameObject> targets = new List<GameObject>();
    private GameObject[] spawns;


    public int currentWave;

    public bool playing;

    // Start is called before the first frame update
    void Start()
    {
        spawnBarrier = GameObject.Find("SpawnBarrier").transform;
        hitBarrier = GameObject.Find("HitBarrier").transform;

        targets = GameObject.FindGameObjectsWithTag("target").ToList();
        spawns = GameObject.FindGameObjectsWithTag("spawn");

        currentWave = 0;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < currentGameObjects.Count; i++)
        {
            currentGameObjects[i].SendMessage("Act");
        }

        if(currentGameObjects.Count <= 0) 
        {
            currentWave += 1;
            AdvanceWave();
        }
    }

    public void AddObject(GameObject obj)
    {
        currentGameObjects.Add(obj);
    }

    public void RemoveObject(GameObject obj)
    {
        currentGameObjects.Remove(obj);
    }

    public GameObject getTarget()
    {
        GameObject t = targets[Random.Range(0, targets.Count)];
        targets.Remove(t);
        return t;
    }

    public void returnTarget(GameObject t)
    {
        targets.Add(t);
    }

    public void AdvanceWave()
    {
        if (!playing)
            return;


        int currentRegion = Mathf.FloorToInt(currentWave / 4f) + 1;
        int regionProgress = currentWave % 4;

        if (regionProgress == 0)
        {
            NextRegion();
        }
                
        switch (regionProgress)
        {
            case 0:
            case 1:
            case 2:
                SpawnCars(Mathf.Clamp(currentRegion + 1, 2, 6));
                break;
            case 3:
                SpawnCars(Mathf.Clamp(currentRegion + 3, 2, 6));
                break;
        }


        /*
        // controls waves and regions
        switch (currentWave)
        {
            case 0:
            case 1:
            case 2:
                SpawnCars(2);
                break;
            case 3:
                SpawnCars(4);
                break;
            case 4:
                NextRegion();
                SpawnCars(3);
                break;
            case 5:
            case 6: 
                SpawnCars(3);
                break;
            case 7:
                NextRegion();
                SpawnCars(3);
                break;
            case 8:
            case 9:
                SpawnCars(5);
                break;
            case 10:
                //GameWin();
                NextRegion();
                SpawnCars(4);
                break;
            case 11:
                SpawnCars(4);
                break;
            case 12:
                SpawnCars(6);
                break;
            case 13:
                GameWin();
                break;



        }
        */
    }

    public void SpawnCars(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(RandomCar(), spawns[Random.Range(0, spawns.Length)].transform.position, Quaternion.identity);
        }
    }

    public GameObject RandomCar()
    {
        return cars[Random.Range(0, cars.Length)];
    }

    public void NextRegion()
    {
        GameObject.Find("WorldManager").GetComponent<WorldManager>().SendMessage("ChangeRegion");
    }

    public void stop()
    {
        playing = false;

        for (int i = 0; i < currentGameObjects.Count; i++)
        {
            currentGameObjects[i].SendMessage("DisableCanAct");
        }
    }

    public void start()
    {
        playing = true;
    }

    public void GameWin()
    {
        GameObject.FindObjectOfType<MenuManager>().GameWinScreen();
    }
}
