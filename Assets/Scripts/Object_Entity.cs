using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Object_Entity : MonoBehaviour
{
    private WorldManager wm;
    private GameDirector gd;

    public int health;

    public GameObject myTarget;

    public float xSpeed, zSpeed;
    private float steeringNoise;
    private float offset;
    public bool canAct = true;

    private Vector3 prevPos;

    private GraphicsAnimator ga;


    // Start is called before the first frame update
    void Start()
    {
        wm = GameObject.Find("WorldManager").GetComponent<WorldManager>();
        //wm.SendMessage("AddObject", this.gameObject);

        gd = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        gd.SendMessage("AddObject", this.gameObject);


        myTarget = gd.getTarget();
        


        offset = Random.Range(0, 360);

        ga = GetComponentInChildren<GraphicsAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.z < wm.hitBarrier.transform.position.z)
        {
            HitBarrier();
        }

        Vector3 delta = transform.position - prevPos;
        ga.SendMessage("Animate", delta.x);


        prevPos = transform.position;
    }

    private void LateUpdate()
    {

    }

    public void Act()
    {
        Vector3 vel = Vector3.zero;

        if (!canAct) {
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.SmoothDamp(transform.position.z, transform.position.z - wm.currentSpeed, ref vel.z, zSpeed * Time.deltaTime));
            return;
        }

        float x = Mathf.SmoothDamp(transform.position.x, myTarget.transform.position.x + Mathf.Sin(Time.time) * steeringNoise, ref vel.x, xSpeed * Time.deltaTime);
        float z = Mathf.SmoothDamp(transform.position.z, myTarget.transform.position.z + Mathf.Sin(Time.time + offset), ref vel.z, zSpeed * Time.deltaTime);

        transform.position = new Vector3(x, transform.position.y, z);

        if(steeringNoise > 0)
            steeringNoise -= Time.deltaTime;

    }

    public void HitBarrier()
    {
        wm.RemoveObject(this.gameObject);
        gd.RemoveObject(this.gameObject);
        gd.returnTarget(myTarget);
        Destroy(this.gameObject);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        steeringNoise = 1.5f;

        BroadcastMessage("ResetShooting");

        if(health <= 0 && canAct)
        {
            Debug.Log(this.name + " has been neutralized");
            Camera.main.SendMessage("TargetNeutralized");
            canAct = false;
            //GetComponentInChildren<Collider>().enabled = false;
        }


    }

    public void DisableCanAct()
    {
        canAct = false;
    }





}
