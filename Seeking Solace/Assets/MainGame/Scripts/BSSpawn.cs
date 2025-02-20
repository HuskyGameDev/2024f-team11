using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSSpawn : MonoBehaviour
{
    public GameObject bs;

    public List<Transform> spawnLocations;
    // Start is called before the first frame update
    void Start()
    {
        int index = Random.Range(0, 2);

        bs.transform.position = spawnLocations[index].position;
    }
}
