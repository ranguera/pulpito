using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleManager : MonoBehaviour
{
    public Transform tentacle_base;
    public GameObject head;
    [ColorUsage(true, true)]
    public Color off;
    [ColorUsage(true, true)]
    public Color on;

    private List<Tentacle> tentacles;
    private Vector3 temp;
    
    public void Init(int num_tentacles)
    {
        tentacles = new List<Tentacle>();
        for (int i = 0; i < num_tentacles; i++)
        {
            tentacles.Add(new Tentacle());
            tentacles[i].tentacle = new List<Transform>();
        }

        InvokeRepeating("TriggerPulse", 3f, .5f);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < tentacles.Count; i++)
        {
            for (int j = 0; j < tentacles[i].tentacle.Count; j++)
            {
                float mov = (j/10f)*Mathf.PerlinNoise((Time.time+(j/10f))*.5f, i*10f) - .5f;
                temp = tentacles[i].tentacle[j].position;
                temp.y = tentacle_base.transform.position.y + mov;
                tentacles[i].tentacle[j].position = temp;
            }
        }
    }

    public void AddLink(int index, Transform t)
    {
        tentacles[index].tentacle.Add(t);
    }

    private void TriggerPulse()
    {
        //StartCoroutine(LightUp());
        //StartCoroutine(LightBody());
        StartCoroutine(Rainbow());
    }

    private IEnumerator LightUp()
    {
        for (int j = 0; j < tentacles[0].tentacle.Count; j++)
        {
            for (int i = 0; i < tentacles.Count; i++)
            {
                tentacles[i].tentacle[j].gameObject.GetComponent<Renderer>().material.color = on;
            }
            yield return 0;

            if (j > 0)
            {
                for (int i = 0; i < tentacles.Count; i++)
                {
                    tentacles[i].tentacle[j-1].gameObject.GetComponent<Renderer>().material.color = off;
                }
                yield return 0;
            }
        }

        for (int i = 0; i < tentacles.Count; i++)
        {
            tentacles[i].tentacle[tentacles[i].tentacle.Count - 1].gameObject.GetComponent<Renderer>().material.color = off;
        }
    }

    private IEnumerator LightBody()
    {
        float h, s, v;
        float ogh, ogs, ogv;
        Color.RGBToHSV(on, out h, out s, out v);
        Color.RGBToHSV(off, out ogh, out ogs, out ogv);

        GetComponent<Renderer>().material.color = on;
        head.GetComponent<Renderer>().material.color = on;

        while(v > ogv)
        {
            v -= .2f;
            GetComponent<Renderer>().material.color = Color.HSVToRGB(h, s, v, true);
            head.GetComponent<Renderer>().material.color = Color.HSVToRGB(h, s, v, true);
            yield return null;
        }
        
    }

    private IEnumerator Rainbow()
    {
        float ogh, ogs, ogv;
        Color.RGBToHSV(off, out ogh, out ogs, out ogv);

        for (int j = 0; j < tentacles[0].tentacle.Count; j++)
        {
            float h, s, v;
            Color.RGBToHSV(new Color(Random.value, Random.value, Random.value), out h, out s, out v);
            if (v < 1f) v = 1f;

            for (int i = 0; i < tentacles.Count; i++)
            {
                tentacles[i].tentacle[j].gameObject.GetComponent<Renderer>().material.color = Color.HSVToRGB(h,s,v,true);
            }
            yield return 0;
        }

        yield return 0;
        yield return 0;

        for (int j = 0; j < tentacles[0].tentacle.Count; j++)
        {
            for (int i = 0; i < tentacles.Count; i++)
            {
                tentacles[i].tentacle[j].gameObject.GetComponent<Renderer>().material.color = Color.HSVToRGB(ogh, ogs, ogv, true);
            }
            yield return 0;
        }

        yield return 0;
    }
}
