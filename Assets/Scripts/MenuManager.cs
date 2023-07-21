using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Canvas menuCanvas;

    public RawImage gameEndHeaderImage;
    public TMP_Text gameEndText;
    public TMP_Text finalScoreText;

    public bool lerpingHeader;
    public bool typingStats;

    public string[] stats;
    public int statIndex;

    public GameDirector gd;
    public WorldManager wm;
    public PlayerManager pm;
    public GameObject[] ingameHUD;

    public int shotsHitPoints;
    public int accuracyPoints;
    public int killPoints;
    //public int healthPoints;
    public int timePoints;
    public int finalScore;

    //public bool victory;
    //public Texture2D gameWinImage;

    public bool hold;

    private void Awake()
    {
        DisplayMenu(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        lerpingHeader = false;

        gd.playing = true;
        wm.playing = true;

        statIndex = 0;

        //victory = false;
        hold = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!menuCanvas.enabled) return;

        if(lerpingHeader)
        {
            lerpHeaderImage();
            return;
        }
        else
        {
            if (typingStats)
            {
                typeStats();
                return;
            }
            else
            {
                if (hold)
                {
                    // display "thank you for playing" splash art

                    if(Input.GetMouseButtonDown(0))
                    {
                        hold = false;
                    }
                    return;
                }
                else
                {
                    //return to main screen
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    SceneManager.LoadScene(0);
                }

            }
        }
    }

    public void DisplayMenu(bool showMenu)
    {
        menuCanvas.enabled = showMenu;

        if (showMenu)
        {
            gd.stop();
            wm.stop();

            for (int i = 0; i < ingameHUD.Length; i++)
            {
                ingameHUD[i].SetActive(false);
            }
        }
    }

    public void GameOverScreen()
    {
        //hold = true;

        GameObject.FindObjectOfType<SoundManager>().explosion(1.0f);
        Destroy(GameObject.FindObjectOfType<CameraController>());

        DisplayMenu(true);

        // lerp header image upwards
        gameEndHeaderImage.rectTransform.localPosition = new Vector3(0, -120, 0);
        lerpingHeader = true;

        // type out text stats
        setStatsString();
        typingStats = true;


    }

    public void GameWinScreen()
    {
        //victory = true;

        //gameEndHeaderImage.texture = gameWinImage;
        //gameEndText.color = new Color(185f / 255f, 181f / 255f, 91f / 255f); // gold-ish colour

        GameOverScreen();
    }

    public void lerpHeaderImage()
    {
        gameEndHeaderImage.rectTransform.localPosition = new Vector3(0, Mathf.Lerp(gameEndHeaderImage.rectTransform.localPosition.y, 20, 0.05f), 0);

        if(gameEndHeaderImage.rectTransform.localPosition.y >= 19)
        {
            lerpingHeader = false;
        }
    }

    public void typeStats()
    {
        //type out stats line by line
        //gameEndText.text = stats[statIndex];
        gameEndText.text = stats[0] + "\n" +
            stats[1] + "\n" +
            stats[2] + "\n" +
            stats[3] + "\n" +
            stats[4];
        finalScoreText.text = finalScore.ToString() + "pts";

        if (Input.GetMouseButtonDown(0))
        {
            //statIndex++;
            //if(statIndex == stats.Length)
            //{
                //gameEndText.text = "";
                //finalScoreText.text = "";
                typingStats = false;

            //}
        }
    }

    public void setStatsString()
    {
        string calc;

        // shot hits
        string shotHits;
        shotsHitPoints = pm.totalShotHits * 10;
        shotHits = string.Format("{0}", "Shots hit :\n");
        calc = string.Format("\t{0} x 10pts = ", pm.totalShotHits);
        shotHits += string.Format("{0, -15}", calc);
        shotHits += shotsHitPoints.ToString() + "pts";
        //shotHits += "\n\n(shoot to continue)";
        Debug.Log(shotHits);

        // shot attempts, shot hits, accuracy
        string accuracyString;
        accuracyPoints = Mathf.FloorToInt((pm.accuracy * 500f) / 10) * 10;
        accuracyString = string.Format("{0}", "Accuracy :\n");
        calc = string.Format("\t{0}% of 500pts = ", Mathf.FloorToInt(pm.accuracy * 10000) / 100);
        accuracyString += string.Format("{0, -15}", calc);
        accuracyString += accuracyPoints.ToString() + "pts";
        //accuracyString += "\n\n(rounded down to nearest 10)";
        Debug.Log(accuracyString);

        // kills
        string killString;
        killPoints = pm.kills * 50;
        killString = string.Format("{0}", "Enemies Neutralized :\n");
        calc = string.Format("\t{0} x 50pts = ", pm.kills);
        killString += string.Format("{0, -15}", calc);
        killString += killPoints.ToString() + "pts";
        Debug.Log(killString);

        /*
        // damage taken
        string healthString;
        healthPoints = (20 - pm.damageTaken) * 100;
        healthString = string.Format("{0}", "Health Left :\n");
        calc = string.Format("{0} x 100pts = ", (20 - pm.damageTaken));
        healthString += string.Format("{0, -15}", calc);
        healthString += healthPoints.ToString() + "pts";
        Debug.Log(healthString);
        */

        // time
        string timeString;
        timePoints = Mathf.FloorToInt(pm.totalTime * 10);
        timeString = string.Format("{0}", "Time Survived :\n");
        calc = string.Format("\t{0} x 10pts = ", pm.totalTime);
        timeString += string.Format("{0, -15}", calc);
        timeString += timePoints.ToString() + "pts";
        Debug.Log(timeString);

        // overall score
        string scoreString;
        finalScore = shotsHitPoints + accuracyPoints + killPoints + timePoints; // + healthPoints
        scoreString = string.Format("{0}", "Final Score :\n");
        scoreString += "\t" + shotsHitPoints.ToString();
        scoreString += " + " + accuracyPoints.ToString();
        scoreString += " + " + killPoints.ToString(); 
        //scoreString += " + " + healthPoints.ToString(); 
        scoreString += " + " + timePoints.ToString() + " = "; 
        //scoreString += "\n\n";
        //scoreString += "\t\t = " + finalScore.ToString() + "pts"; 
        Debug.Log(scoreString);

        stats[0] = shotHits;
        stats[1] = accuracyString;
        stats[2] = killString;
        //stats[3] = healthString;
        stats[3] = timeString;
        stats[4] = scoreString;

        //return shotHits + "\n" + accuracyString + "\n" + killString + "\n" + healthString + "\n" + timeString;
    }

}
