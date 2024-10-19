using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    [SerializeField]
    public GameObject green;
    [SerializeField]
    public GameObject yellow;
    [SerializeField]
    public GameObject red;
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer render = green.GetComponentInChildren<MeshRenderer>();
        render.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
