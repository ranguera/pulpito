using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    private Renderer r;

    [ColorUsage(true, true)]
    private Color og_color;

    [ColorUsage(true, true)]
    private Color color;

    private Vector3 temp;

    void Start()
    {
        r = GetComponent<Renderer>();
        og_color = r.material.color;
    }

    void Update()
    {
        if(r.material.color != og_color)
        {
            r.material.color = Color.Lerp(r.material.color, og_color, Time.deltaTime * Globals.decay_speed);
        }
    }

    public void SetColor()
    {
        if(Globals.random_colors )
        {
            color.r = Random.value*255;
            color.g = Random.value * 255;
            color.b = Random.value * 255;
        }
        else
        {
            color.r = Globals.r;
            color.g = Globals.g;
            color.b = Globals.b;
        }
        
        r.material.color = color;
    }
}
