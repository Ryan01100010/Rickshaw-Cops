using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShooting : MonoBehaviour
{
    public RawImage shootIndicator;

    public float timeToShoot;
    private float timeLeft;

    public AnimationCurve curve;

    public bool canAct;

    public AudioSource audioPlayer;

    // Start is called before the first frame update
    void Start()
    {
        canAct = true;

        timeLeft = timeToShoot + Random.Range(0, timeToShoot);


    }

    // Update is called once per frame
    void Update()
    {
        if (!canAct)
            return;

        Vector2 scale = shootIndicator.rectTransform.localScale;
        float v = (timeLeft / timeToShoot);
        scale.x = curve.Evaluate(v);
        scale.y = curve.Evaluate(v);
        shootIndicator.rectTransform.localScale = scale;

        if(timeLeft < 1f)
            audioPlayer.Play();

        audioPlayer.pitch = (1 - v) * 5f;

        timeLeft -= Time.deltaTime;

        if(timeLeft < 0)
        {
            Shoot();
            timeLeft = timeToShoot;

            audioPlayer.Stop();
            audioPlayer.pitch = 0f;
        }
    }

    void Shoot()
    {
        // deal damage to player
        Debug.Log("Deal damage to player");
        GameObject.FindObjectOfType<PlayerManager>().GetShot();

        GameObject.FindObjectOfType<SoundManager>().shoot(0.4f);

    }

    void ResetShooting()
    {
        timeLeft = timeToShoot;

        audioPlayer.Stop();
        audioPlayer.pitch = 0f;
    }

    public void DisableCanAct()
    {
        canAct = false;
    }
}
