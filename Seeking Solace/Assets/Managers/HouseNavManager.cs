using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HouseNavManager : MonoBehaviour
{
    [SerializeField] GameObject RoomCentersFloor1;
    [SerializeField] GameObject RoomCentersFloor2;

    public GameObject getNearbyRooms(int floorNum)
    {
        List<int> output = new List<int>();

        if (floorNum == 2)
            return RoomCentersFloor2;
        else
            return RoomCentersFloor1;
    }

    public Vector3 getRandomRoomCenter(int floorNum)
    {
        GameObject centers = getNearbyRooms(floorNum);
        Transform output = centers.transform.GetChild(Random.Range(0, centers.transform.childCount));

        Debug.Log("Randomly chose " + output.name);

        return output.position;
    }
}
