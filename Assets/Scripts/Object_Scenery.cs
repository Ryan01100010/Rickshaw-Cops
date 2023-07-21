using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Scenery : MonoBehaviour
{
    private Vector2 spawnLocation;
    private WorldManager wm;

    public bool respawn;

    // Start is called before the first frame update
    void Start()
    {
        wm = GameObject.Find("WorldManager").GetComponent<WorldManager>();
        wm.SendMessage("AddObject", this.gameObject);

        spawnLocation = new Vector2(transform.position.x, transform.position.y);

        respawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.z <= wm.hitBarrier.transform.position.z)
        {
            HitBarrier(gameObject.transform.position.z - wm.hitBarrier.transform.position.z);
        }

        if (gameObject.transform.position.z > wm.spawnBarrier.transform.position.z)
        {
            GetComponent<Renderer>().enabled = false;
        }
        else if (GetComponent<Renderer>().enabled == false)
        {
            GetComponent<Renderer>().enabled = true;
        }
    }

    public void Move(float worldSpeed)
    {
        /*
        // move along with the world, no added movements
        Vector3 position = transform.position;
        position.z -= worldSpeed * Time.deltaTime;

        transform.position = position;
        */

        transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.back * worldSpeed * Time.deltaTime, worldSpeed * Time.deltaTime);  

    }

    public void HitBarrier(float offset)
    {
        if (!respawn)
        {
            wm.SendMessage("RemoveObject", this.gameObject);
            Destroy(gameObject);
        }

        // respawn at spawn barrier
        transform.position = new Vector3(spawnLocation.x, spawnLocation.y, wm.spawnBarrier.position.z + offset);
    }

    public void DeactivateRespawn()
    {
        respawn = false;
    }
}
