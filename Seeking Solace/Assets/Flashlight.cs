using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Camera cam;
    public GameObject ihp; //item hold position object on player

    private Vector3 angOffset;
    // Start is called before the first frame update
    void Start()
    {
        angOffset = new Vector3(90f,0f,0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position == ihp.transform.position)
        {
            transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(angOffset);
        }
    }
}
