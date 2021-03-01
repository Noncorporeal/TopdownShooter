using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [RangeAttribute(5,20)]
    public int MapSize;
    public int seed;//8 Digit Min/Max
    private int roomCount;
    private int[,] roomPoints;

    public GameObject _wall;
    public GameObject _floor;

    public class Tile
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;
        //false = wall exists. True = no wall;

        public bool used;
        public bool isRoom;

        public Tile()
        {
            up = false;
            down = false;
            left = false;
            right = false;
            used = false;
            isRoom = false;
        }

        public void RoomPrep()
        {
            up = true;
            down = true;
            left = true;
            right = true;
            used = true;
            isRoom = true;
        }
    }

    enum direction
    {
        up, down, left, right
    }

    public Tile[,] MapMatrix;

    private void Start()
    {
        MapMatrix = new Tile[MapSize, MapSize];
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
        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                MapMatrix[i, j] = new Tile();
            }
        }
    }

    private void SpawnRooms(ref System.Random rand)
    {

        
        if (MapSize < 9)
        {
            roomCount = MapSize - 6;
            if (roomCount <= 0)
                return;
        }
        else if (MapSize == 9 || MapSize == 10)
        {
            roomCount = 3;
        }
        else if (MapSize >= 11 && MapSize <= 13)
        {
            roomCount = rand.Next(3, 5);
        }
        else if (MapSize >= 14 && MapSize <= 16)
        {
            roomCount = rand.Next(3, 6);
        }
        else if (MapSize >= 17 && MapSize <= 20)
        {
            roomCount = rand.Next(4, 7);
        }
        //Each 2 points represents upper-left and lower-right corner of room

        int[] pointA = new int[2];
        int[] pointB = new int[2];

        System.Random r = new System.Random(seed);

        for (int x = 0; x < roomCount; x++)
        {
            pointA[0] = rand.Next(0, MapSize - 2);
            pointA[1] = rand.Next(0, MapSize - 2);
            pointB[0] = rand.Next(pointA[0] + 1, pointA[0] + 4);
            pointB[1] = rand.Next(pointA[1] + 1, pointA[1] + 4);

            if (pointB[0] >= MapSize)
            {
                pointB[0] = MapSize - 1;
            }
            if (pointB[1] >= MapSize)
            {
                pointB[1] = MapSize - 1;
            }

            for (int j = pointA[1]; j <= pointB[1]; j++)
            {
                for (int i = pointA[0]; i <= pointB[0]; i++)
                {
                    MapMatrix[i, j].RoomPrep();
                    
                    if (i == pointA[0])
                    {
                        if (r.Next(3) != 0)
                            MapMatrix[i, j].left = false;
                    }
                    if (i == pointB[0])
                    {
                        if (i + 1 != MapSize)
                            if (r.Next(3) != 0)
                                MapMatrix[i + 1, j].left = false;
                        if (r.Next(3) != 0)
                            MapMatrix[i, j].right = false;
                    }
                    if (j == pointA[1])
                    {
                        if (r.Next(3) != 0)
                            MapMatrix[i, j].up = false;
                    }
                    if (j == pointB[1])
                    {
                        if (j + 1 != MapSize)
                            if (r.Next(3) != 0)
                                MapMatrix[i, j + 1].up = false;
                        if (r.Next(3) != 0)
                            MapMatrix[i, j].down = false;
                    }
                    
                }
            }
        }
    }

    private void SpawnMainPath(ref System.Random rand, int locX, int locY)//DONE, DONT TOUCH
    {
        /*
         * This section is responsible for creating a maze. It will not spawn a maze around rooms.
         */
        MapMatrix[locX, locY].used = true;
        List<direction> directions = new List<direction>();
        //Check left
        if (locX != 0)
        {
            if (MapMatrix[locX - 1, locY].used == false)
            {
                directions.Add(direction.left);
            }
        }
        //Check right
        if (locX != MapSize - 1)
        {
            if (MapMatrix[locX + 1, locY].used == false)
            {
                directions.Add(direction.right);
            }
        }
        //check up
        if (locY != 0)
        {
            if (MapMatrix[locX, locY - 1].used == false)
            {
                directions.Add(direction.up);
            }
        }
        //check down
        if (locY != MapSize - 1)
        {
            if (MapMatrix[locX, locY + 1].used == false)
            {
                directions.Add(direction.down);
            }
        }

        //check if direction list is empty, also removes us from function at end.
        if (directions.Count == 0)
            return;

        //as long as there are more directions, keep filling them.
        while (directions.Count != 0)
        {
            
            int rng = rand.Next(0, 100);
            rng = rng % (directions.Count);
            switch (directions[rng])
            {
                case direction.up:
                    //double-check that direction hasnt been used yet
                    if (MapMatrix[locX, locY - 1].used)
                    {
                        directions.Remove(direction.up);
                        break;
                    }
                    //remove walls between cells
                    MapMatrix[locX, locY].up = true;
                    MapMatrix[locX, locY - 1].down = true;
                    //move to new cell and repeat
                    SpawnMainPath(ref rand, locX, locY - 1);
                    //remove direction from choices.
                    directions.Remove(direction.up);
                    break;
                case direction.down:
                    if (MapMatrix[locX, locY + 1].used)
                    {
                        directions.Remove(direction.down);
                        break;
                    }
                    MapMatrix[locX, locY].down = true;
                    MapMatrix[locX, locY + 1].up = true;
                    SpawnMainPath(ref rand, locX, locY + 1);
                    directions.Remove(direction.down);
                    break;
                case direction.left:
                    if (MapMatrix[locX - 1, locY].used)
                    {
                        directions.Remove(direction.left);
                        break;
                    }
                    MapMatrix[locX, locY].left = true;
                    MapMatrix[locX - 1, locY].right = true;
                    SpawnMainPath(ref rand, locX - 1, locY);
                    directions.Remove(direction.left);
                    break;
                case direction.right:
                    if (MapMatrix[locX + 1, locY].used)
                    {
                        directions.Remove(direction.right);
                        break;
                    }
                    MapMatrix[locX, locY].right = true;
                    MapMatrix[locX + 1, locY].left = true;
                    SpawnMainPath(ref rand, locX + 1, locY);
                    directions.Remove(direction.right);
                    break;
            }
        }
        return;
    }

    private void SpawnEndPoints(ref System.Random rand) //TODO
    {

    }

    private void PopulateMap(ref System.Random rand)//DONE, DONT TOUCH
    {
        Vector3 upLoc = new Vector3(0, 0, 0);  
        Vector3 downLoc = new Vector3(5, 0, 0);
        Vector3 leftLoc = new Vector3(0, 0, 0);
        Vector3 rightLoc = new Vector3(0, 0, 5);
        Vector3 floorLoc = new Vector3(0, 0, 0);
        GameObject mapMaster = new GameObject("Map Master");

        for (int j = 0; j < MapSize; j++)
        {
            for (int i = 0; i < MapSize; i++)
            {
                GameObject floor = GameObject.Instantiate(_floor);
                floor.transform.position = floorLoc;
                floor.transform.parent = mapMaster.transform;
                if (!MapMatrix[i,j].left)
                {
                    GameObject leftWall = GameObject.Instantiate(_wall);
                    leftWall.transform.position = leftLoc;
                    leftWall.transform.parent = mapMaster.transform;
                    leftWall.layer = 10;
                }
                if (!MapMatrix[i,j].up)
                {
                    GameObject upWall = GameObject.Instantiate(_wall);
                    upWall.transform.position = upLoc;
                    upWall.transform.rotation = Quaternion.Euler(0,-90,0);
                    upWall.transform.parent = mapMaster.transform;
                    upWall.layer = 10;
                }
                if(i == MapSize - 1)
                {
                    if (!MapMatrix[i,j].right)
                    {
                        GameObject rightWall = GameObject.Instantiate(_wall);
                        rightWall.transform.position = rightLoc;
                        rightWall.transform.parent = mapMaster.transform;
                        rightWall.layer = 10;
                    }
                }
                if(j == MapSize - 1)
                {
                    if (!MapMatrix[i,j].down)
                    {
                        GameObject downWall = GameObject.Instantiate(_wall);
                        downWall.transform.position = downLoc;
                        downWall.transform.rotation = Quaternion.Euler(0, -90, 0);
                        downWall.transform.parent = mapMaster.transform;
                        downWall.layer = 10;
                    }
                }
                //move spawn locations 5 units over
                upLoc.z += 5;
                downLoc.z += 5;
                rightLoc.z += 5;
                leftLoc.z += 5;
                floorLoc.z += 5;
            }
            //reset spawn locations
            upLoc.z = 0;
            downLoc.z = 0;
            rightLoc.z = 5;
            leftLoc.z = 0;
            floorLoc.z = 0;
            //move spawn locations down
            upLoc.x += 5;
            downLoc.x += 5;
            rightLoc.x += 5;
            leftLoc.x += 5;
            floorLoc.x += 5;
            
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
