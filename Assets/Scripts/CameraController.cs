using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float roadWidth;
    public float steerSpeed;
    public float hiSpeed;
    public float loSpeed;
    public float tiltMagnitude;

    private float currentDrivingSpeed;


    public WorldManager wm;

    private float timer;


    private Vector3 goalPosition;

    public Transform handlebars;

    // Start is called before the first frame update
    void Start()
    {
        currentDrivingSpeed = loSpeed;

        timer = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        // get input from keyboard
        float x_input = Input.GetAxis("Horizontal");
        float y_input = Input.GetAxis("Vertical");
        Vector2 input = new Vector2(x_input, y_input);


        // set horizontal position / manage horizontal movement
        Vector3 position = transform.position;

        position.x += input.x * steerSpeed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, (-roadWidth / 2), (roadWidth / 2));

        transform.position = position;


        // manage forward movement (advance all other objects)
        if(input.y > 0)
        {
            currentDrivingSpeed = hiSpeed;
        }
        else
        {
            currentDrivingSpeed = loSpeed;
        }

        wm.SetSpeed(currentDrivingSpeed);


        // sideways tilt
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = input.x * -tiltMagnitude;
        transform.rotation = Quaternion.Euler(rotation);

        // handlebars tilt
        Vector3 handleRot = handlebars.rotation.eulerAngles;
        handleRot.z = input.x * -tiltMagnitude * 2;
        handlebars.rotation = Quaternion.Euler(handleRot);


        // timer and data update
        timer -= Time.deltaTime;
        if(timer <= 0f && wm.playing)
        {
            GetComponent<PlayerManager>().AddSpeedData((int)currentDrivingSpeed, 1);

            timer = 1f;
        }
    }

    public void GetHit()
    {
        GameObject.FindObjectOfType<SoundManager>().hit(0.5f);

        if (goalPosition == Vector3.zero)
        {
            goalPosition = transform.position;

            transform.position = goalPosition + (Vector3)Random.insideUnitCircle * 0.5f;
            Invoke("moveBack", 0.05f);

            GetComponentInChildren<ParticleSystem>().Play();
        }
    }

    public void moveBack()
    {
        transform.position = goalPosition;
        goalPosition = Vector3.zero;
    }
}
