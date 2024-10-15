//@Author Sabyasachi Thakur 
//Last Modified On 3/4/2023
using System.Collections.Generic;
using UnityEngine;

namespace RedApple.SubwaySurfer
{
    public class ObjectSpawnerInBlock : MonoBehaviour
    {
        [SerializeField] private Lanes LongBlockLane; // Langes to choose for eache spawnable object
        [SerializeField] private List<Spawnables> spawnableObjs = new List<Spawnables>(); // A list of all spwanble objects

        private int maxCoinChunkCount = 7;
        private Vector3 XPosIncreaseV = Vector3.zero;

        public bool canSpawn = true; // this bool checks either the current object can spawn or not
        private int RanomLaneNoForObstacle;
        private int lastLaneForObstacle=-1;

        private void Start()
        {
            //canSpawn = true;
            //Initilizing the pool for spawnables 
            foreach (Spawnables item in spawnableObjs)
            {
                CreatePool(item.ObjectPrefab, item.MaxSpawnableCounts, item.MinSpawnableCounts, item.ActiveObjectInLanes);
            }
        }

        private void Update()
        {
            // if canSpwan is true then spawn objects in randomm lane 
            if (canSpawn)
            {
                canSpawn = false;
                foreach (Spawnables item in spawnableObjs)
                {
                    SpawnObjectInLane(item.ActiveObjectInLanes, item.xPosIncrease, item.MidLaneOffSet, item.LeftLaneOffset, item.RightLaneOffset);
                }
            }
        }

        /// <summary>
        /// creates a object pool in child of this platform for spawnble objects
        /// </summary>
        /// <param name="Obj"></param> the pool is created based on this object
        /// <param name="count"></param> Max counting for the object in the pool
        /// <param name="PooledObjs"></param> A list of objects who are in current pool
        private void CreatePool(List<GameObject> Obj, int Maxcount, int MinCount, List<GameObject> PooledObjs)
        {
            int count = Random.Range(MinCount, Maxcount);
            for (int i = 0; i < count; i++)
            {
                int r = Random.Range(0, Obj.Count);
                GameObject g = Instantiate(Obj[r], this.transform);
                g.SetActive(false);
                PooledObjs.Add(g);
            }
        }

        /// <summary>
        /// Spwans objects in random lane with user prefernce when ever called
        /// </summary>
        /// <param name="ObjectsPool"></param> the object pool where the object will get selected
        /// <param name="xPosIncrease"></param> X position increase of the object based on the platform
        /// <param name="MidLaneOffset"></param> mid lane position respect too the platform and the object
        /// <param name="LeftLaneOffset"></param> left lane position respect too the platform and the object
        /// <param name="RightLaneOffset"></param> Right lane position respect too the platform and the object
        private void SpawnObjectInLane(List<GameObject> ObjectsPool, float xPosIncrease, Vector3 MidLaneOffset, Vector3 LeftLaneOffset, Vector3 RightLaneOffset)
        {
            int count = 0;
            int RanomLaneNoForCoin = 0;


            foreach (var item in ObjectsPool)
            {
                item.SetActive(true);
                ObstacleType obstacleType = item.GetComponent<ObstacleType>();

                //Randomly selecting a lane here
                if (!obstacleType.IsObstacle)
                {
                    if (count == 0)
                    {
                        RanomLaneNoForCoin = Random.Range(0, 3);
                    }
                    //Debug.Log(count);
                    count++;

                    switch (RanomLaneNoForCoin)
                    {
                        case (int)Lanes.Middle:
                            item.transform.position = this.transform.position + MidLaneOffset + XPosIncreaseV;
                            break;
                        case (int)Lanes.Left:
                            item.transform.position = this.transform.position + LeftLaneOffset + XPosIncreaseV;
                            break;
                        case (int)Lanes.Right:
                            item.transform.position = this.transform.position + RightLaneOffset + XPosIncreaseV;
                            break;
                    }
                    if (count >= maxCoinChunkCount)
                    {
                        count = 0;
                    }
                }

                else
                {

                    ObstacleType obstacle = item.GetComponent<ObstacleType>();

                    switch (obstacle.obstacleType)
                    {
                        case SpawnObstacleType.Slide:
                            RanomLaneNoForObstacle = GetRandomLane( new List<int>() { 0,1,2});
                            break;
                        case SpawnObstacleType.Jump:
                            RanomLaneNoForObstacle = GetRandomLane(new List<int>() { 0, 1, 2 });
                            break;
                        case SpawnObstacleType.Car:
                            RanomLaneNoForObstacle = GetRandomLane(new List<int>() { 0, 1, 2 });
                            break;
                        case SpawnObstacleType.LeftTurn:
                            RanomLaneNoForObstacle = GetRandomLane(new List<int>() {  1, 2 });
                            break;
                        case SpawnObstacleType.RightTurn:
                            RanomLaneNoForObstacle = GetRandomLane(new List<int>() { 0, 1 });
                            break;
                        case SpawnObstacleType.Coin:
                            break;
                        default:
                            break;
                    }
                    lastLaneForObstacle = RanomLaneNoForObstacle;

                    switch (RanomLaneNoForObstacle)
                    {
                        case (int)Lanes.Middle:
                            item.transform.position = this.transform.position + MidLaneOffset + XPosIncreaseV;
                            obstacle.ObstacleDir = ObstacleDirection.middle;
                            ObstacleType.OnSetObsPos?.Invoke();
                            break;
                        case (int)Lanes.Left:
                            item.transform.position = this.transform.position + LeftLaneOffset + XPosIncreaseV;
                            obstacle.ObstacleDir = ObstacleDirection.left;
                            ObstacleType.OnSetObsPos?.Invoke();
                            break;
                        case (int)Lanes.Right:
                            item.transform.position = this.transform.position + RightLaneOffset + XPosIncreaseV;
                            obstacle.ObstacleDir = ObstacleDirection.right;
                            ObstacleType.OnSetObsPos?.Invoke();

                            break;
                    }
                }

                //Increaseing X position of the object 
                XPosIncreaseV.x -= xPosIncrease;
            }
            //resetting the X position vector after all spawn
            XPosIncreaseV.x = 0;

        }
        int GetRandomLane(List<int> laneList)
        {
           
            List<int> customList = laneList;
            int randNo = Random.Range(0,customList.Count);
           
            if(lastLaneForObstacle == customList[randNo])
            {
                customList.RemoveAt(randNo);
                randNo = Random.Range(0, customList.Count);
               
            }
            //Debug.LogError(lastLaneForObstacle + " : lastLaneForObstacle ,rand no : " + customList[randNo]);
            lastLaneForObstacle = customList[randNo];

            return customList[randNo];

        }
    }

}

public enum SpawnObstacleType
{
    Slide = 0,
    Jump = 1,
    Car = 2,
    LeftTurn = 3,
    RightTurn = 4,
    Coin 
}
[System.Serializable]
public class Spawnables
{
    public string Name;
    public List<GameObject> ObjectPrefab;
    public List<GameObject> ActiveObjectInLanes = new List<GameObject>();
    public Vector3 MidLaneOffSet;
    public Vector3 LeftLaneOffset;
    public Vector3 RightLaneOffset;
    public float xPosIncrease;
    public int MaxSpawnableCounts;
    public int MinSpawnableCounts;
    public bool isObstacle;
    public SpawnObstacleType ObstacleType;
}
