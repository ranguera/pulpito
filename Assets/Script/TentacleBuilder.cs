using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleBuilder : MonoBehaviour
{
    public Transform body;
    public TentacleManager tentacleManager;
    public GameObject tentacle_link;
    public int num_tentacles = 8;
    public int num_links = 32;
    public float link_size_decrement = .95f; // 95 %
    
    private int i;
    private GameObject link;
    private GameObject previous_link;

    void Start()
    {
        int angle = 360 / num_tentacles;
        tentacleManager.Init(num_tentacles);

        for (i = 0; i < num_tentacles; i++)
        {
            previous_link = Instantiate(tentacle_link, transform.forward *.6f, transform.rotation, body);
            tentacleManager.AddLink(i, previous_link.transform);
            CreateTentacle();
            transform.Rotate(Vector3.up, angle);
        }
    }

    private void CreateTentacle()
    {
        for (int j = 0; j < num_links; j++)
        {
            link = Instantiate(tentacle_link, previous_link.transform.position + (previous_link.transform.forward*.2f*link_size_decrement), previous_link.transform.rotation, body);
            link.transform.localScale = previous_link.transform.localScale * link_size_decrement;
            previous_link = link;
            tentacleManager.AddLink(i, link.transform);
        }
    }
}
