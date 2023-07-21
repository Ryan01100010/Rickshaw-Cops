using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private int totalShotAttempts, totalShotHits;
    private GameObject mainCam;

    public ParticleSystem ExplosionPS;

    public bool gameOver;

    public Texture2D crosshair;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main.gameObject;

        Cursor.SetCursor(crosshair, new Vector2(crosshair.width / 2, crosshair.height / 2), CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
            return;


        if (Input.GetMouseButtonDown(0))
        {
            totalShotAttempts++;

            GameObject.FindObjectOfType<SoundManager>().shoot(0.5f);

            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hit " + hit.collider.name);

                if (hit.collider.gameObject.GetComponentInParent<Object_Entity>()){
                    totalShotHits++;
                    hit.collider.gameObject.GetComponentInParent<Object_Entity>().TakeDamage(1);

                    Instantiate(ExplosionPS, hit.point, Quaternion.identity);
                }
            }

            mainCam.GetComponent<PlayerManager>().SetShootData(totalShotAttempts, totalShotHits);

        }
    }
}
