using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [RangeAttribute(15,30)]
    public int MapSize;
    public int seed;//8 Digit Min/Max
    public int roomCount;


    public GameObject _wall;
    public GameObject _floor;

    enum direction
    {
        up, down, left, right
    }

    public int[,] MapMatrix;

    private void Start()
    {
        MapMatrix = new int[MapSize, MapSize];
        System.Random rand;
        if (seed != 0)
        {
            rand = new System.Random(seed);
        }
        else
        {
            rand = new System.Random();
            seed = rand.Next(1, 99999999);
            rand = new System.Random(seed);
        }
        GenerateLevel(ref rand);
    }

    private void GenerateLevel(ref System.Random rand)
    {
        InitializeMatrix();
        SpawnRooms(ref rand);
        SpawnMainPath(ref rand, 0, 0);
        SpawnEndPoints(ref rand);
        PopulateMap(ref rand);
        SpawnEnemies(ref rand);
        SpawnItems(ref rand);
        PlacePlayer(ref rand);
    }

    private void InitializeMatrix()
    {
        for (int i = 0; i <= MapSize - 1; i++)
        {
            for (int j= 0; j <= MapSize - 1; j++)
            {
                MapMatrix[i, j] = 0;
            }
        }
    }

    private void SpawnRooms(ref System.Random rand)
    {
        for (int i = 0; i <= roomCount; i++)
        {
            int roomSizeX= rand.Next(1, (int)MapSize / 4);
            int roomSizeY = rand.Next(1, (int)MapSize / 4);
            int roomPointX = rand.Next(1, MapSize - roomSizeX);
            int roomPointY = rand.Next(1, MapSize - roomSizeY);
            for (int j = roomPointX; j <= roomPointX + roomSizeX; j++)
            {
                for (int k = roomPointY; k <= roomPointY + roomSizeY; k++)
                {
                    MapMatrix[j, k] = 1;
                }
            }
        }
    }

    private void SpawnMainPath(ref System.Random rand, int locX, int locY)
    {
       
    }

    private void SpawnEndPoints(ref System.Random rand) //TODO
    {

    }

    private void PopulateMap(ref System.Random rand)
    {
        Vector3 wallSpawn = new Vector3(2.5f, 2.5f, 2.5f);
        Vector3 floorSpawn = new Vector3(0, 0, 0);
        GameObject MapMaster = new GameObject("MapMaster");

        for (int j = 0; j <= MapSize - 1; j++)
        {
            for (int i = 0; i<= MapSize - 1; i++)
            {
                if (MapMatrix[i,j] == 0)
                {
                    GameObject wall = GameObject.Instantiate(_wall);
                    wall.transform.position = wallSpawn;
                    wall.transform.parent = MapMaster.transform;
                    wallSpawn.z += 5;
                    floorSpawn.z += 5;
                }
                else if (MapMatrix[i,j] == 1)
                {
                    GameObject floor = GameObject.Instantiate(_floor);
                    floor.transform.position = floorSpawn;
                    floor.transform.parent = MapMaster.transform;
                    wallSpawn.z += 5;
                    floorSpawn.z += 5;
                }
            }
            wallSpawn.x += 5;
            floorSpawn.x += 5;

            wallSpawn.z = 2.5f;
            floorSpawn.z = 0;
        }
    }

    private void SpawnEnemies(ref System.Random rand)
    {
        //TODO
    }

    private void SpawnItems(ref System.Random rand)
    {
        //TODO
    }

    private void PlacePlayer(ref System.Random rand)
    {

    }
}
