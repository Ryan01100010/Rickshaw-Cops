using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public GameObject text;
    private float timer;

    public AudioClip gunshot;

    // Start is called before the first frame update
    void Start()
    {
        timer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            ToggleTextVisibility();
            Invoke("ToggleTextVisibility", 0.5f);
        }


        if (Input.anyKeyDown)
        {
            GetComponent<AudioSource>().PlayOneShot(gunshot);
            timer = 10f;

            Invoke("GameScene", 1f);
        }
    }

    public void ToggleTextVisibility()
    {
        text.SetActive(!text.activeInHierarchy);
        timer = 1;
    }

    public void GameScene()
    {
        SceneManager.LoadScene(1);

    }
}
