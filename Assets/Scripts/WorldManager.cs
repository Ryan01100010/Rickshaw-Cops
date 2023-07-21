using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public List<GameObject> worldObjects = new List<GameObject>();
    
    public float currentSpeed;

    public Transform spawnBarrier, hitBarrier;

    public GameObject[] regions;
    public int currentRegion;

    public bool playing;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = 0;

        spawnBarrier = GameObject.Find("SpawnBarrier").transform;
        hitBarrier = GameObject.Find("HitBarrier").transform;

        currentRegion = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playing)
            return;

        AdvanceWorld();
    }

    public void AddObject(GameObject obj)
    {
        worldObjects.Add(obj);
    }

    public void RemoveObject(GameObject obj)
    {
        worldObjects.Remove(obj);
    }

    public void AdvanceWorld()
    {
        for (int i = 0; i < worldObjects.Count; i++)
        {
            worldObjects[i].SendMessage("Move", currentSpeed);
        }
    }

    public void SetSpeed(float s)
    {
        currentSpeed = s;
    }

    public void ChangeRegion()
    {
        for (int i = 0; i < worldObjects.Count; i++)
        {
            worldObjects[i].SendMessage("DeactivateRespawn");
        }

        currentRegion++;

        Instantiate(regions[currentRegion % regions.Length], spawnBarrier.position, Quaternion.identity);
    }

    public void stop()
    {
        playing = false;
    }

    public void start()
    {
        playing = true;
    }
}
