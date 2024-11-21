using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseNavManager : MonoBehaviour
{
    [SerializeField] private int[,] connectedRooms = new int[2, 2]
        {
            { 0, 1 },   //Room 1
            { 1, 0 }    //Room 2
          //  1  2
        };

    [SerializeField] Transform[] RoomCenters;

    public List<int> getNearbyRooms(int sourceRoom)
    {
        List<int> output = new List<int>();
        Debug.Log("Getting nearby rooms for room " + sourceRoom);

        for (int i = 0; i < connectedRooms.GetLength(0); i++)
        {
            if (connectedRooms[sourceRoom, i] == 1)
                output.Add(i);
        }

        Debug.Log("Results: " + output.ToString());
        return output;
    }

    public bool isNearby(int sourceRoom, int destinationRoom)
    {
        return connectedRooms[sourceRoom, destinationRoom] == 1;
    }

    public Vector3 getRoomCenter(int areaMask)
    {
        areaMask = (int)Mathf.Log(areaMask, 2) - 3;
        if (areaMask < 0)
            areaMask = 0;
        if (areaMask >= RoomCenters.Length)
            areaMask = RoomCenters.Length - 1;

        return RoomCenters[areaMask].position;
    }
}
