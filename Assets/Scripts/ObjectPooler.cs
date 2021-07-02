using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int pooledAmount;

        public List<GameObject> pooledObjectList;
    }

    public static ObjectPooler Instance;
    public List<Pool> pools;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // For each type of object listed in pools,
        foreach (Pool pool in pools)
        {
            // Make a list and instantiate and store the gameobjects
            List<GameObject> newPooledObjectList = new List<GameObject>();

            for (int x = 0; x < pool.pooledAmount; x++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                newPooledObjectList.Add(obj);
            }

            pool.pooledObjectList = newPooledObjectList;
        }
    }

    // Returns Pool with the associated tag from the list of pools
    Pool SearchTag(string tag)
    {
        foreach (Pool pool in pools)
        {
            if (pool.tag == tag)
            {
                return pool;
            }
        }

        return null;
    }
}
