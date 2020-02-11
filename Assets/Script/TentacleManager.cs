using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TentacleManager : MonoBehaviour
{
    public Transform tentacle_base;
    public GameObject head;

    public Text mode_text;
    public Text color_text;
    public Text decay_text;
    public Text random_colors_text;

    private List<Tentacle> tentacles;

    private Vector3 temp;
    private Vector3 temp2;
    private Modes mode;

    private int tentacle_index;

    private enum Modes
    {
        Pulse,
        PulseHold,
        Random,
        TentacleFlash
    }

    public void Init(int num_tentacles)
    {
        tentacles = new List<Tentacle>();
        for (int i = 0; i < num_tentacles; i++)
        {
            tentacles.Add(new Tentacle());
            tentacles[i].tentacle = new List<Link>();
        }

        Globals.r = 200;
        Globals.g = 200;
        Globals.b = 200;
        Globals.decay_speed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessGlobals();
        ChangeMode();
        ProcessMode();

        for (int i = 0; i < tentacles.Count; i++)
        {
            for (int j = 0; j < tentacles[i].tentacle.Count; j++)
            {
                float mov = (j/20f)*Mathf.PerlinNoise((Time.time+(j/10f))*.5f, i*10f) - .5f;
                temp = tentacles[i].tentacle[j].transform.position;
                temp.y = tentacle_base.transform.position.y + mov;
                tentacles[i].tentacle[j].transform.position = temp;
            }
        }
    }

    public void AddLink(int index, Link l)
    {
        tentacles[index].tentacle.Add(l);
    }

    private void ChangeMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            mode = Modes.Pulse;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            mode = Modes.PulseHold;

        if (Input.GetKeyDown(KeyCode.Alpha3))
            mode = Modes.Random;

        if (Input.GetKeyDown(KeyCode.Alpha4))
            mode = Modes.TentacleFlash;

        mode_text.text = "Mode: " + mode.ToString();
    }

    private void ProcessGlobals()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            Globals.r += Time.deltaTime *40f;
            if (Globals.r > 255f) Globals.r = 255f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            Globals.g += Time.deltaTime* 40f;
            if (Globals.g > 255f) Globals.g = 255f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            Globals.b += Time.deltaTime* 40f;
            if (Globals.b > 255f) Globals.b = 255f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Globals.r -= Time.deltaTime * 40f;
            if (Globals.r < 0f) Globals.r = 0f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            Globals.g -= Time.deltaTime * 40f;
            if (Globals.g < 0f) Globals.g = 0f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Globals.b -= Time.deltaTime * 40f;
            if (Globals.b < 0f) Globals.b = 0f;
        }




        if (Input.GetKey(KeyCode.Z))
        {
            Globals.decay_speed += Time.deltaTime * 20f;
        }

        if (Input.GetKey(KeyCode.X))
        {
            Globals.decay_speed -= Time.deltaTime * 20f;
            if (Globals.decay_speed < 0f) Globals.decay_speed = 0f;
        }



        if (Input.GetKeyDown(KeyCode.C))
        {
            Globals.random_colors = !Globals.random_colors;
        }

        color_text.text = "Color: " + Globals.r.ToString("F0") + " || " + Globals.g.ToString("F0") + " || " + Globals.b.ToString("F0") + " || ";
        decay_text.text = "Decay: " + Globals.decay_speed;
        random_colors_text.text = "Random colors: " + Globals.random_colors;
    }

    private void ProcessMode()
    {
        switch (mode)
        {
            case Modes.Pulse:
                if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
                {
                    StartCoroutine(FlashPulse());
                }
                break;

            case Modes.PulseHold:
                break;

            case Modes.Random:
                if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
                {
                    FlashRandom();
                }
                break;

            case Modes.TentacleFlash:
                if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
                {
                    FlashTentacle();
                }
                break;

            default:
                break;
        }
    }

    private void FlashRandom()
    {
        for (int i = 0; i < 10; i++)
        {
            tentacles[(int)(Random.value * tentacles.Count)].tentacle[(int)(Random.value * tentacles[0].tentacle.Count)].SetColor();
        }
    }

    private void FlashTentacle()
    {
        for (int i = 0; i < tentacles[0].tentacle.Count; i++)
        {
            tentacles[tentacle_index].tentacle[i].SetColor();
        }

        tentacle_index++;
        if (tentacle_index >= tentacles.Count) tentacle_index = 0;
    }

    private IEnumerator FlashPulse()
    {
        for (int i = 0; i < tentacles[0].tentacle.Count; i++)
        {
            for (int j = 0; j < tentacles.Count; j++)
            {
                tentacles[j].tentacle[i].SetColor();
            }
            yield return null;
        }
    }
}
