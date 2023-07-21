using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsAnimator : MonoBehaviour
{

    public RawImage img;

    public Texture2D[] textures;
    private bool flipped;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void Animate(float xVel)
    {
        xVel *= 10000;
        //Debug.Log(xVel);

        //
        if (Mathf.Abs(xVel) > 25f)
        {
            //turn
            img.texture = textures[2];

        }
        else if (Mathf.Abs(xVel) > 15f)
        {
            //slight turn
            img.texture = textures[1];

        }
        else
        {
            //straight
            img.texture = textures[0];

        }

        // flip horizontally
        if (xVel > 0f && !flipped)
        {
            Vector3 scale = img.transform.localScale;
            scale.x *= -1f;
            img.transform.localScale = scale;
            flipped = true;
        }
        else if (xVel < 0f && flipped)
        {
            Vector3 scale = img.transform.localScale;
            scale.x *= -1f;
            img.transform.localScale = scale;
            flipped = false;
        }
    }
}
