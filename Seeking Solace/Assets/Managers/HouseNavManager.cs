using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseNavManager : MonoBehaviour
{
    [SerializeField] private int[,] connectedRooms = new int[5, 5]
        {
            { 0, 1, 1, 1, 1 },  //Living Room
            { 1, 0, 1, 1, 1 },  //Child Room
            { 1, 1, 0, 1, 1 },  //Empty Room 1
            { 1, 1, 1, 0, 1 },  //Empty Room 2
            { 1, 1, 1, 1, 0 }   //Empty Room 3
          //  1  2  3  4  5
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
        areaMask = (int)Mathf.Log(areaMask, 2) - 4;
        if (areaMask < 0)
            areaMask = 0;
        if (areaMask >= RoomCenters.Length)
            areaMask = RoomCenters.Length - 1;

        Debug.Log(areaMask + " -> " + RoomCenters[areaMask]);

        return RoomCenters[areaMask].position;
    }
}
