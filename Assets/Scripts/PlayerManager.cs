using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int damageTaken;
    public LayerMask obstacleMask;
    public List<Collider> tagged = new List<Collider>();

    public int totalSpeed, totalTime;
    public int totalShotAttempts, totalShotHits;
    public float accuracy;
    public int kills;

    public TMP_Text healthText;
    public TMP_Text killsText;
    public TMP_Text timeText;

    public MenuManager mm;

    public bool canTakeDamage;

    // Start is called before the first frame update
    void Start()
    {
        setHealthText(20);
        setKillsText(0);
        setTimeText(0);

        canTakeDamage = true;
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] inRange = Physics.OverlapSphere(transform.position, 0.5f, obstacleMask);
        
        for (int i = 0; i < inRange.Length; i++)
        {
            if (!tagged.Contains(inRange[i]))
            {
                tagged.Add(inRange[i]);

                // running into neutralized enemy
                GetShot();
            }
        }

    }

    public void AddSpeedData(int speed, int time)
    {
        totalSpeed += speed;
        totalTime += time;

        setTimeText(totalTime);
    }

    public void SetShootData(int attempts, int hits)
    {
        totalShotAttempts = attempts;
        totalShotHits = hits;

        accuracy = (float)totalShotHits / (float)totalShotAttempts;
    }

    public void TargetNeutralized()
    {
        kills++;
        setKillsText(kills);
    }

    public void GetShot()
    {
        if (!canTakeDamage)
            return;

        damageTaken++;
        GetComponent<CameraController>().GetHit();

        setHealthText(20 - damageTaken);

        if(damageTaken >= 20)
        {
            GameOver();
        }
    }

    public void setHealthText(int health)
    {
        healthText.text = health.ToString();
    }

    public void setKillsText(int kills)
    {
        killsText.text = "x" + kills.ToString();
    }

    public void setTimeText(int seconds)
    {
        timeText.text = seconds.ToString() + "s";
    }

    public void GameOver()
    {
        canTakeDamage = false;
        GetComponentInChildren<Shooting>().gameOver = true;
        //mm.GameOverScreen();
        GameObject.FindObjectOfType<CameraController>().InvokeRepeating("GetHit", 0, 0.1f);
        mm.Invoke("GameOverScreen", 2f);
    }
}
